using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using SwaggerAPIDocumentation.Implementations;

namespace SwaggerAPIDocumentationTests
{
	[TestFixture]
	public class ModelsGeneratorTests
	{
		private ModelsGenerator _modelsGenerator;

		[SetUp]
		public void SetUp()
		{
			_modelsGenerator = MockRepository.GeneratePartialMock<ModelsGenerator>();
		}

		[Test]
		[TestCase( typeof ( Class1 ), "Class1" )]
		[TestCase( typeof ( Class2 ), "Class2" )]
		[TestCase( typeof ( List<Class1> ), "Class1" )]
		[TestCase( typeof ( Class2[] ), "Class2" )]
		public void GetModels_Always_SetsKeyAsNameOfClass( Type type, String expected )
		{
			var result = _modelsGenerator.GetModels( type );

			Assert.AreEqual( expected, result.Keys.First() );
		}

		[Test]
		[TestCase( typeof ( Class1 ), "Class1" )]
		[TestCase( typeof ( Class2 ), "Class2" )]
		[TestCase( typeof ( List<Class1> ), "Class1" )]
		[TestCase( typeof ( Class2[] ), "Class2" )]
		public void GetModels_Always_SetsValueIdAsNameOfClass( Type type, String expected )
		{
			var result = _modelsGenerator.GetModels( type );

			Assert.AreEqual( expected, result.Values.First().Id );
		}

		[Test]
		public void GetModels_ForClass1_SetsPropertiesCorrectly()
		{
			var result = _modelsGenerator.GetModels( typeof ( Class1 ) );

			Assert.AreEqual( "String", result.Values.First().Properties[ "StringProperty" ].Type );
			Assert.AreEqual( "Int16", result.Values.First().Properties[ "Int16Property" ].Type );
			Assert.AreEqual( "Int32", result.Values.First().Properties[ "Int32Property" ].Type );
			Assert.AreEqual( "array", result.Values.First().Properties[ "ArrayTypesProperty" ].Type );
			Assert.AreEqual( "Type", result.Values.First().Properties[ "ArrayTypesProperty" ].Items.ArrayType );
			Assert.AreEqual( "array", result.Values.First().Properties[ "DateTimeListProperty" ].Type );
			Assert.AreEqual( "DateTime", result.Values.First().Properties[ "DateTimeListProperty" ].Items.ArrayType );
		}

		[Test]
		public void GetModels_ForClass2_SetsPropertiesCorrectly()
		{
			var result = _modelsGenerator.GetModels( typeof ( Class2 ) );

			Assert.AreEqual( "String", result.Values.First().Properties[ "String" ].Type );
			Assert.AreEqual( "Int16", result.Values.First().Properties[ "Int16" ].Type );
			Assert.AreEqual( "Int32", result.Values.First().Properties[ "Int32" ].Type );
			Assert.AreEqual( "array", result.Values.First().Properties[ "ArrayTypes" ].Type );
			Assert.AreEqual( "Type", result.Values.First().Properties[ "ArrayTypes" ].Items.ArrayType );
			Assert.AreEqual( "array", result.Values.First().Properties[ "DateTimes" ].Type );
			Assert.AreEqual( "DateTime", result.Values.First().Properties[ "DateTimes" ].Items.ArrayType );
			Assert.AreEqual( "Class1", result.Values.First().Properties[ "Class1" ].Type );

			Assert.IsTrue( result.Last().Key == "Class1" );
		}

		[Test]
		public void GetModels_ForClass1List_SetsPropertiesCorrectly()
		{
			var result = _modelsGenerator.GetModels( typeof ( List<Class1> ) );

			Assert.AreEqual( "String", result.Values.First().Properties[ "StringProperty" ].Type );
			Assert.AreEqual( "Int16", result.Values.First().Properties[ "Int16Property" ].Type );
			Assert.AreEqual( "Int32", result.Values.First().Properties[ "Int32Property" ].Type );
			Assert.AreEqual( "array", result.Values.First().Properties[ "ArrayTypesProperty" ].Type );
			Assert.AreEqual( "Type", result.Values.First().Properties[ "ArrayTypesProperty" ].Items.ArrayType );
			Assert.AreEqual( "array", result.Values.First().Properties[ "DateTimeListProperty" ].Type );
			Assert.AreEqual( "DateTime", result.Values.First().Properties[ "DateTimeListProperty" ].Items.ArrayType );

			Assert.AreEqual( "Class1", result.Keys.First() );
		}

		[Test]
		public void GetModels_ForClass1Array_SetsPropertiesCorrectly()
		{
			var result = _modelsGenerator.GetModels( typeof ( Class1[] ) );

			Assert.AreEqual( "String", result.Values.First().Properties[ "StringProperty" ].Type );
			Assert.AreEqual( "Int16", result.Values.First().Properties[ "Int16Property" ].Type );
			Assert.AreEqual( "Int32", result.Values.First().Properties[ "Int32Property" ].Type );
			Assert.AreEqual( "array", result.Values.First().Properties[ "ArrayTypesProperty" ].Type );
			Assert.AreEqual( "Type", result.Values.First().Properties[ "ArrayTypesProperty" ].Items.ArrayType );
			Assert.AreEqual( "array", result.Values.First().Properties[ "DateTimeListProperty" ].Type );
			Assert.AreEqual( "DateTime", result.Values.First().Properties[ "DateTimeListProperty" ].Items.ArrayType );

			Assert.AreEqual( "Class1", result.Keys.First() );
		}

		[Test]
		public void GetModels_ForClass3_CreatesKeyValuePairModelForDictionary()
		{
			var result = _modelsGenerator.GetModels( typeof ( Class3 ) );

			Assert.AreEqual( "String", result[ "KeyValuePair" ].Properties[ "Key" ].Type );
			Assert.AreEqual( "Class1", result[ "KeyValuePair" ].Properties[ "Value" ].Type );

			Assert.IsTrue( result.ContainsKey( "KeyValuePair" ) );
			Assert.IsTrue( result.ContainsKey( "Class1" ) );
		}
	}

	public class Class1
	{
		public String StringProperty { get; set; }
		public Int16 Int16Property { get; set; }
		public Int32 Int32Property { get; set; }
		public Type[] ArrayTypesProperty { get; set; }
		public List<DateTime> DateTimeListProperty { get; set; }
	}

	public class Class2
	{
		public String String { get; set; }
		public Int16 Int16 { get; set; }
		public Int32 Int32 { get; set; }
		public Type[] ArrayTypes { get; set; }
		public List<DateTime> DateTimes { get; set; }
		public Class1 Class1 { get; set; }
	}

	public class Class3
	{
		public Dictionary<String, Class1> Dictionary { get; set; }
	}
}