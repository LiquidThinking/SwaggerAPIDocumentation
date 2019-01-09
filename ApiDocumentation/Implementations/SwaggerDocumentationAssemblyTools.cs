using System;
using System.Collections.Generic;
using System.Linq;
using SwaggerAPIDocumentation.Interfaces;

namespace SwaggerAPIDocumentation.Implementations
{
	internal sealed class SwaggerDocumentationAssemblyTools : ISwaggerDocumentationAssemblyTools
	{
		private List<Type> _assemblyTypes;
		private readonly IControllerAssemblyProvider _controllerAssemblyProvider;

		private static Type DocumentationAttributeType => typeof( ApiDocumentationAttribute );

		public SwaggerDocumentationAssemblyTools( IControllerAssemblyProvider controllerAssemblyProvider )
		{
			_controllerAssemblyProvider = controllerAssemblyProvider;
		}

		public List<Type> GetTypesThatAreDecoratedWithApiDocumentationAttribute( IEnumerable<Type> controllerTypes )
		{
			return ( from type in controllerTypes
				where TypeIsDecoratedWithApiDocumentationAttribute( type )
				select type ).ToList();
		}

		public List<Type> GetApiControllerTypes( params Type[] baseControllerTypes )
		{
			return GetControllerTypes().SelectMany( x => x ).ToList();

			IEnumerable<IEnumerable<Type>> GetControllerTypes()
			{
				foreach ( var controllerType in baseControllerTypes )
				{
					yield return ( from type in _assemblyTypes ?? ( _assemblyTypes = GetAllAssemblyTypes() )
						where TypeInheritsFromBaseApiController( controllerType, type )
						select type ).ToList();
				}
			}
		}

		private List<Type> GetAllAssemblyTypes()
		{
			return _controllerAssemblyProvider
				.GetControllerAssemblies()
				.SelectMany( x => x.GetTypes() )
				.ToList();
		}

		private bool TypeIsDecoratedWithApiDocumentationAttribute( Type type )
		{
			return TypeHasAnyClassAttributes( type ) || MethodsHaveAnyAttributes( type );
		}

		private static bool MethodsHaveAnyAttributes( Type type )
		{
			return type.GetMethods().Any( x => Attribute.GetCustomAttributes( x, DocumentationAttributeType, false ).Any() );
		}

		private static bool TypeHasAnyClassAttributes( Type type )
		{
			return Attribute.GetCustomAttributes( type, DocumentationAttributeType, false ).Any();
		}

		private static bool TypeInheritsFromBaseApiController( Type baseControllerType, Type type )
		{
			return type.IsSubclassOf( baseControllerType );
		}
	}
}