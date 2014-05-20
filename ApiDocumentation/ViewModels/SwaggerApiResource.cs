using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentation.ViewModels
{
	internal class SwaggerApiResource
	{
		public String apiVersion { get; set; }
		public String swaggerVersion { get; set; }
		public String basePath { get; set; }
		public String resourcePath { get; set; }
		public List<SwaggerApiEndpoint> apis { get; set; }
		public Dictionary<String, ApiDocModel> models { get; set; }
	}
}
