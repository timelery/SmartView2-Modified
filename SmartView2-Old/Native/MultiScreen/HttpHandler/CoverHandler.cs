using MediaLibrary.DataModels;
using SmartView2.Native.HTTP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Resources;

namespace SmartView2.Native.MultiScreen.HttpHandler
{
	internal class CoverHandler : IHttpHandler
	{
		private readonly List<Content> sharedFiles;

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

		public CoverHandler(List<Content> sharedFiles)
		{
			this.sharedFiles = sharedFiles;
			this.Prefix = "/ms/cover/*";
		}

		public HttpResponse HandleRequest(HttpRequest request)
		{
			Stream fileStream;
			string str = request.Path.Substring(this.Prefix.Length - 1);
			Content content = this.sharedFiles.First<Content>((Content f) => f.ID == new Guid(str));
			if (content == null)
			{
				return new HttpResponse(request, HttpCode.NotFound, "text/html", "");
			}
			MemoryStream memoryStream = new MemoryStream();
			if (!content.Preview.AbsoluteUri.StartsWith("pack://"))
			{
				fileStream = new FileStream(content.Preview.AbsolutePath, FileMode.Open, FileAccess.Read);
			}
			else
			{
				fileStream = Application.GetResourceStream(content.Preview).Stream;
			}
			Image.FromStream(fileStream).Save(memoryStream, ImageFormat.Png);
			return new HttpResponse(request, HttpCode.Ok, "image/png", memoryStream);
		}
	}
}