using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentation.ViewModels
{
	internal class SwaggerContents
	{
		public String apiVersion { get; set; }
		public String swaggerVersion { get; set; }
		public List<SwaggerApiSummary> apis { get; set; }
	}
}