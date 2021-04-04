using System.Collections.Generic;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace TRMDataManager.App_Start
{
    /// <summary>Extend all documented endpoints with ability to provided auth token.</summary>
    /// <remarks>
    /// source: https://stackoverflow.com/questions/51117655
    /// </remarks>
    /// <seealso cref="AuthTokenOperation"/>
    public class AuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }

            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                description = "Provide the authorization headers. Remember to add \"bearer \" in front of your token.",
                required = false,
                type = "string",
            });
        }
    }
}