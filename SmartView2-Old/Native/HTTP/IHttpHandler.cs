using System;

namespace SmartView2.Native.HTTP
{
	public interface IHttpHandler
	{
		string Prefix
		{
			get;
		}

		HttpResponse HandleRequest(HttpRequest request);
	}
}