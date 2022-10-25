using SharedListApi.Configuration;
using SharedListApi.Data;
using SharedListApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// inject cosmos db client
builder.Services.AddSingleton<IDataService, CosmosDbDataService>();
builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
builder.Services.AddScoped<IShareCheckListService, ShareCheckListService>();
builder.Services.AddScoped<ICheckListService, CheckListService>();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();



app.Run();
