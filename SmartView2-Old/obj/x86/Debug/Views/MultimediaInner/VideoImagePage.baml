   M S B A M L   `   `   `  ����E  ASmartView2, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:SmartView2.Native.MediaLibrarySmartView2.Native.MediaLibrary  5   E  5clr-namespace:SmartView2.CommonSmartView2.Common  6}  ;"clr-namespace:SmartView2.BehaviorsSmartView2.Behaviors  6�  =#clr-namespace:SmartView2.ConvertersSmartView2.Converters  6�  O,clr-namespace:SmartView2.Resources.AnimationSmartView2.Resources.Animation  6  3clr-namespace:SmartView2.ViewsSmartView2.Views  6N  S.clr-namespace:SmartView2.Views.MultimediaInner SmartView2.Views.MultimediaInner  6{  5    /SmartView2.Views.MultimediaInner.VideoImagePage   6   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35K GUIFoundation, Version=1.1.0.22849, Culture=neutral, PublicKeyToken=nullW SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     6H   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     6�   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6�   @d2http://schemas.microsoft.com/expression/blend/2008     6  ;ml,clr-namespace:SmartView2.Native.MediaLibrary     6E  2commonclr-namespace:SmartView2.Common     6}  0b"clr-namespace:SmartView2.Behaviors     6�  9	converter#clr-namespace:SmartView2.Converters     6�  B	animation,clr-namespace:SmartView2.Resources.Animation     6  0viewsclr-namespace:SmartView2.Views     6N  E
innerViews.clr-namespace:SmartView2.Views.MultimediaInner     6{  ,  &UIFoundation.Interactivity.Interaction    	Behaviors  5      0  *UIFoundation.Behaviors.VisualStateBehavior     VisualStateSource 6   �� *
ViewType+ � 	Resources 5         
ItemByName �� Source �� GroupDescriptions2  ,System.Windows.Data.PropertyGroupDescription   PropertyName1   +SmartView2.Common.BoolToVisibilityConverter  BoolToVisibilityConverter  ArtistTitle  ArtistTitleBrush �� Color  ArtistTitleBgr  ArtistTitleBgrBrush��%*  &	        &	 e     &	 x     &	 �     7  &	 �     &	 �     7  �� 5       6.   �� *	ItemsList+ 5        5	   
   $ FirstLetter��6#   5        5      6'   ��5      #8A8E97��6)   �� 5      8   6/   ��5      #29303A��6,   �� 5      8   62   5      .��5      �  J� 	AllowDrop . ���	#00FFFFFF��5      �� 5       �� 
TargetType" �6   .�5   
   �� 	 �� Property	 �@�ContextMenu
 �� Value
 5      �� 5       � DataContext 6   �� *PlacementTarget+ �� RelativeSource �� *Self+.@�5      m� ��6   �� *COM_SID_ADD_FILE+  LocalizationSource �� Source# [  m� Command 6i   �� *DataContext.AddFilesCommand+m� 5      ��6   �� *COM_SID_ADD_FOLDER+# [  6k   �� *DataContext.AddFolderCommand+5      5      �� 5   
   	 �@�Tag
 61   �� *DataContext+ ��  �� AncestorType" �5   	     5!      2   ,SmartView2.Behaviors.GridDragAndDropBehavior     Command 6"   �� *ContentDropCommand+.+�5#      � -   5        $��DateListView��5#      $y�	Collapsed=�6   .+�5$      7   1SmartView2.Views.MultimediaInner.TileFileListView  5%      � 5&      -   5        $��TitleListView��5&      $y�	Collapsed=�6    .+�5'      ��   ListViewNoHeaderStyle#��[ 6   ���	#00FFFFFF$��0q�6W    . $��Stretch=�6|    %� IsVirtualizing . ! %� IsVirtualizingWhenGrouping . ��.  ��6#  �� # [  
 � Tag 6^  �� *DataContext+ �� %   SmartView2.Views.MultimediaPage"    5)      ;   5SmartView2.Behaviors.ListBoxExtendedSelectionBehavior     ExtendedSelectionEnabled 6/   �� *DataContext.IsSelectionMode+ �� Mode$ TwoWay=� �� "     SelectedItems 6�   �� *DataContext.SelectedItems+$ OneWayToSource=� �� "   �� 
GroupStyle 5+   
   (	  "System.Windows.Controls.GroupStyle	  5,       	  HeaderTemplate 5-      �� 5.      .R�5/      �   SubHeaderGridStyle#��[ 6   .+�50      ��  	 SubHeaderTitileStyle#��[	 6   51      52       	  ContainerStyle 54      �� 55      " ��6   .�56      �� 	 � ��
 57      �� 58       �� 
TargetType" ��6&   .R�59      �� .+�5:      �� �� 5;      $��0,0,0,0q�6)   5<      5=      5?      5@      5B      ��5D   
   �� 5E      .R�5F      � ���	#00FFFFFF$��0,3,0,3q�6*     5H      .
   (SmartView2.Behaviors.DoubleClickBehavior
   
  Command 6(   �� *$"DataContext.ItemDoubleClickCommand+ �� " �� 
  CommandParameter 6�   �� .+�5J      �  � ColumnDefinitions 5K      �� 5L      $
��Auto �6%   �� 5M      .+�5O      �� $��_border��6   $��0q�6(   ._�5P      �� $��35,0,0,0q�6   $	��103��6,   $	��103��69   $
��Fill=�6E   ��6T   �� *	Preview+5Q      �� 5R      $��	40,0,35,0q�6   $��1��6/   $��Center=�6?   $��Arial)�6Z   $
��Bold"�6w   $
��20pt(�6�   $��30��6�   $��CharacterEllipsis=�6�   ��6�   �� *Name+5S      ��5T      �� 5U      " �6   .�5V      �� 	 �@�ContextMenu
 5W      �� 5X      ���	#FF000000 6;   �� *PlacementTarget+ �� *Self+.@�5Y      m� ��6#   �� *COM_SID_OPEN+# [  6o   �� *Tag.ItemDoubleClickCommand+ m� CommandParameter 6�   �� *DataContext+��5Z      �� 5[      " m�6$    �� Triggers 5\       �� 5]   "     �� Binding  6.   �� *DataContext.ContentType+! �� Value	! Video6Z   .a�5^   $   �� 	 � y�$
 	Collapsed=�6K   5_   #   5a      5c      m� 5d      ��6#   �� *COM_SID_PLAY+# [  6o   �� *Tag.ItemDoubleClickCommand+ 6�   �� *DataContext+��5e      �� 5f      " m�6$    5g       �� 5h   "     6.   �� *DataContext.ContentType+	! Image6Z   .a�5i   $   �� 	 � y�$
 	Collapsed=�6K   5j   #   5l      5n      m� 5o      ��6#   �� *MAPP_SID_ADD_TO_QUEUE+# [  6x   �� *Tag.SendToQueueCommand+ 6�   �� *DataContext+m� 5p      ��6#   �� *MAPP_SID_PLAY_ON_TV+# [  6v   �� *Tag.PlayOnTvCommand+ 6�   �� *DataContext+m� 5q      ��6#   �� *MAPP_SID_REMOVE_FILES+# [  6x   �� *Tag.RemoveFilesCommand+ 6�   �� *DataContext+5r      5t      �� 5u      	 �@�Tag
 6;   �� *DataContext+ �� " ��5v      5x      " �� Triggers" 5y      �� 5z      $
! true��6     6)   �� *
IsSelected+ �� $ ListViewItemL�.a�5{      �� # �� 
TargetName$# _border��6   	 � ��
 �	#FF4FB2F66L   5|      5~      ��5�   
   �� 5�      " ��6   .�5�      �� 	 � ��
 5�      �� 5�      " ��6"   .R�5�      �� $��_layoutRoot��6   $��Stretch=�6.   $��Stretch=�6L   ���	#00FFFFFF .   5�      7   1SmartView2.Behaviors.ItemsControlDragItemBehavior  $   Command$ 67   �� *DataContext.DragItem+ �� " ��7   1SmartView2.Behaviors.ItemsControlMoveItemBehavior%   IsSelectionMode% 6�   �� *Tag.IsSelectionMode+$ OneWay=� �� " ��&   SelectedItems& 6  �� *Tag.SelectedItems+$ OneWay=� �� " ��>   8SmartView2.Behaviors.ListBoxItemSelectionByClickBehavior  5�      '   	IsEnabled' 6>   �� *GE(b:ListBoxExtendedSelectionBehavior.ExtendedSelectionEnabledAttached)+ �� $ ListViewL�._�5�      � $��Stretch=�6   $��Stretch=�69   .+�5�      �� �� 5�      $��	_checkBox��6!   $��Right=�62   $	��Top=�6N   $��0,5,25,0q�6f   $y�Hidden=�6x   ( d� 	IsChecked( 6�   �� *
IsSelected+ �� $ ListViewItemL�5�      5�      ) �� Triggers) 5�      P� 5�      * P� Property* �@��
IsSelected+ P� Value+ . . �5�      �� $# _layoutRoot��6   	 � ��
 �	#FF1F252D6U   �� 5�      	 � ��
 �	#FF4FB2F66=   5�      P� 5�      * �@J�IsMouseOver+ . . �5�      �� $# _layoutRoot��6   	 � ��
 �	#FF4FB2F66V   �� 5�      $# _layoutRoot��6   	 � ��
 �	#FF4FB2F66U   �� 5�      	 � ��
 �	#FFFFFFFF6=   5�      ^� 5�      , ^� 
Conditions, 5�      �� 5�      - �� Binding- 6$   �� *
IsSelected+ �� *Self+. �� Value. true6i   �� 5�      - 6$   �� *GE(b:ListBoxExtendedSelectionBehavior.ExtendedSelectionEnabledAttached)+ �� $ ListViewL�	. false6�   �� 5�      - 6$   �� *86(b:ListBoxExtendedSelectionBehavior.IsEnabledAttached)+ �� $ ListViewL�. True6�   5�      �� 5�      $
! True��6"     6/   �� *GE(b:ListBoxExtendedSelectionBehavior.ExtendedSelectionEnabledAttached)+ �� $ ListViewL�.a�5�      �� $# 	_checkBox��6   	 � y�$
 Visible=�6V   5�      5�      5�      5�      5�   	   5�      � 5�      -   5        $��FolderListView��5�      $y�	Collapsed=�6!   .+�5�      � y�6   �� *IsInRootFolder+ 
 _visibilityConverter/ �� 	Converter#/ [
 .+�5�   
   5   /SmartView2.Views.MultimediaInner.FolderListView  5�   	   � 5�      y�6   �� *IsInsideFolder+#/ [
 0 � RowDefinitions0 5�   
   �� 5�      $
��Auto �6   �� 5�      $��* �6   .+�5�   
   6   0SmartView2.Views.MultimediaInner.NavigationTitle  $��0��6%   1   BackCommand1 62   �� *VideoImageBackCommand+2   	TitleText2 6`   �� *CurrentFolder.Name+  5�   
   $��1��6&   5�   	   5�      � 5�      -   5        $��NoContentList��5�      $y�	Collapsed=�6    .+�5�      ��    MultimediaGuidTextBlockStyle#��[ 6   $��CharacterEllipsis=�6H   ��6i   �� *(&MAPP_SID_CLICK_PLUS_BUTTON_ADD_CONTENT+# [ 5�      '  !System.Windows.VisualStateManager3   VisualStateGroups3 5�      %  System.Windows.VisualStateGroup  5�      4   Name$4 VideoImageView��6   5   States.5 5�   
      System.Windows.VisualState  6   Name$6 Title��6   7   
