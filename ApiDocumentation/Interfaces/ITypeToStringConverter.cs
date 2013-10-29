using System;

namespace SwaggerAPIDocumentation.Interfaces
{
	internal interface ITypeToStringConverter
	{
		string GetApiOperationType( Type typeToConvert );
	}
}