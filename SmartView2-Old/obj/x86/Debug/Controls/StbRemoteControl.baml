   M S B A M L   `   `   `  ����E  ASmartView2, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:SmartView2.ControlsSmartView2.Controls  5      3clr-namespace:SmartView2.ViewsSmartView2.Views  5      *    $SmartView2.Controls.StbRemoteControl   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35K GUIFoundation, Version=1.1.0.22849, Culture=neutral, PublicKeyToken=nullW SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6controls!clr-namespace:SmartView2.Controls     5      0viewsclr-namespace:SmartView2.Views     5      $	��945��5      $	��570��6   .��5	      �   � ColumnDefinitions  5
      �� 5      $
��Auto �6   �� 5      $��40 �6   �� 5      $
��Auto �6   �� 5      $��40 �6   �� 5      $
��Auto �6   .+�5      � $��0��6    � RowDefinitions 5      �� 5   
   $	��210 �6   �� 5   
   �� 5   
   $��3 �6   �� 5   
   $��80 �6   .+�5      .   (SmartView2.Controls.VolumeChannelControl  $��0��6&   '   !SmartView2.Controls.NumPadControl  5      $��1��6   �� 5         NumberButtonDividerStyle#��[  6   $��2��6@   %   SmartView2.Controls.PlayControl  5      $��3��6   5      � 5      $��2��6    5      �� 5   
   $	��350 �6   �� 5    
   $��* �6   �� 5!   
   $��* �6   �� 5"   
   $��3 �6   �� 5#   
   $��80 �6   .+�5%      )   #SmartView2.Controls.TouchPadControl  $��0��6!   � 5&      $��1��6     5'   
   �� 5(      �� 5)      �� 5*      .+�5,   
   ��  " RemoteControlKeypadButtonStyle#��[ 6   $��0��6I   $��Keypad��6Y   $
��NONE��5-      ��5.      �� *DataContext.ShowKeypadCommand+ �� RelativeSource ��  �� AncestorType$ views:RemoteControlPageL��� 5/   
      RemoteControlBackButtonStyle#��[ 6   $��1��6G   $
��Back��6W   $
��NONE��50      ��6)   �� *Return+�� 51   
      RemoteControlExitButtonStyle#��[ 6   $��2��6G   $
��Exit��6W   $
��NONE��52      ��6)   �� *Exit+53   	   %   SmartView2.Controls.AbcdControl  54      $��2��6   �� 55        PlayerDividerStyle#��[ 6   $��3��6:   '   !SmartView2.Controls.RewindControl  56      $��4��6   57      � 58      $��4��6   .+�59      �  5:   
   �� 5;      �� 5<      �� 5=      �� 5>      �� 5?      �� 5@      �� 5A      �� 5B      �� 5C      .+�5E   
   �� $��0��6    $  RemoteControlStandartButtonStyle#��[ 6   ��5F      �� *COM_TV_SID_SMART_HUB_UPPER+  LocalizationSource �� Source# [ ��5G      �� *
SmartHub+�� 5H   
   $��1��6     MenuButtonDividerStyle#��[ 6   �� 5I   
   $��2��6   #��[ 6   ��5J      �� *COM_SID_MENU+# [ ��5K      �� *Menu+�� 5L   
   $��3��6   #��[ 6   ��5M      �� *COM_BUTTON_SEARCH_UPPER+# [ ��5N      �� *Search+�� 5O   
   $��4��6   #��[ 6   ��5P      �� *COM_TV_SID_CH_LIST_UPPER+# [ ��5Q      �� *ChannelList+�� 5R   
   $��5��6   #��[ 6   �� 5S   
   $��6��6   #��[ 6   ��5T      �� *COM_HTS_SID_GUIDE_UPPER+# [ ��5U      �� *Guide+�� 5V   
   $��7��6   #��[ 6   ��5W      �� *COM_SID_INFO_UPPER+# [ ��5X      �� *Info+�� 5Y   
   $��8��6   #��[ 6   ��5Z      �� *MAPP_SID_SUBTITLE_UPPER+# [ ��5[      �� *	StbMenu+5\   	   5]      5^      5_      