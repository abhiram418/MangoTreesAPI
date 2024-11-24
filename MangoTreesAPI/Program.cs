using MangoTreesAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Developer add-ons
// Detailed return when problem comes
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));

// Configure Custom Services logic
ServiceMiddleware.ConfigureCustomServices(builder.Services);

// Configure authentication logic
AuthenticationMiddleware.ConfigureAuthentication(builder.Services, builder.Configuration);

// Configure Swagger authentication logic
AuthenticationMiddleware.ConfigureSwaggerAuth(builder.Services);

// Configure AWS logic
AWSMiddleware.ConfigureAWS(builder.Services, builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => "MangoTrees API is Working");

app.Run();
