using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;

namespace SwaggerAPIDocumentationTests
{
	public abstract class BaseAutomatedMockWireupTest<T>
	{
		protected T ObjectUnderTest;
		protected readonly MockDictionary Mocks = new MockDictionary();


		//This base class automatically generates all the mock objects required by the constructor of the object
		//under test.  To use it, inherit from the class, use the type of the object under test as the generic parameter
		//and then run tests on the ObjectUnderTest parameter

		[SetUp]
		public void TestFixtureSetUp()
		{
			Mocks.Clear();
			var type = typeof ( T );
			var ctor = type.GetConstructors().OrderByDescending( x => x.GetParameters().Count() ).First();
			foreach ( var parameter in ctor.GetParameters() )
			{
				var mockedItem = MockRepository.GenerateMock( parameter.ParameterType, new Type[ 0 ] );
				Mocks.Add( parameter.ParameterType, mockedItem );
			}
			ObjectUnderTest = (T) ctor.Invoke( Mocks.Values.ToArray() );
		}
	}
}