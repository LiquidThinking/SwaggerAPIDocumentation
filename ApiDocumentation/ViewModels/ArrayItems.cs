using System;
using Newtonsoft.Json;

namespace SwaggerAPIDocumentation.ViewModels
{
	internal class ArrayItems
	{
		[JsonProperty("$ref")]
		public String ArrayType { get; set; }
	}
}