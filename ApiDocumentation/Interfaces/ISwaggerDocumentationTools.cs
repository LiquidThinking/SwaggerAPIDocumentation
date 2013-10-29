using System;
using System.Collections.Generic;
using SwaggerAPIDocumentation.ViewModels;

namespace SwaggerAPIDocumentation.Interfaces
{
	internal interface ISwaggerDocumentationTools
	{
		List<SwaggerApiEndpoint> GetControllerApiEndpoints(Type controllerType);
		Dictionary<String, ApiDocModel> GetControllerModels(Type controllerType);
	}
}
