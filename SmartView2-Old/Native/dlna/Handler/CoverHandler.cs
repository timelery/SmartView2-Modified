using MediaLibrary.DataModels;
using SmartView2.Core;
using SmartView2.Native.HTTP;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;

namespace SmartView2.Native.DLNA.Handler
{
	internal class CoverHandler : IHttpHandler
	{
		private readonly IDataLibrary dataLibrary;

		public string Prefix
		{
			get
			{
				return JustDecompileGenerated_get_Prefix();
			}
			set
			{
				JustDecompileGenerated_set_Prefix(value);
			}
		}

		private string JustDecompileGenerated_Prefix_k__BackingField;

		public string JustDecompileGenerated_get_Prefix()
		{
			return this.JustDecompileGenerated_Prefix_k__BackingField;
		}

		private void JustDecompileGenerated_set_Prefix(string value)
		{
			this.JustDecompileGenerated_Prefix_k__BackingField = value;
		}

		public CoverHandler(IDataLibrary dataLibrary)
		{
			this.dataLibrary = dataLibrary;
			this.Prefix = "/cover/*";
		}

		public HttpResponse HandleRequest(HttpRequest request)
		{
			string str = request.Path.Substring(this.Prefix.Length - 1);
			Content itemById = this.dataLibrary.GetItemById(new Guid(str), this.dataLibrary.RootFolder) as Content;
			if (itemById == null)
			{
				return new HttpResponse(request, HttpCode.NotFound, "text/html", "");
			}
			MemoryStream memoryStream = new MemoryStream();
			Image.FromFile(itemById.Preview.AbsolutePath).Save(memoryStream, ImageFormat.Jpeg);
			return new HttpResponse(request, HttpCode.Ok, "image/jpeg", memoryStream);
		}
	}
}