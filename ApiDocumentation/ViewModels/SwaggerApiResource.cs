using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentation.ViewModels
{
	internal class SwaggerApiResource
	{
		public String ApiVersion { get; set; }
		public String SwaggerVersion { get; set; }
		public String BasePath { get; set; }
		public String ResourcePath { get; set; }
		public List<SwaggerApiEndpoint> Apis { get; set; }
		public Dictionary<String, ApiDocModel> Models { get; set; }
	}
}
