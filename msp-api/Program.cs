using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
const string MyAllowAllOrigins = "MyAllowAllOrigins";
const string tokenScheme = nameof(tokenScheme);

// Add services to the container.
var config = MSA.Action_Config.ConfigManager.AppSetting;
builder.Services.AddSingleton<MSA.Action_Config.Action_Config>();

builder.Services.AddControllers().AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters
            .Add(new JsonStringEnumConverter()));
//builder.services.addscoped < msl.>
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowAllOrigins,
    builder =>
    {
        builder
             //.AllowAnyOrigin()
             .WithOrigins(config["ConnectionStrings:FE"] ?? "No string was found")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        // .SetIsOriginAllowed(origin => true);
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowAllOrigins);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Environment.CurrentDirectory, "Content")),
    RequestPath = new PathString("/Content")

});

app.UseAuthorization();

app.MapControllers();

app.Run();
