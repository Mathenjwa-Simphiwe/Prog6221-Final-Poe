   M S B A M L   `   `   `  ����C  ?POEFinal, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:POEFinalPOEFinal  5   	       POEFinal.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:POEFinal     5   	     � Title$  Cybersecurity Assistant��5   	   $	��600��6)   $	��800��66   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   o� $��0��6   .@�5      m� $��Tasks��6   .@�5      m� -   5        $��Add Task��5      6-   m� 5      -   5        $��
View Tasks��5      6/   5      m� 5      -   5        $
��Quiz��5      6%   m� 5      -   5        $��Activity Log��5      6-   5      �� 5   
   -   5         ��Name$ ChatOutputBox��5      $��1��6-    �� 
IsReadOnly$
 True��5      " �� VerticalScrollBarVisibility$
 Auto=�6(   $��5q�5      $��10q�6!   $��14(�6.   � 5    
   $��2��6   $��5q�6    � ColumnDefinitions 5!      �� 5"      $��* �6#   �� 5#      $
��Auto �6#   .+�5%      �� -   5         ��Name$ InputTextBox��5%      $��0��6,   $��Center=�5&      $��14(�68   5'      �� 5(      -   5         ��Name$ 	AskButton��5(      $��1��6(   $	��Ask��5)      $��5,0,0,0q�6#   $
��10,5q�64   5*      5+      5,      5-      