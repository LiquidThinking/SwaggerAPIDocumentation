using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using SwaggerAPIDocumentation.Interfaces;

namespace SwaggerAPIDocumentation.Implementations
{
	internal class SwaggerDocumentationAssemblyTools : ISwaggerDocumentationAssemblyTools
	{
		private Type DocumentationAttributeType => typeof ( ApiDocumentationAttribute );

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
					yield return ( from type in GetTypesFromTypeAssembly( controllerType )
						where TypeInheritsFromBaseApiController( controllerType, type )
						select type ).ToList();
				}
			}
		}

		private bool TypeIsDecoratedWithApiDocumentationAttribute( Type type )
		{
			return TypeHasAnyClassAttributes( type ) || MethodsHaveAnyAttributes( type );
		}

		private bool MethodsHaveAnyAttributes( Type type )
		{
			return type.GetMethods().Any( x => Attribute.GetCustomAttributes( x, DocumentationAttributeType, false ).Any() );
		}

		private bool TypeHasAnyClassAttributes( Type type )
		{
			return Attribute.GetCustomAttributes( type, DocumentationAttributeType, false ).Any();
		}

		protected virtual bool TypeInheritsFromBaseApiController( Type baseControllerType, Type type )
		{
			return type.IsSubclassOf( baseControllerType );
		}

		protected virtual IEnumerable<Type> GetTypesFromTypeAssembly( Type type )
		{
			return Assembly.GetAssembly( type ).GetTypes();
		}
	}
}