   M S B A M L   `   `   `  ����@  <QLQTT, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:QLQTTQLQTT  5   	       QLQTT.MaintainCourse   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:QLQTT     5   	     � WindowStartupLocation$  CenterScreen=�5       � Title$ MaintainCourse��5   	   $	��550��6    $	��650��60   $	��550��5	   	   $	��650��6   .��5
      � $	��500��6   $	��600��6    � RowDefinitions 5   
   �� 5      $
��0.1* �6   �� 5      $��0.125* �6   �� 5      $��0.125* �6   �� 5      $��0.45* �6   �� 5      $
��0.2* �6   .+�5   
   �� $��Bảo trì Khóa học��6   $��0��6+   $��24(�5      $��Center=�6   $
��Bold"�5      $��Center=�6"   �� 5   
   $��Mã khóa học:��6   $��1��6'   $��35��64   $��16(�5      $	��140��6   $��Center=�6*   $
��Left=�5      $��10,0,0,0q�6+   �� 5   
   -   5         ��Name$ txtMaKH��5      $��1��6#   $��35��60   $��16(�5      $	��140��6    $��Center=�6,   $
��Left=�5      $��	150,0,0,0q�6-   $��Center=�5      �� 5   
   $��Chọn ngày:��6   $��1��6%   $��35��62   $��16(�5      $	��140��6   $��Center=�6*   $��Right=�5      $��	0,0,150,0q�6,   (  "System.Windows.Controls.DatePicker  5   
   -   5          Name$ txtDate��5      $��1��6&   $��35��63   $��16(�5       $	��140��6    $��Center=�6,   $��Right=�5!      $��0,0,10,0q�6.   $��Center=�5"      �� 5#   
   -   5         ��Name$ btnApply��5#      $��Tìm kiếm��6#   $��2��66   $��16(�5$      $��Center=�6   $��5q�6:   $��Center=�5%      $��	0,0,250,0q�6.   $	��100��5&      6   �� 5'   
   -   5        $ btnCancelApply��5'      $��Tải lại��6)   $��2��6;   $��16(�5(      $��Center=�6   $��5q�6:   $��Center=�5)      $��	250,0,0,0q�6.   $	��100��5*      6   &   System.Windows.Controls.DataGrid  5+   
   -   5          Name$ dtgKH��5,   /      AutoGenerateColumns .  $��Stretch=�6      CanUserAddRows .  $��3��5.      $��Stretch=�6    	   CanUserDeleteRows	 .  
   CanUserReorderColumns
 .     CanUserResizeRows .     CanUserResizeColumns .     Columns 53      0  *System.Windows.Controls.DataGridTextColumn  54      ,  &System.Windows.Controls.DataGridColumn   Header$ Mã Khóa học��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 55   %   �� *MaKh+  56      $ Ngày bắt đầu��6%    57   %   �� *NgayBd+ �� StringFormat$ {0:dd/MM/yyyy}��  59      $ Ngày kết thúc��6%    5:   %   �� *NgayKt+$ {0:dd/MM/yyyy}��  5<      $ Mô tả��6%    5=   %   �� *MoTa+4  .System.Windows.Controls.DataGridTemplateColumn  5>      $ Cập nhật��6)      CellTemplate 5?      �� 5@      .R�5A      �� $ btnUpdateKH��68   -   5        $��Cập nhật��5A   %   5B   %   5C      5E        5F      $
 Xóa��6)    5G      �� 5H      .R�5I      �� $ btnDeleteKH��5J   %   -   5        $
��Xóa��5I   %   
���  ��5L   %   5M      5O         	CellStyle 5Q      �� 5R       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    �� Triggers 5S      P� 5T       P� Property �@ 
IsSelected P� Value . . �5U      ��  �� Property � �� �� Value
 ���� 5V   9   �� 5W       � ��
 �   �5X   9   �� 5Y       � ��
 ���� 5Z   9   5[      5]      5_      �� 5`   
   -   5        $ btnAddKH��5b   +   $��Thêm mới��5`      $��Center=�6$   $��
0,10,250,0q�5a      $	��Top=�6%   $��14(�5b      $	��100��6   $��40��5c      $��4��5d      �� 5e   
   -	   5        $ 	btnCancel��5g   +   $��Thoát��5e      $��Center=�6!   $��
250,10,0,0q�5f      $	��Top=�6%   $��14(�5g      $	��100��6   $��40��5h      $��4��5i      �� 5j   
   -
   5         ��Name$ lblExitWarning��5j      
���  ��$��12(�5k      $��Center=�6   $	��Top=�5l      $��4��6(   $��0,60,0,0q�5m      5n      5o      