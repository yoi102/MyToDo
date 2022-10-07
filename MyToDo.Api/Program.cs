using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyToDo.Api;
using MyToDo.Api.Context;
using MyToDo.Api.Context.Repository;
using MyToDo.Api.Context.UnitOfWork;
using MyToDo.Api.Extensions;
using MyToDo.Api.Service;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
//builder.WebHost.UseUrls(new[] { "http://*:800" });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //注释
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";


    //第二个参数为是否显示控制器注释,我们选择true
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename), true);



    typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
    {
        //添加文档介绍
        options.SwaggerDoc(version, new OpenApiInfo
        {
            Title = $"MyToDo",
            Version = version,
            Description = $"MyToDo:{version}版本"
        });
    });
});
builder.Services.AddDbContext<MyToDoContext>(options =>
{

    var connectionString = builder.Configuration.GetConnectionString("ToDoConnection");

    options.UseSqlite(connectionString);



}).AddUnitOfWork<MyToDoContext>()
.AddCustomRepository<ToDo, ToDoRepository>()
.AddCustomRepository<Memo, MemoRepository>()
.AddCustomRepository<User, UserRepository>();

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

    app.UseSwaggerUI(options =>
    {

        /*
         options.SwaggerEndpoint($"/swagger/V1/swagger.json",$"版本选择:V1");
        */
        //如果只有一个版本也要和上方保持一致
        typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
        {
            //切换版本操作
            //参数一是使用的哪个json文件,参数二就是个名字
            options.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"版本选择:{version}");
        });
    });
}
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyToDo.Api v1"));


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
