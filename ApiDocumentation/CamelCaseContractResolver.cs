using Newtonsoft.Json.Serialization;

namespace SwaggerAPIDocumentation
{
	internal class CamelCaseContractResolver : DefaultContractResolver
	{
		protected override string ResolvePropertyName( string propertyName )
		{
			return char.ToLower( propertyName[ 0 ] ) + propertyName.Substring( 1 );
		}
	}
}