using System;

namespace SwaggerAPIDocumentation.Interfaces
{
	public interface IJsonSerializer
	{
		String SerializeObject( Object objectToBeEncoded );
	}
}
