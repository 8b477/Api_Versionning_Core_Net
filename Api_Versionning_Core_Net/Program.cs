using Api_Versionning_Core_Net.Configuration;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

#region Configuration du versionning
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    // Permet de ne pas avoir de controle de version pour chaque controller,
    // (sorte de protection en cas d'implémentation d'un nouvrau controller rapide sans versioning).
    config.AssumeDefaultVersionWhenUnspecified = true;
    // Permet d'utiliser le header d'un appel avec des paramètres de versionning d'api (ou deprecated / fin de vie).
    config.ReportApiVersions = true;
    config.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(config =>
{
    // une URI sous la forme ..
    config.GroupNameFormat = "'v'VVV";
    // Permet de changer mes <VVV> par la version de l'api dispo dans l'attribut du controller.
    config.SubstituteApiVersionInUrl = true;
});
#endregion

builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<ConfigurationSwaggerOptions>();

builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // On remplace la liste de swagger<UI> avec les versions lsitées par le service ApiVersionDescription.

    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var item in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{item.GroupName}/swagger.json", item.ApiVersion.ToString());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
