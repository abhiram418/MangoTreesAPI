using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using MangoTreesAPI.Models;
using MangoTreesAPI.Tables;
using static MangoTreesAPI.Models.ResponseMessages;
namespace MangoTreesAPI.Services
{
    public class CustomerService
    {
        private readonly ProductService productService;
        private readonly AuthService authService;
        private readonly ManagementService managementService;
        private readonly IDynamoDBContext context;
        private readonly IMapper mapper;
        public CustomerService(ProductService _productService, AuthService _authService, ManagementService _managementService, IDynamoDBContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
            productService = _productService;
            authService = _authService;
            managementService = _managementService;
        }

        public async Task<string> AddUserDataAsync(UserRequestModel data, string otp)
        {
            var isUserValid = await authService.ValidateUserOTPAsync(data.PhoneNumber, otp);

            if (isUserValid == true)
            {
                var userData = mapper.Map<UsersModels>(data);
                var address = mapper.Map<AddressCollection>(data.Address);
                var user = mapper.Map<UsersCollection>(userData);
                var authUser = mapper.Map<UserAuthenticationModel>(user);

                user.AddressList = [address.AddressId];
                user.Cart = await AddCartAsync(user.UserId);

                await context.SaveAsync(address);
                await authService.AddUserAuthenticationDataAsync(authUser);
                await context.SaveAsync(user);

                return SignupResponse.SuccessProperty1;
            }
            else
            {
                return SignupResponse.FailureProperty2;
            }
        }
        public async Task<UsersModels> GetUserDataAsync(string userId)
        {
            var userData = await context.LoadAsync<UsersCollection>(userId);
            var user = mapper.Map<UsersModels>(userData);
            return user;
        }
        private async Task UpdateUserDataAsync(UsersModels userData, string userId)
        {
            var user = mapper.Map<UsersCollection>(userData);
            user.UserId = userId;
            await context.SaveAsync(user);
        }
        public async Task UpdateCustomerDetailsAsync(UserEditDetailsModel userEditDetailsModel, string userId)
        {
            var userData = await GetUserDataAsync(userId);
            var user = mapper.Map<UsersCollection>(userData);
            user.UserId = userId;
            user.FirstName = userEditDetailsModel.FirstName;
            user.LastName = userEditDetailsModel.LastName;
            if (userEditDetailsModel.Email != null)
            {
                user.Email = userEditDetailsModel.Email;
            }
            user.Gender = userEditDetailsModel.Gender;
            await context.SaveAsync(user);
        }
        public async Task UpdateCustomerPasswordAsync(ResetPasswordModel resetPassword)
        {
            var otpVaid = await authService.ValidateUserOTPAsync(resetPassword.PhoneNumber, resetPassword.OTP);
            if(otpVaid != true)
            {
                throw new Exception("User Not Valid");
            }

            var scanConditions = new List<ScanCondition>
            {
                new ScanCondition("PhoneNumber", Amazon.DynamoDBv2.DocumentModel.ScanOperator.Equal, resetPassword.PhoneNumber)
            };
            var userAuthenticationResultsData = await context.ScanAsync<UserAuthenticationCollection>(scanConditions).GetRemainingAsync();
            var userResultsData = await context.ScanAsync<UsersCollection>(scanConditions).GetRemainingAsync();

            var userAuthenticationData = userAuthenticationResultsData.FirstOrDefault();
            var userData = userResultsData.FirstOrDefault();

            var userAuthentication = mapper.Map<UserAuthenticationModel>(userAuthenticationData);
            var user = mapper.Map<UsersModels>(userData);

            userAuthentication.Password = resetPassword.Password;
            user.Password = resetPassword.Password;

            await UpdateUserDataAsync(user, userData!.UserId);
            var userAuthenticationResetData = mapper.Map<UserAuthenticationCollection>(userAuthentication);
            userAuthenticationResetData.UserName = userAuthenticationData!.UserName;
            await context.SaveAsync(userAuthenticationResetData);
        }

        public async Task<string> AddCartAsync(string userId)
        {
            var cart = new CartCollection() { UserId = userId };
            await context.SaveAsync(cart);
            return cart.CartId;
        }
        public async Task<CartModel> GetCartAsync(string cartId)
        {
            var cartData = await context.LoadAsync<CartCollection>(cartId);
            var cart = mapper.Map<CartModel>(cartData);
            return cart;
        }
        public async Task<string[]> AddToCartAsync(string cartId, string[] itemsData)
        {
            var cartData = await GetCartAsync(cartId);
            var items = new List<string>();
            foreach (var productId in itemsData)
            {
                var productData = await productService.GetProductDataAsync(productId);
                var inventoryData = await productService.GetInventoryDataAsync(productData.InventoryId);
                if (productData != null && productData.Availability)
                {
                    if(inventoryData?.ExpirationDate == null)
                    {
                        items.Add(productId);
                    }
                    else
                    {
                        if(inventoryData?.ExpirationDate > DateTime.Now)
                        {
                            items.Add(productId);
                        }
                    }
                }
            }
            var cart = mapper.Map<CartCollection>(cartData);
            cart.CartId = cartId;
            cart.Items = items.ToArray();
            await context.SaveAsync(cart);
            return items.ToArray();
        }

