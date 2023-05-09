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
        public virtual DbSet<DangMuon> DangMuon { get; set; }
        public virtual DbSet<HoaDonDoi> HoaDonDoi { get; set; }
        public virtual DbSet<HoaDonMuon> HoaDonMuon { get; set; }
        public virtual DbSet<HoaDonThanhToan> HoaDonThanhToan { get; set; }
        public virtual DbSet<KhoaHoc> KhoaHoc { get; set; }
        public virtual DbSet<KhoanNo> KhoanNo { get; set; }
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
                    .HasName("PK__CHI_TIET__1B1CC560DF2BC1AA");

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

            modelBuilder.Entity<DangMuon>(entity =>
            {
                entity.HasKey(e => new { e.MaSv, e.MaQtt, e.MaKc })
                    .HasName("PK__DANG_MUO__3694C44CD6C0A604");

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

            modelBuilder.Entity<HoaDonDoi>(entity =>
            {
                entity.HasKey(e => e.MaHdd)
                    .HasName("PK__HOA_DON___3C90E8FCB8AFB4B6");

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

                entity.Property(e => e.MoTa).HasColumnType("text");

                entity.Property(e => e.NgayDoi).HasColumnType("date");

                entity.Property(e => e.NgayTaoHd)
                    .HasColumnName("NgayTaoHD")
                    .HasColumnType("date");

                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasDefaultValueSql("(N'Đăng ký')");

                entity.HasOne(d => d.MaKcNavigation)
                    .WithMany(p => p.HoaDonDoi)
                    .HasForeignKey(d => d.MaKc)
                    .HasConstraintName("FK__HOA_DON_DO__MaKC__46E78A0C");

                entity.HasOne(d => d.MaQttNavigation)
                    .WithMany(p => p.HoaDonDoi)
                    .HasForeignKey(d => d.MaQtt)
                    .HasConstraintName("FK__HOA_DON_D__MaQTT__45F365D3");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.HoaDonDoi)
                    .HasForeignKey(d => d.MaSv)
                    .HasConstraintName("FK__HOA_DON_DO__MaSV__44FF419A");
            });

            modelBuilder.Entity<HoaDonMuon>(entity =>
            {
                entity.HasKey(e => e.MaHdm)
                    .HasName("PK__HOA_DON___3C90E8C53D682384");

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

                entity.Property(e => e.MoTa).HasColumnType("text");

                entity.Property(e => e.NgayMuon).HasColumnType("date");

                entity.Property(e => e.NgayTaoHd)
                    .HasColumnName("NgayTaoHD")
                    .HasColumnType("date");

                entity.Property(e => e.TrangThai)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasDefaultValueSql("(N'Đăng ký')");

                entity.HasOne(d => d.MaKcNavigation)
                    .WithMany(p => p.HoaDonMuon)
                    .HasForeignKey(d => d.MaKc)
                    .HasConstraintName("FK__HOA_DON_MU__MaKC__403A8C7D");

                entity.HasOne(d => d.MaQttNavigation)
                    .WithMany(p => p.HoaDonMuon)
                    .HasForeignKey(d => d.MaQtt)
                    .HasConstraintName("FK__HOA_DON_M__MaQTT__3F466844");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.HoaDonMuon)
                    .HasForeignKey(d => d.MaSv)
                    .HasConstraintName("FK__HOA_DON_MU__MaSV__3E52440B");
            });

            modelBuilder.Entity<HoaDonThanhToan>(entity =>
            {
                entity.HasKey(e => e.MaHdtt)
                    .HasName("PK__HOA_DON___141754EBA82CF4A2");

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

                entity.Property(e => e.MoTa).HasColumnType("text");

                entity.Property(e => e.NgayTra).HasColumnType("date");

                entity.Property(e => e.SoTien).HasColumnType("money");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.HoaDonThanhToan)
                    .HasForeignKey(d => d.MaSv)
                    .HasConstraintName("FK__HOA_DON_TH__MaSV__4E88ABD4");
            });

            modelBuilder.Entity<KhoaHoc>(entity =>
            {
                entity.HasKey(e => e.MaKh)
                    .HasName("PK__KHOA_HOC__2725CF1EE4133E98");

                entity.ToTable("KHOA_HOC");

                entity.Property(e => e.MaKh)
                    .HasColumnName("MaKH")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MoTa).HasColumnType("text");

                entity.Property(e => e.NgayBd)
                    .HasColumnName("NgayBD")
                    .HasColumnType("date");

                entity.Property(e => e.NgayKt)
                    .HasColumnName("NgayKT")
                    .HasColumnType("date");
            });

            modelBuilder.Entity<KhoanNo>(entity =>
            {
                entity.HasKey(e => e.MaKn)
                    .HasName("PK__KHOAN_NO__2725CF147E3ECBAA");

                entity.ToTable("KHOAN_NO");

                entity.Property(e => e.MaKn)
                    .HasColumnName("MaKN")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.HanTra).HasColumnType("date");

                entity.Property(e => e.MaSv)
                    .HasColumnName("MaSV")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MoTa).HasColumnType("text");

                entity.Property(e => e.TienNo).HasColumnType("money");

                entity.HasOne(d => d.MaSvNavigation)
                    .WithMany(p => p.KhoanNo)
                    .HasForeignKey(d => d.MaSv)
                    .HasConstraintName("FK__KHOAN_NO__MaSV__4AB81AF0");
            });

            modelBuilder.Entity<KichCo>(entity =>
            {
                entity.HasKey(e => e.MaKc)
                    .HasName("PK__KICH_CO__2725CF039586DEC0");

                entity.ToTable("KICH_CO");

                entity.Property(e => e.MaKc)
                    .HasColumnName("MaKC")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MoTa).HasColumnType("text");

                entity.Property(e => e.TenKc)
                    .IsRequired()
                    .HasColumnName("TenKC")
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<QuanTuTrang>(entity =>
            {
                entity.HasKey(e => e.MaQtt)
                    .HasName("PK__QUAN_TU___396E9990C6E8B85B");

                entity.ToTable("QUAN_TU_TRANG");

                entity.Property(e => e.MaQtt)
                    .HasColumnName("MaQTT")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.GiaTien).HasColumnType("money");

                entity.Property(e => e.MoTa).HasColumnType("text");

                entity.Property(e => e.TenQtt)
                    .IsRequired()
                    .HasColumnName("TenQTT")
                    .HasMaxLength(35);
            });

            modelBuilder.Entity<SinhVien>(entity =>
            {
                entity.HasKey(e => e.MaSv)
                    .HasName("PK__SINH_VIE__2725081AAA977BE5");

                entity.ToTable("SINH_VIEN");

                entity.Property(e => e.MaSv)
                    .HasColumnName("MaSV")
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Anh).IsRequired();

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

                entity.Property(e => e.MoTa).HasColumnType("text");

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
