using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SwaggerAPIDocumentation;
using SwaggerAPIDocumentation.Implementations;
using SwaggerAPIDocumentation.ViewModels;

namespace SwaggerAPIDocumentationTests
{
	[TestFixture]
	public class ApiDocApiParametersBuilderTests
	{
		private ApiDocApiParametersBuilder _builder;


		private object[] TestCases = new[]
		{
			new object[]
			{
				"/Fixtures/{fixtureId}", new ApiDocApiParametersExpected( "fixtureId", "", true, "integer", "path" )
			},
			new object[]
			{
				"/Fixtures/{fixtureId:string}", new ApiDocApiParametersExpected( "fixtureId", "", true, "string", "path" )
			},
			new object[]
			{
				"/Fixtures/?fixtureId={fixtureId}", new ApiDocApiParametersExpected( "fixtureId", "", true, "integer", "query" )
			},
			new object[]
			{
				"/Fixtures/123?date={date:DateTime}", new ApiDocApiParametersExpected( "date", "", true, "DateTime", "query" )
			},
			new object[]
			{
				"/Fixtures/{name=fixtureId;optional=true}", new ApiDocApiParametersExpected( "fixtureId", "", false, "integer", "path" )
			},
			new object[]
			{
				"/Fixtures/{name=fixtureName;optional=true;type=string;description=The name of the fixture}", new ApiDocApiParametersExpected( "fixtureName", "The name of the fixture", false, "string", "path" )
			}
			//Check type, description and multiple ways and querystring parameters
		};


		[SetUp]
		public void SetUp()
		{
			_builder = new ApiDocApiParametersBuilder();
		}

		[TestCaseSource( "TestCases" )]
		public void BuildParameter_WithUrlWithOneParameter_ReturnsTheCorrectParameter( string url, ApiDocApiParametersExpected expected )
		{
			var result = _builder.GetApiDocApiParameters( url );
			var parameter = result[ 0 ];
			Assert.AreEqual( expected.name, parameter.name );
			Assert.AreEqual( expected.required, parameter.required );
			Assert.AreEqual( expected.type, parameter.type );
			Assert.AreEqual( expected.description, parameter.description );
			Assert.AreEqual( expected.paramType, parameter.paramType );
		}


		[Test]
		public void BuildParameter_WithBadParameters_FailsGracefully()
		{
			var result = _builder.GetApiDocApiParameters( "/Search?filter={name=filter;optional=true;type=String;The part of the user or team name to search for}" );
			var parameter = result[ 0 ];
			Assert.AreEqual( "filter", parameter.name );
			Assert.IsFalse( parameter.required );
			Assert.AreEqual( String.Empty, parameter.description );
		}
	}

	public class ApiDocApiParametersExpected
	{
		public ApiDocApiParametersExpected( string name, string description, bool required, string type, string paramType )
		{
			this.name = name;
			this.description = description;
			this.required = required;
			this.type = type;
			this.paramType = paramType;
		}

		public String name { get; set; }
		public String description { get; set; }
		public Boolean required { get; set; }
		public String type { get; set; }
		public String paramType { get; set; }
	}
}