        public async Task<string> AddAddressAsync(AddressModel addressData, string userId)
        {
            var address = mapper.Map<AddressCollection>(addressData);
            await context.SaveAsync(address);
            var userData = await GetUserDataAsync(userId);
            userData.AddressList.Add(address.AddressId);
            await UpdateUserDataAsync(userData, userId);
            return address.AddressId;
        }
        public async Task<AddressResponseModel[]> GetAddressListAsync(string userId)
        {
            var userData = await GetUserDataAsync(userId);
            var addressListData = new List<AddressResponseModel>();
            foreach (var id in userData.AddressList)
            {
                var address = await GetAddressAsync(id);
                if(address != null)
                {
                    addressListData.Add(address);
                }
            }

            return addressListData.ToArray();
        }
        public async Task<AddressResponseModel> GetAddressAsync(string addressId)
        {
            var addressData = await context.LoadAsync<AddressCollection>(addressId);
            var address = mapper.Map<AddressResponseModel>(addressData);
            return address;
        }
        public async Task UpdateAddressAsync(AddressModel addressData, string addressId)
        {
            var updatedAddress = mapper.Map<AddressCollection>(addressData);
            updatedAddress.AddressId = addressId;
            await context.SaveAsync(updatedAddress);
        }
        public async Task DeleteAddressAsync(string addressId, string userId)
        {
            var userData = await GetUserDataAsync(userId);
            if(userData.AddressList.Count > 1)
            {
                await context.DeleteAsync<AddressCollection>(addressId);
            }
            else
            {
                throw new Exception("You must have at least one address saved. Please add a new address before deleting the current one");
            }
        }


        public async Task<string> PostOrderAsync(OrderRequestModel orderRequestData, string userId)
        {
            var orderData = mapper.Map<OrderModel>(orderRequestData);
            orderData.DeliveryMethod = await PostDeliveryMethodAsync(orderRequestData.DeliveryMethod);
            orderData.OrderItems = await PostOrderItemListAsync(orderRequestData.OrderItems);
            var order = mapper.Map<OrderCollection>(orderData);
            var transactionData = new TransactionModel()
            {
                OrderId = order.OrderId,
                UserId = userId,
                TransactionDate = order.OrderDate,
                PaymentMethod = order.PaymentMethod,
                Status = OrderStatusEnum.Pending,
            };

            var user = await GetUserDataAsync(userId);
            user.OrderHistory.Add(order.OrderId);

            await context.SaveAsync(order);
            await UpdateUserDataAsync(user, userId);
            await managementService.AddTransactionDataAsync(transactionData);

            return order.OrderId;
        }
        public async Task UpdateOrderAsync(string orderId, OrderStatusEnum orderStatus)
        {
            var orderData = await GetOrderAsync(orderId);
            var transactionId = await managementService.UpdateTransactionDataAsync(orderId, orderStatus);
            orderData.OrderStatus = orderStatus;
            var order = mapper.Map<OrderCollection>(orderData);
            order.OrderId = orderId;
            await context.SaveAsync(order);
        }
        public async Task<OrderModel[]> GetOrdersListAsync(string userId)
        {
            var userData = await GetUserDataAsync(userId);
            if(userData.OrderHistory.Count == 0)
            {
                return [];
            }
            var ordersListData = new List<OrderModel>();
            foreach (var id in userData.OrderHistory)
            {
                var order = await GetOrderAsync(id);
                if(order != null && order.OrderStatus != OrderStatusEnum.AwaitingPayment && order.OrderStatus != OrderStatusEnum.PaymentFailed)
                {
                    ordersListData.Add(order);
                }
            }
            return ordersListData.ToArray();
        }
        private async Task<OrderModel> GetOrderAsync(string orderId)
        {
            var orderData = await context.LoadAsync<OrderCollection>(orderId);
            var order = mapper.Map<OrderModel>(orderData);
            return order;
        }

        private async Task<string> PostDeliveryMethodAsync(DeliveryMethodModel deliveryMethodData)
        {
            var deliveryMethod = mapper.Map<DeliveryMethodCollection>(deliveryMethodData);
            await context.SaveAsync(deliveryMethod);
            return deliveryMethod.DeliveryMethodId;
        }

        private async Task<string[]> PostOrderItemListAsync(OrderItemModel[] orderItemsData)
        {
            var orderItemsIdList = new List<string>();
            foreach (var orderItem in orderItemsData)
            {
                var orderId = await PostOrderItemAsync(orderItem);
                if(orderId != null)
                {
                    orderItemsIdList.Add(orderId);
                }
            }
            return orderItemsIdList.ToArray();
        }
        private async Task<string> PostOrderItemAsync(OrderItemModel orderItemData)
        {
            var orderItem = mapper.Map<OrderItemCollection>(orderItemData);
            await context.SaveAsync(orderItem);
            return orderItem.OrderItemId;
        }
    }
}
