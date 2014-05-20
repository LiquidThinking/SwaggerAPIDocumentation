using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentation.ViewModels
{
	internal class SwaggerApiEndpoint
	{
		public String path { get; set; }
		public List<ApiDocApiOperations> operations { get; set; }
	}
}
