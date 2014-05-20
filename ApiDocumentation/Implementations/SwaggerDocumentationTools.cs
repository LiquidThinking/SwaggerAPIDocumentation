using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using SwaggerAPIDocumentation.Interfaces;
using SwaggerAPIDocumentation.ViewModels;

namespace SwaggerAPIDocumentation.Implementations
{
	internal class SwaggerDocumentationTools : ISwaggerDocumentationTools
	{
		private readonly ITypeToStringConverter _typeToStringConverter;
		private readonly IModelsGenerator _modelsGenerator;

		private const string ParameterRegex = @"{(.*?)}";

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
				Path = GetPath( x.Key.Url ),
				Operations = GetApiOperations( x )
			} ).OrderBy( x => x.Path.Count() ).ToList();
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
					Type = _typeToStringConverter.GetApiOperationType( attributeAndReturnType.Key.ReturnType ?? attributeAndReturnType.Value ),
					Method = attributeAndReturnType.Key.RequestType.ToString(),
					Notes = attributeAndReturnType.Key.Description,
					Nickname = "",
					Summary = "",
					Parameters = GetParameters( attributeAndReturnType )
				}
			};
		}

		private List<ApiDocApiParameters> GetParameters( KeyValuePair<ApiDocumentationAttribute, Type> attributeAndReturnType )
		{
			var regex = new Regex( ParameterRegex );
			var url = attributeAndReturnType.Key.Url;

			var result = ( from parameter in GetParameterFromUrl( regex, url )
			               let name = parameter.Split( ':' )[ 0 ]
			               let type = GetType( parameter )
			               select new ApiDocApiParameters
			               {
				               Required = true,
				               ParamType = GetParamType( parameter, url ),
				               Name = name,
				               Description = "",
				               Type = type
			               } ).ToList();

			if ( attributeAndReturnType.Key.FormBody != null )
				result.Add( new ApiDocApiParameters
				{
					ParamType = "body",
					Type = _typeToStringConverter.GetApiOperationType( attributeAndReturnType.Key.FormBody )
				} );

			return result;
		}

		private static IEnumerable<string> GetParameterFromUrl( Regex regex, string url )
		{
			return ( from Match m in regex.Matches( url ) select m.Groups[ 1 ].Value );
		}

		private static string GetType( string stripped )
		{
			return ( stripped.Split( ':' ).Count() == 1 ) ? "integer" : stripped.Split( ':' )[ 1 ];
		}

		private static string GetParamType( String match, string url )
		{
			return ( url.IndexOf(match, StringComparison.Ordinal) < ( ( url.IndexOf( '?' ) == -1 ) ? url.Length : url.IndexOf( '?' ) ) ? "path" : "query" );
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