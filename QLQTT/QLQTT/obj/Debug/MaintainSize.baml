   M S B A M L   `   `   `  ����@  <QLQTT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:QLQTTQLQTT  5   	       QLQTT.MaintainSize   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:QLQTT     5   	     � WindowStartupLocation$  CenterScreen=�5       � Title$ MaintainSize��5   	   $	��550��6   $	��650��6.   $	��550��5	   	   $	��650��6   .��5
      � $	��500��6   $	��600��6    � RowDefinitions 5   
   �� 5      $
��0.1* �6   �� 5      $��0.125* �6   �� 5      $��0.125* �6   �� 5      $��0.45* �6   �� 5      $
��0.2* �6   .+�5   
   �� $��Bảo trì Kích cỡ��6   $��0��6*   $��24(�5      $��Center=�6   $
��Bold"�5      $��Center=�6"   �� 5   
   $��Tên Kích cỡ:��6   $��1��6'   $��35��64   $��16(�5      $	��140��6   $��Center=�6*   $
��Left=�5      $��10,0,0,0q�6+   �� 5   
   -   5         ��Name$ txtTenKC��5      $��1��6$   $��35��61   $��16(�5      $	��140��6    $��Center=�6,   $
��Left=�5      $��	150,0,0,0q�6-   $��Center=�5      �� 5   
   -   5         ��Name$ btnApply��5      $��Tìm kiếm��6#   $��2��66   $��16(�5      $��Center=�6   $��5q�6:   $��Center=�5      $��	0,0,250,0q�6.   $	��100��5      6   �� 5    
   -   5        $ btnCancelApply��5       $��Tải lại��6)   $��2��6;   $��16(�5!      $��Center=�6   $��5q�6:   $��Center=�5"      $��	250,0,0,0q�6.   $	��100��5#      6   &   System.Windows.Controls.DataGrid  5$   
   -   5          Name$ dtgKC��5%   /      AutoGenerateColumns .  $��Stretch=�6      CanUserAddRows .  $��3��5'      $��Stretch=�6       CanUserDeleteRows .  	   CanUserReorderColumns	 .  
   CanUserResizeRows
 .     CanUserResizeColumns .     Columns 5,      0  *System.Windows.Controls.DataGridTextColumn  5-      ,  &System.Windows.Controls.DataGridColumn   Header$ Mã Kích cỡ��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5.   %   �� *MaKc+  5/      $ Tên Kích cỡ��6%    50   %   �� *TenKc+  51      $ Mô tả��6%    52   %   �� *MoTa+4  .System.Windows.Controls.DataGridTemplateColumn  53      $ Cập nhật��6)      CellTemplate 54      �� 55      .R�56      �� $ btnUpdateKC��68   -   5        $��Cập nhật��56   %   57   %   58      5:        5;      $
 Xóa��6)    5<      �� 5=      .R�5>      �� $ btnDeleteKC��5?   %   -   5        $
��Xóa��5>   %   
���  ��5A   %   5B      5D         	CellStyle 5F      �� 5G       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    �� Triggers 5H      P� 5I       P� Property �@ 
IsSelected P� Value . . �5J      ��  �� Property � �� �� Value
 ���� 5K   9   �� 5L       � ��
 �   �5M   9   �� 5N       � ��
 ���� 5O   9   5P      5R      5T      �� 5U   
   -   5        $ btnAddKC��5W   +   $��Thêm mới��5U      $��Center=�6$   $��
0,10,250,0q�5V      $	��Top=�6%   $��14(�5W      $	��100��6   $��40��5X      $��4��5Y      �� 5Z   
   -   5        $ 	btnCancel��5\   +   $��Thoát��5Z      $��Center=�6!   $��
250,10,0,0q�5[      $	��Top=�6%   $��14(�5\      $	��100��6   $��40��5]      $��4��5^      �� 5_   
   -	   5         ��Name$ lblExitWarning��5_      
���  ��$��12(�5`      $��Center=�6   $	��Top=�5a      $��4��6(   $��0,60,0,0q�5b      5c      5d      