using MediaLibrary.DataModels;
using SmartView2.Controls;
using SmartView2.ViewModels.MultimediaInner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartView2.Views.MultimediaInner
{
    /// <summary>
    /// Interaction logic for MediaPlayerPage.xaml
    /// </summary>
    public partial class MediaPlayerPage : UserControl
    {
        public MediaPlayerPage()
        {
            InitializeComponent();
        }

        private void _fullScreenButton_Click(object sender, RoutedEventArgs e)
        {
            //this._mediaElement.SwitchFullScreenMode();
        }

        private void _mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaPlayerViewModel dataContext = (MediaPlayerViewModel)base.DataContext;
            if (dataContext.ContentType == ContentType.Video)
            {
                dataContext.MediaElementState = MediaElementState.Stop;
                return;
            }
            if (!dataContext.CanGetNextFile(null))
            {
                dataContext.MediaElementState = MediaElementState.Stop;
                return;
            }
            dataContext.GetNextFile.Execute(null);
        }

        private void _photoListBox_ManipulationBoundaryFeedback(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }
    }
}
