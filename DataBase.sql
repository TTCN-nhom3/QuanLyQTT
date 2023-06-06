use master
go
create database QLQTT
go
use QLQTT
go
create table QUAN_TU_TRANG(
	MaQTT char(10) primary key,
	TenQTT nvarchar(35) not null,
	GiaTien money not null default 0 check (GiaTien >= 0),
	SoLuong int not null default 0 check (SoLuong >= 0),
	MoTa nvarchar(50) null
)
create table KICH_CO(
	MaKC char(10) primary key,
	TenKC nvarchar(20) not null,
	MoTa nvarchar(50) null
)
create table KHOA_HOC(
	MaKH char(10) primary key,
	NgayBD date not null,
	NgayKT date not null,
	MoTa nvarchar(50) null
)
go
create table CHI_TIET_QTT(
	MaQTT char(10) foreign key references QUAN_TU_TRANG(MaQTT),
	MaKC char(10) foreign key references KICH_CO(MaKC),
	primary key (MaQTT, MaKC),
	SoLuongCT int not null default 0 check (SoLuongCT >= 0)
)
create table SINH_VIEN(
	MaSV char(10) primary key,
	TenSV nvarchar(35) not null,
	Anh varbinary(max) null,
	MatKhau varchar(20) not null,
	MaKH char(10) foreign key references KHOA_HOC(MaKH),
	MoTa nvarchar(50) null
)
go
create table DANG_MUON(
	MaSV char(10) foreign key references SINH_VIEN(MaSV),
	MaQTT char(10) foreign key references QUAN_TU_TRANG(MaQTT),
	MaKC char(10) foreign key references KICH_CO(MaKC),
	primary key (MaSV, MaQTT, MaKC)
)
create table MUON(
	MaMuon char(10) primary key,
	MaSV char(10) foreign key references SINH_VIEN(MaSV),
	MaQTT char(10) foreign key references QUAN_TU_TRANG(MaQTT),
	MaKC char(10) foreign key references KICH_CO(MaKC),
	NgayDK date not null,
	NgayMuon date default null,
	TrangThai nvarchar(20) not null default N'Đang xử lý',
	MoTa nvarchar(50) null
)
create table DOI(
	MaDoi char(10) primary key,
	MaSV char(10) foreign key references SINH_VIEN(MaSV),
	MaQTT char(10) foreign key references QUAN_TU_TRANG(MaQTT),
	MaKC char(10) foreign key references KICH_CO(MaKC),
	NgayDK date not null,
	NgayDoi date default null,
	TrangThai nvarchar(20) not null default N'Đang xử lý',
	MoTa nvarchar(50) null
)
create table MAT(
	MaMat char(10) primary key,
	MaSV char(10) foreign key references SINH_VIEN(MaSV),
	MaQTT char(10) foreign key references QUAN_TU_TRANG(MaQTT),
	MaKC char(10) foreign key references KICH_CO(MaKC),
	NgayMat date not null,
	SoTien money not null check (SoTien > 0),
	MoTa nvarchar(50) null
)
create table CONG_NO(
	MaCN char(10) primary key,
	MaSV char(10) foreign key references SINH_VIEN(MaSV),
	SoTien money not null check (SoTien > 0),
	HanTra date not null,
	MoTa nvarchar(50) null
)
create table HOA_DON_THANH_TOAN(
	MaHDTT char(10) primary key,
	MaSV char(10) foreign key references SINH_VIEN(MaSV),
	SoTien money check (SoTien > 0),
	NgayTra date not null,
	MoTa nvarchar(50) null,
)
go
use QLQTT
go
insert into QUAN_TU_TRANG (MaQTT, TenQTT, GiaTien, SoLuong) values
	('QTT0000001', N'Áo thun', 40, 3500),
	('QTT0000002', N'Áo dài', 80, 3500),
	('QTT0000003', N'Quần', 60, 3500),
	('QTT0000004', N'Mũ', 40, 3500),
	('QTT0000005', N'Dép', 40, 3500),
	('QTT0000006', N'Thắt lưng', 30, 3500)
