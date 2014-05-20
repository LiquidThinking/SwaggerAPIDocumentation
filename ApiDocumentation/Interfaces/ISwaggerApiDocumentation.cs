using System;

namespace SwaggerAPIDocumentation.Interfaces
{
	public interface ISwaggerApiDocumentation
	{
		string GetSwaggerApiList();
		string GetControllerDocumentation( Type controllerType, String baseUrl );
	}
}