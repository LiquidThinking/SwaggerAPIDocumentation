using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentation.ViewModels
{
	internal class ApiDocModel
	{
		public String id { get; set; }
		public Dictionary<String,ApiDocModelProperty> properties { get; set; }
	}
}
