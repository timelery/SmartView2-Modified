   M S B A M L   `   `   `  ����E  ASmartView2, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:SmartView2.ControlsSmartView2.Controls  5      =#clr-namespace:SmartView2.ConvertersSmartView2.Converters  5      5clr-namespace:SmartView2.CommonSmartView2.Common  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35K GUIFoundation, Version=1.1.0.22849, Culture=neutral, PublicKeyToken=nullW SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891    +SmartView2.Common.BoolToVisibilityConverter   _visibilityConverter9   3SmartView2.Common.MillisecondsTimeToStringConverter  _timeConverter  _controlTemplate  �� 
TargetType(   "SmartView2.Controls.MediaPlayerExt ��Name �Name � RowDefinitions � ColumnDefinitions ��Name �� RelativeSource �� 
TargetType �� Triggers	 �� Binding
 �� Value �� Property �� Value r�Name r� LoadedBehavior r� UnloadedBehavior)   #SmartView2.Controls.MediaElementExt   Volume r� Volume ��Name �� ElementName ��Name ��Name � Minimum   Duration � Maximum   Position � Value �� IsMoveToPointEnabled  SliderStyle �� 	Converter'  !System.Windows.VisualStateManager   VisualStateGroups%  System.Windows.VisualStateGroup   Name   States   System.Windows.VisualState    
Storyboard! �� TargetProperty	" W� To# g� Duration$ �� Target%  System.Windows.Markup.Reference�� 5      F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     :x,http://schemas.microsoft.com/winfx/2006/xaml     5      +local!clr-namespace:SmartView2.Controls  5      0coverter#clr-namespace:SmartView2.Converters  5      *commonclr-namespace:SmartView2.Common  5      %�  &	        &	      &	 &     7  7   '  Z    7     5      6%     5      6-   �� 5      "   6-   .R�5	      �� $ PART_LayoutHost��6   ._�5
      � $ PART_LayoutRoot��6   
������  5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � $ PART_ClickListener��6   
������ .+�5      �  5      �� 5      $��48 �6!   �� 5      $��* �6!   �� 5      $��48 �6!   .+�5      �� $ 
Part_Image��6   $��3��6(   ��5      �� *	Preview+ �� *TemplatedParent+��5      �� 5      " ��6    5      �� 5      	 6"   �� *HasDuration+ �� *TemplatedParent+	
 False6s   .a�5      ��  � y�$ Visible=�6?   5      �� 5      	 6"   �� *HasDuration+ �� *TemplatedParent+
 True6s   .a�5      ��  � y�$ Hidden=�6?   5       5"      5$      r� 5%      $ PART_MediaElement��6   $��3��66   $ Manual=�6J   $ Manual=�6b   $��3��5&      # z 6,   �� 5'      $ PART_ImageLeftContent��6   $��0��6>   ��5(      �� *ImageLeftContent+ �� *TemplatedParent+��5)      �� 5*      " ��6    5+      �� 5,      	 6"   �� *HasDuration+ �� *TemplatedParent+	
 False6s   .a�5-      ��  � y�$ Visible=�6?   5.      �� 5/      	 6"   �� *HasDuration+ �� *TemplatedParent+
 True6s   .a�50      ��  � y�$ 	Collapsed=�6?   51      53      55      �� 56      $ PART_ImageRightContent��6   $��2��6?   ��57      �� *ImageRightContent+ �� *TemplatedParent+��58      �� 59      " ��6    5:      �� 5;      	 6"   �� *HasDuration+ �� *TemplatedParent+	
 False6s   .a�5<      ��  � y�$ Visible=�6?   5=      �� 5>      	 6"   �� *HasDuration+ �� *TemplatedParent+
 True6s   .a�5?      ��  � y�$ 	Collapsed=�6?   5@      5B      5D      5E      5F      �� 5G   
   $  PART_OnVideoContentControl��6   #��z��6?   � 5H   
   $ PART_CCDataLayout��6   $��Center=�6*   $��Center=�6E   ��5I      �� *ActualHeight+$ PART_MediaElement����5J      �� *ActualWidth+$ PART_MediaElement��� 5K   
   $��Bottom=�6    5L      �� 5M      $ ProgressBarRow��6   $
��Auto �64   �� 5N      $ 	BottomBar��6   $
��Auto �6/   .+�5P      �� $ PART_ProgressBar��6   $��0��6-   $ 0Q�6:   # z 5Q      # z 5R       . 8��  y�5T      �� *HasDuration+8  �� 5U      $" PART_BottomBarContentControl��6   $��1��6C   ��5V      �� *	BottomBar+ �� *TemplatedParent+��5W      �� 5X      " �6    5Y      �� 5Z      	 6   �� *IsContentFades+ �� *TemplatedParent+
 True6r   .a�5[      ��  � ��$ 0��6/   5\      �� 5]      	 6   �� *IsContentFades+ �� *TemplatedParent+	
 False6r   .a�5^      ��  � ��$ 1��6/   5_      5a      5c      5d   	    5e        5f   
   $$ GROUP_OnVideoContentVisibility��6   . 5g        STATE_Visible��.  5h      �� .��5i      W� $! Opacity�6    $" 1X�6D   $# 
00:00:00.3D�6K   $ 5j         *PART_OnVideoContentControl+W� 5k      $# 
00:00:00.3D�6    $! Opacity�66   $" 1X�6Z   $ 5l         *PART_ProgressBar+W� 5m      $# 
00:00:00.3D�6    $! Opacity�66   $" 1X�6Z   $ 5n         *PART_BottomBarContentControl+5o      5p        5q      STATE_Hidden��.  5r      �� .��5s      W� $! Opacity�6    $" 0X�6D   $# 
00:00:00.3D�6K   $ 5t         *PART_OnVideoContentControl+W� 5u      $# 
00:00:00.3D�6    $! Opacity�66   $" 0X�6Z   $ 5v         *PART_ProgressBar+W� 5w      $# 
00:00:00.3D�6    $! Opacity�66   $" 0X�6Z   $ 5x         *PART_BottomBarContentControl+5y      5z      5{      5}      5~      �� 5      "  6
   .�5�      ��  � ��8   6)   5�      5�      