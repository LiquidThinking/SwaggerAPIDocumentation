using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentation.Interfaces
{
	internal interface ISwaggerDocumentationAssemblyTools
	{
		List<Type> GetTypesThatAreDecoratedWithApiDocumentationAttribute( List<Type> controllerTypes );
		List<Type> GetApiControllerTypes( Type baseControllerType );
	}
}