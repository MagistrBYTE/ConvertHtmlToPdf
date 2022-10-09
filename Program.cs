using Convert.DataLayer;
using Convert.FilePreparation;
using Convert.FileStorage;
using Convert.Info;
using Convert.Infrastructure;
using Converter.ConverterFile;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//
// Базовые сервисы для работы
//
builder.Services.AddCors(x => x.AddDefaultPolicy(builder => builder
	.AllowAnyHeader()
	.AllowAnyMethod()
	.AllowAnyOrigin()));
builder.Services.AddOptions();
builder.Services.AddHttpContextAccessor();

//
// Сервисы контролеров и сессии
//
builder.Services.AddControllers();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

//
// Сервисы проекта
//
builder.Services.AddTransient<IConverterFileService, ConverterFileService>();
builder.Services.AddTransient<IFilePreparationService, FilePreparationService>();
builder.Services.AddTransient<IFileStorage, LocalDirectoryFileStorage>();
builder.Services.AddTransient<IInfoService, InfoService>();


//
// Инфраструктура
//
builder.Services.AddHangfireBlock(builder.Configuration); 
builder.Services.AddFileConvertedDb(builder.Configuration);


var app = builder.Build();

//---------------------------------------------------------------------------------------------------------------------
//
// Конфигурация конвейера обработки запроса
//
//---------------------------------------------------------------------------------------------------------------------

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Error");

	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseFileServer();
app.UseStaticFiles();

app.UseHangfireDashboard();

app.UseSession();

app.UseCors();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
	endpoints.MapHangfireDashboard();
});

app.InitFileConvertedDb(dbContext => dbContext.Database.Migrate());

app.Run();
