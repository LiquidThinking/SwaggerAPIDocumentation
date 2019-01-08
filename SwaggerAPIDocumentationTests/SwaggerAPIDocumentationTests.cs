using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using SwaggerAPIDocumentation;
using SwaggerAPIDocumentation.Interfaces;
using SwaggerAPIDocumentation.ViewModels;

namespace SwaggerAPIDocumentationTests
{
	[TestFixture]
	public class SwaggerApiDocumentationTests
	{
		private ISwaggerDocumentationAssemblyTools _swaggerDocumentationAssemblyTools;
		private ISwaggerDocumentationCreator _swaggerDocumentationCreator;
		private IJsonSerializer _jsonSerializer;
		private SwaggerApiDocumentation _swaggerApiDocumentation;

		[SetUp]
		public void SetUp()
		{
			_swaggerDocumentationAssemblyTools = MockRepository.GenerateMock<ISwaggerDocumentationAssemblyTools>();
			_swaggerDocumentationCreator = MockRepository.GenerateMock<ISwaggerDocumentationCreator>();
			_jsonSerializer = MockRepository.GenerateMock<IJsonSerializer>();

			_swaggerApiDocumentation = new SwaggerApiDocumentation( _swaggerDocumentationAssemblyTools, _swaggerDocumentationCreator, _jsonSerializer, new TestControllerTypeProvider() );
		}

		[Test]
		public void Controller_Always_CanGetInstance()
		{
			var apiDocumentation = new SwaggerApiDocumentation( new TestControllerTypeProvider() );

			Assert.IsNotNull( apiDocumentation );
		}

		[Test]
		public void GetControllerDocumentation_WhenControllerTypeIsTestController_CallsGetApiResourceWithTestController()
		{
			_swaggerApiDocumentation.GetControllerDocumentation( typeof( TestController ), null );

			_swaggerDocumentationCreator.AssertWasCalled( x => x.GetApiResource( typeof( TestController ), null ) );
		}

		[Test]
		public void GetControllerDocumentation_WhenControllerTypeIsTestController_CallsSerializeObjectWithGetApiResourceResult()
		{
			var expected = new SwaggerApiResource();
			_swaggerDocumentationCreator.Stub( x => x.GetApiResource( typeof( TestController ), null ) ).Return( expected );

			_swaggerApiDocumentation.GetControllerDocumentation( typeof( TestController ), null );

			_jsonSerializer.AssertWasCalled( x => x.SerializeObject( expected ) );
		}

		[Test]
		public void GetControllerDocumentation_Always_ReturnsSerializeObjectResult()
		{
			const string expected = "Expected Output";
			_jsonSerializer.Stub( x => x.SerializeObject( Arg<Object>.Is.Anything ) ).Return( expected );

			var result = _swaggerApiDocumentation.GetControllerDocumentation( typeof( TestController ), null );

			Assert.AreEqual( expected, result );
		}

		[Test]
		public void GetSwaggerApiList_WhenControllerTypeIsTestController_CallsGetApiControllerTypesWithTestController()
		{
			_swaggerApiDocumentation.GetSwaggerApiList();

			_swaggerDocumentationAssemblyTools.AssertWasCalled( x => x.GetApiControllerTypes( typeof( TestController ) ) );
		}

		[Test]
		public void GetSwaggerApiList_AlwaysCallsGetTypesThatAreDecoratedWithApiDocumentationAttribute_WithGetApiControllersType()
		{
			var expected = new List<Type>();
			_swaggerDocumentationAssemblyTools.Stub( x => x.GetApiControllerTypes( Arg<Type>.Is.Anything ) ).Return( expected );

			_swaggerApiDocumentation.GetSwaggerApiList();

			_swaggerDocumentationAssemblyTools.AssertWasCalled( x => x.GetTypesThatAreDecoratedWithApiDocumentationAttribute( expected ) );
		}

		[Test]
		public void GetSwaggerApiList_AlwaysCallsGetSwaggerResourceList_WithGetTypesThatAreDecoratedWithApiDocumentationAttributeResult()
		{
			var expected = new List<Type>();
			_swaggerDocumentationAssemblyTools.Stub( x => x.GetTypesThatAreDecoratedWithApiDocumentationAttribute( Arg<List<Type>>.Is.Anything ) ).Return( expected );

			_swaggerApiDocumentation.GetSwaggerApiList();

			_swaggerDocumentationCreator.AssertWasCalled( x => x.GetSwaggerResourceList( expected ) );
		}

		[Test]
		public void GetSwaggerApiList_AlwaysCallsSerializeObject_WithGetSwaggerResourceListResult()
		{
			var expected = new SwaggerContents();
			_swaggerDocumentationCreator.Stub( x => x.GetSwaggerResourceList( Arg<List<Type>>.Is.Anything ) ).Return( expected );

			_swaggerApiDocumentation.GetSwaggerApiList();

			_jsonSerializer.AssertWasCalled( x => x.SerializeObject( Arg<Object>.Is.Anything ) );
		}

		[Test]
		public void GetSwaggerApiList_Always_ReturnsSerializeObjectResult()
		{
			const string expected = "Expected Result";
			_jsonSerializer.Stub( x => x.SerializeObject( Arg<Object>.Is.Anything ) ).Return( expected );

			var result = _swaggerApiDocumentation.GetSwaggerApiList();

			Assert.AreEqual( expected, result );
		}
	}

	public class TestController : System.Web.Mvc.Controller
	{
	}

	public class TestControllerTypeProvider : IBaseApiControllerTypeProvider
	{
		public List<Type> GetApiBaseControllerTypes()
		{
			return new List<Type> { typeof( TestController ) };
		}
	}
}