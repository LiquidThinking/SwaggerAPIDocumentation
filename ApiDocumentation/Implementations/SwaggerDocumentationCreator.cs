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
				apiVersion = ApiVersion,
				swaggerVersion = SwaggerVersion,
				apis = GetSwaggerApiSummaries( controllerTypes )
			};
		}

		public SwaggerApiResource GetApiResource( Type controllerType, String baseUrl )
		{
			return new SwaggerApiResource
			{
				apiVersion = ApiVersion,
				swaggerVersion = SwaggerVersion,
				basePath = baseUrl,
				resourcePath = GetControllerPath( controllerType ),
				apis = _swaggerDocumentationTools.GetControllerApiEndpoints( controllerType ),
				models = _swaggerDocumentationTools.GetControllerModels( controllerType )
			};
		}

		private static List<SwaggerApiSummary> GetSwaggerApiSummaries( IEnumerable<Type> controllerTypes )
		{
			return controllerTypes.Select( GetSwaggerApiSummary ).OrderBy( x => x.path ).ToList();
		}

		private static SwaggerApiSummary GetSwaggerApiSummary( Type controllerType )
		{
			return new SwaggerApiSummary
			{
				path = GetControllerPath( controllerType ),
				description = "Operations"
			};
		}

		private static string GetControllerPath( Type controllerType )
		{
			return $"/{controllerType.Name.Replace( ControllerEnding, "" )}";
		}
	}
}