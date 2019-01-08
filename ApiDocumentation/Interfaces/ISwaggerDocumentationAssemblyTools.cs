using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentation.Interfaces
{
	internal interface ISwaggerDocumentationAssemblyTools
	{
		List<Type> GetTypesThatAreDecoratedWithApiDocumentationAttribute( IEnumerable<Type> controllerTypes );
		List<Type> GetApiControllerTypes( params Type[] baseControllerTypes );
	}
}