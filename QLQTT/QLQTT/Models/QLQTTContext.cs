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
        public virtual DbSet<Doi> Doi { get; set; }
        public virtual DbSet<HoaDonThanhToan> HoaDonThanhToan { get; set; }
        public virtual DbSet<KhoaHoc> KhoaHoc { get; set; }
        public virtual DbSet<KichCo> KichCo { get; set; }
        public virtual DbSet<Mat> Mat { get; set; }
        public virtual DbSet<Muon> Muon { get; set; }
        public virtual DbSet<QuanTuTrang> QuanTuTrang { get; set; }
        public virtual DbSet<SinhVien> SinhVien { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-LRK3UC2;Initial Catalog=QLQTT;Integrated Security=True")
                .EnableSensitiveDataLogging();
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChiTietQtt>(entity =>
            {
                entity.HasKey(e => new { e.MaQtt, e.MaKc })
                    .HasName("PK__CHI_TIET__1B1CC560BC25FB29");

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
                    .HasName("PK__CONG_NO__27258E0E86FAE27A");

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

                entity.Property(e => e.SoTien).HasColumnType("money");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.CongNo)
                    .HasForeignKey(d => d.MaSv)
                    .HasConstraintName("FK__CONG_NO__MaSV__4E88ABD4");
            });

            modelBuilder.Entity<DangMuon>(entity =>
            {
                entity.HasKey(e => new { e.MaSv, e.MaQtt, e.MaKc })
                    .HasName("PK__DANG_MUO__3694C44C0A78F411");

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

            modelBuilder.Entity<Doi>(entity =>
            {
                entity.HasKey(e => e.MaDoi)
                    .HasName("PK__DOI__3D89F553F8C444BB");

                entity.ToTable("DOI");

                entity.Property(e => e.MaDoi)
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

                entity.Property(e => e.NgayDk)
                    .HasColumnName("NgayDK")
                    .HasColumnType("date");

                entity.Property(e => e.NgayDoi).HasColumnType("date");

                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Đang xử lý')");

                entity.HasOne(d => d.MaKcNavigation)
                    .WithMany(p => p.Doi)
                    .HasForeignKey(d => d.MaKc)
                    .HasConstraintName("FK__DOI__MaKC__440B1D61");

                entity.HasOne(d => d.MaQttNavigation)
                    .WithMany(p => p.Doi)
                    .HasForeignKey(d => d.MaQtt)
                    .HasConstraintName("FK__DOI__MaQTT__4316F928");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.Doi)
                    .HasForeignKey(d => d.MaSv)
                    .HasConstraintName("FK__DOI__MaSV__4222D4EF");
            });

            modelBuilder.Entity<HoaDonThanhToan>(entity =>
            {
                entity.HasKey(e => e.MaHdtt)
                    .HasName("PK__HOA_DON___141754EB507F6082");

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
                    .HasConstraintName("FK__HOA_DON_TH__MaSV__52593CB8");
            });

            modelBuilder.Entity<KhoaHoc>(entity =>
            {
                entity.HasKey(e => e.MaKh)
                    .HasName("PK__KHOA_HOC__2725CF1E1EE76788");

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
                    .HasName("PK__KICH_CO__2725CF03C33AA33F");

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

            modelBuilder.Entity<Mat>(entity =>
            {
                entity.HasKey(e => e.MaMat)
                    .HasName("PK__MAT__3A5BBB7C563616FE");

                entity.ToTable("MAT");

                entity.Property(e => e.MaMat)
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

                entity.Property(e => e.NgayMat).HasColumnType("date");

                entity.Property(e => e.SoTien).HasColumnType("money");

                entity.HasOne(d => d.MaKcNavigation)
                    .WithMany(p => p.Mat)
                    .HasForeignKey(d => d.MaKc)
                    .HasConstraintName("FK__MAT__MaKC__4AB81AF0");

                entity.HasOne(d => d.MaQttNavigation)
                    .WithMany(p => p.Mat)
                    .HasForeignKey(d => d.MaQtt)
                    .HasConstraintName("FK__MAT__MaQTT__49C3F6B7");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.Mat)
                    .HasForeignKey(d => d.MaSv)
                    .HasConstraintName("FK__MAT__MaSV__48CFD27E");
            });

            modelBuilder.Entity<Muon>(entity =>
            {
                entity.HasKey(e => e.MaMuon)
                    .HasName("PK__MUON__0A9BE5E0D6742A7A");

                entity.ToTable("MUON");

                entity.Property(e => e.MaMuon)
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

                entity.Property(e => e.NgayDk)
                    .HasColumnName("NgayDK")
                    .HasColumnType("date");

                entity.Property(e => e.NgayMuon).HasColumnType("date");

                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("(N'Đang xử lý')");

                entity.HasOne(d => d.MaKcNavigation)
                    .WithMany(p => p.Muon)
                    .HasForeignKey(d => d.MaKc)
                    .HasConstraintName("FK__MUON__MaKC__3D5E1FD2");

                entity.HasOne(d => d.MaQttNavigation)
                    .WithMany(p => p.Muon)
                    .HasForeignKey(d => d.MaQtt)
                    .HasConstraintName("FK__MUON__MaQTT__3C69FB99");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.Muon)
                    .HasForeignKey(d => d.MaSv)
                    .HasConstraintName("FK__MUON__MaSV__3B75D760");
            });

            modelBuilder.Entity<QuanTuTrang>(entity =>
            {
                entity.HasKey(e => e.MaQtt)
                    .HasName("PK__QUAN_TU___396E999043D3C6A7");

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
                    .HasName("PK__SINH_VIE__2725081AAC0786E8");

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
                    .IsUnicode(false);

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
