   M S B A M L   `   `   `  ����@  <QLQTT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:QLQTTQLQTT  5   	       QLQTT.MainTainQTTDetail   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:QLQTT     5   	     � WindowStartupLocation$  CenterScreen=�5       � Title$ MainTainQTTDetail��5   	   $	��550��6#   $	��650��63   $	��550��5	   	   $	��650��6   .��5
      � $	��500��6   $	��600��6    � RowDefinitions 5   
   �� 5      $
��0.1* �6   �� 5      $��0.125* �6   �� 5      $��0.125* �6   �� 5      $��0.45* �6   �� 5      $
��0.2* �6   .+�5   
   �� $+��%Bảo trì Chi tiết Quân tư trang��6   $��0��69   $��24(�5      $��Center=�6   $
��Bold"�5      $��Center=�6"   �� 5   
   $��Tên Quân tư trang:��6   $��1��6-   $��35��6:   $��16(�5      $	��140��6   $��Center=�6*   $
��Left=�5      $��10,0,0,0q�6+   �� 5   
   -   5         ��Name$ 	txtTenQTT��5      $��1��6%   $��35��62   $��16(�5      $	��140��6    $��Center=�6,   $
��Left=�5      $��	150,0,0,0q�6-   $��Center=�5      �� 5   
   $��Tên Kích cỡ:��6   $��1��6'   $��35��64   $��16(�5      $	��140��6   $��Center=�6*   $��Right=�5      $��	0,0,150,0q�6,   �� 5   
   -   5        $ txtTenKC��5      $��1��6$   $��35��61   $��16(�5       $	��140��6    $��Center=�6,   $��Right=�5!      $��0,0,10,0q�6.   $��Center=�5"      �� 5#   
   -   5         ��Name$ btnApply��5#      $��Tìm kiếm��6#   $��2��66   $��16(�5$      $��Center=�6   $��5q�6:   $��Center=�5%      $��	0,0,250,0q�6.   $	��100��5&      6   �� 5'   
   -   5        $ btnCancelApply��5'      $��Tải lại��6)   $��2��6;   $��16(�5(      $��Center=�6   $��5q�6:   $��Center=�5)      $��	250,0,0,0q�6.   $	��100��5*      6   &   System.Windows.Controls.DataGrid  5+   
   -   5          Name$ dtgQTTDetail��5-         AutoGenerateColumns .  $��Stretch=�5,         CanUserAddRows .  $��3��50      $��Stretch=�6       CanUserDeleteRows .  	   CanUserReorderColumns	 .  
   CanUserResizeRows
 .     CanUserResizeColumns .     Columns 55      0  *System.Windows.Controls.DataGridTextColumn  56      ,  &System.Windows.Controls.DataGridColumn   Header$ Mã Quân tư trang��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 57   %   �� *MaQtt+  58      $ Tên Quân tư trang��6%    59   %   �� *qtt+  5:      $ Mã Kích cỡ��6%    5;   %   �� *MaKc+  5<      $ Tên Kích cỡ��6%    5=   %   �� *kc+  5>      $ Số lượng��6%    5?   %   �� *	SoLuongCt+4  .System.Windows.Controls.DataGridTemplateColumn  5@      $ Cập nhật��6)      CellTemplate 5A      �� 5B      .R�5C      �� $ btnUpdateQTTDetail��68   -   5        $��Cập nhật��5C   %   5D   %   5E      5G        5H      $
 Xóa��6)    5I      �� 5J      .R�5K      �� $ btnDeleteQTTDetail��5L   %   -   5        $
��Xóa��5K   %   
���  ��5N   %   5O      5Q         	CellStyle 5S      �� 5T       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    �� Triggers 5U      P� 5V       P� Property �@ 
IsSelected P� Value . . �5W      ��  �� Property � �� �� Value
 ���� 5X   9   �� 5Y       � ��
 �   �5Z   9   �� 5[       � ��
 ���� 5\   9   5]      5_      5a      �� 5b   
   -   5        $ btnAddQTTDetail��5d   +   $��Thêm mới��5b      $��Center=�6$   $��
0,10,250,0q�5c      $	��Top=�6%   $��14(�5d      $	��100��6   $��40��5e      $��4��5f      �� 5g   
   -	   5        $ 	btnCancel��5i   +   $��Thoát��5g      $��Center=�6!   $��
250,10,0,0q�5h      $	��Top=�6%   $��14(�5i      $	��100��6   $��40��5j      $��4��5k      �� 5l   
   -
   5         ��Name$ lblExitWarning��5l      
���  ��$��12(�5m      $��Center=�6   $	��Top=�5n      $��4��6(   $��0,60,0,0q�5o      5p      5q      