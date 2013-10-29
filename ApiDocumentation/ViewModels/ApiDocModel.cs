using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentation.ViewModels
{
	internal class ApiDocModel
	{
		public String Id { get; set; }
		public Dictionary<String,ApiDocModelProperty> Properties { get; set; }
	}
}
