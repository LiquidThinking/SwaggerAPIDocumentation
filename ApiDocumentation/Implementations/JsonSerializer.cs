using System;
using Newtonsoft.Json;
using SwaggerAPIDocumentation.Interfaces;

namespace SwaggerAPIDocumentation.Implementations
{
	internal class JsonSerializer : IJsonSerializer
	{
		public String SerializeObject( Object objectToBeEncoded )
		{
			return JsonConvert.SerializeObject( objectToBeEncoded, Formatting.Indented, new JsonSerializerSettings
			{
				ContractResolver = new CamelCaseContractResolver(),
				
			} );
		}
	}
}