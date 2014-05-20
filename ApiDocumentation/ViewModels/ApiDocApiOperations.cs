using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentation.ViewModels
{
	internal class ApiDocApiOperations
	{
		public String method { get; set; }
		public String summary { get; set; }
		public String notes { get; set; }
		public String type { get; set; }
		public String nickname { get; set; }
		public List<ApiDocApiParameters> parameters { get; set; }
	}
}
