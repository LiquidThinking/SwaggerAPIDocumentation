using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using SwaggerAPIDocumentation.Implementations;
using SwaggerAPIDocumentation.Interfaces;
using SwaggerAPIDocumentation.ViewModels;

namespace SwaggerAPIDocumentationTests
{
	[TestFixture]
	public class SwaggerDocumentationCreatorTests
	{
		private ISwaggerDocumentationTools _swaggerDocumentationTools;
		private SwaggerDocumentationCreator _swaggerDocumentationCreator;

		[SetUp]
		public void SetUp()
		{
			_swaggerDocumentationTools = MockRepository.GenerateMock<ISwaggerDocumentationTools>();
			_swaggerDocumentationCreator = new SwaggerDocumentationCreator( _swaggerDocumentationTools );
		}

		[Test]
		public void Controller_Always_CanGetInstance()
		{
			var swaggerDocumentationCreator = new SwaggerDocumentationCreator();

			Assert.IsNotNull( swaggerDocumentationCreator );
		}

		[Test]
		[TestCase( typeof ( FixturesController ), "/Fixtures" )]
		[TestCase( typeof ( TeamsController ), "/Teams" )]
		public void SwaggerApiSummary_ForAllControllers_CorrectHasForwardSlashAddedAndControllerEndRemoved( Type controllerType, String expected )
		{
			var result = _swaggerDocumentationCreator.GetSwaggerResourceList( new List<Type>
			{
				controllerType
			} );
			Assert.AreEqual( expected, result.apis[ 0 ].path );
		}

		[Test]
		public void SwaggerApiSummary_ForAllController_ReturnsSwaggerApiSummaryForEachController()
		{
			var result = _swaggerDocumentationCreator.GetSwaggerResourceList( new List<Type>
			{
				typeof ( FixturesController ),
				typeof ( TeamsController )
			} );
			Assert.AreEqual( "/Fixtures", result.apis[ 0 ].path );
			Assert.AreEqual( "/Teams", result.apis[ 1 ].path );
		}

		[Test]
		public void GetApiDocApiGroup_Always_ReturnsCorrectBasePath()
		{
			var result = _swaggerDocumentationCreator.GetApiResource(typeof(FixturesController), "http://localhost/api/v1");
			Assert.AreEqual( "http://localhost/api/v1", result.basePath );
		}

		[Test]
		[TestCase( typeof ( FixturesController ), "/Fixtures" )]
		[TestCase( typeof ( TeamsController ), "/Teams" )]
		public void GetApiDocApiGroup_Always_CorrectHasForwardSlashAddedAndControllerEndRemoved( Type controllerType, String expected )
		{
			var result = _swaggerDocumentationCreator.GetApiResource(controllerType, null);
			Assert.AreEqual( expected, result.resourcePath );
		}

		[Test]
		[TestCase( typeof ( FixturesController ) )]
		[TestCase( typeof ( TeamsController ) )]
		public void GetApiDocApiGroup_WhenTypeIsSupplied_CallsGetControllerApisWithThatTypeParameter( Type type )
		{
			_swaggerDocumentationCreator.GetApiResource(type, null);
			_swaggerDocumentationTools.AssertWasCalled( x => x.GetControllerApiEndpoints( type ) );
		}

		[Test]
		public void GetApiDocApiGroup_Always_ReturnsGetControllerApisResult()
		{
			var expected = new List<SwaggerApiEndpoint>();
			_swaggerDocumentationTools.Stub( x => x.GetControllerApiEndpoints( Arg<Type>.Is.Anything ) ).Return( expected );

			var result = _swaggerDocumentationCreator.GetApiResource(typeof(FixturesController), null);
			Assert.AreEqual( expected, result.apis );
		}

		[Test]
		[TestCase( typeof ( FixturesController ) )]
		[TestCase( typeof ( TeamsController ) )]
		public void GetApiDocApiGroup_WhenTypeIsSupplied_CallsGetControllerModelsWithThatTypeParameter( Type type )
		{
			_swaggerDocumentationCreator.GetApiResource(type, null);
			_swaggerDocumentationTools.AssertWasCalled( x => x.GetControllerModels( type ) );
		}

		[Test]
		public void GetApiDocApiGroup_AlwaysCallsGetControllerModel_WithControllerType()
		{
			var result = _swaggerDocumentationCreator.GetApiResource(typeof(FixturesController), null);

			_swaggerDocumentationTools.AssertWasCalled( x => x.GetControllerModels( typeof ( FixturesController ) ) );
		}

		[Test]
		public void GetApiDocApiGroup_Always_ReturnsGetControllerModelsResult()
		{
			var expected = new Dictionary<String, ApiDocModel>();
			_swaggerDocumentationTools.Stub( x => x.GetControllerModels( Arg<Type>.Is.Anything ) ).Return( expected );

			var result = _swaggerDocumentationCreator.GetApiResource(typeof(FixturesController), null);
			Assert.AreEqual( expected, result.models );
		}
	}

	public class FixturesController {}

	public class TeamsController {}
}