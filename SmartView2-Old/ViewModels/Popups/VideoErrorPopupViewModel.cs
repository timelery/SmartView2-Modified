using SmartView2.Core;
using SmartView2.Core.Commands;
using SmartView2.Properties;
using SmartView2.Views.Popups;
using System;
using System.Windows.Input;
using UIFoundation.Navigation;

namespace SmartView2.ViewModels.Popups
{
    [RelatedView(typeof(VideoErrorPopup))]
    public class VideoErrorPopupViewModel : PopupViewModel
    {
        private string text;

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.SetProperty<string>(ref this.text, value, "Text");
            }
        }

        public ICommand OkCommand { get; private set; }

        public VideoErrorPopupViewModel()
        {
            this.OkCommand = (ICommand)new Command((Action<object>)(arg => this.Controller.ClosePopup()));
        }

        public override void OnNavigateTo(object p)
        {
            if (p == null || !(p is MessageType))
                return;
            switch ((MessageType)p)
            {
                case MessageType.SourceConflict:
                    this.Text = ResourcesModel.Instanse.MAPP_SID_SOURCE_CONFLICT;
                    break;
                case MessageType.NotConnected:
                    this.Text = ResourcesModel.Instanse.MAPP_SID_SOUCRE_NOT_CONNECTED;
                    break;
                case MessageType.OtherMode:
                    this.Text = ResourcesModel.Instanse.MAPP_SID_OTHER_MODE;
                    break;
                case MessageType.LowPriority:
                    this.Text = ResourcesModel.Instanse.MAPP_SID_LOW_PRIORITY;
                    break;
                case MessageType.AnalogConflict:
                    this.Text = ResourcesModel.Instanse.MAPP_SID_UNABLE_CHANGE_SOURCE_RESTRICTION;
                    break;
                default:
                    this.Text = "Some Error:'(";
                    break;
            }
        }
    }
}