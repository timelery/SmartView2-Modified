   M S B A M L   `   `   `  ����E  ASmartView2, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:SmartView2.ControlsSmartView2.Controls  5      0    *SmartView2.Views.Popups.QueueSettingsPopup   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35K GUIFoundation, Version=1.1.0.22849, Culture=neutral, PublicKeyToken=nullW SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6controls!clr-namespace:SmartView2.Controls     5      $��Stretch=�5      $��Stretch=�6,   $	��714��6H   $	��448��6T   .��5      ��    HeaderBackgroundBrush#��[  6   $��5q�6@   #��[  5	      ._�5
      �   LightBackgroundBrush#��[ 6     � RowDefinitions  5      �� 5   
   $
��Auto �6   �� 5   
   $��* �6   �� 5   
   $
��Auto �6   .+�5      � $��0��6   #��[  6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��5,0,0,0q�6$     WindowHeaderStyle#��[ 65   ��5      �� *COM_SID_SLIDESHOW_SETTINGS+  LocalizationSource �� Source# [ �� 5   
   $��1��6    �� IsCancel . $��Right=�61     HeaderCloseButton#��[ 5      $��0 0 0 5q�6<   ��6M   �� *CloseCommand+5   	   � 5      $��1��6   $��30q�6     5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5   
   � $��0��6     5       �� 5!      �� 5"      .+�5$      �� $
��Left=�6   $��15(�61     CommonHeaderTextBlockStyle#��[ 6?   ��5%      �� *COM_SID_SPEED+# [ � 5&      $��1��6    5'      �� 5(      �� 5)      �� 5*      .+�5,      -   'SmartView2.Controls.BindableRadioButton  $��0��6+    	� 	GroupName$ 	SpeedGoup��6;    ! QueueSettingsRadioButtonStyle#��[ 6Q      
RadioValue$ normal��5-   +   ��6?   �� *COM_SID_NORMAL+# [    RadioBinding 5.   +   �� *SettingsModel.Speed+  5/      $��1��6+   $ 	SpeedGoup��6;   #��[ 6Q   $
 slow��50   +   ��6=   �� *COM_SID_SLOW+# [  51   +   �� *SettingsModel.Speed+  52      $��2��6+   $ 	SpeedGoup��6;   #��[ 6Q   $
 fast��53   +   ��6=   �� *COM_SID_FAST+# [  54   +   �� *SettingsModel.Speed+55      56      �� 57   
   $��1��6   $��0,1,0,0q�6   
�������68   � 58   
   $��1��6     59      �� 5:      �� 5;      �� 5<      �� 5=       5?      �� 5@      �� 5A      �� 5B      .+�5D      �� $
��Left=�6   $��15(�61   $��0,15,0,0q�6?   #��[ 5E      ��5F      �� *COM_SID_EFFECTS+# [   5G      $��1��6)   $��0��66   $ 
EffectGoup��6F   #��[ 5H   )   $
 none��6`   ��5I   )   �� *COM_SID_NONE+# [  5J   )   �� *SettingsModel.Effect+  5K      $��1��6)   $��1��66   $ 
EffectGoup��6F   #��[ 5L   )   $ blind��6`   ��5M   )   �� *COM_SID_BLIND+# [  5N   )   �� *SettingsModel.Effect+  5O      $��1��6)   $��2��66   $ 
EffectGoup��6F   #��[ 5P   )   $ spiral��6`   ��5Q   )   �� *COM_SID_SPIRAL+# [  5R   )   �� *SettingsModel.Effect+  5S      $��2��6)   $��0��66   $ 
EffectGoup��6F   #��[ 5T   )   $ checker��6`   ��5U   )   �� *COM_SID_CHECKER+# [  5V   )   �� *SettingsModel.Effect+  5W      $��2��6)   $��1��66   $ 
EffectGoup��6F   #��[ 5X   )   $ linear��6`   ��5Y   )   �� *COM_SID_LINEAR+# [  5Z   )   �� *SettingsModel.Effect+  5[      $��2��6)   $��2��66   $ 
EffectGoup��6F   #��[ 5\   )   $ stairs��6`   ��5]   )   �� *COM_SID_STAIRS+# [  5^   )   �� *SettingsModel.Effect+  5_      $��3��6)   $��0��66   $ 
EffectGoup��6F   #��[ 5`   )   $ random��6`   ��5a   )   �� *COM_SID_RANDOM+# [  5b   )   �� *SettingsModel.Effect+5c      5d   	   � 5e      $��2��6     LightPopupBackgroundBrush#��[ 6    5f   
   �� 5g      �� 5h      .+�5j   
   ��   TheMostCommonButtonStyle#��[ 6   $��Right=�6C    �� 	IsDefault . $	��174��5k      $��40��6   $��
0,15,18,15q�6)   ��6=   �� *	OkCommand+��5l      �� *COM_TV_SID_OK+# [ �� 5m   
   #��[ 6   $
��Left=�6C   $��1��6^    . $	��174��5n   "   $��
18,15,0,15q�6.   $��40��6B   ��6N   �� *CloseCommand+��5o      �� *COM_TV_SID_CANCEL+# [ 5p   	   5q      5r      5s      