insert into KICH_CO values
	('KCO0000001', N'None', N'Không có kích cỡ'),
	('KCO0000002', N'Size 1', '< 50 kg & < 155 cm'),
	('KCO0000003', N'Size 2', '50 - 60 kg & 155 - 165 cm'),
	('KCO0000004', N'Size 3', '61 - 70 kg & 166 - 175 cm'),
	('KCO0000005', N'Size 4', '71 - 75 kg & 176 - 180 cm'),
	('KCO0000006', N'Size 5', '76 - 80 kg & 181 - 185 cm'),
	('KCO0000007', N'Size 6', '81 - 90 kg & 155 - 165 cm'),
	('KCO0000008', N'Size 7', '> 90 kg & > 185 cm')
insert into KHOA_HOC (MaKH, NgayBD, NgayKT) values
	('SEM0000001', '03-03-2023', '04-03-2023'),
	('SEM0000002', '04-04-2023', '05-04-2023'),
	('SEM0000003', '05-05-2023', '06-05-2023'),
	('SEM0000004', '06-06-2023', '07-06-2023'),
	('SEM0000005', '07-07-2023', '08-07-2023')
go
insert into CHI_TIET_QTT values
	('QTT0000001', 'KCO0000002', 500),
	('QTT0000001', 'KCO0000003', 600),
	('QTT0000001', 'KCO0000004', 600),
	('QTT0000001', 'KCO0000005', 600),
	('QTT0000001', 'KCO0000006', 600),
	('QTT0000001', 'KCO0000007', 300),
	('QTT0000001', 'KCO0000008', 300),
	('QTT0000002', 'KCO0000002', 500),
	('QTT0000002', 'KCO0000003', 600),
	('QTT0000002', 'KCO0000004', 600),
	('QTT0000002', 'KCO0000005', 600),
	('QTT0000002', 'KCO0000006', 600),
	('QTT0000002', 'KCO0000007', 300),
	('QTT0000002', 'KCO0000008', 300),
	('QTT0000003', 'KCO0000002', 500),
	('QTT0000003', 'KCO0000003', 600),
	('QTT0000003', 'KCO0000004', 600),
	('QTT0000003', 'KCO0000005', 600),
	('QTT0000003', 'KCO0000006', 600),
	('QTT0000003', 'KCO0000007', 300),
	('QTT0000003', 'KCO0000008', 300),
	('QTT0000004', 'KCO0000001', 3500),
	('QTT0000005', 'KCO0000001', 3500),
	('QTT0000006', 'KCO0000001', 3500)
insert into SINH_VIEN (MaSV , TenSV, MatKhau, MaKH, Anh)
select '2020608162', N'Nguyễn Linh Trường', '123', 'SEM0000003', BulkColumn
from Openrowset(Bulk 'D:\Pictures\DataBase\truong.jpg', Single_Blob) as image
insert into SINH_VIEN (MaSV , TenSV, MatKhau, MaKH, Anh)
select '2020600630', N'Trần Thị Hồng Thắm', '123', 'SEM0000003', BulkColumn
from Openrowset(Bulk 'D:\Pictures\DataBase\tham.png', Single_Blob) as image
insert into SINH_VIEN (MaSV , TenSV, MatKhau, MaKH, Anh)
select '2020601080', N'Vũ Trung Kiên', '123', 'SEM0000003', BulkColumn
from Openrowset(Bulk 'D:\Pictures\DataBase\kien.png', Single_Blob) as image
insert into SINH_VIEN (MaSV , TenSV, MatKhau, MaKH, Anh)
select '2020606074', N'Đỗ Danh Đức', '123', 'SEM0000003', BulkColumn
from Openrowset(Bulk 'D:\Pictures\DataBase\duc.png', Single_Blob) as image
go
insert into DANG_MUON (MaSV, MaQTT, MaKC) values
	('2020608162', 'QTT0000001', 'KCO0000005'),
	('2020608162', 'QTT0000002', 'KCO0000005'),
	('2020608162', 'QTT0000003', 'KCO0000004'),
	('2020608162', 'QTT0000004', 'KCO0000001'),
	('2020608162', 'QTT0000005', 'KCO0000001'),
	('2020608162', 'QTT0000006', 'KCO0000001'),
	('2020600630', 'QTT0000001', 'KCO0000002'),
	('2020600630', 'QTT0000002', 'KCO0000002'),
	('2020600630', 'QTT0000003', 'KCO0000002'),
	('2020600630', 'QTT0000004', 'KCO0000001'),
	('2020600630', 'QTT0000005', 'KCO0000001'),
	('2020600630', 'QTT0000006', 'KCO0000001'),
	('2020601080', 'QTT0000001', 'KCO0000003'),
	('2020601080', 'QTT0000002', 'KCO0000003'),
	('2020601080', 'QTT0000003', 'KCO0000004'),
	('2020601080', 'QTT0000004', 'KCO0000001'),
	('2020601080', 'QTT0000005', 'KCO0000001'),
	('2020601080', 'QTT0000006', 'KCO0000001'),
	('2020606074', 'QTT0000001', 'KCO0000005'),
	('2020606074', 'QTT0000002', 'KCO0000005'),
	('2020606074', 'QTT0000003', 'KCO0000005'),
	('2020606074', 'QTT0000004', 'KCO0000001'),
	('2020606074', 'QTT0000005', 'KCO0000001'),
	('2020606074', 'QTT0000006', 'KCO0000001')
