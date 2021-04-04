using System.Collections.Generic;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace TRMDataManager.App_Start
{
    /// <summary>Adds a page to get a authentication token from the Swagger UI.</summary>
    /// <remarks>
    /// source: https://stackoverflow.com/questions/51117655
    /// </remarks>
    /// <seealso cref="AuthorizationOperationFilter"/>
    public class AuthTokenOperation : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.paths.Add("/token", new PathItem
            {
                post = new Operation
                {
                    tags = new List<string>{ "Auth" },
                    consumes = new List<string>
                    {
                        "application/x-www-form-urlencoded"
                    },
                    parameters = new List<Parameter>
                    {
                        new Parameter
                        {
                            type = "string",
                            name = "grant_type",
                            required = true,
                            @in = "formData",
                            @default = "password"
                        },
                        new Parameter
                        {
                            type = "string",
                            name = "username",
                            required = true,
                            @in = "formData"
                        },
                        new Parameter
                        {
                            type = "string",
                            name = "password",
                            required = true,
                            @in = "formData"
                        },
                    }
                }
            });
        }
    }
}