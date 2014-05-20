using System;
using System.Web.Mvc;

namespace SwaggerAPIDocumentation
{
	[AttributeUsage( AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true )]
	public class ApiDocumentationAttribute : Attribute
	{
		public String Url { get; private set; }
		public String Description { get; private set; }
		public Type ReturnType { get; private set; }
		public HttpVerbs RequestType { get; private set; }
		public Type FormBody { get; private set; }

		public ApiDocumentationAttribute( String url, String description = "", Type returnType = null, HttpVerbs requestType = HttpVerbs.Get, Type formBody = null )
		{
			Url = url;
			Description = description;
			ReturnType = returnType;
			RequestType = requestType;
			FormBody = formBody;
		}		
	}
}