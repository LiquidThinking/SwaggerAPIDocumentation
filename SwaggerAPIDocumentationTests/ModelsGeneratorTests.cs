using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using SwaggerAPIDocumentation;
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
		[TestCase( typeof( Class1 ), "Class1" )]
		[TestCase( typeof( Class2 ), "Class2" )]
		[TestCase( typeof( List<Class1> ), "Class1" )]
		[TestCase( typeof( Class2[] ), "Class2" )]
		public void GetModels_Always_SetsKeyAsNameOfClass( Type type, String expected )
		{
			var result = _modelsGenerator.GetModels( type );

			Assert.AreEqual( expected, result.Keys.First() );
		}

		[Test]
		[TestCase( typeof( Class1 ), "Class1" )]
		[TestCase( typeof( Class2 ), "Class2" )]
		[TestCase( typeof( List<Class1> ), "Class1" )]
		[TestCase( typeof( Class2[] ), "Class2" )]
		public void GetModels_Always_SetsValueIdAsNameOfClass( Type type, String expected )
		{
			var result = _modelsGenerator.GetModels( type );

			Assert.AreEqual( expected, result.Values.First().id );
		}

		[Test]
		public void GetModels_ForClass1_SetsPropertiesCorrectly()
		{
			var result = _modelsGenerator.GetModels( typeof( Class1 ) );

			Assert.AreEqual( "String", result.Values.First().properties[ "StringProperty" ].type );
			Assert.AreEqual( "Int16", result.Values.First().properties[ "Int16Property" ].type );
			Assert.AreEqual( "Int32", result.Values.First().properties[ "Int32Property" ].type );
			Assert.AreEqual( "array", result.Values.First().properties[ "ArrayTypesProperty" ].type );
			Assert.AreEqual( "Type", result.Values.First().properties[ "ArrayTypesProperty" ].items.ArrayType );
			Assert.AreEqual( "array", result.Values.First().properties[ "DateTimeListProperty" ].type );
			Assert.AreEqual( "DateTime", result.Values.First().properties[ "DateTimeListProperty" ].items.ArrayType );
		}

		[Test]
		public void GetModels_ForClass2_SetsPropertiesCorrectly()
		{
			var result = _modelsGenerator.GetModels( typeof( Class2 ) );

			Assert.AreEqual( "String", result.Values.First().properties[ "String" ].type );
			Assert.AreEqual( "Int16", result.Values.First().properties[ "Int16" ].type );
			Assert.AreEqual( "Int32", result.Values.First().properties[ "Int32" ].type );
			Assert.AreEqual( "array", result.Values.First().properties[ "ArrayTypes" ].type );
			Assert.AreEqual( "Type", result.Values.First().properties[ "ArrayTypes" ].items.ArrayType );
			Assert.AreEqual( "array", result.Values.First().properties[ "DateTimes" ].type );
			Assert.AreEqual( "DateTime", result.Values.First().properties[ "DateTimes" ].items.ArrayType );
			Assert.AreEqual( "Class1", result.Values.First().properties[ "Class1" ].type );

			Assert.IsTrue( result.Last().Key == "Class1" );
		}

		[Test]
		public void GetModels_ForClass1List_SetsPropertiesCorrectly()
		{
			var result = _modelsGenerator.GetModels( typeof( List<Class1> ) );

			Assert.AreEqual( "String", result.Values.First().properties[ "StringProperty" ].type );
			Assert.AreEqual( "Int16", result.Values.First().properties[ "Int16Property" ].type );
			Assert.AreEqual( "Int32", result.Values.First().properties[ "Int32Property" ].type );
			Assert.AreEqual( "array", result.Values.First().properties[ "ArrayTypesProperty" ].type );
			Assert.AreEqual( "Type", result.Values.First().properties[ "ArrayTypesProperty" ].items.ArrayType );
			Assert.AreEqual( "array", result.Values.First().properties[ "DateTimeListProperty" ].type );
			Assert.AreEqual( "DateTime", result.Values.First().properties[ "DateTimeListProperty" ].items.ArrayType );

			Assert.AreEqual( "Class1", result.Keys.First() );
		}

		[Test]
		public void GetModels_ForClass1Array_SetsPropertiesCorrectly()
		{
			var result = _modelsGenerator.GetModels( typeof( Class1[] ) );

			Assert.AreEqual( "String", result.Values.First().properties[ "StringProperty" ].type );
			Assert.AreEqual( "Int16", result.Values.First().properties[ "Int16Property" ].type );
			Assert.AreEqual( "Int32", result.Values.First().properties[ "Int32Property" ].type );
			Assert.AreEqual( "array", result.Values.First().properties[ "ArrayTypesProperty" ].type );
			Assert.AreEqual( "Type", result.Values.First().properties[ "ArrayTypesProperty" ].items.ArrayType );
			Assert.AreEqual( "array", result.Values.First().properties[ "DateTimeListProperty" ].type );
			Assert.AreEqual( "DateTime", result.Values.First().properties[ "DateTimeListProperty" ].items.ArrayType );

			Assert.AreEqual( "Class1", result.Keys.First() );
		}

		[Test]
		public void GetModels_ForClass3_CreatesKeyValuePairModelForDictionary()
		{
			var result = _modelsGenerator.GetModels( typeof( Class3 ) );

			Assert.AreEqual( "String", result[ "KeyValuePair" ].properties[ "Key" ].type );
			Assert.AreEqual( "Class1", result[ "KeyValuePair" ].properties[ "Value" ].type );

			Assert.IsTrue( result.ContainsKey( "KeyValuePair" ) );
			Assert.IsTrue( result.ContainsKey( "Class1" ) );
		}

		[Test]
		public void GetModels_ForClassWithNoOptionalProperties_HasAllPropertiesInRequiredList()
		{
			var result = _modelsGenerator.GetModels( typeof( AllRequiredClass ) );
			Assert.IsTrue( result.Values.First().required.Contains( "RequiredProp1" ) );
			Assert.IsTrue( result.Values.First().required.Contains( "RequiredProp2" ) );
		}

		[Test]
		public void GetModels_ForClassWithOneOptionalProperty_ExlcudesOptionalPropertiesFromRequiredList()
		{
			var result = _modelsGenerator.GetModels( typeof( OneOptionalClass ) );
			Assert.IsTrue( result.Values.First().required.Contains( "RequiredProp" ) );
			Assert.IsFalse( result.Values.First().required.Contains( "OptionalProp" ) );
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

	public class AllRequiredClass
	{
		public String RequiredProp1 { get; set; }
		public Int32 RequiredProp2 { get; set; }
	}

	public class OneOptionalClass
	{
		public String RequiredProp { get; set; }

		[Optional]
		public String OptionalProp { get; set; }
	}

}