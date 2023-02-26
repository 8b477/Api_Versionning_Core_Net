using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api_Versionning_Core_Net.Configuration
{
    public class ConfigurationSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {

        #region Injection de dépendance
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigurationSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        #endregion

        public void Configure(SwaggerGenOptions options)
        {
            // Configuration du swagger pour la dévouverte des versions des apis (dynamiquement)
            foreach (var item in _provider.ApiVersionDescriptions)
            {
                // On balaye tous les controllers avec l'apiVersion pour les lister dans le swagger 'ui'.
                options.SwaggerDoc(item.GroupName, CreateVersionInfo(item));

            }
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "Test de ma versui API",
                Version = description.ApiVersion.ToString()
            };

            // On peut gérer la fin de vie d'une api
            if(description.IsDeprecated)
            {
                info.Description = "Fin de vie pour cette API !";
            }    
            return info;
        }
    }
}
