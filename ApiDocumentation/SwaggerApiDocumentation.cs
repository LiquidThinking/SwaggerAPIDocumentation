using System;
using System.Web.Mvc;
using SwaggerAPIDocumentation.Implementations;
using SwaggerAPIDocumentation.Interfaces;

namespace SwaggerAPIDocumentation
{
	public class SwaggerApiDocumentation<TBaseApiControllerType> : ISwaggerApiDocumentation
		where TBaseApiControllerType : Controller
	{
		private readonly ISwaggerDocumentationAssemblyTools _swaggerDocumentationAssemblyTools;
		private readonly ISwaggerDocumentationCreator _swaggerDocumentationCreator;
		private readonly IJsonSerializer _jsonSerializer;

		public SwaggerApiDocumentation() : this( new SwaggerDocumentationAssemblyTools(), new SwaggerDocumentationCreator(), new JsonSerializer() ) {}

		public SwaggerApiDocumentation( IJsonSerializer jsonSerializer ) : this( new SwaggerDocumentationAssemblyTools(), new SwaggerDocumentationCreator(), jsonSerializer ) {}

		internal SwaggerApiDocumentation(
			ISwaggerDocumentationAssemblyTools swaggerDocumentationAssemblyTools,
			ISwaggerDocumentationCreator swaggerDocumentationCreator,
			IJsonSerializer jsonSerializer )
		{
			_swaggerDocumentationAssemblyTools = swaggerDocumentationAssemblyTools;
			_swaggerDocumentationCreator = swaggerDocumentationCreator;
			_jsonSerializer = jsonSerializer;
		}

		public String GetSwaggerApiList()
		{
			var allApiControllers = _swaggerDocumentationAssemblyTools.GetApiControllerTypes( typeof ( TBaseApiControllerType ) );
			var pertinentApiControllers = _swaggerDocumentationAssemblyTools.GetTypesThatAreDecoratedWithApiDocumentationAttribute( allApiControllers );
			var swaggerContents = _swaggerDocumentationCreator.GetSwaggerResourceList( pertinentApiControllers );


			return _jsonSerializer.SerializeObject( swaggerContents );
		}

		public String GetControllerDocumentation( Type controllerType, String baseUrl )
		{
			var apiResource = _swaggerDocumentationCreator.GetApiResource( controllerType, baseUrl );
			return _jsonSerializer.SerializeObject( apiResource );
		}
	}
}