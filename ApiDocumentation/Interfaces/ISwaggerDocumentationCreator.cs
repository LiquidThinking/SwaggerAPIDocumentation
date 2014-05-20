using System;
using System.Collections.Generic;
using SwaggerAPIDocumentation.ViewModels;

namespace SwaggerAPIDocumentation.Interfaces
{
	internal interface ISwaggerDocumentationCreator
	{
		SwaggerContents GetSwaggerResourceList( IEnumerable<Type> controllerTypes );
		SwaggerApiResource GetApiResource( Type controllerType, String baseUrl );
	}
}