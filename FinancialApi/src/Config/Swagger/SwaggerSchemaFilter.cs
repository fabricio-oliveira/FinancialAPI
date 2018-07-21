using System;
using System.Linq;
using System.Reflection;
using FinancialApi.src.Config.Swagger;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinancialApi.src.Config
{
    //public class SwaggerSchemaFilter : ISchemaFilter
    //{
    //    public void Apply(Schema schema, SchemaFilterContext context)
    //    {
    //        if (schema?.Properties == null)
    //            return;

    //        var excludedProperties = type.GetProperties()
    //                                     .Where(t =>
    //                                            t.GetCustomAttribute<SwaggerExcludeAttribute>()
    //                                            != null);

    //        foreach (var excludedProperty in excludedProperties)
    //        {
    //            if (schema.Properties.ContainsKey(excludedProperty.Name))
    //                schema.Properties.Remove(excludedProperty.Name);
    //        }
    //    }

    //}
}
