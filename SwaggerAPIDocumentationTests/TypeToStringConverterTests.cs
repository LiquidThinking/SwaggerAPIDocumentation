using System;
using System.Collections.Generic;
using NUnit.Framework;
using SwaggerAPIDocumentation;
using SwaggerAPIDocumentation.Implementations;

namespace SwaggerAPIDocumentationTests
{
	public class TypeToStringConverterTests
	{
		private TypeToStringConverter _typeToStringConverter;

		[SetUp]
		public void SetUp()
		{
			_typeToStringConverter = new TypeToStringConverter();
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_AttributeTypeIsNullAndReturnTypeIsString()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, null );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType ?? typeof ( String ) );

			Assert.That( result == typeof ( String ).Name );
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_AttributeTypeIsNullAndReturnTypeIsBoolean()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, null );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType ?? typeof ( Boolean ) );

			Assert.That( result == typeof ( Boolean ).Name );
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_WhenTypeIsNullableDecimal()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, null );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType ?? typeof ( Decimal? ) );

			Assert.That( result == typeof ( Decimal ).Name );
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_WhenTypeIsNullableBoolean()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, null );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType ?? typeof ( Boolean? ) );

			Assert.That( result == typeof ( Boolean ).Name );
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_AttributeTypeIsNullAndReturnTypeIsListOfInt32()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, null );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType ?? typeof ( List<Int32> ) );

			Assert.That( result == String.Format( "array[{0}]", typeof ( Int32 ).Name ) );
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_AttributeTypeIsNullAndReturnTypeIsListOfObject()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, null );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType ?? typeof ( List<Object> ) );

			Assert.That( result == String.Format( "array[{0}]", typeof ( Object ).Name ) );
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_AttributeTypeIsNullAndReturnTypeIsArrayOfInt64()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, null );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType ?? typeof ( Int64[] ) );

			Assert.That( result == String.Format( "array[{0}]", typeof ( Int64 ).Name ) );
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_AttributeTypeIsNullAndReturnTypeIsArrayOfString()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, null );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType ?? typeof ( String[] ) );

			Assert.That( result == String.Format( "array[{0}]", typeof ( String ).Name ) );
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_AttributeTypeIsString()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, typeof ( String ) );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType );

			Assert.That( result == typeof ( String ).Name );
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_AttributeTypeIsBoolean()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, typeof ( Boolean ) );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType );

			Assert.That( result == typeof ( Boolean ).Name );
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_AttributeTypeIsListOfObject()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, typeof ( List<Object> ) );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType );

			Assert.That( result == String.Format( "array[{0}]", typeof ( Object ).Name ) );
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_AttributeTypeIsListOfInt16()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, typeof ( List<Int16> ) );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType );

			Assert.That( result == String.Format( "array[{0}]", typeof ( Int16 ).Name ) );
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_AttributeTypeIsArrayOfString()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, typeof ( String[] ) );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType );

			Assert.That( result == String.Format( "array[{0}]", typeof ( String ).Name ) );
		}

		[Test]
		public void GetApiOperationType_ReturnsCorrectType_AttributeTypeIsArrayOfInt64()
		{
			var apiDocumentationAttribute = new ApiDocumentationAttribute( null, null, typeof ( Int64[] ) );

			var result = _typeToStringConverter.GetApiOperationType( apiDocumentationAttribute.ReturnType );

			Assert.That( result == String.Format( "array[{0}]", typeof ( Int64 ).Name ) );
		}
	}

	[ApiDocumentation( "ClassAttribute1" )]
	[ApiDocumentation( "ClassAttribute2" )]
	public class MethodAttributeTestClass
	{
		[ApiDocumentation( "StringReturnMethod" )]
		public String StringReturnMethod()
		{
			return "";
		}

		[ApiDocumentation( "Int32ReturnMethod" )]
		public Int32 Int32ReturnMethod()
		{
			return 1;
		}
	}
}