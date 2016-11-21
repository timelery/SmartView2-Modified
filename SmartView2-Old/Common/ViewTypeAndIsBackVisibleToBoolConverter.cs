using SmartView2.Core;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SmartView2.Common
{
    public class ViewTypeAndIsBackVisibleToBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            object obj = values[0];
            bool? nullable1 = values[1] != DependencyProperty.UnsetValue ? values[1] as bool? : new bool?();
            if (obj is MusicViewType)
            {
                switch ((MusicViewType)obj)
                {
                    case MusicViewType.Track:
                    case MusicViewType.ArtistDetailed:
                    case MusicViewType.AlbumDetailed:
                    case MusicViewType.GenreDetailed:
                        return (object)true;
                    case MusicViewType.Folder:
                        bool? nullable2 = nullable1;
                        if ((!nullable2.GetValueOrDefault() ? 0 : (nullable2.HasValue ? 1 : 0)) != 0)
                            return (object)true;
                        return (object)false;
                    default:
                        return (object)false;
                }
            }
            else
            {
                if (!(obj is VideoPhotoViewType))
                    return (object)false;
                switch ((VideoPhotoViewType)obj)
                {
                    case VideoPhotoViewType.Date:
                    case VideoPhotoViewType.Title:
                        return (object)true;
                    case VideoPhotoViewType.Folder:
                        bool? nullable3 = nullable1;
                        if ((!nullable3.GetValueOrDefault() ? 0 : (nullable3.HasValue ? 1 : 0)) != 0)
                            return (object)true;
                        return (object)false;
                    default:
                        return (object)false;
                }
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}