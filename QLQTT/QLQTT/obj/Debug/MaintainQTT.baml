   M S B A M L   `   `   `  ����@  <QLQTT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:QLQTTQLQTT  5   	       QLQTT.MaintainQTT   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:QLQTT     5   	     � WindowStartupLocation$  CenterScreen=�5       � Title$ MaintainQTT��5   	   $	��550��6   $	��650��6-   $	��550��5	   	   $	��650��6   .��5
      � $	��500��6   $	��600��6    � RowDefinitions 5   
   �� 5      $
��0.1* �6   �� 5      $��0.125* �6   �� 5      $��0.125* �6   �� 5      $��0.45* �6   �� 5      $
��0.2* �6   .+�5   
   �� $ ��Bảo trì Quân tư trang��6   $��0��60   $��24(�5      $��Center=�6   $
��Bold"�5      $��Center=�6"   �� 5   
   $��Tên Quân tư trang:��6   $��1��6-   $��35��6:   $��16(�5      $	��140��6   $��Center=�6*   $
��Left=�5      $��10,0,0,0q�6+   �� 5   
   -   5         ��Name$ 	txtTenQTT��5      $��1��6%   $��35��62   $��16(�5      $	��140��6    $��Center=�6,   $
��Left=�5      $��	150,0,0,0q�6-   $��Center=�5      �� 5   
   -   5         ��Name$ btnApply��5      $��Tìm kiếm��6#   $��2��66   $��16(�5      $��Center=�6   $��5q�6:   $��Center=�5      $��	0,0,250,0q�6.   $	��100��5      6   �� 5    
   -   5        $ btnCancelApply��5       $��Tải lại��6)   $��2��6;   $��16(�5!      $��Center=�6   $��5q�6:   $��Center=�5"      $��	250,0,0,0q�6.   $	��100��5#      6   &   System.Windows.Controls.DataGrid  5$   
   -   5          Name$ dtgQTT��5%   /      AutoGenerateColumns .  $��Stretch=�6      CanUserAddRows .  $��3��5'      $��Stretch=�6       CanUserDeleteRows .  	   CanUserReorderColumns	 .  
   CanUserResizeRows
 .     CanUserResizeColumns .     Columns 5,      0  *System.Windows.Controls.DataGridTextColumn  5-      ,  &System.Windows.Controls.DataGridColumn   Header$ Mã Quân tư trang��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5.   %   �� *MaQtt+  5/      $ Tên Quân tư trang��6%    50   %   �� *TenQtt+  51      $ 
Đơn giá��6%    52   %   �� *	GiaTien+  53      $ Số lượng��6%    54   %   �� *	SoLuong+  55      $ Mô tả��6%    56   %   �� *MoTa+4  .System.Windows.Controls.DataGridTemplateColumn  57      $ Cập nhật��6)      CellTemplate 58      �� 59      .R�5:      �� $ btnUpdateQTT��68   -   5        $��Cập nhật��5:   %   5;   %   5<      5>        5?      $
 Xóa��6)    5@      �� 5A      .R�5B      �� $ btnDeleteQTT��5C   %   -   5        $
��Xóa��5B   %   
���  ��5E   %   5F      5H         	CellStyle 5J      �� 5K       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    �� Triggers 5L      P� 5M       P� Property �@ 
IsSelected P� Value . . �5N      ��  �� Property � �� �� Value
 ���� 5O   9   �� 5P       � ��
 �   �5Q   9   �� 5R       � ��
 ���� 5S   9   5T      5V      5X      �� 5Y   
   -   5        $ 	btnAddQTT��5[   +   $��Thêm mới��5Y      $��Center=�6$   $��
0,10,250,0q�5Z      $	��Top=�6%   $��14(�5[      $	��100��6   $��40��5\      $��4��5]      �� 5^   
   -   5        $ 	btnCancel��5`   +   $��Thoát��5^      $��Center=�6!   $��
250,10,0,0q�5_      $	��Top=�6%   $��14(�5`      $	��100��6   $��40��5a      $��4��5b      �� 5c   
   -	   5         ��Name$ lblExitWarning��5c      
���  ��$��12(�5d      $��Center=�6   $	��Top=�5e      $��4��6(   $��0,60,0,0q�5f      5g      5h      