using System;
using System.Collections.Generic;

namespace SwaggerAPIDocumentationTests
{
	public class MockDictionary : Dictionary<Type, object>
	{
		public T Get<T>()
		{
			return (T) this[ typeof ( T ) ];
		}
	}
}