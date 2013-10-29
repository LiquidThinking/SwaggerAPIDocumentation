using System;

namespace SwaggerAPIDocumentation.Interfaces
{
	internal interface IJsonSerializer
	{
		String SerializeObject( Object objectToBeEncoded );
	}
}
