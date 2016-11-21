using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace SmartView2.Properties
{
	public class ResourcesModel : INotifyPropertyChanged
	{
		private static ResourcesModel resourcesModel;

		public string COM_AUTO_DISCOVERY
		{
			get
			{
				return Resources.COM_AUTO_DISCOVERY;
			}
		}

		public string COM_BDP_SID_POPUP_SOYV_DISC_CHECK_NETWORK_SETTINGS_DESCRIPTION_1_TEXT
		{
			get
			{
				return Resources.COM_BDP_SID_POPUP_SOYV_DISC_CHECK_NETWORK_SETTINGS_DESCRIPTION_1_TEXT;
			}
		}

		public string COM_BDP_SID_SMARTHUB_RESET_COMPLETE_TEXT
		{
			get
			{
				return Resources.COM_BDP_SID_SMARTHUB_RESET_COMPLETE_TEXT;
			}
		}

		public string COM_BDP_STR_MODE_REPEAT_TRACK_ONE_KR_SONG
		{
			get
			{
				return Resources.COM_BDP_STR_MODE_REPEAT_TRACK_ONE_KR_SONG;
			}
		}

		public string COM_BUTTON_SEARCH
		{
			get
			{
				return Resources.COM_BUTTON_SEARCH;
			}
		}

		public string COM_BUTTON_SEARCH_UPPER
		{
			get
			{
				return Resources.COM_BUTTON_SEARCH_UPPER;
			}
		}

		public string COM_CHECK_FOR_UPDATES
		{
			get
			{
				return Resources.COM_CHECK_FOR_UPDATES;
			}
		}

		public string COM_EXIT_SMART_VIEW
		{
			get
			{
				return Resources.COM_EXIT_SMART_VIEW;
			}
		}

		public string COM_HTS_SID_GUIDE
		{
			get
			{
				return Resources.COM_HTS_SID_GUIDE;
			}
		}

		public string COM_HTS_SID_GUIDE_UPPER
		{
			get
			{
				return Resources.COM_HTS_SID_GUIDE_UPPER;
			}
		}

		public string COM_IDS_CONTENTS_WIZARD_FILLCOLOR
		{
			get
			{
				return Resources.COM_IDS_CONTENTS_WIZARD_FILLCOLOR;
			}
		}

		public string COM_IDS_FONT_TEXT
		{
			get
			{
				return Resources.COM_IDS_FONT_TEXT;
			}
		}

		public string COM_IDS_REPORT_SORTBY
		{
			get
			{
				return Resources.COM_IDS_REPORT_SORTBY;
			}
		}

		public string COM_IDS_STR_ENTER_PIN
		{
			get
			{
				return Resources.COM_IDS_STR_ENTER_PIN;
			}
		}

		public string COM_IDS_TXT_RESET_ALL
		{
			get
			{
				return Resources.COM_IDS_TXT_RESET_ALL;
			}
		}

		public string COM_IDWS_MOIP_VERSION
		{
			get
			{
				return Resources.COM_IDWS_MOIP_VERSION;
			}
		}

		public string COM_INTRODUCTION
		{
			get
			{
				return Resources.COM_INTRODUCTION;
			}
		}

		public string COM_LFD_DO_YOU_WANT_TO_EXIT_THE_APPLICATION
		{
			get
			{
				return Resources.COM_LFD_DO_YOU_WANT_TO_EXIT_THE_APPLICATION;
			}
		}

		public string COM_LFD_REMOTE_CONTROL
		{
			get
			{
				return Resources.COM_LFD_REMOTE_CONTROL;
			}
		}

		public string COM_LIST_TITLE_VERSION
		{
			get
			{
				return Resources.COM_LIST_TITLE_VERSION;
			}
		}

		public string COM_MAPP_DON_T_SHOW_AGAIN
		{
			get
			{
				return Resources.COM_MAPP_DON_T_SHOW_AGAIN;
			}
		}

		public string COM_MAPP_I_AGREE
		{
			get
			{
				return Resources.COM_MAPP_I_AGREE;
			}
		}

		public string COM_SAMSUNG_SMART_VIEW
		{
			get
			{
				return Resources.COM_SAMSUNG_SMART_VIEW;
			}
		}

		public string COM_SID_3D_IS_NOT_SUPPORTED
		{
			get
			{
				return Resources.COM_SID_3D_IS_NOT_SUPPORTED;
			}
		}

		public string COM_SID_ADD_FILE
		{
			get
			{
				return Resources.COM_SID_ADD_FILE;
			}
		}

		public string COM_SID_ADD_FILE_TO_LIBRARY
		{
			get
			{
				return Resources.COM_SID_ADD_FILE_TO_LIBRARY;
			}
		}

		public string COM_SID_ADD_FOLDER
		{
			get
			{
				return Resources.COM_SID_ADD_FOLDER;
			}
		}

		public string COM_SID_ADD_FOLDER_TO_LIBRARY
		{
			get
			{
				return Resources.COM_SID_ADD_FOLDER_TO_LIBRARY;
			}
		}

		public string COM_SID_ALBUM
		{
			get
			{
				return Resources.COM_SID_ALBUM;
			}
		}

		public string COM_SID_ALBUM_TITLE
		{
			get
			{
				return Resources.COM_SID_ALBUM_TITLE;
			}
		}

		public string COM_SID_ALBUMS
		{
			get
			{
				return Resources.COM_SID_ALBUMS;
			}
		}

		public string COM_SID_AN_UNKNOWN_ERROR_HAS_OCCURRED
		{
			get
			{
				return Resources.COM_SID_AN_UNKNOWN_ERROR_HAS_OCCURRED;
			}
		}

		public string COM_SID_ARTIST
		{
			get
			{
				return Resources.COM_SID_ARTIST;
			}
		}

		public string COM_SID_ARTIST_TITLE
		{
			get
			{
				return Resources.COM_SID_ARTIST_TITLE;
			}
		}

		public string COM_SID_AUTHENTICATION_FAILED
		{
			get
			{
				return Resources.COM_SID_AUTHENTICATION_FAILED;
			}
		}

		public string COM_SID_AV
		{
			get
			{
				return Resources.COM_SID_AV;
			}
		}

		public string COM_SID_BACK
		{
			get
			{
				return Resources.COM_SID_BACK;
			}
		}

		public string COM_SID_BLACK
		{
			get
			{
				return Resources.COM_SID_BLACK;
			}
		}

		public string COM_SID_BLIND
		{
			get
			{
				return Resources.COM_SID_BLIND;
			}
		}

		public string COM_SID_CANCEL_ADDING
		{
			get
			{
				return Resources.COM_SID_CANCEL_ADDING;
			}
		}

		public string COM_SID_CAPTION_OPTIONS_KOR_MSG
		{
			get
			{
				return Resources.COM_SID_CAPTION_OPTIONS_KOR_MSG;
			}
		}

		public string COM_SID_CC_SETTING
		{
			get
			{
				return Resources.COM_SID_CC_SETTING;
			}
		}

		public string COM_SID_CC_SETTINGS
		{
			get
			{
				return Resources.COM_SID_CC_SETTINGS;
			}
		}

		public string COM_SID_CHANNEL_LIST
		{
			get
			{
				return Resources.COM_SID_CHANNEL_LIST;
			}
		}

		public string COM_SID_CHECKER
		{
			get
			{
				return Resources.COM_SID_CHECKER;
			}
		}

		public string COM_SID_CHINESE
		{
			get
			{
				return Resources.COM_SID_CHINESE;
			}
		}

		public string COM_SID_CLOSE
		{
			get
			{
				return Resources.COM_SID_CLOSE;
			}
		}

		public string COM_SID_COMPONENT
		{
			get
			{
				return Resources.COM_SID_COMPONENT;
			}
		}

		public string COM_SID_CONNECT
		{
			get
			{
				return Resources.COM_SID_CONNECT;
			}
		}

		public string COM_SID_CURRENT_VERSION
		{
			get
			{
				return Resources.COM_SID_CURRENT_VERSION;
			}
		}

		public string COM_SID_CYAN
		{
			get
			{
				return Resources.COM_SID_CYAN;
			}
		}

		public string COM_SID_DATE
		{
			get
			{
				return Resources.COM_SID_DATE;
			}
		}

		public string COM_SID_DELETE
		{
			get
			{
				return Resources.COM_SID_DELETE;
			}
		}

		public string COM_SID_DEVICE_LIST
		{
			get
			{
				return Resources.COM_SID_DEVICE_LIST;
			}
		}

		public string COM_SID_DISCONNECT
		{
			get
			{
				return Resources.COM_SID_DISCONNECT;
			}
		}

		public string COM_SID_DOWNLOAD_NOW
		{
			get
			{
				return Resources.COM_SID_DOWNLOAD_NOW;
			}
		}

		public string COM_SID_EFFECT
		{
			get
			{
				return Resources.COM_SID_EFFECT;
			}
		}

		public string COM_SID_EFFECTS
		{
			get
			{
				return Resources.COM_SID_EFFECTS;
			}
		}

		public string COM_SID_ENGLISH
		{
			get
			{
				return Resources.COM_SID_ENGLISH;
			}
		}

		public string COM_SID_ENTER_PIN
		{
			get
			{
				return Resources.COM_SID_ENTER_PIN;
			}
		}

		public string COM_SID_ENTER_TEXT
		{
			get
			{
				return Resources.COM_SID_ENTER_TEXT;
			}
		}

		public string COM_SID_ERROR
		{
			get
			{
				return Resources.COM_SID_ERROR;
			}
		}

		public string COM_SID_EXIT
		{
			get
			{
				return Resources.COM_SID_EXIT;
			}
		}

		public string COM_SID_EXIT_FULL_SCREEN_MODE
		{
			get
			{
				return Resources.COM_SID_EXIT_FULL_SCREEN_MODE;
			}
		}

		public string COM_SID_FAST
		{
			get
			{
				return Resources.COM_SID_FAST;
			}
		}

		public string COM_SID_FOLDER
		{
			get
			{
				return Resources.COM_SID_FOLDER;
			}
		}

		public string COM_SID_FONT_STYLE_1
		{
			get
			{
				return Resources.COM_SID_FONT_STYLE_1;
			}
		}

		public string COM_SID_FONT_STYLE_2
		{
			get
			{
				return Resources.COM_SID_FONT_STYLE_2;
			}
		}

		public string COM_SID_FONT_STYLE_3
		{
			get
			{
				return Resources.COM_SID_FONT_STYLE_3;
			}
		}

		public string COM_SID_FONT_STYLE_4
		{
			get
			{
				return Resources.COM_SID_FONT_STYLE_4;
			}
		}

		public string COM_SID_FONT_STYLE_KR_STYLE
		{
			get
			{
				return Resources.COM_SID_FONT_STYLE_KR_STYLE;
			}
		}

		public string COM_SID_FRENCH
		{
			get
			{
				return Resources.COM_SID_FRENCH;
			}
		}

		public string COM_SID_FULL_SCREEN
		{
			get
			{
				return Resources.COM_SID_FULL_SCREEN;
			}
		}

		public string COM_SID_GENRE
		{
			get
			{
				return Resources.COM_SID_GENRE;
			}
		}

		public string COM_SID_GERMAN
		{
			get
			{
				return Resources.COM_SID_GERMAN;
			}
		}

		public string COM_SID_GRAY
		{
			get
			{
				return Resources.COM_SID_GRAY;
			}
		}

		public string COM_SID_GREEN
		{
			get
			{
				return Resources.COM_SID_GREEN;
			}
		}

		public string COM_SID_HDMI1
		{
			get
			{
				return Resources.COM_SID_HDMI1;
			}
		}

		public string COM_SID_HDMI2
		{
			get
			{
				return Resources.COM_SID_HDMI2;
			}
		}

		public string COM_SID_HDMI3
		{
			get
			{
				return Resources.COM_SID_HDMI3;
			}
		}

		public string COM_SID_HDMI4
		{
			get
			{
				return Resources.COM_SID_HDMI4;
			}
		}

		public string COM_SID_HOME_M_MAIN
		{
			get
			{
				return Resources.COM_SID_HOME_M_MAIN;
			}
		}

		public string COM_SID_HOME_M_MAIN_UPPER
		{
			get
			{
				return Resources.COM_SID_HOME_M_MAIN_UPPER;
			}
		}

		public string COM_SID_INFO_CASE
		{
			get
			{
				return Resources.COM_SID_INFO_CASE;
			}
		}

		public string COM_SID_INFO_UPPER
		{
			get
			{
				return Resources.COM_SID_INFO_UPPER;
			}
		}

		public string COM_SID_INPUT_TEXT
		{
			get
			{
				return Resources.COM_SID_INPUT_TEXT;
			}
		}

		public string COM_SID_ITALIAN
		{
			get
			{
				return Resources.COM_SID_ITALIAN;
			}
		}

		public string COM_SID_KOREAN
		{
			get
			{
				return Resources.COM_SID_KOREAN;
			}
		}

		public string COM_SID_LANGUAGE
		{
			get
			{
				return Resources.COM_SID_LANGUAGE;
			}
		}

		public string COM_SID_LANGUAGE_SETTINGS
		{
			get
			{
				return Resources.COM_SID_LANGUAGE_SETTINGS;
			}
		}

		public string COM_SID_LATEST_VERSION
		{
			get
			{
				return Resources.COM_SID_LATEST_VERSION;
			}
		}

		public string COM_SID_LICENSE
		{
			get
			{
				return Resources.COM_SID_LICENSE;
			}
		}

		public string COM_SID_LINEAR
		{
			get
			{
				return Resources.COM_SID_LINEAR;
			}
		}

		public string COM_SID_LOADING_DOT
		{
			get
			{
				return Resources.COM_SID_LOADING_DOT;
			}
		}

		public string COM_SID_MENU
		{
			get
			{
				return Resources.COM_SID_MENU;
			}
		}

		public string COM_SID_MESSAGE
		{
			get
			{
				return Resources.COM_SID_MESSAGE;
			}
		}

		public string COM_SID_MOBILE_APP
		{
			get
			{
				return Resources.COM_SID_MOBILE_APP;
			}
		}

		public string COM_SID_MOVE_DOWN
		{
			get
			{
				return Resources.COM_SID_MOVE_DOWN;
			}
		}

		public string COM_SID_MOVE_TO_NEXT
		{
			get
			{
				return Resources.COM_SID_MOVE_TO_NEXT;
			}
		}

		public string COM_SID_MOVE_TO_PREVIOUS
		{
			get
			{
				return Resources.COM_SID_MOVE_TO_PREVIOUS;
			}
		}

		public string COM_SID_MOVE_UP
		{
			get
			{
				return Resources.COM_SID_MOVE_UP;
			}
		}

		public string COM_SID_MULTIMEDIA
		{
			get
			{
				return Resources.COM_SID_MULTIMEDIA;
			}
		}

		public string COM_SID_MUSIC
		{
			get
			{
				return Resources.COM_SID_MUSIC;
			}
		}

		public string COM_SID_NO
		{
			get
			{
				return Resources.COM_SID_NO;
			}
		}

		public string COM_SID_NO_SIGNAL
		{
			get
			{
				return Resources.COM_SID_NO_SIGNAL;
			}
		}

		public string COM_SID_NONE
		{
			get
			{
				return Resources.COM_SID_NONE;
			}
		}

		public string COM_SID_NORMAL
		{
			get
			{
				return Resources.COM_SID_NORMAL;
			}
		}

		public string COM_SID_NOT_AVAILABLE
		{
			get
			{
				return Resources.COM_SID_NOT_AVAILABLE;
			}
		}

		public string COM_SID_NOW_PLAYING
		{
			get
			{
				return Resources.COM_SID_NOW_PLAYING;
			}
		}

		public string COM_SID_OPACITY
		{
			get
			{
				return Resources.COM_SID_OPACITY;
			}
		}

		public string COM_SID_OPAQUE
		{
			get
			{
				return Resources.COM_SID_OPAQUE;
			}
		}

		public string COM_SID_OPEN
		{
			get
			{
				return Resources.COM_SID_OPEN;
			}
		}

		public string COM_SID_OPEN_PLAY
		{
			get
			{
				return Resources.COM_SID_OPEN_PLAY;
			}
		}

		public string COM_SID_OPEN_SOURCE_LICENSE
		{
			get
			{
				return Resources.COM_SID_OPEN_SOURCE_LICENSE;
			}
		}

		public string COM_SID_PAUSE
		{
			get
			{
				return Resources.COM_SID_PAUSE;
			}
		}

		public string COM_SID_PHOTO
		{
			get
			{
				return Resources.COM_SID_PHOTO;
			}
		}

		public string COM_SID_PHOTOS
		{
			get
			{
				return Resources.COM_SID_PHOTOS;
			}
		}

		public string COM_SID_PLAY
		{
			get
			{
				return Resources.COM_SID_PLAY;
			}
		}

		public string COM_SID_PLAY_PAUSE
		{
			get
			{
				return Resources.COM_SID_PLAY_PAUSE;
			}
		}

		public string COM_SID_POWER
		{
			get
			{
				return Resources.COM_SID_POWER;
			}
		}

		public string COM_SID_PRE_CH
		{
			get
			{
				return Resources.COM_SID_PRE_CH;
			}
		}

		public string COM_SID_PREVIEW
		{
			get
			{
				return Resources.COM_SID_PREVIEW;
			}
		}

		public string COM_SID_RANDOM
		{
			get
			{
				return Resources.COM_SID_RANDOM;
			}
		}

		public string COM_SID_RED
		{
			get
			{
				return Resources.COM_SID_RED;
			}
		}

		public string COM_SID_REMOVE_FILES
		{
			get
			{
				return Resources.COM_SID_REMOVE_FILES;
			}
		}

		public string COM_SID_REMOVE_FROM_QUEUE
		{
			get
			{
				return Resources.COM_SID_REMOVE_FROM_QUEUE;
			}
		}

		public string COM_SID_RETRY
		{
			get
			{
				return Resources.COM_SID_RETRY;
			}
		}

		public string COM_SID_RUSSIAN
		{
			get
			{
				return Resources.COM_SID_RUSSIAN;
			}
		}

		public string COM_SID_SCART
		{
			get
			{
				return Resources.COM_SID_SCART;
			}
		}

		public string COM_SID_SETTINGS
		{
			get
			{
				return Resources.COM_SID_SETTINGS;
			}
		}

		public string COM_SID_SLIDE_SHOW_SETTINGS
		{
			get
			{
				return Resources.COM_SID_SLIDE_SHOW_SETTINGS;
			}
		}

		public string COM_SID_SLIDESHOW_SETTINGS
		{
			get
			{
				return Resources.COM_SID_SLIDESHOW_SETTINGS;
			}
		}

		public string COM_SID_SLOW
		{
			get
			{
				return Resources.COM_SID_SLOW;
			}
		}

		public string COM_SID_SONG_TITLE
		{
			get
			{
				return Resources.COM_SID_SONG_TITLE;
			}
		}

		public string COM_SID_SONGS
		{
			get
			{
				return Resources.COM_SID_SONGS;
			}
		}

		public string COM_SID_SORT
		{
			get
			{
				return Resources.COM_SID_SORT;
			}
		}

		public string COM_SID_SOURCE
		{
			get
			{
				return Resources.COM_SID_SOURCE;
			}
		}

		public string COM_SID_SPANISH
		{
			get
			{
				return Resources.COM_SID_SPANISH;
			}
		}

		public string COM_SID_SPEED
		{
			get
			{
				return Resources.COM_SID_SPEED;
			}
		}

		public string COM_SID_SPIRAL
		{
			get
			{
				return Resources.COM_SID_SPIRAL;
			}
		}

		public string COM_SID_STAIRS
		{
			get
			{
				return Resources.COM_SID_STAIRS;
			}
		}

		public string COM_SID_STANDART
		{
			get
			{
				return Resources.COM_SID_STANDART;
			}
		}

		public string COM_SID_STB
		{
			get
			{
				return Resources.COM_SID_STB;
			}
		}

		public string COM_SID_STOP_RECORDING
		{
			get
			{
				return Resources.COM_SID_STOP_RECORDING;
			}
		}

		public string COM_SID_THIS_CHANNEL_LOCKED_MSG
		{
			get
			{
				return Resources.COM_SID_THIS_CHANNEL_LOCKED_MSG;
			}
		}

		public string COM_SID_TIME
		{
			get
			{
				return Resources.COM_SID_TIME;
			}
		}

		public string COM_SID_TITLE
		{
			get
			{
				return Resources.COM_SID_TITLE;
			}
		}

		public string COM_SID_TOOLS
		{
			get
			{
				return Resources.COM_SID_TOOLS;
			}
		}

		public string COM_SID_TOOLS_UPPER
		{
			get
			{
				return Resources.COM_SID_TOOLS_UPPER;
			}
		}

		public string COM_SID_TRANSLUCENT
		{
			get
			{
				return Resources.COM_SID_TRANSLUCENT;
			}
		}

		public string COM_SID_TRANSPARENT
		{
			get
			{
				return Resources.COM_SID_TRANSPARENT;
			}
		}

		public string COM_SID_TV
		{
			get
			{
				return Resources.COM_SID_TV;
			}
		}

		public string COM_SID_VIDEO
		{
			get
			{
				return Resources.COM_SID_VIDEO;
			}
		}

		public string COM_SID_VIDEOS
		{
			get
			{
				return Resources.COM_SID_VIDEOS;
			}
		}

		public string COM_SID_WIRED
		{
			get
			{
				return Resources.COM_SID_WIRED;
			}
		}

		public string COM_SID_WIRELESS
		{
			get
			{
				return Resources.COM_SID_WIRELESS;
			}
		}

		public string COM_SID_YELLOW
		{
			get
			{
				return Resources.COM_SID_YELLOW;
			}
		}

		public string COM_SID_YES
		{
			get
			{
				return Resources.COM_SID_YES;
			}
		}

		public string COM_STR_BDVIDEO
		{
			get
			{
				return Resources.COM_STR_BDVIDEO;
			}
		}

		public string COM_STR_DISC_MENU
		{
			get
			{
				return Resources.COM_STR_DISC_MENU;
			}
		}

		public string COM_STR_DISC_MENU_UPPER
		{
			get
			{
				return Resources.COM_STR_DISC_MENU_UPPER;
			}
		}

		public string COM_STR_KEYPAD_FLS_CAPITAL_2
		{
			get
			{
				return Resources.COM_STR_KEYPAD_FLS_CAPITAL_2;
			}
		}

		public string COM_TERMS_N_CONDITIONS
		{
			get
			{
				return Resources.COM_TERMS_N_CONDITIONS;
			}
		}

		public string COM_THIS_CHANNEL_IS_SCRAMBLED
		{
			get
			{
				return Resources.COM_THIS_CHANNEL_IS_SCRAMBLED;
			}
		}

		public string COM_TV_BASIC
		{
			get
			{
				return Resources.COM_TV_BASIC;
			}
		}

		public string COM_TV_DO_NOT_SHOW_AGAIN
		{
			get
			{
				return Resources.COM_TV_DO_NOT_SHOW_AGAIN;
			}
		}

		public string COM_TV_SID_3D_IS_NOT_SUPPORTED
		{
			get
			{
				return Resources.COM_TV_SID_3D_IS_NOT_SUPPORTED;
			}
		}

		public string COM_TV_SID_AUDIO_DESCRIPTION_ON_START_MDISPLAY_MSG
		{
			get
			{
				return Resources.COM_TV_SID_AUDIO_DESCRIPTION_ON_START_MDISPLAY_MSG;
			}
		}

		public string COM_TV_SID_CANCEL
		{
			get
			{
				return Resources.COM_TV_SID_CANCEL;
			}
		}

		public string COM_TV_SID_CH_CAPITAL
		{
			get
			{
				return Resources.COM_TV_SID_CH_CAPITAL;
			}
		}

		public string COM_TV_SID_CH_LIST_LOWER
		{
			get
			{
				return Resources.COM_TV_SID_CH_LIST_LOWER;
			}
		}

		public string COM_TV_SID_CH_LIST_UPPER
		{
			get
			{
				return Resources.COM_TV_SID_CH_LIST_UPPER;
			}
		}

		public string COM_TV_SID_CLOSING_MULTI_DISPLAY_TV_MSG_KR_NETWORK
		{
			get
			{
				return Resources.COM_TV_SID_CLOSING_MULTI_DISPLAY_TV_MSG_KR_NETWORK;
			}
		}

		public string COM_TV_SID_CMD_POWER_OFF
		{
			get
			{
				return Resources.COM_TV_SID_CMD_POWER_OFF;
			}
		}

		public string COM_TV_SID_CONNECTING_DOT_ABBR_15
		{
			get
			{
				return Resources.COM_TV_SID_CONNECTING_DOT_ABBR_15;
			}
		}

		public string COM_TV_SID_DATA_SERVICE_CHANNEL
		{
			get
			{
				return Resources.COM_TV_SID_DATA_SERVICE_CHANNEL;
			}
		}

		public string COM_TV_SID_DEFAULT
		{
			get
			{
				return Resources.COM_TV_SID_DEFAULT;
			}
		}

		public string COM_TV_SID_DEPRESSED
		{
			get
			{
				return Resources.COM_TV_SID_DEPRESSED;
			}
		}

		public string COM_TV_SID_DEVICE
		{
			get
			{
				return Resources.COM_TV_SID_DEVICE;
			}
		}

		public string COM_TV_SID_DROP_SHADOW
		{
			get
			{
				return Resources.COM_TV_SID_DROP_SHADOW;
			}
		}

		public string COM_TV_SID_EDGE_COLOR
		{
			get
			{
				return Resources.COM_TV_SID_EDGE_COLOR;
			}
		}

		public string COM_TV_SID_EDGE_TYPE
		{
			get
			{
				return Resources.COM_TV_SID_EDGE_TYPE;
			}
		}

		public string COM_TV_SID_GAMES
		{
			get
			{
				return Resources.COM_TV_SID_GAMES;
			}
		}

		public string COM_TV_SID_HTS
		{
			get
			{
				return Resources.COM_TV_SID_HTS;
			}
		}

		public string COM_TV_SID_KEYPAD
		{
			get
			{
				return Resources.COM_TV_SID_KEYPAD;
			}
		}

		public string COM_TV_SID_KEYPAD_KR_BLANK
		{
			get
			{
				return Resources.COM_TV_SID_KEYPAD_KR_BLANK;
			}
		}

		public string COM_TV_SID_NO_EDGE
		{
			get
			{
				return Resources.COM_TV_SID_NO_EDGE;
			}
		}

		public string COM_TV_SID_OK
		{
			get
			{
				return Resources.COM_TV_SID_OK;
			}
		}

		public string COM_TV_SID_ON_NOW
		{
			get
			{
				return Resources.COM_TV_SID_ON_NOW;
			}
		}

		public string COM_TV_SID_PLEASE_TRY_AGAIN
		{
			get
			{
				return Resources.COM_TV_SID_PLEASE_TRY_AGAIN;
			}
		}

		public string COM_TV_SID_RAISED
		{
			get
			{
				return Resources.COM_TV_SID_RAISED;
			}
		}

		public string COM_TV_SID_RECORDING_STOPPED
		{
			get
			{
				return Resources.COM_TV_SID_RECORDING_STOPPED;
			}
		}

		public string COM_TV_SID_REFRESH
		{
			get
			{
				return Resources.COM_TV_SID_REFRESH;
			}
		}

		public string COM_TV_SID_SMART_HUB_LOWER
		{
			get
			{
				return Resources.COM_TV_SID_SMART_HUB_LOWER;
			}
		}

		public string COM_TV_SID_SMART_HUB_UPPER
		{
			get
			{
				return Resources.COM_TV_SID_SMART_HUB_UPPER;
			}
		}

		public string COM_TV_SID_TEXT_INPUT
		{
			get
			{
				return Resources.COM_TV_SID_TEXT_INPUT;
			}
		}

		public string COM_TV_SID_THIS_SERVICE_IS_NOT_AVAILABLE
		{
			get
			{
				return Resources.COM_TV_SID_THIS_SERVICE_IS_NOT_AVAILABLE;
			}
		}

		public string COM_TV_SID_TRY_AGAIN
		{
			get
			{
				return Resources.COM_TV_SID_TRY_AGAIN;
			}
		}

		public string COM_TV_SID_UNIFORM
		{
			get
			{
				return Resources.COM_TV_SID_UNIFORM;
			}
		}

		public string COM_TV_SID_VOL_CAPITAL
		{
			get
			{
				return Resources.COM_TV_SID_VOL_CAPITAL;
			}
		}

		public string COM_UNIVERSAL_REMOTE_SETUP
		{
			get
			{
				return Resources.COM_UNIVERSAL_REMOTE_SETUP;
			}
		}

		public string COM_WHITE
		{
			get
			{
				return Resources.COM_WHITE;
			}
		}

		public string COM_WI_FI
		{
			get
			{
				return Resources.COM_WI_FI;
			}
		}

		public string ConnectToTv
		{
			get
			{
				return Resources.ConnectToTv;
			}
		}

		public string contentdirectory
		{
			get
			{
				return Resources.contentdirectory;
			}
		}

		public string Copyright
		{
			get
			{
				return Resources.Copyright;
			}
		}

		public CultureInfo Culture
		{
			get
			{
				return Resources.Culture;
			}
			set
			{
				Resources.Culture = value;
				PropertyInfo[] properties = this.GetType().GetProperties();
				for (int i = 0; i < (int)properties.Length; i++)
				{
					this.OnPropertyChanged(properties[i].Name);
				}
			}
		}

		public string description
		{
			get
			{
				return Resources.description;
			}
		}

		public string error
		{
			get
			{
				return Resources.error;
			}
		}

		public static ResourcesModel Instanse
		{
			get
			{
				if (ResourcesModel.resourcesModel == null)
				{
					ResourcesModel.resourcesModel = new ResourcesModel();
				}
				return ResourcesModel.resourcesModel;
			}
		}

		public string MAP_SID_ACCES_YOUR_SAVED_ON_TV
		{
			get
			{
				return Resources.MAP_SID_ACCES_YOUR_SAVED_ON_TV;
			}
		}

		public string MAP_SID_WINDOWS_PC_APPLICATION
		{
			get
			{
				return Resources.MAP_SID_WINDOWS_PC_APPLICATION;
			}
		}

		public string MAPP_ADDED_TO_QUEUE_LIST
		{
			get
			{
				return Resources.MAPP_ADDED_TO_QUEUE_LIST;
			}
		}

		public string MAPP_SID_ACCESS_YOUR_SAVED_PHOTO_VIDEOS_MUSIC
		{
			get
			{
				return Resources.MAPP_SID_ACCESS_YOUR_SAVED_PHOTO_VIDEOS_MUSIC;
			}
		}

		public string MAPP_SID_ADD_CONTENT_TO_A_TRANSFER_QUEUE_SIMPLY
		{
			get
			{
				return Resources.MAPP_SID_ADD_CONTENT_TO_A_TRANSFER_QUEUE_SIMPLY;
			}
		}

		public string MAPP_SID_ADD_TO_QUEUE
		{
			get
			{
				return Resources.MAPP_SID_ADD_TO_QUEUE;
			}
		}

		public string MAPP_SID_ADDED_TO_QUEUE
		{
			get
			{
				return Resources.MAPP_SID_ADDED_TO_QUEUE;
			}
		}

		public string MAPP_SID_ADDING_CONTENT_CANCELED
		{
			get
			{
				return Resources.MAPP_SID_ADDING_CONTENT_CANCELED;
			}
		}

		public string MAPP_SID_ADDING_FILES
		{
			get
			{
				return Resources.MAPP_SID_ADDING_FILES;
			}
		}

		public string MAPP_SID_ADDITIONAL_OPTIONS
		{
			get
			{
				return Resources.MAPP_SID_ADDITIONAL_OPTIONS;
			}
		}

		public string MAPP_SID_ANOTHER_DEVICE_CURRENTLY_PAIR_TV
		{
			get
			{
				return Resources.MAPP_SID_ANOTHER_DEVICE_CURRENTLY_PAIR_TV;
			}
		}

		public string MAPP_SID_APP_REQUIRES_ACTIVE_WIFI_CONNTECTION_NOW
		{
			get
			{
				return Resources.MAPP_SID_APP_REQUIRES_ACTIVE_WIFI_CONNTECTION_NOW;
			}
		}

		public string MAPP_SID_AUDIO_DESCRIPTION_ACTIVE
		{
			get
			{
				return Resources.MAPP_SID_AUDIO_DESCRIPTION_ACTIVE;
			}
		}

		public string MAPP_SID_AUDIO_ONLY
		{
			get
			{
				return Resources.MAPP_SID_AUDIO_ONLY;
			}
		}

		public string MAPP_SID_CANNOT_WATCH_TV_MOBILE_DEVICE_CURRENT_TV_MODE
		{
			get
			{
				return Resources.MAPP_SID_CANNOT_WATCH_TV_MOBILE_DEVICE_CURRENT_TV_MODE;
			}
		}

		public string MAPP_SID_CAPTION_BACKGROUND
		{
			get
			{
				return Resources.MAPP_SID_CAPTION_BACKGROUND;
			}
		}

		public string MAPP_SID_CAPTION_BACKGROUND_COLOR
		{
			get
			{
				return Resources.MAPP_SID_CAPTION_BACKGROUND_COLOR;
			}
		}

		public string MAPP_SID_CAPTION_BACKGROUND_OPACITY
		{
			get
			{
				return Resources.MAPP_SID_CAPTION_BACKGROUND_OPACITY;
			}
		}

		public string MAPP_SID_CAPTION_WINDOW
		{
			get
			{
				return Resources.MAPP_SID_CAPTION_WINDOW;
			}
		}

		public string MAPP_SID_CAPTION_WINDOW_COLOR
		{
			get
			{
				return Resources.MAPP_SID_CAPTION_WINDOW_COLOR;
			}
		}

		public string MAPP_SID_CAPTION_WINDOW_OPACITY
		{
			get
			{
				return Resources.MAPP_SID_CAPTION_WINDOW_OPACITY;
			}
		}

		public string MAPP_SID_CC_OFF
		{
			get
			{
				return Resources.MAPP_SID_CC_OFF;
			}
		}

		public string MAPP_SID_CC_ON
		{
			get
			{
				return Resources.MAPP_SID_CC_ON;
			}
		}

		public string MAPP_SID_CHANGE_CHANNLE_PC_CONTINUTE
		{
			get
			{
				return Resources.MAPP_SID_CHANGE_CHANNLE_PC_CONTINUTE;
			}
		}

		public string MAPP_SID_CHANGE_TV
		{
			get
			{
				return Resources.MAPP_SID_CHANGE_TV;
			}
		}

		public string MAPP_SID_CHARACTER_COLOR
		{
			get
			{
				return Resources.MAPP_SID_CHARACTER_COLOR;
			}
		}

		public string MAPP_SID_CHARACTER_SIZE
		{
			get
			{
				return Resources.MAPP_SID_CHARACTER_SIZE;
			}
		}

		public string MAPP_SID_CHARACTERS
		{
			get
			{
				return Resources.MAPP_SID_CHARACTERS;
			}
		}

		public string MAPP_SID_CHARACTTER_OPACITY
		{
			get
			{
				return Resources.MAPP_SID_CHARACTTER_OPACITY;
			}
		}

		public string MAPP_SID_CHECK_CABLE
		{
			get
			{
				return Resources.MAPP_SID_CHECK_CABLE;
			}
		}

		public string MAPP_SID_CHECK_NETWORK_CONNECTION_BOTH_TV_PC
		{
			get
			{
				return Resources.MAPP_SID_CHECK_NETWORK_CONNECTION_BOTH_TV_PC;
			}
		}

		public string MAPP_SID_CHECK_PIN_TV_AND_TENTER_BELOW
		{
			get
			{
				return Resources.MAPP_SID_CHECK_PIN_TV_AND_TENTER_BELOW;
			}
		}

		public string MAPP_SID_CHOOSE_TV_FROM_LIST_ABOCE_TO_GET_STARTED
		{
			get
			{
				return Resources.MAPP_SID_CHOOSE_TV_FROM_LIST_ABOCE_TO_GET_STARTED;
			}
		}

		public string MAPP_SID_CLICK_PLUS_BUTTON_ADD_CONTENT
		{
			get
			{
				return Resources.MAPP_SID_CLICK_PLUS_BUTTON_ADD_CONTENT;
			}
		}

		public string MAPP_SID_CLOSE_CAPTION_SETTINGS
		{
			get
			{
				return Resources.MAPP_SID_CLOSE_CAPTION_SETTINGS;
			}
		}

		public string MAPP_SID_CLOSED_CAPTION
		{
			get
			{
				return Resources.MAPP_SID_CLOSED_CAPTION;
			}
		}

		public string MAPP_SID_CLOSED_CAPTION_MODE
		{
			get
			{
				return Resources.MAPP_SID_CLOSED_CAPTION_MODE;
			}
		}

		public string MAPP_SID_CLOSED_CAPTION_OPTIONS_RESET
		{
			get
			{
				return Resources.MAPP_SID_CLOSED_CAPTION_OPTIONS_RESET;
			}
		}

		public string MAPP_SID_CLOSED_CAPTIONS
		{
			get
			{
				return Resources.MAPP_SID_CLOSED_CAPTIONS;
			}
		}

		public string MAPP_SID_CONNECT_TO_TV
		{
			get
			{
				return Resources.MAPP_SID_CONNECT_TO_TV;
			}
		}

		public string MAPP_SID_CONNECTING_TV_RETURN_PREVIOUS_PAGE
		{
			get
			{
				return Resources.MAPP_SID_CONNECTING_TV_RETURN_PREVIOUS_PAGE;
			}
		}

		public string MAPP_SID_CONNECTION_LOST
		{
			get
			{
				return Resources.MAPP_SID_CONNECTION_LOST;
			}
		}

		public string MAPP_SID_CONNECTION_WITH_DEVICE_LOST
		{
			get
			{
				return Resources.MAPP_SID_CONNECTION_WITH_DEVICE_LOST;
			}
		}

		public string MAPP_SID_CONTENT_ALREADY_EXISTS_IN_LIBRARY
		{
			get
			{
				return Resources.MAPP_SID_CONTENT_ALREADY_EXISTS_IN_LIBRARY;
			}
		}

		public string MAPP_SID_CONTENT_IS_ALREADY_SELECTED
		{
			get
			{
				return Resources.MAPP_SID_CONTENT_IS_ALREADY_SELECTED;
			}
		}

		public string MAPP_SID_CONTENT_SENT_BECAUSE_FORMAT_NOT
		{
			get
			{
				return Resources.MAPP_SID_CONTENT_SENT_BECAUSE_FORMAT_NOT;
			}
		}

		public string MAPP_SID_COULD_NOT_CONNECT_TO_MEDIA_SHARE
		{
			get
			{
				return Resources.MAPP_SID_COULD_NOT_CONNECT_TO_MEDIA_SHARE;
			}
		}

		public string MAPP_SID_DEVICE_CONNECTION_INFO
		{
			get
			{
				return Resources.MAPP_SID_DEVICE_CONNECTION_INFO;
			}
		}

		public string MAPP_SID_DON_T_SEE_TV_CHECK_FOLLOWING
		{
			get
			{
				return Resources.MAPP_SID_DON_T_SEE_TV_CHECK_FOLLOWING;
			}
		}

		public string MAPP_SID_DRAG_CONTENT_HERE
		{
			get
			{
				return Resources.MAPP_SID_DRAG_CONTENT_HERE;
			}
		}

		public string MAPP_SID_EDGE
		{
			get
			{
				return Resources.MAPP_SID_EDGE;
			}
		}

		public string MAPP_SID_ENJOY_MULTIMEDIA_IN_MOBILE_ON_TV
		{
			get
			{
				return Resources.MAPP_SID_ENJOY_MULTIMEDIA_IN_MOBILE_ON_TV;
			}
		}

		public string MAPP_SID_ENJOY_MULTIMEDIA_MUSIC_PC_TV
		{
			get
			{
				return Resources.MAPP_SID_ENJOY_MULTIMEDIA_MUSIC_PC_TV;
			}
		}

		public string MAPP_SID_ENTERED_INCORRECT_PIN
		{
			get
			{
				return Resources.MAPP_SID_ENTERED_INCORRECT_PIN;
			}
		}

		public string MAPP_SID_ENTERED_INCORRECT_PIN_CHECK_PIN_AGAIN
		{
			get
			{
				return Resources.MAPP_SID_ENTERED_INCORRECT_PIN_CHECK_PIN_AGAIN;
			}
		}

		public string MAPP_SID_ENTERED_INCORRECT_PIN_TRY_AGAIN
		{
			get
			{
				return Resources.MAPP_SID_ENTERED_INCORRECT_PIN_TRY_AGAIN;
			}
		}

		public string MAPP_SID_ERROR_OCCURED_WHILE_CONNECTING_TO_TV
		{
			get
			{
				return Resources.MAPP_SID_ERROR_OCCURED_WHILE_CONNECTING_TO_TV;
			}
		}

		public string MAPP_SID_EXIT_FULL_SCREE
		{
			get
			{
				return Resources.MAPP_SID_EXIT_FULL_SCREE;
			}
		}

		public string MAPP_SID_FILES_SUCCESSFULLY_ADDED_TO_LIBRARY
		{
			get
			{
				return Resources.MAPP_SID_FILES_SUCCESSFULLY_ADDED_TO_LIBRARY;
			}
		}

		public string MAPP_SID_GET_STARTED_TURN_ON_TV_SELECT_BUTTON
		{
			get
			{
				return Resources.MAPP_SID_GET_STARTED_TURN_ON_TV_SELECT_BUTTON;
			}
		}

		public string MAPP_SID_GO_TO_APP_STORE
		{
			get
			{
				return Resources.MAPP_SID_GO_TO_APP_STORE;
			}
		}

		public string MAPP_SID_IN_CURRENT_TV_MODE_CLONE_VIEW
		{
			get
			{
				return Resources.MAPP_SID_IN_CURRENT_TV_MODE_CLONE_VIEW;
			}
		}

		public string MAPP_SID_INSTALLATION
		{
			get
			{
				return Resources.MAPP_SID_INSTALLATION;
			}
		}

		public string MAPP_SID_LOW_PRIORITY
		{
			get
			{
				return Resources.MAPP_SID_LOW_PRIORITY;
			}
		}

		public string MAPP_SID_MAKE_SURE_TV_TURNED_ON
		{
			get
			{
				return Resources.MAPP_SID_MAKE_SURE_TV_TURNED_ON;
			}
		}

		public string MAPP_SID_MIX_CONNECT_NETWORK
		{
			get
			{
				return Resources.MAPP_SID_MIX_CONNECT_NETWORK;
			}
		}

		public string MAPP_SID_NEW_VERSION_SAMRT_VIEW_AVAILABLE_DOWNLOAD
		{
			get
			{
				return Resources.MAPP_SID_NEW_VERSION_SAMRT_VIEW_AVAILABLE_DOWNLOAD;
			}
		}

		public string MAPP_SID_NO_STORE_CHANNELS_RUN_AUTO_PROGRAM
		{
			get
			{
				return Resources.MAPP_SID_NO_STORE_CHANNELS_RUN_AUTO_PROGRAM;
			}
		}

		public string MAPP_SID_NO_WIFI_CONNECTION
		{
			get
			{
				return Resources.MAPP_SID_NO_WIFI_CONNECTION;
			}
		}

		public string MAPP_SID_NOT_AVAILABLE_INPUT_SOURCE
		{
			get
			{
				return Resources.MAPP_SID_NOT_AVAILABLE_INPUT_SOURCE;
			}
		}

		public string MAPP_SID_NOT_AVAILABLE_TV_RECORIDNG
		{
			get
			{
				return Resources.MAPP_SID_NOT_AVAILABLE_TV_RECORIDNG;
			}
		}

		public string MAPP_SID_NOT_CONNECTED
		{
			get
			{
				return Resources.MAPP_SID_NOT_CONNECTED;
			}
		}

		public string MAPP_SID_NOT_LISTED_TURN_ON_TV_SAME_NETWORK
		{
			get
			{
				return Resources.MAPP_SID_NOT_LISTED_TURN_ON_TV_SAME_NETWORK;
			}
		}

		public string MAPP_SID_NOT_SET_UP_UNIVERSAL_REMOTE_YET_CONTROL_TV
		{
			get
			{
				return Resources.MAPP_SID_NOT_SET_UP_UNIVERSAL_REMOTE_YET_CONTROL_TV;
			}
		}

		public string MAPP_SID_NOT_SUPORTED_YET
		{
			get
			{
				return Resources.MAPP_SID_NOT_SUPORTED_YET;
			}
		}

		public string MAPP_SID_OTHER_MODE
		{
			get
			{
				return Resources.MAPP_SID_OTHER_MODE;
			}
		}

		public string MAPP_SID_PALY_TV_GAMES_MOBILE
		{
			get
			{
				return Resources.MAPP_SID_PALY_TV_GAMES_MOBILE;
			}
		}

		public string MAPP_SID_PARENTAL_LOCK
		{
			get
			{
				return Resources.MAPP_SID_PARENTAL_LOCK;
			}
		}

		public string MAPP_SID_PIN_ENTERD_SHORT_TRY
		{
			get
			{
				return Resources.MAPP_SID_PIN_ENTERD_SHORT_TRY;
			}
		}

		public string MAPP_SID_PIN_ENTRY_TIME_LIMIT_EXPIRED
		{
			get
			{
				return Resources.MAPP_SID_PIN_ENTRY_TIME_LIMIT_EXPIRED;
			}
		}

		public string MAPP_SID_PIN_ERROR
		{
			get
			{
				return Resources.MAPP_SID_PIN_ERROR;
			}
		}

		public string MAPP_SID_PLAY_ON_TV
		{
			get
			{
				return Resources.MAPP_SID_PLAY_ON_TV;
			}
		}

		public string MAPP_SID_PLEASE_SMART_VIEW_DOWNLOAD
		{
			get
			{
				return Resources.MAPP_SID_PLEASE_SMART_VIEW_DOWNLOAD;
			}
		}

		public string MAPP_SID_READY_FOR_CONNECTION
		{
			get
			{
				return Resources.MAPP_SID_READY_FOR_CONNECTION;
			}
		}

		public string MAPP_SID_REMIND_ME_LATER
		{
			get
			{
				return Resources.MAPP_SID_REMIND_ME_LATER;
			}
		}

		public string MAPP_SID_REMOVE_FILE_HEADER
		{
			get
			{
				return Resources.MAPP_SID_REMOVE_FILE_HEADER;
			}
		}

		public string MAPP_SID_REMOVE_FILE_MESSAGE
		{
			get
			{
				return Resources.MAPP_SID_REMOVE_FILE_MESSAGE;
			}
		}

		public string MAPP_SID_REMOVE_FILES
		{
			get
			{
				return Resources.MAPP_SID_REMOVE_FILES;
			}
		}

		public string MAPP_SID_REMOVE_FOLDER
		{
			get
			{
				return Resources.MAPP_SID_REMOVE_FOLDER;
			}
		}

		public string MAPP_SID_REMOVE_FOLDER_HEADER
		{
			get
			{
				return Resources.MAPP_SID_REMOVE_FOLDER_HEADER;
			}
		}

		public string MAPP_SID_RESET_ALL_CLOSED_CAPTION_OPTIONS_DEFAULT
		{
			get
			{
				return Resources.MAPP_SID_RESET_ALL_CLOSED_CAPTION_OPTIONS_DEFAULT;
			}
		}

		public string MAPP_SID_REVIEW_AGREE_TERMS_CONDITIONS_SMART_VIEW
		{
			get
			{
				return Resources.MAPP_SID_REVIEW_AGREE_TERMS_CONDITIONS_SMART_VIEW;
			}
		}

		public string MAPP_SID_SAMSUNG_SMART_VIEW_2_0
		{
			get
			{
				return Resources.MAPP_SID_SAMSUNG_SMART_VIEW_2_0;
			}
		}

		public string MAPP_SID_SCRAMLED_CHANNEL
		{
			get
			{
				return Resources.MAPP_SID_SCRAMLED_CHANNEL;
			}
		}

		public string MAPP_SID_SEE_TWO_SETS_CAPTION_SCREEN_TURN_OFF
		{
			get
			{
				return Resources.MAPP_SID_SEE_TWO_SETS_CAPTION_SCREEN_TURN_OFF;
			}
		}

		public string MAPP_SID_SEND_CONTENT_TV_DRAG_QUEUE_DISPLAY
		{
			get
			{
				return Resources.MAPP_SID_SEND_CONTENT_TV_DRAG_QUEUE_DISPLAY;
			}
		}

		public string MAPP_SID_SEND_TO_PC
		{
			get
			{
				return Resources.MAPP_SID_SEND_TO_PC;
			}
		}

		public string MAPP_SID_SEND_TO_TV
		{
			get
			{
				return Resources.MAPP_SID_SEND_TO_TV;
			}
		}

		public string MAPP_SID_SMART_VIEW_2_0
		{
			get
			{
				return Resources.MAPP_SID_SMART_VIEW_2_0;
			}
		}

		public string MAPP_SID_SMART_VIEW_NOT_SUPPORT_YOUR_TV
		{
			get
			{
				return Resources.MAPP_SID_SMART_VIEW_NOT_SUPPORT_YOUR_TV;
			}
		}

		public string MAPP_SID_SMARTVIEW_ONLY_2014_RANGE_MSG
		{
			get
			{
				return Resources.MAPP_SID_SMARTVIEW_ONLY_2014_RANGE_MSG;
			}
		}

		public string MAPP_SID_SOMETHING_WENT_WRONG
		{
			get
			{
				return Resources.MAPP_SID_SOMETHING_WENT_WRONG;
			}
		}

		public string MAPP_SID_SOUCRE_NOT_CONNECTED
		{
			get
			{
				return Resources.MAPP_SID_SOUCRE_NOT_CONNECTED;
			}
		}

		public string MAPP_SID_SOURCE_CONFLICT
		{
			get
			{
				return Resources.MAPP_SID_SOURCE_CONFLICT;
			}
		}

		public string MAPP_SID_STB_MENU
		{
			get
			{
				return Resources.MAPP_SID_STB_MENU;
			}
		}

		public string MAPP_SID_SUBTITLE
		{
			get
			{
				return Resources.MAPP_SID_SUBTITLE;
			}
		}

		public string MAPP_SID_SUBTITLE_UPPER
		{
			get
			{
				return Resources.MAPP_SID_SUBTITLE_UPPER;
			}
		}

		public string MAPP_SID_SURE_WANT_TO_REMOVE_SELECTED_FOLDERS
		{
			get
			{
				return Resources.MAPP_SID_SURE_WANT_TO_REMOVE_SELECTED_FOLDERS;
			}
		}

		public string MAPP_SID_TAKE_CONTROL_GAMES_INSTALLED_TV
		{
			get
			{
				return Resources.MAPP_SID_TAKE_CONTROL_GAMES_INSTALLED_TV;
			}
		}

		public string MAPP_SID_THERE_IS_NO_CONTENT_TO_PLAY
		{
			get
			{
				return Resources.MAPP_SID_THERE_IS_NO_CONTENT_TO_PLAY;
			}
		}

		public string MAPP_SID_THIS_CONTENT_IS_ALREADY_SELECTED
		{
			get
			{
				return Resources.MAPP_SID_THIS_CONTENT_IS_ALREADY_SELECTED;
			}
		}

		public string MAPP_SID_TRY_AGAIN_CLICKING_REFRESH_NOT_SUPPORTED
		{
			get
			{
				return Properties.Resources.MAPP_SID_TRY_AGAIN_CLICKING_REFRESH_NOT_SUPPORTED;
			}
		}

		public string MAPP_SID_TURN_OFF_TV_APP_ALSO_CLOSE
		{
			get
			{
				return Resources.MAPP_SID_TURN_OFF_TV_APP_ALSO_CLOSE;
			}
		}

		public string MAPP_SID_TV_CHANNEL_CHANGE
		{
			get
			{
				return Resources.MAPP_SID_TV_CHANNEL_CHANGE;
			}
		}

		public string MAPP_SID_TV_CURRENT_STATUS_PREVENTS_SCREEN_SHARING_WINDOW
		{
			get
			{
				return Resources.MAPP_SID_TV_CURRENT_STATUS_PREVENTS_SCREEN_SHARING_WINDOW;
			}
		}

		public string MAPP_SID_TV_CURRENTLY_RECORDING_BEFORE_CHANGE_SOURCE
		{
			get
			{
				return Resources.MAPP_SID_TV_CURRENTLY_RECORDING_BEFORE_CHANGE_SOURCE;
			}
		}

		public string MAPP_SID_TV_IS_NOT_COMPLETELY_LOADED
		{
			get
			{
				return Resources.MAPP_SID_TV_IS_NOT_COMPLETELY_LOADED;
			}
		}

		public string MAPP_SID_TV_NO_LONGER_AVAILABLE_ANDROID_DEVICE
		{
			get
			{
				return Resources.MAPP_SID_TV_NO_LONGER_AVAILABLE_ANDROID_DEVICE;
			}
		}

		public string MAPP_SID_TV_NOT_READY_CONNECTION_TRY_AGAIN
		{
			get
			{
				return Resources.MAPP_SID_TV_NOT_READY_CONNECTION_TRY_AGAIN;
			}
		}

		public string MAPP_SID_UNABLE_CHANGE_SOURCE_NOT_CONNECTED
		{
			get
			{
				return Resources.MAPP_SID_UNABLE_CHANGE_SOURCE_NOT_CONNECTED;
			}
		}

		public string MAPP_SID_UNABLE_CHANGE_SOURCE_RESTRICTION
		{
			get
			{
				return Resources.MAPP_SID_UNABLE_CHANGE_SOURCE_RESTRICTION;
			}
		}

		public string MAPP_SID_UNABLE_CONNECT_TV
		{
			get
			{
				return Resources.MAPP_SID_UNABLE_CONNECT_TV;
			}
		}

		public string MAPP_SID_UNABLE_SEND_TV_TRY
		{
			get
			{
				return Resources.MAPP_SID_UNABLE_SEND_TV_TRY;
			}
		}

		public string MAPP_SID_UNABLE_SHOW_TV_STATUS_CHANGED
		{
			get
			{
				return Resources.MAPP_SID_UNABLE_SHOW_TV_STATUS_CHANGED;
			}
		}

		public string MAPP_SID_UNIVERSAL_REMOTE_NOT_SET_UP
		{
			get
			{
				return Resources.MAPP_SID_UNIVERSAL_REMOTE_NOT_SET_UP;
			}
		}

		public string MAPP_SID_USE_KEYBOARD_BELOW_ENTER_APPEAR
		{
			get
			{
				return Resources.MAPP_SID_USE_KEYBOARD_BELOW_ENTER_APPEAR;
			}
		}

		public string MAPP_SID_USE_KEYBOARD_TO_ENTER_TEXT_TV
		{
			get
			{
				return Resources.MAPP_SID_USE_KEYBOARD_TO_ENTER_TEXT_TV;
			}
		}

		public string MAPP_SID_USE_MOBILE_DEVICE_REMOTE_CONTROL_SECOND
		{
			get
			{
				return Resources.MAPP_SID_USE_MOBILE_DEVICE_REMOTE_CONTROL_SECOND;
			}
		}

		public string MAPP_SID_USE_MOBILE_DEVICE_SECOND_TV
		{
			get
			{
				return Resources.MAPP_SID_USE_MOBILE_DEVICE_SECOND_TV;
			}
		}

		public string MAPP_SID_USE_PC_REMOTE_SECOND_TV
		{
			get
			{
				return Resources.MAPP_SID_USE_PC_REMOTE_SECOND_TV;
			}
		}

		public string MAPP_SID_YOU_CAN_NOT_WATCH_2ND_TV_ANYMORE
		{
			get
			{
				return Resources.MAPP_SID_YOU_CAN_NOT_WATCH_2ND_TV_ANYMORE;
			}
		}

		public string MAPP_SID_YOU_HAVE_THE_LATEST_VERION
		{
			get
			{
				return Resources.MAPP_SID_YOU_HAVE_THE_LATEST_VERION;
			}
		}

		public string OpenSourceLicense
		{
			get
			{
				return Resources.OpenSourceLicense;
			}
		}

		public string UpdateAvailableHeader
		{
			get
			{
				return Resources.UpdateAvailableHeader;
			}
		}

		public string UpdateAvailableText
		{
			get
			{
				return Resources.UpdateAvailableText;
			}
		}

		public string x_featurelist
		{
			get
			{
				return Resources.x_featurelist;
			}
		}

		private ResourcesModel()
		{
			this.Culture = CultureInfo.CreateSpecificCulture("en-US");
		}

		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
			if (propertyChangedEventHandler != null)
			{
				propertyChangedEventHandler(this, new PropertyChangedEventArgs(name));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}