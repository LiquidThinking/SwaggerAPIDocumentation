using System;

namespace SwaggerAPIDocumentation.Interfaces
{
	public interface ISwaggerApiDocumentation
	{
		string GetSwaggerAPIList();
		string GetControllerDocumentation( Type controllerType, String baseUrl );
	}
}