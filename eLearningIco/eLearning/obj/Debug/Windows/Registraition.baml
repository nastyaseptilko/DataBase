   M S B A M L   `   `   `  ����D  @eLearning, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:eLearning.WindowseLearning.Windows  5   	   I)clr-namespace:eLearning.ThemesUserControleLearning.ThemesUserControl  5   	   %    eLearning.Windows.Registraition   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:eLearning.Windows     5   	   V RMaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   AThemesUserControl)clr-namespace:eLearning.ThemesUserControl  5   	   $	��500��5
   	   $	��400��6     � WindowStyle$
  None=�6"    � 
ResizeMode$ NoResize=�65    � WindowStartupLocation$ CenterScreen=�6K   ��5      �� 5   
    �� ImageSource$, &/eLearning;component/Images/london.jpg��6    �� Opacity$	 0.5Q�6J   .��5      � .+�5   
   � .+�5      �� -   5         ��Name$ btnClose��5      $	��Top=�6T   $��Right=�6l    0  ,MaterialDesignFloatingActionMiniAccentButton#��[  6�   ��6'   Z� ��6=   Z� .��5      '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.4.0.1044, Culture=neutral, PublicKeyToken=null]]   Kind$ Close=�6*   ���	#FFA2A2A2$��30��6N   $��30��6Y   5      �� 5       �� Orientation$ 
Horizontal=�6   $��Center=�62   $	��Top=�6O   $��60��6g   $��30q�6s   .+�5      �� -   5         ��Name$ 	formLogin��5      $��Вход��6/   	 � Cursor$
	 Hand��6;   ���#ac86e8$��25(�6^   $
��5 15q�6l   6z   �� 5      $��/��6   
�������$��40(�68   �� 5      $��Регистрация��6   
�������$��40(�6B   $��5 0 0 0q�6P   5      �� 5      $ Vertical=�6   $	��300��60   $	��210��6<   $��Center=�6I   .+�5      �� $��50��6   
 �� CornerRadius$
 15��6%   
�������$��1q�6J   $��Center=�6^    � UseLayoutRounding . ._�5      �� -   5         ��Name$ txbLogin��5      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 
Логин��60   
�������$��25(�6j   $��5 0 0 2q�6x   $��0q�6�    �� 	MaxLength$ 25��6�   5       �� 5"      $��50��6   $
 15��6%   
�������$��1q�6J   $��Center=�6^    . $��0 30 0 0q�6�   ._�5#      G� -   5         G�Name$ txbPassword1��5#   "   $ Пароль��68   
�������$��25(�6s   $��5 0 0 2q�6�   $��0q�6�   5$      �� 5&      $��50��6   $
 15��6%   
�������$��1q�6J   $��Center=�6^    . $��0 30 0 0q�6�   ._�5'      G� -   5        $ txbPassword2��5'   "   $% Повторить пароль��68   
�������$��25(�6}   $��5 0 0 2q�6�   $��0q�6�   5(      5)      �� 5+      -   5        $ btnRegister��5+      $��Регистрация��6*    �� 	IsDefault . $��Bottom=�6Q   $��Center=�6l   $��0 0 0 50q�6�   $	��200��6�   $��45��6�   $��25(�6�   6�   �� 5,      $�� ��6   $��Bottom=�6!   $��Center=�6<   $��0 0 0 10q�6Y   $��15(�6k   5-      5.      5/      