using SmartView2.Common.Enums;
using SmartView2.Models.Settings;
using System;

namespace SmartView2.Common.EnumsConverters
{
    public class EnumConverters<T>
    where T : struct, IConvertible
    {
        public EnumConverters()
        {
        }

        public static string GetLocalizatedString(T value)
        {
            string cOMSIDKOREAN;
            if ((object)value is Language)
            {
                Language? nullable = (Language?)((object)value as Language?);
                switch ((nullable.HasValue ? nullable.GetValueOrDefault() : Language.English))
                {
                    case Language.Korean:
                        {
                            cOMSIDKOREAN = "English (United States)";
                            break;
                        }
                    case Language.English:
                        {
                            cOMSIDKOREAN = "English (United States)";
                            break;
                        }
                    case Language.German:
                        {
                            cOMSIDKOREAN = "English (United States)";
                            break;
                        }
                    case Language.French:
                        {
                            cOMSIDKOREAN = "English (United States)";
                            break;
                        }
                    case Language.Russian:
                        {
                            cOMSIDKOREAN = "English (United States)";
                            break;
                        }
                    case Language.Italian:
                        {
                            cOMSIDKOREAN = "English (United States)";
                            break;
                        }
                    case Language.Spanish:
                        {
                            cOMSIDKOREAN = "English (United States)";
                            break;
                        }
                    case Language.Chinese:
                        {
                            cOMSIDKOREAN = "English (United States)";
                            break;
                        }
                    default:
                        {
                            cOMSIDKOREAN = "English (United States)";
                            break;
                        }
                }
            }
            else if ((object)value is SettingsModel.Opacity)
            {
                SettingsModel.Opacity? nullable1 = (SettingsModel.Opacity?)((object)value as SettingsModel.Opacity?);
                switch ((nullable1.HasValue ? nullable1.GetValueOrDefault() : SettingsModel.Opacity.Default))
                {
                    case SettingsModel.Opacity.Default:
                        {
                            cOMSIDKOREAN = "Default";
                            break;
                        }
                    case SettingsModel.Opacity.Translucent:
                        {
                            cOMSIDKOREAN = "Translucent";
                            break;
                        }
                    case SettingsModel.Opacity.Transparency:
                        {
                            cOMSIDKOREAN = "Transparent";
                            break;
                        }
                    case SettingsModel.Opacity.Opacity:
                        {
                            cOMSIDKOREAN = "Opacity";
                            break;
                        }
                    default:
                        {
                            cOMSIDKOREAN = "Default";
                            break;
                        }
                }
            }
            else if ((object)value is SettingsModel.FontStyles)
            {
                SettingsModel.FontStyles? nullable2 = (SettingsModel.FontStyles?)((object)value as SettingsModel.FontStyles?);
                switch ((nullable2.HasValue ? nullable2.GetValueOrDefault() : SettingsModel.FontStyles.Default))
                {
                    case SettingsModel.FontStyles.Default:
                        {
                            cOMSIDKOREAN = "Default";
                            break;
                        }
                    case SettingsModel.FontStyles.FontStyle1:
                        {
                            cOMSIDKOREAN = "Font Style 1";
                            break;
                        }
                    case SettingsModel.FontStyles.FontStyle2:
                        {
                            cOMSIDKOREAN = "Font Style 2";
                            break;
                        }
                    case SettingsModel.FontStyles.FontStyle3:
                        {
                            cOMSIDKOREAN = "Font Style 3";
                            break;
                        }
                    case SettingsModel.FontStyles.FontStyle4:
                        {
                            cOMSIDKOREAN = "Font Style 4";
                            break;
                        }
                    default:
                        {
                            cOMSIDKOREAN = "Default";
                            break;
                        }
                }
            }
            else if (!((object)value is SettingsModel.EdgeTypes))
            {
                cOMSIDKOREAN = value.ToString();
            }
            else
            {
                SettingsModel.EdgeTypes? nullable3 = (SettingsModel.EdgeTypes?)((object)value as SettingsModel.EdgeTypes?);
                switch ((nullable3.HasValue ? nullable3.GetValueOrDefault() : SettingsModel.EdgeTypes.None))
                {
                    case SettingsModel.EdgeTypes.None:
                        {
                            cOMSIDKOREAN = "No Edge";
                            break;
                        }
                    case SettingsModel.EdgeTypes.Raised:
                        {
                            cOMSIDKOREAN = "Raised";
                            break;
                        }
                    case SettingsModel.EdgeTypes.Depressed:
                        {
                            cOMSIDKOREAN = "Depressed";
                            break;
                        }
                    case SettingsModel.EdgeTypes.Uniform:
                        {
                            cOMSIDKOREAN = "Uniform";
                            break;
                        }
                    case SettingsModel.EdgeTypes.DropShadow:
                        {
                            cOMSIDKOREAN = "Drop Shadow";
                            break;
                        }
                    default:
                        {
                            cOMSIDKOREAN = "No Edge";
                            break;
                        }
                }
            }
            return cOMSIDKOREAN;
        }
    }
}