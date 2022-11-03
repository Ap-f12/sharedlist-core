
using SharedListApi.Filters;
using SharedListApi.Configuration;
using SharedListApi.Data;
using SharedListApi.Services;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddControllers(options =>
//{
//    options.Filters.Add(new ValidateTokenFilter(new UserRegistrationService()));
//});


builder.Services.AddControllers();

builder.Services.AddSingleton<IDataService, CosmosDbDataService>();
builder.Services.AddScoped<IUserRegistrationService, UserRegistrationService>();
builder.Services.AddScoped<IShareCheckListService, ShareCheckListService>();
builder.Services.AddScoped<ICheckListService, CheckListService>();
builder.Services.AddScoped<ValidateTokenFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();






app.Run();
