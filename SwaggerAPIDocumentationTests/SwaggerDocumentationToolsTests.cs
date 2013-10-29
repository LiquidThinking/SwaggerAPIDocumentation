using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NUnit.Framework;
using Rhino.Mocks;
using SwaggerAPIDocumentation;
using SwaggerAPIDocumentation.Implementations;
using SwaggerAPIDocumentation.Interfaces;
using SwaggerAPIDocumentation.ViewModels;

namespace SwaggerAPIDocumentationTests
{
	[TestFixture]
	internal class SwaggerDocumentationToolsTests : BaseAutomatedMockWireupTest<SwaggerDocumentationTools>
	{
		[Test]
		public void Controller_Always_CanGetInstance()
		{
			var swaggerDocumentationTools = new SwaggerDocumentationTools();

			Assert.IsNotNull( swaggerDocumentationTools );
		}

		[Test]
		public void GetControllerAEndPoints_WhenControllerTypeIsTestClass1_ReturnsCorrectPaths()
		{
			var result = this.ObjectUnderTest.GetControllerApiEndpoints( typeof ( TestClass1 ) );

			Assert.IsTrue( result.Any( x => x.Path == "TestMethod1" ) );
			Assert.IsTrue( result.Any( x => x.Path == "TestMethod2" ) );
			Assert.IsTrue( result.Any( x => x.Path == "Class1" ) );
			Assert.IsTrue( result.Any( x => x.Path == "Class2" ) );
		}

		[Test]
		public void GetControllerAEndPoints_WhenControllerTypeIsTestClass1_ReturnsCorrectMethods()
		{
			var result = this.ObjectUnderTest.GetControllerApiEndpoints( typeof ( TestClass1 ) );

			Assert.IsTrue( result.First( x => x.Path == "Class1" ).Operations[ 0 ].Method == "Get" );
			Assert.IsTrue( result.First( x => x.Path == "Class2" ).Operations[ 0 ].Method == "Post" );
			Assert.IsTrue( result.First( x => x.Path == "TestMethod1" ).Operations[ 0 ].Method == "Put" );
			Assert.IsTrue( result.First( x => x.Path == "TestMethod2" ).Operations[ 0 ].Method == "Get" );
		}

		[Test]
		public void GetControllerAEndPoints_WhenControllerTypeIsTestClass1_ReturnsCorrectTypes()
		{
			var swaggerDocumentationTools = new SwaggerDocumentationTools();
			var result = swaggerDocumentationTools.GetControllerApiEndpoints( typeof ( TestClass1 ) );

			Assert.IsTrue( result.First( x => x.Path == "Class1" ).Operations[ 0 ].Type == "Boolean" );
			Assert.IsTrue( result.First( x => x.Path == "Class2" ).Operations[ 0 ].Type == "String" );
			Assert.IsTrue( result.First( x => x.Path == "TestMethod1" ).Operations[ 0 ].Type == "Int32" );
			Assert.IsTrue( result.First( x => x.Path == "TestMethod2" ).Operations[ 0 ].Type == "Object" );
		}

		[Test]
		public void GetControllerAEndPoints_WhenControllerTypeIsTestClass1_ReturnsCorrectNotes()
		{
			var swaggerDocumentationTools = new SwaggerDocumentationTools();
			var result = swaggerDocumentationTools.GetControllerApiEndpoints( typeof ( TestClass1 ) );

			Assert.IsTrue( result.First( x => x.Path == "Class1" ).Operations[ 0 ].Notes == "Class1 Description" );
			Assert.IsTrue( result.First( x => x.Path == "Class2" ).Operations[ 0 ].Notes == String.Empty );
			Assert.IsTrue( result.First( x => x.Path == "TestMethod1" ).Operations[ 0 ].Notes == String.Empty );
			Assert.IsTrue( result.First( x => x.Path == "TestMethod2" ).Operations[ 0 ].Notes == "TestMethod2 Description" );
		}

		[Test]
		public void GetControllerAEndPoints_WhenControllerTypeIsTestClass3_ReturnsCorrectParameters()
		{
			var swaggerDocumentationTools = new SwaggerDocumentationTools();
			var result = swaggerDocumentationTools.GetControllerApiEndpoints( typeof ( TestClass3 ) );

			Assert.IsTrue( result.First( x => x.Path == "/Fixtures/{fixtureId}" ).Operations[ 0 ].Parameters.Any( x => x.Name == "fixtureId" ) );
			Assert.IsTrue( result.First( x => x.Path == "/Fixtures/{fixtureId}" ).Operations[ 0 ].Parameters.Any( x => x.Name == "name" && x.Type == "string" && x.ParamType == "query" ) );
			Assert.IsTrue( result.First( x => x.Path == "/Fixtures/{fixtureId}" ).Operations[ 0 ].Parameters.Any( x => x.Type == "Class1" && x.ParamType == "body" ) );
		}

		[Test]
		public void GetControllerModels_ForEachReturnTypeInTestClass2_CallsGetModelsWithThatReturnType()
		{
			( (IModelsGenerator) Mocks[ typeof ( IModelsGenerator ) ] ).Stub( x => x.GetModels( Arg<Type>.Is.Anything ) ).Return( new Dictionary<string, ApiDocModel>() );

			ObjectUnderTest.GetControllerModels( typeof ( TestClass2 ) );

			( (IModelsGenerator) Mocks[ typeof ( IModelsGenerator ) ] ).AssertWasCalled( x => x.GetModels( typeof ( String ) ) );
			( (IModelsGenerator) Mocks[ typeof ( IModelsGenerator ) ] ).AssertWasCalled( x => x.GetModels( typeof ( Int64 ) ) );
			( (IModelsGenerator) Mocks[ typeof ( IModelsGenerator ) ] ).AssertWasCalled( x => x.GetModels( typeof ( TestClass1 ) ) );
			( (IModelsGenerator) Mocks[ typeof ( IModelsGenerator ) ] ).AssertWasNotCalled( x => x.GetModels( typeof ( Int32 ) ) );
		}
	}

	[ApiDocumentation( "Class1", returnType: typeof ( Boolean ), description: "Class1 Description" )]
	[ApiDocumentation( "Class2", requestType: HttpVerbs.Post, returnType: typeof ( String ) )]
	public class TestClass1
	{
		[ApiDocumentation( "TestMethod1", requestType: HttpVerbs.Put )]
		[ApiDocumentation( "TestMethod2", returnType: typeof ( Object ), description: "TestMethod2 Description" )]
		public Int32 TestMethod()
		{
			return 0;
		}
	}

	[ApiDocumentation( "", returnType: typeof ( String ) )]
	public class TestClass2
	{
		[ApiDocumentation( "" )]
		public Int64 MyMethod1()
		{
			return 0;
		}

		[ApiDocumentation( "", returnType: typeof ( TestClass1 ) )]
		public Int32 MyMethod2()
		{
			return 0;
		}
	}

	public class TestClass3
	{
		[ApiDocumentation( "/Fixtures/{fixtureId}?name={name:string}", formBody: typeof ( Class1 ) )]
		public void MyMethod() {}
	}
}