using System.Collections.Generic;
using System.Linq;

namespace SwaggerAPIDocumentation.Implementations
{
	internal static class Helpers
	{
		public static void Merge<TKey, TValue>( this IDictionary<TKey, TValue> currentDictionary, IDictionary<TKey, TValue> dictionaryToMerge )
		{
			dictionaryToMerge.ToList().ForEach( x =>
			{
				if ( !currentDictionary.Keys.Contains( x.Key ) )
					currentDictionary.Add( x.Key, x.Value );
			} );
		}
	}
}