using Newtonsoft.Json;
using SmartView2.Properties;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SmartView2
{
	public class UpdateController
	{
		public bool ForceUpdate
		{
			get;
			private set;
		}

		public bool UpdateFound
		{
			get;
			private set;
		}

		public string UpdateUrl
		{
			get;
			private set;
		}

		public UpdateController()
		{
		}

		public async Task CheckForUpdateAsync()
		{
			UpdateController.VersionInfo versionFromServerAsync;
			Version version;
			if (this.IsUpdateCheckRequired())
			{
				versionFromServerAsync = await this.GetVersionFromServerAsync();
			}
			else
			{
				versionFromServerAsync = this.GetCachedVersion();
			}
			UpdateController.VersionInfo versionInfo = versionFromServerAsync;
			Version.TryParse(versionInfo.Version, out version);
			Version version1 = Assembly.GetExecutingAssembly().GetName().Version;
			this.UpdateFound = version > version1;
			this.ForceUpdate = version.Minor > version1.Minor;
			this.UpdateUrl = versionInfo.Url;
		}

		private UpdateController.VersionInfo GetCachedVersion()
		{
			string lastVersionInfo = Settings.Default.LastVersionInfo;
			if (string.IsNullOrEmpty(lastVersionInfo))
			{
				return new UpdateController.VersionInfo();
			}
			return JsonConvert.DeserializeObject<UpdateController.VersionInfo>(lastVersionInfo);
		}

		private async Task<UpdateController.VersionInfo> GetVersionFromServerAsync()
		{
			UpdateController.VersionInfo versionInfo;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(Settings.Default.VersionCheckUrl);
				httpWebRequest.Timeout = 10000;
				using (HttpWebResponse responseAsync = (HttpWebResponse)await httpWebRequest.GetResponseAsync())
				{
					string end = (new StreamReader(responseAsync.GetResponseStream())).ReadToEnd();
					Settings.Default.LastVersionInfo = end;
					Settings.Default.LastVersionCheck = DateTime.Today;
					Settings.Default.Save();
					versionInfo = JsonConvert.DeserializeObject<UpdateController.VersionInfo>(end);
				}
			}
			catch
			{
				UpdateController.VersionInfo versionInfo1 = new UpdateController.VersionInfo()
				{
					Version = "1.0.0.0",
					Force = false,
					Url = string.Empty
				};
				versionInfo = versionInfo1;
			}
			return versionInfo;
		}

		private bool IsUpdateCheckRequired()
		{
			return DateTime.Today > Settings.Default.LastVersionCheck.Date;
		}

		private class VersionInfo
		{
			public bool Force
			{
				get;
				set;
			}

			public string Url
			{
				get;
				set;
			}

			public string Version
			{
				get;
				set;
			}

			public VersionInfo()
			{
			}
		}
	}
}