Storyboard.7 5�      �� .��5�      T� 8 g� Duration$8 0D�6,   9 �� 
TargetName$9 TitleListView��69   : �� TargetProperty$: 
Visibility�6`   ..�5�      i�   System.Windows.Visibility;   Visible< R� Value#< Z; 6'   5�      T� 5�      $8 0D�6,   $9 DateListView��69   $: 
Visibility�6_   ..�5�      i� =   	Collapsed#< Z= 6'   5�      T� 5�      $8 0D�6,   $9 FolderListView��69   $: 
Visibility�6a   ..�5�      i� #< Z= 6'   5�      T� 5�      $8 0D�6,   $9 NoContentList��69   $: 
Visibility�6`   ..�5�      i� #< Z= 6'   5�      5�      5�        5�   
   $
6 Date��6   .7 5�      �� .��5�      T� $8 0D�6,   $9 TitleListView��69   $: 
Visibility�6`   ..�5�      i� #< Z= 6'   5�      T� 5�      $8 0D�6,   $9 DateListView��69   $: 
Visibility�6_   ..�5�      i� #< Z; 6'   5�      T� 5�      $8 0D�6,   $9 FolderListView��69   $: 
Visibility�6a   ..�5�      i� #< Z= 6'   5�      T� 5�      $8 0D�6,   $9 NoContentList��69   $: 
Visibility�6`   ..�5�      i� #< Z= 6'   5�      5�      5�        5�   
   $6 Folder��6   .7 5�      �� .��5�      T� $8 0D�6,   $9 TitleListView��69   $: 
Visibility�6`   ..�5�      i� #< Z= 6'   5�      T� 5�      $8 0D�6,   $9 DateListView��69   $: 
Visibility�6_   ..�5�      i� #< Z= 6'   5�      T� 5�      $8 0D�6,   $9 FolderListView��69   $: 
Visibility�6a   ..�5�      i� #< Z; 6'   5�      T� 5�      $8 0D�6,   $9 NoContentList��69   $: 
Visibility�6`   ..�5�      i� #< Z= 6'   5�      5�      5�        5�   
   $6 	NoContent��6   .7 5�      �� .��5�      T� $8 0D�6,   $9 TitleListView��69   $: 
Visibility�6`   ..�5�      i� #< Z= 6'   5�      T� 5�      $8 0D�6,   $9 DateListView��69   $: 
Visibility�6_   ..�5�      i� #< Z= 6'   5�      T� 5�      $8 0D�6,   $9 FolderListView��69   $: 
Visibility�6a   ..�5�      i� #< Z= 6'   5�      T� 5�      $8 0D�6,   $9 NoContentList��69   $: 
Visibility�6`   ..�5�      i� #< Z; 6'   5�      5�      5�      5�   	   5�      5      