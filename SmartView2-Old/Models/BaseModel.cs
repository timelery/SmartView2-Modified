using System;
using System.ComponentModel;
using System.Threading;

namespace SmartView2.Models
{
	public class BaseModel : INotifyPropertyChanged
	{
		public BaseModel()
		{
		}

		protected void OnPropertyChanged(object sender, string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}