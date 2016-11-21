using MediaLibrary.DataModels;
using SmartView2.Core;
using SmartView2.Native.HTTP;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SmartView2.Native.MultiScreen.HttpHandler
{
	internal class FileHandler : IHttpHandler
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

		public FileHandler(List<Content> sharedFiles)
		{
			this.sharedFiles = sharedFiles;
			this.Prefix = "/ms/file/*";
		}

		public HttpResponse HandleRequest(HttpRequest request)
		{
			string str = request.Path.Substring(this.Prefix.Length - 1);
			char[] chrArray = new char[] { '.' };
			string str1 = str.Split(chrArray)[0];
			Content content = this.sharedFiles.First<Content>((Content f) => f.ID == new Guid(str1));
			if (content == null)
			{
				return new HttpResponse(request, HttpCode.NotFound, "text/html", "");
			}
			string mimeByFileType = MimeTypeSolver.GetMimeByFileType(Path.GetExtension(content.Path));
			Stream fileStream = new FileStream(content.Path, FileMode.Open, FileAccess.Read);
			if (mimeByFileType.StartsWith("image"))
			{
				using (Image image = Image.FromStream(fileStream))
				{
					if (image.Width > 15360 || image.Height > 8640)
					{
						fileStream = new MemoryStream();
						double num = Math.Min(15360 / (double)image.Width, 8640 / (double)image.Height);
						(new Bitmap(image, new Size((int)((double)image.Width * num), (int)((double)image.Height * num)))).Save(fileStream, image.RawFormat);
					}
				}
			}
			HttpResponse httpResponse = new HttpResponse(request, HttpCode.Ok, mimeByFileType, fileStream);
			httpResponse.Headers["Accept-Ranges"] = "bytes";
			return httpResponse;
		}
	}
}