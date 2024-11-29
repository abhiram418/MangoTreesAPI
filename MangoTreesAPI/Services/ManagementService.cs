using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using MangoTreesAPI.Models;
using MangoTreesAPI.Tables;
using System.Transactions;

namespace MangoTreesAPI.Services
{
    public class ManagementService
    {
        private readonly IMapper mapper;
        private readonly IDynamoDBContext context;
        public ManagementService(IDynamoDBContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }

        public async Task<string> AddTransactionDataAsync(TransactionModel transactionData)
        {
            var transaction = mapper.Map<TransactionCollection>(transactionData);
            await context.SaveAsync(transaction);
            return transaction.TransactionId;
        }
        public async Task<string> UpdateTransactionDataAsync(string orderId, OrderStatusEnum transactioStatus)
        {
            var transactionData = await GetTransactionDataAsync(orderId);
            var transaction = mapper.Map<TransactionCollection>(transactionData);
            transaction.Status = transactioStatus;
            transaction.TransactionId = transactionData.TransactionId!;
            await context.SaveAsync(transactionData);
            return transaction.TransactionId;
        }
        public async Task<TransactionModel> GetTransactionDataAsync(string orderId)
        {
            var transactionData = await context.LoadAsync<TransactionCollection>(orderId);
            var transaction = mapper.Map<TransactionModel>(transactionData);
            return transaction;
        }
    }
}
