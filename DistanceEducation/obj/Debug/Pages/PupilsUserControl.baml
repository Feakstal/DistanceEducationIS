   M S B A M L   `   `   `  ����L  HDistanceEducation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:DistanceEducation.PagesDistanceEducation.Pages  5      /    )DistanceEducation.Pages.PupilsUserControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7local%clr-namespace:DistanceEducation.Pages     5      .��5	      � ���#FFFFFF  � RowDefinitions  5
   
   �� 5      $��1* �6   �� 5      $��5* �6   �� 5      $��1* �6   .+�5   
   �� $��0 0 0 1q�6   ���#E5E5E5._�5      � $��0��6   .+�5      � $��Center=�6   $
��Left=�67   $��20q�6R   .+�5      �� $��Ученики��6       tblockDefault#��[  6/   $��24(�6V   $
��Bold"�5      $��Rockwell)�6.   
���   �6D   5      5      5      � 5   
   $��1��6   .+�5      �� -   5         ��Name$ LvPupils��5      $��Center=�6)   $��Center=�6M    �� View 5      �� 5      .P�5      �� $��ID��6)    �� Width$
 auto��65    �� DisplayMemberBinding 6B   �� *
EmployeeID+�� 5      $��Фамилия��6)   $
 auto��6:    6G   �� *	Surname+�� 5      $��Имя��6)   $
 auto��66    6C   �� *Name+�� 5      $��Отчество��6)   $
 auto��6;    6H   �� *
FatherName+�� 5       $��
Класс��6)   $
 auto��68    6E   �� *Grade.GradeName+�� 5!      $��Статус��6)   $
 auto��69    6F   �� *PupilStatus.PupilStatusName+5"      5$      5%      5&      5'      