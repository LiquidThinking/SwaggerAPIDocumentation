using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwaggerAPIDocumentation
{
	[AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
	public class OptionalAttribute : Attribute
	{
	}
}
