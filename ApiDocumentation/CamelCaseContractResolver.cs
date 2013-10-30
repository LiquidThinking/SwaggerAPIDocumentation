using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SwaggerAPIDocumentation.ViewModels;

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