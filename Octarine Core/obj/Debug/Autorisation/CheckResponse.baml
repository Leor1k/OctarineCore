   M S B A M L   `   `   `  ����H  DOctarine Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Octarine_Core.AutorisationOctarine_Core.Autorisation  5   	   .    (Octarine_Core.Autorisation.CheckResponse   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:Octarine_Core.Autorisation     5   	     � Title$  CheckResponse��5   	   $	��450��6   $	��350��6,    � WindowStyle$
 None=�68    � 
ResizeMode$ 	CanResize=�6K   ��6b   Z� .��5	      � .+�5
   
   ��    MainGray#���   6   �� 5   
   $
��Left=�6   $��
31,287,0,0q�6/   $
��Wrap=�6C   $	��Api��6W   $	��Top=�6b    �� TextAlignment$ Center=�6z   $��16(�6�   
�������6�   �� 5   
   $
��Left=�6   $��
31,311,0,0q�6/   $
��Wrap=�6C   $��	WebSocket��6W   $	��Top=�6h   $ Center=�6�   $��16(�6�   
�������6�   �� 5   
   $
��Left=�6   $��
31,340,0,0q�6/   $
��Wrap=�6C   $��MinIO��6W   $	��Top=�6d   $ Center=�6|   $��16(�6�   
�������6�   �� 5   
   $
��Left=�6   $��
31,369,0,0q�6/   $
��Wrap=�6C   $��Статус��6W   $	��Top=�6e   $ Center=�6}   $��16(�6�   
�������6�   �� 5   
   -   5         ��Name$ ApiTB��5      $
��Left=�6#   $��207,287,0,0q�6>   $
��Wrap=�6S   $	��Top=�6g   $��16(�6   
�������6�   �� 5   
   -   5        $ WebSocketTB��5      $
��Left=�6)   $��207,311,0,0q�6D   $
��Wrap=�6Y   $	��Top=�6m   $��16(�6�   
�������6�   �� 5   
   -   5        $ MinIoTB��5      $
��Left=�6%   $��207,340,0,0q�6@   $
��Wrap=�6U   $	��Top=�6i   $��16(�6�   
�������6�   �� 5   
   -   5        $ StatusTB��5      $
��Left=�6&   $��
82,369,0,0q�6A   $
��Wrap=�6U   $	��Top=�6i   $��16(�6�   
������� J� RenderTransformOrigin$ 0.5,0.5&�6�   $	��258��6�   $��71��6�   z�5      V� 5      .~�5      ��  �� ScaleX$ 1Q�6%   �� 5      �� 5      T� 5      5      5      (� 5   
   $��100,70,100,225q�6   .��5      � .+�5      A� $��70��6   $��70��6%    J� Opacity$	 0.3Q�61   $ 0.5,0.5&�6?   ��5       
� 5!      .��5"      �  � Color$ 	#0034D399��6+   	 � Offset$		 0.7Q�6=   � 5#      $ 	#3434D399��6+   $	 1Q�6=   5$      z�5&      �� 5'      $ 1Q�6)   
 �� ScaleY$
 1Q�64    J� Effect 5)      -  'System.Windows.Media.Effects.BlurEffect  5*         Radius$ 10Q�6%    � Triggers 5,      :� 5-       :� RoutedEvent$ Loaded��6'   .[�5.      �� .��5/   "   ��  g� RepeatBehavior$ Forever��6-   .��51   &   W�  �� TargetProperty$ Opacity�52   !    W� From$	 0.2X�53   !   	 W� To$	 0.5X�6,    g� Duration$ 0:0:2D�65    g� AutoReverse . 54   !   W� 56   &   $7 1(Ellipse.RenderTransform).(ScaleTransform.ScaleX)�57   !   $	 0.9X�58   !   $	 1.1X�6,   $ 0:0:2D�65    . 59   !   W� 5:   &   $7 1(Ellipse.RenderTransform).(ScaleTransform.ScaleY)�5;   !   $	 0.9X�5<   !   $	 1.1X�6,   $ 0:0:2D�65    . 5=   !   5>   #   5?      5@      5B      �� 5E        LogoTime#��[ 6   $��Uniform=�6;    �� BitmapScalingMode$ HighQuality=�5F      $��55��6>    5G      3  -System.Windows.Media.Effects.DropShadowEffect  5H         Color$ #34D399��5I         	Direction$ 0Q�5J         ShadowDepth$ 0Q�5K         
BlurRadius$ 15Q�5L         Opacity$	 0.5Q�5M      z�5O      �� 5P      -   5        rotateTransform�� �� Angle$ 0Q�5P   C    �� CenterX$
 27.5Q�6M    �� CenterY$
 27.5Q�6\    5R      :� 5S      $ Loaded��6'   .[�5T      �� .��5U   "   �� .��5W   &   W�  �� 
TargetName$ rotateTransform��5X   !   $ Angle�5Y   !   $ 0X�5Z   !   $	 360X�6*   $ 0:0:10D�63   $ Forever��5[   !   W� 5]   &   $2 ,(Image.Effect).(DropShadowEffect.BlurRadius)�5^   !   $ 10X�5_   !   $ 20X�6+   $ 0:0:2D�63    . $ Forever��5`   4   W� 5b   &   $/ )(Image.Effect).(DropShadowEffect.Opacity)�5c   !   $	 0.3X�5d   !   $	 0.7X�6,   $ 0:0:2D�65    . $ Forever��5e   4   5f   #   5g      5h      5j      5k      5l      5n      5o      