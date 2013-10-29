using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentation.ViewModels
{
	internal class SwaggerApiEndpoint
	{
		public String Path { get; set; }
		public List<ApiDocApiOperations> Operations { get; set; }
	}
}
