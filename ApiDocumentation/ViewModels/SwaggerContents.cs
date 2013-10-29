using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentation.ViewModels
{
	internal class SwaggerContents
	{
		public String ApiVersion { get; set; }
		public String SwaggerVersion { get; set; }
		public List<SwaggerApiSummary> Apis { get; set; }
	}
}