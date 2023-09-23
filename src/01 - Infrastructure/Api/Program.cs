using Api.Configurations;
using Api.Extensions.Middlewares;
using Application.Configurations;
using Application.Extensions.Autorizacao;
using Data.Configurations;
using Domain.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.WebApiConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddDependencyInjectionsApp();
builder.Services.AddDependencyInjectionsDomain();

var app = builder.Build();

app.Services.ConfigurarBancoDados();
app.UseCors("Production");

app.UseMiddleware<MiddlewareException>();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var apiDescriptorProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerConfig(apiDescriptorProvider);
app.Run();
