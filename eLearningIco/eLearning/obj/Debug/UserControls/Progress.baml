   M S B A M L   `   `   `  ����D  @eLearning, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null?$clr-namespace:eLearning.UserControlseLearning.UserControls  5      %    eLearning.UserControls.Progress   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6local$clr-namespace:eLearning.UserControls     5      $	��700��5      .��5	      �� $��Visible=�6   .��5   
   � .+�5      ��   �� Orientation$  Vertical=�6   
�������.+�5      �� $  
Horizontal=�6!   $��Center=�6:   $
��Left=�6U   $��40 20q�6q   .+�5      �� $ ��Пользователь: ��6    $��DemiBold"�66   $��Center=�6L   $
��Left=�6g   $��30(�6�   
����  �6�   �� 5      -   5         ��Name$ txbUser��5       $��Center=�61   $��30(�6L   $
��Left=�6Z   $
��Bold"�6u   ���#8c55916�   5      �� 5      $��20 10q�6   .+�5      �� $
��Left=�6    $&�� Пройденные тесты:��6;   $��0 0 0 5q�6T   $��18(�6e   $��DemiBold"�6s   
���   �6�   &   System.Windows.Controls.DataGrid  5      -   5          Name$ winGrid��5      ���#ac86e8   AutoGenerateColumns . 
�������   GridLinesVisibility$	 All=�6t   5      �� 5      $��20 10 q�6   .+�5      �� $
��Left=�6    $+��%Не пройденные тесты:��6;   $��0 0 0 5q�6W   $��DemiBold"�6h   $��18(�6~   
���   �6�     5      -   5        $ notPassedGrid��5      ���#ac86e8
������� . $	 All=�6y   5      �� 5      $��20 10q�6   .+�5       �� $
��Left=�6    $:��4Пройденные тесты по словарю:��6;   $��DemiBold"�6_   $��0 0 0 5q�6u   $��18(�6�   
���   �6�     5!      -   5        $ winDictTestGrid��5!       . ���#ac86e8
�������$	 All=�6{   5"      �� 5$      $��20 10q�6   .+�5%      �� $
��Left=�6    $?��9Не пройденные тесты по словарю:��6;   $��DemiBold"�6b   $��0 0 0 5q�6x   $��18(�6�   
���   �6�     5&      -   5        $ notPassedDictTestGrid��5&       . ���#ac86e8
�������$	 All=�6�   5'      5(      5*      5+      5,      