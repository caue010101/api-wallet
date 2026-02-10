using Repository.Interfaces.Users;
using Services.Interfaces.Users;
using Repository.Users;
using Services.Users;
using Repository.Interfaces.Wallets;
using Services.Interfaces.Wallets;
using Repository.Wallets;
using Services.Wallets;
using Services.Transactions;
using Services.Interfaces.Transactions;
using Repository.Transactions;
using Repository.Interfaces.Transactions;

var builder = WebApplication.CreateBuilder();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<DapperContext>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IWalletService, WalletService>();

builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<ITransactionService, TransactionService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
