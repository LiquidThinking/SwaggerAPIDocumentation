using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SwaggerAPIDocumentation.Implementations;
using SwaggerAPIDocumentation.ViewModels;

namespace SwaggerAPIDocumentationTests
{
	[TestFixture]
	public class NewtonSoftTests
	{
		[Test]
		public void CanSerializeDictionary()
		{
			SwaggerApiResource swaggerApiResource = new SwaggerApiResource
			{
				Models = new Dictionary<String, ApiDocModel>
				{
					{
						"Test", new ApiDocModel()
					}
				}
			};

			JsonSerializer jsonSerializer = new JsonSerializer();
			var result = jsonSerializer.SerializeObject( swaggerApiResource );
			Assert.IsNotNull( result );
		}
	}
}