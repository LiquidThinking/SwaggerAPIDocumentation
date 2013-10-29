using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentation.ViewModels
{
	internal class ApiDocApiOperations
	{
		public String Method { get; set; }
		public String Summary { get; set; }
		public String Notes { get; set; }
		public String Type { get; set; }
		public String Nickname { get; set; }
		public List<ApiDocApiParameters> Parameters { get; set; }
	}
}
