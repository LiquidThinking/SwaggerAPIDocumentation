using System.Collections.Generic;
using System.Reflection;

namespace SwaggerAPIDocumentation.Interfaces
{
	public interface IControllerAssemblyProvider
	{
		List<Assembly> GetControllerAssemblies();
	}
}