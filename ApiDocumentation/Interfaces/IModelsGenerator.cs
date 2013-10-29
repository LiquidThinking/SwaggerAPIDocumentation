using System;
using System.Collections.Generic;
using SwaggerAPIDocumentation.ViewModels;

namespace SwaggerAPIDocumentation.Interfaces
{
	internal interface IModelsGenerator {
		Dictionary<String, ApiDocModel> GetModels( Type type );
	}
}