using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentation
{
	public interface IBaseApiControllerTypeProvider
	{
		List<Type> GetApiBaseControllerTypes();
	}
}