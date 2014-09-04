using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SwaggerAPIDocumentation.Interfaces;
using SwaggerAPIDocumentation.ViewModels;

namespace SwaggerAPIDocumentation.Implementations
{
	internal class SwaggerDocumentationTools : ISwaggerDocumentationTools
	{
		private readonly ITypeToStringConverter _typeToStringConverter;
		private readonly IModelsGenerator _modelsGenerator;
		private readonly ApiDocApiParametersBuilder _apiDocApiParametersBuilder = new ApiDocApiParametersBuilder();

		public SwaggerDocumentationTools() : this( new TypeToStringConverter(), new ModelsGenerator() ) {}

		public SwaggerDocumentationTools( ITypeToStringConverter typeToStringConverter, IModelsGenerator modelsGenerator )
		{
			_typeToStringConverter = typeToStringConverter;
			_modelsGenerator = modelsGenerator;
		}

		public List<SwaggerApiEndpoint> GetControllerApiEndpoints( Type controllerType )
		{
			var apiDocumentationAttributesAndReturnTypes = GetApiDocumentationAttributesAndReturnTypes( controllerType );

			return apiDocumentationAttributesAndReturnTypes.Select( x => new SwaggerApiEndpoint
			{
				path = GetPath( x.Key.Url ),
				operations = GetApiOperations( x )
			} ).OrderBy( x => x.path.Count() ).ToList();
		}

		private static string GetPath( String url )
		{
			return ( url.IndexOf( '?' ) == -1 ) ? url : url.Substring( 0, url.IndexOf( '?' ) );
		}

		private List<ApiDocApiOperations> GetApiOperations( KeyValuePair<ApiDocumentationAttribute, Type> attributeAndReturnType )
		{
			return new List<ApiDocApiOperations>
			{
				new ApiDocApiOperations
				{
					type = _typeToStringConverter.GetApiOperationType( attributeAndReturnType.Key.ReturnType ?? attributeAndReturnType.Value ),
					method = attributeAndReturnType.Key.RequestType.ToString(),
					notes = attributeAndReturnType.Key.Description,
					nickname = "",
					summary = "",
					parameters = GetParameters( attributeAndReturnType )
				}
			};
		}

		private List<ApiDocApiParameters> GetParameters( KeyValuePair<ApiDocumentationAttribute, Type> attributeAndReturnType )
		{
			var url = attributeAndReturnType.Key.Url;
			var result = _apiDocApiParametersBuilder.GetApiDocApiParameters( url );

			if ( attributeAndReturnType.Key.FormBody != null )
				result.Add( new ApiDocApiParameters
				{
					paramType = "body",
					type = _typeToStringConverter.GetApiOperationType( attributeAndReturnType.Key.FormBody )
				} );

			return result;
		}


		private Dictionary<ApiDocumentationAttribute, Type> GetApiDocumentationAttributesAndReturnTypes( Type controllerType )
		{
			var result = new Dictionary<ApiDocumentationAttribute, Type>();

			var methods = GetControllerMethods( controllerType );
			var methodAttributesAndReturnTypes = GetMethodsAttributesAndReturnTypes( methods );
			var classAttributesAndReturnTypes = GetClassAttributesAndReturnTypes( controllerType );

			result.Merge( methodAttributesAndReturnTypes );
			result.Merge( classAttributesAndReturnTypes );

			return result;
		}

		public Dictionary<string, ApiDocModel> GetControllerModels( Type controllerType )
		{
			var result = new Dictionary<String, ApiDocModel>();
			foreach ( var apiDocumentationAttributesAndReturnType in GetApiDocumentationAttributesAndReturnTypes( controllerType ) )
			{
				result.Merge( _modelsGenerator.GetModels( apiDocumentationAttributesAndReturnType.Key.ReturnType ?? apiDocumentationAttributesAndReturnType.Value ) );
			}

			return result;
		}

		private IEnumerable<MethodInfo> GetControllerMethods( Type controllerType )
		{
			return controllerType.GetMethods();
		}

		private Dictionary<ApiDocumentationAttribute, Type> GetMethodsAttributesAndReturnTypes( IEnumerable<MethodInfo> methodInfos )
		{
			return ( from methodInfo in methodInfos
			         from attribute in GetApiDocumentationAttributes( methodInfo )
			         select new KeyValuePair<ApiDocumentationAttribute, Type>( (ApiDocumentationAttribute) attribute, methodInfo.ReturnType )
				).ToDictionary( x => x.Key, x => x.Value );
		}

		private Dictionary<ApiDocumentationAttribute, Type> GetClassAttributesAndReturnTypes( Type controllerType )
		{
			return ( from attr in GetApiDocumentationAttributes( controllerType )
			         let attribute = (ApiDocumentationAttribute) attr
			         select new KeyValuePair<ApiDocumentationAttribute, Type>( attribute, attribute.ReturnType )
				).ToDictionary( x => x.Key, x => x.Value );
		}

		private IEnumerable<Attribute> GetApiDocumentationAttributes( MemberInfo controllerType )
		{
			return Attribute.GetCustomAttributes( controllerType, typeof ( ApiDocumentationAttribute ), false );
		}
	}
}