using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QLQTT.Models
{
    public partial class QLQTTContext : DbContext
    {
        public QLQTTContext()
        {
        }

        public QLQTTContext(DbContextOptions<QLQTTContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChiTietQtt> ChiTietQtt { get; set; }
        public virtual DbSet<CongNo> CongNo { get; set; }
        public virtual DbSet<DangMuon> DangMuon { get; set; }
        public virtual DbSet<HoaDonDoi> HoaDonDoi { get; set; }
        public virtual DbSet<HoaDonMuon> HoaDonMuon { get; set; }
        public virtual DbSet<HoaDonThanhToan> HoaDonThanhToan { get; set; }
        public virtual DbSet<KhoaHoc> KhoaHoc { get; set; }
        public virtual DbSet<KichCo> KichCo { get; set; }
        public virtual DbSet<QuanTuTrang> QuanTuTrang { get; set; }
        public virtual DbSet<SinhVien> SinhVien { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-M75UEQH\\SQLEXPRESS;Initial Catalog=QLQTT;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietQtt>(entity =>
            {
                entity.HasKey(e => new { e.MaQtt, e.MaKc })
                    .HasName("PK__CHI_TIET__1B1CC560E60A82DA");

                entity.ToTable("CHI_TIET_QTT");

                entity.Property(e => e.MaQtt)
                    .HasColumnName("MaQTT")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaKc)
                    .HasColumnName("MaKC")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.SoLuongCt).HasColumnName("SoLuongCT");

                entity.HasOne(d => d.MaKcNavigation)
                    .WithMany(p => p.ChiTietQtt)
                    .HasForeignKey(d => d.MaKc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CHI_TIET_Q__MaKC__2F10007B");

                entity.HasOne(d => d.MaQttNavigation)
                    .WithMany(p => p.ChiTietQtt)
                    .HasForeignKey(d => d.MaQtt)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CHI_TIET___MaQTT__2E1BDC42");
            });

            modelBuilder.Entity<CongNo>(entity =>
            {
                entity.HasKey(e => e.MaCn)
                    .HasName("PK__CONG_NO__27258E0E9632AEEF");

                entity.ToTable("CONG_NO");

                entity.Property(e => e.MaCn)
                    .HasColumnName("MaCN")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HanTra).HasColumnType("date");

                entity.Property(e => e.MaSv)
                    .HasColumnName("MaSV")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MoTa).HasMaxLength(50);

                entity.Property(e => e.TienNo).HasColumnType("money");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.CongNo)
                    .HasForeignKey(d => d.MaSv)
                    .HasConstraintName("FK__CONG_NO__MaSV__49C3F6B7");
            });

            modelBuilder.Entity<DangMuon>(entity =>
            {
                entity.HasKey(e => new { e.MaSv, e.MaQtt, e.MaKc })
                    .HasName("PK__DANG_MUO__3694C44CA9F605F9");

                entity.ToTable("DANG_MUON");

                entity.Property(e => e.MaSv)
                    .HasColumnName("MaSV")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaQtt)
                    .HasColumnName("MaQTT")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaKc)
                    .HasColumnName("MaKC")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Đang mượn')");

                entity.HasOne(d => d.MaKcNavigation)
                    .WithMany(p => p.DangMuon)
                    .HasForeignKey(d => d.MaKc)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DANG_MUON__MaKC__38996AB5");

                entity.HasOne(d => d.MaQttNavigation)
                    .WithMany(p => p.DangMuon)
                    .HasForeignKey(d => d.MaQtt)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DANG_MUON__MaQTT__37A5467C");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.DangMuon)
                    .HasForeignKey(d => d.MaSv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DANG_MUON__MaSV__36B12243");
            });

            modelBuilder.Entity<HoaDonDoi>(entity =>
            {
                entity.HasKey(e => e.MaHdd)
                    .HasName("PK__HOA_DON___3C90E8FCE1F35803");

                entity.ToTable("HOA_DON_DOI");

                entity.Property(e => e.MaHdd)
                    .HasColumnName("MaHDD")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaKc)
                    .HasColumnName("MaKC")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaQtt)
                    .HasColumnName("MaQTT")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaSv)
                    .HasColumnName("MaSV")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MoTa).HasMaxLength(50);

                entity.Property(e => e.NgayDoi).HasColumnType("date");

                entity.Property(e => e.NgayTaoHd)
                    .HasColumnName("NgayTaoHD")
                    .HasColumnType("date");

                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Đăng ký')");

                entity.HasOne(d => d.MaKcNavigation)
                    .WithMany(p => p.HoaDonDoi)
                    .HasForeignKey(d => d.MaKc)
                    .HasConstraintName("FK__HOA_DON_DO__MaKC__44FF419A");

                entity.HasOne(d => d.MaQttNavigation)
                    .WithMany(p => p.HoaDonDoi)
                    .HasForeignKey(d => d.MaQtt)
                    .HasConstraintName("FK__HOA_DON_D__MaQTT__440B1D61");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.HoaDonDoi)
                    .HasForeignKey(d => d.MaSv)
                    .HasConstraintName("FK__HOA_DON_DO__MaSV__4316F928");
            });

            modelBuilder.Entity<HoaDonMuon>(entity =>
            {
                entity.HasKey(e => e.MaHdm)
                    .HasName("PK__HOA_DON___3C90E8C5EE1E19BB");

                entity.ToTable("HOA_DON_MUON");

                entity.Property(e => e.MaHdm)
                    .HasColumnName("MaHDM")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaKc)
                    .HasColumnName("MaKC")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaQtt)
                    .HasColumnName("MaQTT")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaSv)
                    .HasColumnName("MaSV")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MoTa).HasMaxLength(50);

                entity.Property(e => e.NgayMuon).HasColumnType("date");

                entity.Property(e => e.NgayTaoHd)
                    .HasColumnName("NgayTaoHD")
                    .HasColumnType("date");

                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Đăng ký')");

                entity.HasOne(d => d.MaKcNavigation)
                    .WithMany(p => p.HoaDonMuon)
                    .HasForeignKey(d => d.MaKc)
                    .HasConstraintName("FK__HOA_DON_MU__MaKC__3E52440B");

                entity.HasOne(d => d.MaQttNavigation)
                    .WithMany(p => p.HoaDonMuon)
                    .HasForeignKey(d => d.MaQtt)
                    .HasConstraintName("FK__HOA_DON_M__MaQTT__3D5E1FD2");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.HoaDonMuon)
                    .HasForeignKey(d => d.MaSv)
                    .HasConstraintName("FK__HOA_DON_MU__MaSV__3C69FB99");
            });

            modelBuilder.Entity<HoaDonThanhToan>(entity =>
            {
                entity.HasKey(e => e.MaHdtt)
                    .HasName("PK__HOA_DON___141754EBB9B45D24");

                entity.ToTable("HOA_DON_THANH_TOAN");

                entity.Property(e => e.MaHdtt)
                    .HasColumnName("MaHDTT")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaSv)
                    .HasColumnName("MaSV")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MoTa).HasMaxLength(50);

                entity.Property(e => e.NgayTra).HasColumnType("date");

                entity.Property(e => e.SoTien).HasColumnType("money");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.HoaDonThanhToan)
                    .HasForeignKey(d => d.MaSv)
                    .HasConstraintName("FK__HOA_DON_TH__MaSV__4D94879B");
            });

            modelBuilder.Entity<KhoaHoc>(entity =>
            {
                entity.HasKey(e => e.MaKh)
                    .HasName("PK__KHOA_HOC__2725CF1E17352482");

                entity.ToTable("KHOA_HOC");

                entity.Property(e => e.MaKh)
                    .HasColumnName("MaKH")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MoTa).HasMaxLength(50);

                entity.Property(e => e.NgayBd)
                    .HasColumnName("NgayBD")
                    .HasColumnType("date");

                entity.Property(e => e.NgayKt)
                    .HasColumnName("NgayKT")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<KichCo>(entity =>
            {
                entity.HasKey(e => e.MaKc)
                    .HasName("PK__KICH_CO__2725CF03213D198D");

                entity.ToTable("KICH_CO");

                entity.Property(e => e.MaKc)
                    .HasColumnName("MaKC")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MoTa).HasMaxLength(50);

                entity.Property(e => e.TenKc)
                    .IsRequired()
                    .HasColumnName("TenKC")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<QuanTuTrang>(entity =>
            {
                entity.HasKey(e => e.MaQtt)
                    .HasName("PK__QUAN_TU___396E9990209CA574");

                entity.ToTable("QUAN_TU_TRANG");

                entity.Property(e => e.MaQtt)
                    .HasColumnName("MaQTT")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GiaTien).HasColumnType("money");

                entity.Property(e => e.MoTa).HasMaxLength(50);

                entity.Property(e => e.TenQtt)
                    .IsRequired()
                    .HasColumnName("TenQTT")
                    .HasMaxLength(35);
            });

            modelBuilder.Entity<SinhVien>(entity =>
            {
                entity.HasKey(e => e.MaSv)
                    .HasName("PK__SINH_VIE__2725081ADE5B0C53");

                entity.ToTable("SINH_VIEN");

                entity.Property(e => e.MaSv)
                    .HasColumnName("MaSV")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaKh)
                    .HasColumnName("MaKH")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MatKhau)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MoTa).HasMaxLength(50);

                entity.Property(e => e.TenSv)
                    .IsRequired()
                    .HasColumnName("TenSV")
                    .HasMaxLength(35);

                entity.HasOne(d => d.MaKhNavigation)
                    .WithMany(p => p.SinhVien)
                    .HasForeignKey(d => d.MaKh)
                    .HasConstraintName("FK__SINH_VIEN__MaKH__33D4B598");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