insert into MUON (MaMuon, MaSV, MaQTT, MaKC, NgayDK, NgayMuon, TrangThai) values
	('BOR0000001', '2020608162', 'QTT0000001', 'KCO0000005', '04-15-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000002', '2020608162', 'QTT0000002', 'KCO0000005', '04-15-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000003', '2020608162', 'QTT0000003', 'KCO0000004', '04-15-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000004', '2020608162', 'QTT0000004', 'KCO0000001', '04-15-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000005', '2020608162', 'QTT0000005', 'KCO0000001', '04-15-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000006', '2020608162', 'QTT0000006', 'KCO0000001', '04-15-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000007', '2020600630', 'QTT0000001', 'KCO0000002', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000008', '2020600630', 'QTT0000002', 'KCO0000002', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000009', '2020600630', 'QTT0000003', 'KCO0000002', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000010', '2020600630', 'QTT0000004', 'KCO0000001', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000011', '2020600630', 'QTT0000005', 'KCO0000001', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000012', '2020600630', 'QTT0000006', 'KCO0000001', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000013', '2020601080', 'QTT0000001', 'KCO0000003', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000014', '2020601080', 'QTT0000002', 'KCO0000003', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000015', '2020601080', 'QTT0000003', 'KCO0000004', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000016', '2020601080', 'QTT0000004', 'KCO0000001', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000017', '2020601080', 'QTT0000005', 'KCO0000001', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000018', '2020601080', 'QTT0000006', 'KCO0000001', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000019', '2020606074', 'QTT0000001', 'KCO0000005', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000020', '2020606074', 'QTT0000002', 'KCO0000005', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000021', '2020606074', 'QTT0000003', 'KCO0000005', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000022', '2020606074', 'QTT0000004', 'KCO0000001', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000023', '2020606074', 'QTT0000005', 'KCO0000001', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000024', '2020606074', 'QTT0000006', 'KCO0000001', '04-16-2023', '05-01-2023', N'Hoàn thành'),
	('BOR0000025', '2020608162', 'QTT0000001', 'KCO0000005', '05-07-2023', '05-08-2023', N'Hoàn thành')
insert into DOI (MaDoi, MaSV, MaQTT, MaKC, NgayDK, NgayDoi, TrangThai) values
	('CHA0000001', '2020600630', 'QTT0000003', 'KCO0000003', '05-01-2023', '05-02-2023', N'Hoàn thành')
insert into MAT (MaMat, MaSV, MaQTT, MaKC, NgayMat, SoTien) values
	('LOS0000001', '2020608162', 'QTT0000001', 'KCO0000005', '05-07-2023', 40)
insert into CONG_NO (MaCN, MaSV, SoTien, HanTra) values
	('DEB0000001', '2020608162', 40, '07-05-2023')
insert into DOI (MaDoi, MaSV, MaQTT, MaKC, NgayDK) values
	('CHA0000002', '2020608162', 'QTT0000003', 'KCO0000005', '05-01-2023')