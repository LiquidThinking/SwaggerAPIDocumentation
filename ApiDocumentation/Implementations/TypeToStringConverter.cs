using System;
using System.Collections.Generic;
using SwaggerAPIDocumentation.Interfaces;

namespace SwaggerAPIDocumentation.Implementations
{
	internal class TypeToStringConverter : ITypeToStringConverter
	{
		public string GetApiOperationType( Type typeToConvert )
		{
			if ( IsList( typeToConvert ) )
				return ListTypeToText( typeToConvert );
			if ( IsArray( typeToConvert ) )
				return ArrayTypeToText( typeToConvert );
			if ( IsNullableType( typeToConvert ) )
				return GetNullableTypeName( typeToConvert );
			return RemoveGenericInfo( typeToConvert );
		}

		private static string GetNullableTypeName( Type typeToConvert )
		{
			return Nullable.GetUnderlyingType( typeToConvert ).Name;
		}

		private static bool IsNullableType( Type typeToConvert )
		{
			return Nullable.GetUnderlyingType( typeToConvert ) != null;
		}

		private string ArrayTypeToText( Type returnType )
		{
			return String.Format( "array[{0}]", returnType.GetElementType().Name );
		}

		private bool IsArray( Type returnType )
		{
			return returnType.IsArray;
		}

		private bool IsList( Type returnType )
		{
			return returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof ( List<> );
		}

		private string ListTypeToText( Type returnType )
		{
			return String.Format( "array[{0}]", returnType.GetGenericArguments()[ 0 ].Name );
		}

		public string RemoveGenericInfo( Type type )
		{
			var name = type.Name;
			var index = name.IndexOf( '`' );
			return index == -1 ? name : name.Substring( 0, index );
		}
	}
}