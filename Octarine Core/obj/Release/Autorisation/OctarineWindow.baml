   M S B A M L   `   `   `  ���� H  DOctarine Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]3clr-namespace:Octarine_Core.Resource.UsersIntefeces%Octarine_Core.Resource.UsersIntefeces  C&clr-namespace:Octarine_Core.PropertiesOctarine_Core.Properties  /    )Octarine_Core.Autorisation.OctarineWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Eusers3clr-namespace:Octarine_Core.Resource.UsersIntefeces     =
properties&clr-namespace:Octarine_Core.Properties     -     � Title$  Octarine App��$
��1080��$
��1920�� � WindowStyle$ SingleBorderWindow=� � 
ResizeMode$ 	CanResize=� � WindowState$ 	Maximized=�.��� -    �Name$ MainGrid��   MainGray#���   .+��� -    ��Name$ UpBrd��  	MainWhite#���  
 � Tag$ static��$	��0.3q� �� CornerRadius$ 5�� �� Padding$ 0q�$��0,10,0,0q�$
��1910��$��44��$	��Top=�  SecondGreen#���  ._�]� -   	 ]�Name$	 UpBar��$��44��$��Center=�.+��� -   
 ��Name$
 CloseBtn��$��35��$��Right=�$��35��$��0,0,15,0q���Z� .��(� .���� $%��/Resource/Icons/CloseButton.png�� �� BitmapScalingMode$ HighQuality=��� -   $
 SizeBtn��$��35��$��Right=�$��35��$��0,0,15,0q���Z� .��(� .���� $$��/Resource/Icons/WindowFill.png��$ HighQuality=��� -   $
 HideBtn��$��35��$��Right=�$��35��$��Right=�$��0,0,15,0q���Z� .��(� $��35��.���� $ ��/Resource/Icons/Line 3.png��$ HighQuality=��� $��40��$ HighQuality=�$&�� /Resource/Icons/OctarineLogo.png��$��15,0,0,0q� J� RenderTransformOrigin$ 0.5,0.5&�$
��Left=�z�V� .~���  �� ScaleX$ -1Q��� �� T� ]� -   $	 MainDock����.  $��3,5,0,25q�$
��Left=�.+���   
LightGreen#���  $	��0.3q�$ 10��$ 0q�$��5,55,5,0q�#���  ._��� -	    ��Name$ 	LeftStack��$	��300��.+�6   0Octarine_Core.Resource.UsersIntefeces.UsersBrick  -
     Name$ UsersEnteredBrick��$��5,5,5,5q��� $��1��#���  $��0,0,0,5q��� -   $ 	ChatStack��$	��800��$��0,3,0,0q��� $	��0.3q�$��82��#���  $��Center=�$��Center=�._��� -   $ SpeshialNatonsStack�� �� Orientation$ 
Horizontal=�$	��299��.+��� -   $
 SearchFriendBtn��$��Center=�$��55��$��55��$��10,0,0,0q���Z� ��Z� ����  �� ImageSource$" /Resource/Icons/IconUser.png�� j� Stretch$
 Fill=��� -   $
 OpenSettingsBtn��$��55��$��55��$��10,0,0,0q���Z� ��Z� ���� $  /Resource/Icons/Zalupa.png���� $��55��$��55��$��10,0,0,0q���Z� ��Z� ���� $# /Resource/Icons/ZaPupapng.png��$
 None=��� -   $ 
InfoBorder��$ dynamic��$ 70��$��321,70,25,25q�#���  ._��� .+��� $&�� /Resource/Icons/OctarineLogo.png��$	��200��$��0,10,0,0q��� (   "Octarine_Core.Properties.Resources   WhatsNew#��Z $��Montserrat Medium)�$��48(� �� TextAlignment$ Center=�$��0,10,0,0q��� -   $ 	NewsStack��$	��100��.+��� $��Куплю гараж��$��Montserrat Medium)�$��48(�$ Center=��� -   $ SearchFrindBrd��$ dynamic��$ 70��$��321,70,25,25q�#���  ._�� -   $ FriendsSystemGrid��.+��� $ 
Horizontal=�$��40,6,40,923q�.+��� -   $
 YourFriendBtn��   YourFriends#��Z $��Montserrat Medium)�#���   $	��120���� -   $
 SearchYourFriendBtn��   LetsSeachFriend#��Z $��Montserrat Light)�#���   $	��120��$��5,0,0,0q�� -   $ SearchFriendGrd��$y�Hidden=�.+��� $��
0,100,0,10q�.+��� -   $ SearchfriendStack��$��5,0,5,0q�.+��� $
��Wrap=�   WhoAreWeLookingFor#��Z $	��Top=�$ Center=�$��48(�#���  �� $��50��$ 
Horizontal=�.+��� -    ��Name$ SearchingUserIdTb��$��36(�$��47��$
��1500���� -   $��45��$��45��$��15,0,0,0q���Z� ���� $ /Resource/Icons/Lupa.png���� -   $ SearchStack��$	��674��� -   $ AllFriendsGrid��.+��� $��	0,100,0,0q�.+��� $
��Wrap=�#��Z $ Center=�$��48(��� -   $ FriendsAllStack��$	��808���� -   $ ErrorAutbrd��$ dynamic��#���  $��1q�$��538,860,242,136q�#���   ._��� -    ��Name$ 
ErrorOutTb��$
��Wrap=�$��	TextBlock��$��24(�$ Center=�$��5,5,5,5q��� -   $ 
ChatWindow��$ dynamic��$ 70��$��321,70,25,25q�#���  ._�� -   $ ChatGrid��.+�� -    $ IngoGrid��$��	0,0,0,794q��� -!    ��Name$	 Scr��$
��Auto=�$��Disabled=�$��25,180,25,105q�.���� -"   $ MainChatStack��$��	0,0,0,100q�� -#   $ TextBoxGrid��$	��100��$��Bottom=�.+���  �� RadiusX$ 30�� �� RadiusY$ 30��#���   $��
15,0,15,30q��� -$   $
 SendMessageBtn��$��Right=�$��60��$��60��$��	0,0,40,30q���Z� ���� $ /Resource/Icons/Send.png���� -%   $ SendedMesageTb�� �� TextWrapping$
 Wrap=�$��39,12,105,42q�$��22(�$��45���� -&   $ settingsBorder��#���  $ dynamic��$��1q�$��25,13,25,25q�#���   ._�]� -'   $	 MainDockSettings����.  .+��� -(   $ ChapterStack��$	��254��$��2,0,0,0q�#���  .+��� $��Аккаунт��$��62��#���   $��	10,5,10,0q�$��18(�$	��0.7q�#���  � -)   $ AccauntSettingsGrid��$
��1610��.+��� $��Center=�$ 15��$ 15��$	��966��$��0,31,0,0q�$	��Top=�$
��1544��#���  �� $��1064,64,55,0q�$
��Wrap=�$)��#Настройки аккаунта��$	��Top=�$��48(��� -*   $ ChangeNameTb��$
��Left=�$��214,217,0,0q�$
 Wrap=�$	��Top=�$	��348��$��50��$��36(��� $��90,220,1401,0q�$
��Wrap=�$��Имя��$	��Top=�$��36(��� $��901,156,467,0q�$
��Wrap=�$��Фото профиля��$	��Top=�$��36(��� $
��Left=�$ 12��$ 12��$	��261��$��1178,133,0,0q�$	��Top=�$	��262��#���   $ 0.5,0.5&�z�V� .~��� �� ��  �� Angle$ 0Q�T� �� -+    ��Name$ UsersImageSettings��$ HighQuality=�$��1202,156,194,654q�$��/OctarineLogo.ico���� -,   $
 SaveChanges��$��Сохранить ��$
��Left=�$��1018,916,0,0q�$	��Top=�$��60��$	��250��#���  $��24(��� --   $��
Выйти��$
��Left=�$��1300,917,0,0q�$	��Top=�$��60��$	��250��#���   $��24(��� -.   $
 ChanheImageBtn��$��Выбрать��$��Right=�$	��Top=�$��0,399,253,0q�$��24(�#���   � -/   $ FirstErorrGrid��$��321,54,25,967q�� -0   $ OarticalsGrid��$��1339,204,57,204q