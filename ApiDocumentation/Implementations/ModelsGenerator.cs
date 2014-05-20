using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SwaggerAPIDocumentation.Interfaces;
using SwaggerAPIDocumentation.ViewModels;

namespace SwaggerAPIDocumentation.Implementations
{
	internal class ModelsGenerator : IModelsGenerator
	{
		private readonly ITypeToStringConverter _typeToStringConverter;

		public ModelsGenerator() : this( new TypeToStringConverter() ) {}

		public ModelsGenerator( ITypeToStringConverter typeToStringConverter )
		{
			_typeToStringConverter = typeToStringConverter;
		}

		private readonly List<Type> _processedTypes = new List<Type>();

		private readonly List<Type> _exclusions = new List<Type>
		{
			typeof ( String ),
			typeof ( Type ),
			typeof ( DateTime ),
			typeof ( TimeSpan ),
			typeof ( Decimal ),
			typeof ( Boolean )
		};

		public virtual Dictionary<String, ApiDocModel> GetModels( Type type )
		{
			if ( IsAList( type ) )
				type = GetGenericArgument( type, 0 );
			if ( IsArray( type ) )
				type = type.GetElementType();

			var name = _typeToStringConverter.GetApiOperationType( type );
			var apiDocModels = InitializeApiDocModels( name );

			ProcessType( type, apiDocModels );

			return apiDocModels;
		}

		private void ProcessType( Type type, Dictionary<string, ApiDocModel> apiDocModels )
		{
			foreach ( var property in type.GetProperties() )
			{
				var propertyType = property.PropertyType;
				var modelProperty = DefaultModelProperty( property );

				if ( IsAList( propertyType ) )
				{
					propertyType = GetGenericArgument( propertyType, 0 );
					modelProperty = GetArrayModelProperty( propertyType );
				}
				else if ( IsArray( propertyType ) )
				{
					propertyType = propertyType.GetElementType();
					modelProperty = GetArrayModelProperty( propertyType );
				}
				else if ( IsDictionary( propertyType ) )
				{
					var key = GetGenericArgument( propertyType, 0 );
					var value = GetGenericArgument( propertyType, 1 );

					GetNonPrimitiveModels( key, apiDocModels );
					GetNonPrimitiveModels( value, apiDocModels );

					apiDocModels.Merge( GetKeyValuePairModel( key, value ) );

					modelProperty = GetArrayModelProperty( typeof ( KeyValuePair<,> ) );
				}

				GetNonPrimitiveModels( propertyType, apiDocModels );
				apiDocModels.First().Value.properties.Add( property.Name, modelProperty );
			}
		}

		private Type GetGenericArgument( Type type, int index )
		{
			return type.GetGenericArguments()[ index ];
		}

		private ApiDocModelProperty DefaultModelProperty( PropertyInfo property )
		{
			return new ApiDocModelProperty
			{
				type = _typeToStringConverter.GetApiOperationType( property.PropertyType )
			};
		}

		private ApiDocModelProperty GetArrayModelProperty( Type arrayType )
		{
			return new ApiDocModelProperty
			{
				type = "array",
				items = new ArrayItems
				{
					ArrayType = _typeToStringConverter.GetApiOperationType( arrayType )
				}
			};
		}

		private static bool IsDictionary( Type type )
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof ( Dictionary<,> );
		}

		private static bool IsArray( Type type )
		{
			return type.IsArray;
		}

		private static bool IsAList( Type type )
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof ( List<> );
		}

		private void GetNonPrimitiveModels( Type propertyType, Dictionary<string, ApiDocModel> apiDocModels )
		{
			if ( TypeShouldBeProcessed( propertyType ) && TypeHasNotAlreadyBeenProcessed( propertyType ) )
			{
				_processedTypes.Add( propertyType );
				apiDocModels.Merge( GetModels( propertyType ) );
			}
		}

		private bool TypeHasNotAlreadyBeenProcessed( Type propertyType )
		{
			return !_processedTypes.Contains( propertyType );
		}

		private bool TypeShouldBeProcessed( Type propertyType )
		{
			return !propertyType.IsPrimitive && !_exclusions.Contains( propertyType );
		}

		private Dictionary<String, ApiDocModel> GetKeyValuePairModel( Type key, Type value )
		{
			return new Dictionary<String, ApiDocModel>
			{
				{
					"KeyValuePair", new ApiDocModel
					{
						id = "KeyValuePair",
						properties = new Dictionary<String, ApiDocModelProperty>
						{
							{
								"Key", new ApiDocModelProperty
								{
									type = _typeToStringConverter.GetApiOperationType( key )
								}
							},
							{
								"Value", new ApiDocModelProperty
								{
									type = _typeToStringConverter.GetApiOperationType( value )
								}
							}
						}
					}
				}
			};
		}

		private Dictionary<String, ApiDocModel> InitializeApiDocModels( string name )
		{
			return new Dictionary<String, ApiDocModel>
			{
				{
					name, new ApiDocModel
					{
						id = name,
						properties = new Dictionary<String, ApiDocModelProperty>()
					}
				}
			};
		}
	}
}