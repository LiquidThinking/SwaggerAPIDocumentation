using System;
using SwaggerAPIDocumentation.Implementations;
using SwaggerAPIDocumentation.Interfaces;

namespace SwaggerAPIDocumentation
{
	public class SwaggerApiDocumentation : ISwaggerApiDocumentation
	{
		private readonly ISwaggerDocumentationAssemblyTools _swaggerDocumentationAssemblyTools;
		private readonly ISwaggerDocumentationCreator _swaggerDocumentationCreator;
		private readonly IBaseApiControllerTypeProvider _baseApiControllerTypeProvider;
		private readonly IJsonSerializer _jsonSerializer;

		public SwaggerApiDocumentation(
			IBaseApiControllerTypeProvider baseApiControllerTypeProvider, 
			IControllerAssemblyProvider controllerAssemblyProvider )
			: this( new SwaggerDocumentationAssemblyTools( controllerAssemblyProvider ), new SwaggerDocumentationCreator(), new JsonSerializer(), baseApiControllerTypeProvider )
		{
		}

		public SwaggerApiDocumentation( 
			IJsonSerializer jsonSerializer, 
			IBaseApiControllerTypeProvider baseApiControllerTypeProvider, 
			IControllerAssemblyProvider controllerAssemblyProvider )
			: this( new SwaggerDocumentationAssemblyTools( controllerAssemblyProvider ), new SwaggerDocumentationCreator(), jsonSerializer, baseApiControllerTypeProvider )
		{
		}

		internal SwaggerApiDocumentation(
			ISwaggerDocumentationAssemblyTools swaggerDocumentationAssemblyTools,
			ISwaggerDocumentationCreator swaggerDocumentationCreator,
			IJsonSerializer jsonSerializer, IBaseApiControllerTypeProvider baseApiControllerTypeProvider )
		{
			_swaggerDocumentationAssemblyTools = swaggerDocumentationAssemblyTools;
			_swaggerDocumentationCreator = swaggerDocumentationCreator;
			_jsonSerializer = jsonSerializer;
			_baseApiControllerTypeProvider = baseApiControllerTypeProvider;
		}

		public String GetSwaggerApiList()
		{
			var allApiControllers = _swaggerDocumentationAssemblyTools
				.GetApiControllerTypes( _baseApiControllerTypeProvider.GetApiBaseControllerTypes().ToArray() );

			var pertinentApiControllers = _swaggerDocumentationAssemblyTools
				.GetTypesThatAreDecoratedWithApiDocumentationAttribute( allApiControllers );

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