using System;

namespace SwaggerAPIDocumentation.Interfaces
{
	internal interface ISwaggerApiDocumentation
	{
		string GetSwaggerAPIList();
		string GetControllerDocumentation( Type controllerType, String baseUrl );
	}
}