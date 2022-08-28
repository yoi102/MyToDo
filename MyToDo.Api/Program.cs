using AutoMapper;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyToDo.Api.Context;
using MyToDo.Api.Context.Repository;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Api.Extensions;
using MyToDo.Api.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//builder.WebHost.UseUrls(new[] { "http://*:800" });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyToDoContext>(options =>
{

    var connectionString = builder.Configuration.GetConnectionString("ToDoConnection");

    options.UseSqlite(connectionString);



}).AddUnitOfWork<MyToDoContext>()
.AddCustomRepository<ToDo,ToDoRepository>()
.AddCustomRepository<Memo,MemoRepository>()
.AddCustomRepository<User,UserRepository>();

builder.Services.AddTransient<IToDoService, ToDoService>();
builder.Services.AddTransient<IMemoService, MemoService>();
builder.Services.AddTransient<ILoginService, LoginService>();

//添加AutoMapper
var autoMapperConfig = new MapperConfiguration(config =>
{
    config.AddProfile(new AutoMapperProFile());

});


builder.Services.AddSingleton(autoMapperConfig.CreateMapper());
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyToDo.Api", Version = "v1" });
});

//builder.WebHost.UseUrls("http://*:2233");//这里可以是数组，表示启动多个端口
//builder.WebHost.ConfigureKestrel(options => options.ListenLocalhost(8090, opts => opts.Protocols = HttpProtocols.Http1));

var app = builder.Build();

//app.Urls.Add("https://localhost:8091");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyToDo.Api v1"));


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
