using System;
using System.Collections.Generic;
using System.Linq;
using SwaggerAPIDocumentation.Interfaces;
using SwaggerAPIDocumentation.ViewModels;

namespace SwaggerAPIDocumentation.Implementations
{
	internal class SwaggerDocumentationCreator : ISwaggerDocumentationCreator
	{
		private const string ApiVersion = "1.0";
		private const string SwaggerVersion = "1.2";
		private const string ControllerEnding = "Controller";

		private readonly ISwaggerDocumentationTools _swaggerDocumentationTools;

		public SwaggerDocumentationCreator() : this( new SwaggerDocumentationTools() ) {}

		public SwaggerDocumentationCreator( ISwaggerDocumentationTools swaggerDocumentationTools )
		{
			_swaggerDocumentationTools = swaggerDocumentationTools;
		}

		public SwaggerContents GetSwaggerResourceList( IEnumerable<Type> controllerTypes )
		{
			return new SwaggerContents
			{
				ApiVersion = ApiVersion,
				SwaggerVersion = SwaggerVersion,
				Apis = GetSwaggerApiSummaries( controllerTypes )
			};
		}

		public SwaggerApiResource GetApiResource( Type controllerType, String baseUrl )
		{
			return new SwaggerApiResource
			{
				ApiVersion = ApiVersion,
				SwaggerVersion = SwaggerVersion,
				BasePath = baseUrl,
				ResourcePath = GetControllerPath( controllerType ),
				Apis = _swaggerDocumentationTools.GetControllerApiEndpoints( controllerType ),
				Models = _swaggerDocumentationTools.GetControllerModels( controllerType )
			};
		}

		private List<SwaggerApiSummary> GetSwaggerApiSummaries( IEnumerable<Type> controllerTypes )
		{
			return controllerTypes.Select( GetSwaggerApiSummary ).OrderBy( x => x.Path ).ToList();
		}

		private SwaggerApiSummary GetSwaggerApiSummary( Type controllerType )
		{
			return new SwaggerApiSummary
			{
				Path = GetControllerPath( controllerType ),
				Description = "Operations"
			};
		}

		private string GetControllerPath( Type controllerType )
		{
			return String.Format( "/{0}", controllerType.Name.Replace( ControllerEnding, "" ) );
		}
	}
}