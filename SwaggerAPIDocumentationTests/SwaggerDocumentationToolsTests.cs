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

			Assert.IsTrue( result.Any( x => x.path == "TestMethod1" ) );
			Assert.IsTrue( result.Any( x => x.path == "TestMethod2" ) );
			Assert.IsTrue( result.Any( x => x.path == "Class1" ) );
			Assert.IsTrue( result.Any( x => x.path == "Class2" ) );
		}

		[Test]
		public void GetControllerAEndPoints_WhenControllerTypeIsTestClass1_ReturnsCorrectMethods()
		{
			var result = this.ObjectUnderTest.GetControllerApiEndpoints( typeof ( TestClass1 ) );

			Assert.IsTrue( result.First( x => x.path == "Class1" ).operations[ 0 ].method == "Get" );
			Assert.IsTrue( result.First( x => x.path == "Class2" ).operations[ 0 ].method == "Post" );
			Assert.IsTrue( result.First( x => x.path == "TestMethod1" ).operations[ 0 ].method == "Put" );
			Assert.IsTrue( result.First( x => x.path == "TestMethod2" ).operations[ 0 ].method == "Get" );
		}

		[Test]
		public void GetControllerAEndPoints_WhenControllerTypeIsTestClass1_ReturnsCorrectTypes()
		{
			var swaggerDocumentationTools = new SwaggerDocumentationTools();
			var result = swaggerDocumentationTools.GetControllerApiEndpoints( typeof ( TestClass1 ) );

			Assert.IsTrue( result.First( x => x.path == "Class1" ).operations[ 0 ].type == "Boolean" );
			Assert.IsTrue( result.First( x => x.path == "Class2" ).operations[ 0 ].type == "String" );
			Assert.IsTrue( result.First( x => x.path == "TestMethod1" ).operations[ 0 ].type == "Int32" );
			Assert.IsTrue( result.First( x => x.path == "TestMethod2" ).operations[ 0 ].type == "Object" );
		}

		[Test]
		public void GetControllerAEndPoints_WhenControllerTypeIsTestClass1_ReturnsCorrectNotes()
		{
			var swaggerDocumentationTools = new SwaggerDocumentationTools();
			var result = swaggerDocumentationTools.GetControllerApiEndpoints( typeof ( TestClass1 ) );

			Assert.IsTrue( result.First( x => x.path == "Class1" ).operations[ 0 ].notes == "Class1 Description" );
			Assert.IsTrue( result.First( x => x.path == "Class2" ).operations[ 0 ].notes == String.Empty );
			Assert.IsTrue( result.First( x => x.path == "TestMethod1" ).operations[ 0 ].notes == String.Empty );
			Assert.IsTrue( result.First( x => x.path == "TestMethod2" ).operations[ 0 ].notes == "TestMethod2 Description" );
		}

		[Test]
		public void GetControllerAEndPoints_WhenControllerTypeIsTestClass3_ReturnsCorrectParameters()
		{
			var swaggerDocumentationTools = new SwaggerDocumentationTools();
			var result = swaggerDocumentationTools.GetControllerApiEndpoints( typeof ( TestClass3 ) );

			Assert.IsTrue( result.First( x => x.path == "/Fixtures/{fixtureId}" ).operations[ 0 ].parameters.Any( x => x.name == "fixtureId" ) );
			Assert.IsTrue( result.First( x => x.path == "/Fixtures/{fixtureId}" ).operations[ 0 ].parameters.Any( x => x.name == "name" && x.type == "string" && x.paramType == "query" ) );
			Assert.IsTrue( result.First( x => x.path == "/Fixtures/{fixtureId}" ).operations[ 0 ].parameters.Any( x => x.type == "Class1" && x.paramType == "body" ) );
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