using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ECFA_MVC.Models;

public partial class EcfaContext : DbContext
{
    public EcfaContext()
    {
    }

    public EcfaContext(DbContextOptions<EcfaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Action> Actions { get; set; }

    public virtual DbSet<Atsfaq> Atsfaqs { get; set; }

    public virtual DbSet<AtsfaqBak> AtsfaqBaks { get; set; }

    public virtual DbSet<CodeList267> CodeList267s { get; set; }

    public virtual DbSet<CodeList539> CodeList539s { get; set; }

    public virtual DbSet<Db版早收清單539> Db版早收清單539s { get; set; }

    public virtual DbSet<Dmad> Dmads { get; set; }

    public virtual DbSet<DownloadCount> DownloadCounts { get; set; }

    public virtual DbSet<Ecfadetail> Ecfadetails { get; set; }

    public virtual DbSet<Ecfadoc> Ecfadocs { get; set; }

    public virtual DbSet<Ecfamenu> Ecfamenus { get; set; }

    public virtual DbSet<Ecfamenu201311Bk> Ecfamenu201311Bks { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Faq> Faqs { get; set; }

    public virtual DbSet<Forum> Forums { get; set; }

    public virtual DbSet<Hdmad> Hdmads { get; set; }

    public virtual DbSet<Law> Laws { get; set; }

    public virtual DbSet<LawType> LawTypes { get; set; }

    public virtual DbSet<Link> Links { get; set; }

    public virtual DbSet<LogCount> LogCounts { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsAttachment> NewsAttachments { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<PageAttachment> PageAttachments { get; set; }

    public virtual DbSet<PagesSub> PagesSubs { get; set; }

    public virtual DbSet<PrgMenu> PrgMenus { get; set; }

    public virtual DbSet<PrgMenu201110> PrgMenu201110s { get; set; }

    public virtual DbSet<RelatedDoc> RelatedDocs { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<SlideNews> SlideNews { get; set; }

    public virtual DbSet<UrlList> UrlLists { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<WebAd> WebAds { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Chinese_Taiwan_Stroke_90_CI_AS");

        modelBuilder.Entity<Action>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("Action");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAttachment)
                .HasMaxLength(200)
                .HasColumnName("ntAttachment");
            entity.Property(e => e.NtAttr1)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr1");
            entity.Property(e => e.NtAttr2)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr2");
            entity.Property(e => e.NtAttr3)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr3");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtContent)
                .HasColumnType("ntext")
                .HasColumnName("ntContent");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        modelBuilder.Entity<Atsfaq>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("ATSFAQ");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtContent).HasColumnName("ntContent");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtRelated)
                .HasMaxLength(300)
                .HasColumnName("ntRelated");
            entity.Property(e => e.NtTitle).HasColumnName("ntTitle");
        });

        modelBuilder.Entity<AtsfaqBak>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ATSFAQ_bak");

            entity.Property(e => e.NtContent).HasColumnName("ntContent");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ntId");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtRelated)
                .HasMaxLength(300)
                .HasColumnName("ntRelated");
            entity.Property(e => e.NtTitle).HasColumnName("ntTitle");
        });

        modelBuilder.Entity<CodeList267>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CodeList267");

            entity.Property(e => e.CnCname).HasColumnName("CN_CNAME");
            entity.Property(e => e.CnCode)
                .HasMaxLength(255)
                .HasColumnName("CN_CODE");
            entity.Property(e => e.CnEname).HasColumnName("CN_ENAME");
            entity.Property(e => e.CnTax)
                .HasMaxLength(25)
                .HasColumnName("CN_TAX");
            entity.Property(e => e.Ex)
                .HasMaxLength(25)
                .HasColumnName("EX");
            entity.Property(e => e.Memo).HasColumnName("MEMO");
            entity.Property(e => e.Psr).HasColumnName("PSR");
            entity.Property(e => e.Sk)
                .HasMaxLength(255)
                .HasColumnName("SK");
            entity.Property(e => e.TwCname).HasColumnName("TW_CNAME");
            entity.Property(e => e.TwCode)
                .HasMaxLength(100)
                .HasColumnName("TW_CODE");
            entity.Property(e => e.TwEname).HasColumnName("TW_ENAME");
            entity.Property(e => e.TwTax)
                .HasMaxLength(25)
                .HasColumnName("TW_TAX");
        });

        modelBuilder.Entity<CodeList539>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("CodeList539");

            entity.Property(e => e.CnCname).HasColumnName("CN_CNAME");
            entity.Property(e => e.CnCode)
                .HasMaxLength(255)
                .HasColumnName("CN_CODE");
            entity.Property(e => e.CnEname).HasColumnName("CN_ENAME");
            entity.Property(e => e.CnTax)
                .HasMaxLength(255)
                .HasColumnName("CN_TAX");
            entity.Property(e => e.Ex)
                .HasMaxLength(255)
                .HasColumnName("EX");
            entity.Property(e => e.Memo).HasColumnName("MEMO");
            entity.Property(e => e.Psr).HasColumnName("PSR");
            entity.Property(e => e.Sk)
                .HasMaxLength(255)
                .HasColumnName("SK");
            entity.Property(e => e.TwCname).HasColumnName("TW_CNAME");
            entity.Property(e => e.TwCode)
                .HasMaxLength(255)
                .HasColumnName("TW_CODE");
            entity.Property(e => e.TwEname).HasColumnName("TW_ENAME");
            entity.Property(e => e.TwTax)
                .HasMaxLength(255)
                .HasColumnName("TW_TAX");
        });

        modelBuilder.Entity<Db版早收清單539>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DB版早收清單539");

            entity.Property(e => e.資料行0)
                .HasMaxLength(50)
                .HasColumnName("資料行 0");
            entity.Property(e => e.資料行1)
                .HasMaxLength(50)
                .HasColumnName("資料行 1");
            entity.Property(e => e.資料行2)
                .HasMaxLength(50)
                .HasColumnName("資料行 2");
            entity.Property(e => e.資料行3)
                .HasMaxLength(50)
                .HasColumnName("資料行 3");
            entity.Property(e => e.資料行4)
                .HasMaxLength(50)
                .HasColumnName("資料行 4");
            entity.Property(e => e.資料行5)
                .HasMaxLength(50)
                .HasColumnName("資料行 5");
            entity.Property(e => e.資料行6)
                .HasMaxLength(50)
                .HasColumnName("資料行 6");
            entity.Property(e => e.資料行7)
                .HasMaxLength(50)
                .HasColumnName("資料行 7");
            entity.Property(e => e.資料行8)
                .HasMaxLength(50)
                .HasColumnName("資料行 8");
            entity.Property(e => e.資料行9)
                .HasMaxLength(50)
                .HasColumnName("資料行 9");
        });

        modelBuilder.Entity<Dmad>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("DMAd");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAttachment)
                .HasMaxLength(200)
                .HasColumnName("ntAttachment");
            entity.Property(e => e.NtAttr1)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr1");
            entity.Property(e => e.NtAttr2)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr2");
            entity.Property(e => e.NtAttr3)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr3");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtCategory).HasColumnName("ntCategory");
            entity.Property(e => e.NtContent)
                .HasColumnType("ntext")
                .HasColumnName("ntContent");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtFile).HasColumnName("ntFile");
            entity.Property(e => e.NtFile2).HasColumnName("ntFile2");
            entity.Property(e => e.NtFileName)
                .HasMaxLength(150)
                .HasColumnName("ntFileName");
            entity.Property(e => e.NtFileName2)
                .HasMaxLength(150)
                .HasColumnName("ntFileName2");
            entity.Property(e => e.NtFileSize)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ntFileSize");
            entity.Property(e => e.NtFileSize2)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ntFileSize2");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        modelBuilder.Entity<DownloadCount>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("DownloadCount");

            entity.Property(e => e.LcCount).HasColumnName("lcCount");
            entity.Property(e => e.LcId).HasColumnName("lcId");
            entity.Property(e => e.LcModifyDate)
                .HasColumnType("datetime")
                .HasColumnName("lcModifyDate");
            entity.Property(e => e.LcSource)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("lcSource");
            entity.Property(e => e.LcTitle)
                .HasMaxLength(200)
                .HasColumnName("lcTitle");
        });

        modelBuilder.Entity<Ecfadetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("ECFAdetail");

            entity.Property(e => e.Keyame)
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasColumnName("keyame");
            entity.Property(e => e.NtAttachment)
                .HasMaxLength(200)
                .HasColumnName("ntAttachment");
            entity.Property(e => e.NtAttr1)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr1");
            entity.Property(e => e.NtAttr2)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr2");
            entity.Property(e => e.NtAttr3)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr3");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtContent)
                .HasColumnType("ntext")
                .HasColumnName("ntContent");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle).HasColumnName("ntTitle");
            entity.Property(e => e.Tblname)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasColumnName("tblname");
        });

        modelBuilder.Entity<Ecfadoc>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("ECFADoc");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAttachment)
                .HasMaxLength(200)
                .HasColumnName("ntAttachment");
            entity.Property(e => e.NtAttr1)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr1");
            entity.Property(e => e.NtAttr2)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr2");
            entity.Property(e => e.NtAttr3)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr3");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtCategory).HasColumnName("ntCategory");
            entity.Property(e => e.NtContent)
                .HasColumnType("ntext")
                .HasColumnName("ntContent");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtFile).HasColumnName("ntFile");
            entity.Property(e => e.NtFileName)
                .HasMaxLength(150)
                .HasColumnName("ntFileName");
            entity.Property(e => e.NtFileSize)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ntFileSize");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        modelBuilder.Entity<Ecfamenu>(entity =>
        {
            entity.HasKey(e => e.NodeId);

            entity.ToTable("ECFAMenu");

            entity.Property(e => e.Block).HasColumnName("BLOCK");
            entity.Property(e => e.IconPath).HasMaxLength(200);
            entity.Property(e => e.NodeName).HasMaxLength(200);
            entity.Property(e => e.NodeType)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.PrgPath).HasMaxLength(200);
            entity.Property(e => e.PrgTarget).HasMaxLength(50);
            entity.Property(e => e.SetDate).HasColumnType("datetime");
            entity.Property(e => e.SetUser).HasMaxLength(50);
        });

        modelBuilder.Entity<Ecfamenu201311Bk>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("ECFAMenu201311_bk");

            entity.Property(e => e.IconPath).HasMaxLength(200);
            entity.Property(e => e.NodeId).ValueGeneratedOnAdd();
            entity.Property(e => e.NodeName).HasMaxLength(200);
            entity.Property(e => e.PrgPath).HasMaxLength(200);
            entity.Property(e => e.PrgTarget).HasMaxLength(50);
            entity.Property(e => e.SetDate).HasColumnType("datetime");
            entity.Property(e => e.SetUser).HasMaxLength(50);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("Event");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAttachment)
                .HasMaxLength(200)
                .HasColumnName("ntAttachment");
            entity.Property(e => e.NtAttr1)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr1");
            entity.Property(e => e.NtAttr2)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr2");
            entity.Property(e => e.NtAttr3)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr3");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtContent)
                .HasColumnType("ntext")
                .HasColumnName("ntContent");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        modelBuilder.Entity<Faq>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("FAQ");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAttachment)
                .HasMaxLength(200)
                .HasColumnName("ntAttachment");
            entity.Property(e => e.NtAttr1)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr1");
            entity.Property(e => e.NtAttr2)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr2");
            entity.Property(e => e.NtAttr3)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr3");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtContent)
                .HasColumnType("ntext")
                .HasColumnName("ntContent");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        modelBuilder.Entity<Forum>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("Forum");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAttachment)
                .HasMaxLength(200)
                .HasColumnName("ntAttachment");
            entity.Property(e => e.NtAttr1)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr1");
            entity.Property(e => e.NtAttr2)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr2");
            entity.Property(e => e.NtAttr3)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr3");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtContent)
                .HasColumnType("ntext")
                .HasColumnName("ntContent");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtFile).HasColumnName("ntFile");
            entity.Property(e => e.NtFile2).HasColumnName("ntFile2");
            entity.Property(e => e.NtFileName)
                .HasMaxLength(150)
                .HasColumnName("ntFileName");
            entity.Property(e => e.NtFileName2)
                .HasMaxLength(150)
                .HasColumnName("ntFileName2");
            entity.Property(e => e.NtFileSize)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ntFileSize");
            entity.Property(e => e.NtFileSize2)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ntFileSize2");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        modelBuilder.Entity<Hdmad>(entity =>
        {
            entity.HasKey(e => e.NtId).HasName("PK_HomeDMAd");

            entity.ToTable("HDMAd");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtFile).HasColumnName("ntFile");
            entity.Property(e => e.NtFile2).HasColumnName("ntFile2");
            entity.Property(e => e.NtFileName)
                .HasMaxLength(150)
                .HasColumnName("ntFileName");
            entity.Property(e => e.NtFileName2)
                .HasMaxLength(150)
                .HasColumnName("ntFileName2");
            entity.Property(e => e.NtFileSize)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ntFileSize");
            entity.Property(e => e.NtFileSize2)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ntFileSize2");
            entity.Property(e => e.NtModHistory).HasColumnName("ntModHistory");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        modelBuilder.Entity<Law>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("Law");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtCrDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrDate");
            entity.Property(e => e.NtCrUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrUser");
            entity.Property(e => e.NtFile).HasColumnName("ntFile");
            entity.Property(e => e.NtFileName)
                .HasMaxLength(200)
                .HasColumnName("ntFileName");
            entity.Property(e => e.NtKeyWord)
                .HasMaxLength(300)
                .HasColumnName("ntKeyWord");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtPubDate)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(300)
                .HasColumnName("ntTitle");
            entity.Property(e => e.NtTypeId).HasColumnName("ntTypeId");
        });

        modelBuilder.Entity<LawType>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("LawType");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtCrDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrDate");
            entity.Property(e => e.NtCrUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrUser");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtNodeId).HasColumnName("ntNodeId");
            entity.Property(e => e.NtTypeName)
                .HasMaxLength(200)
                .HasColumnName("ntTypeName");
        });

        modelBuilder.Entity<Link>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(50)
                .HasColumnName("ntTitle");
            entity.Property(e => e.Target).HasMaxLength(50);
            entity.Property(e => e.Type)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.UpdDate).HasColumnType("datetime");
            entity.Property(e => e.UpdUser).HasMaxLength(50);
        });

        modelBuilder.Entity<LogCount>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("LogCount");

            entity.Property(e => e.LcCount).HasColumnName("lcCount");
            entity.Property(e => e.LcId).HasColumnName("lcId");
            entity.Property(e => e.LcModifyDate)
                .HasColumnType("datetime")
                .HasColumnName("lcModifyDate");
            entity.Property(e => e.LcNodeId).HasColumnName("lcNodeId");
            entity.Property(e => e.LcParentId).HasColumnName("lcParentId");
            entity.Property(e => e.LcTarget)
                .HasMaxLength(50)
                .HasColumnName("lcTarget");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAttachment)
                .HasMaxLength(200)
                .HasColumnName("ntAttachment");
            entity.Property(e => e.NtAttr1)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr1");
            entity.Property(e => e.NtAttr2)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr2");
            entity.Property(e => e.NtAttr3)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr3");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtContent)
                .HasColumnType("ntext")
                .HasColumnName("ntContent");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        modelBuilder.Entity<NewsAttachment>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("NewsAttachment");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtFile).HasColumnName("ntFile");
            entity.Property(e => e.NtFileName)
                .HasMaxLength(150)
                .HasColumnName("ntFileName");
            entity.Property(e => e.NtFileSize)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ntFileSize");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAttachment)
                .HasMaxLength(200)
                .HasColumnName("ntAttachment");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtContent).HasColumnName("ntContent");
            entity.Property(e => e.NtModHistory).HasColumnName("ntModHistory");
            entity.Property(e => e.NtNodeId).HasColumnName("ntNodeId");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        modelBuilder.Entity<PageAttachment>(entity =>
        {
            entity.HasKey(e => e.NtId).HasName("PK_PagesAttr");

            entity.ToTable("PageAttachment");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtFile).HasColumnName("ntFile");
            entity.Property(e => e.NtFileName)
                .HasMaxLength(150)
                .HasColumnName("ntFileName");
            entity.Property(e => e.NtFileSize)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ntFileSize");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtSort).HasColumnName("ntSort");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        modelBuilder.Entity<PagesSub>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("Pages_Sub");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtContent).HasColumnName("ntContent");
            entity.Property(e => e.NtPageId).HasColumnName("ntPageId");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtSort).HasColumnName("ntSort");
        });

        modelBuilder.Entity<PrgMenu>(entity =>
        {
            entity.HasKey(e => e.NodeId);

            entity.ToTable("PrgMenu");

            entity.Property(e => e.AuthCode).HasMaxLength(50);
            entity.Property(e => e.IconPath).HasMaxLength(200);
            entity.Property(e => e.NodeName).HasMaxLength(200);
            entity.Property(e => e.Old)
                .HasMaxLength(200)
                .HasColumnName("old");
            entity.Property(e => e.PrgId).HasMaxLength(200);
            entity.Property(e => e.PrgPath).HasMaxLength(200);
            entity.Property(e => e.PrgTarget).HasMaxLength(50);
            entity.Property(e => e.SetDate).HasColumnType("datetime");
            entity.Property(e => e.SetUser).HasMaxLength(50);
        });

        modelBuilder.Entity<PrgMenu201110>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("PrgMenu_201110");

            entity.Property(e => e.AuthCode).HasMaxLength(50);
            entity.Property(e => e.IconPath).HasMaxLength(200);
            entity.Property(e => e.NodeId).ValueGeneratedOnAdd();
            entity.Property(e => e.NodeName).HasMaxLength(200);
            entity.Property(e => e.PrgId).HasMaxLength(200);
            entity.Property(e => e.PrgPath).HasMaxLength(200);
            entity.Property(e => e.PrgTarget).HasMaxLength(50);
            entity.Property(e => e.SetDate).HasColumnType("datetime");
            entity.Property(e => e.SetUser).HasMaxLength(50);
        });

        modelBuilder.Entity<RelatedDoc>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("RelatedDoc");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtContent).HasColumnName("ntContent");
            entity.Property(e => e.NtNodeId).HasColumnName("ntNodeId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("Service");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAttachment)
                .HasMaxLength(200)
                .HasColumnName("ntAttachment");
            entity.Property(e => e.NtAttr1)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr1");
            entity.Property(e => e.NtAttr2)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr2");
            entity.Property(e => e.NtAttr3)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr3");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtContent)
                .HasColumnType("ntext")
                .HasColumnName("ntContent");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtFile).HasColumnName("ntFile");
            entity.Property(e => e.NtFileName)
                .HasMaxLength(150)
                .HasColumnName("ntFileName");
            entity.Property(e => e.NtFileSize)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ntFileSize");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        modelBuilder.Entity<SlideNews>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtContent)
                .HasColumnType("ntext")
                .HasColumnName("ntContent");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
        });

        modelBuilder.Entity<UrlList>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("UrlList");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAttachment)
                .HasMaxLength(200)
                .HasColumnName("ntAttachment");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.Url).HasColumnName("url");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Sequence);

            entity.ToTable("users");

            entity.Property(e => e.Sequence).HasColumnName("sequence");
            entity.Property(e => e.AuthCode).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.SetDate).HasColumnType("datetime");
            entity.Property(e => e.SetUser).HasMaxLength(50);
            entity.Property(e => e.UserGroup).HasMaxLength(50);
            entity.Property(e => e.UserId).HasMaxLength(50);
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.Ntid);

            entity.ToTable("Video");

            entity.Property(e => e.Ntid).HasColumnName("ntid");
            entity.Property(e => e.NtBlock).HasColumnName("ntBlock");
            entity.Property(e => e.NtCrDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrDate");
            entity.Property(e => e.NtCrUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrUser");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtPubDate)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtSort).HasColumnName("ntSort");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(100)
                .HasColumnName("ntTitle");
            entity.Property(e => e.NtUrl)
                .HasMaxLength(255)
                .HasColumnName("ntUrl");
        });

        modelBuilder.Entity<WebAd>(entity =>
        {
            entity.HasKey(e => e.NtId);

            entity.ToTable("WebAd");

            entity.Property(e => e.NtId).HasColumnName("ntId");
            entity.Property(e => e.NtAttachment)
                .HasMaxLength(200)
                .HasColumnName("ntAttachment");
            entity.Property(e => e.NtAttr1)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr1");
            entity.Property(e => e.NtAttr2)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr2");
            entity.Property(e => e.NtAttr3)
                .HasColumnType("ntext")
                .HasColumnName("ntAttr3");
            entity.Property(e => e.NtAuthor)
                .HasMaxLength(200)
                .HasColumnName("ntAuthor");
            entity.Property(e => e.NtContent)
                .HasColumnType("ntext")
                .HasColumnName("ntContent");
            entity.Property(e => e.NtCrtDate)
                .HasColumnType("datetime")
                .HasColumnName("ntCrtDate");
            entity.Property(e => e.NtCrtUser)
                .HasMaxLength(50)
                .HasColumnName("ntCrtUser");
            entity.Property(e => e.NtFile).HasColumnName("ntFile");
            entity.Property(e => e.NtFileName)
                .HasMaxLength(150)
                .HasColumnName("ntFileName");
            entity.Property(e => e.NtFileSize)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ntFileSize");
            entity.Property(e => e.NtModDate)
                .HasColumnType("datetime")
                .HasColumnName("ntModDate");
            entity.Property(e => e.NtModUser)
                .HasMaxLength(50)
                .HasColumnName("ntModUser");
            entity.Property(e => e.NtParentId).HasColumnName("ntParentId");
            entity.Property(e => e.NtPubDate)
                .HasColumnType("datetime")
                .HasColumnName("ntPubDate");
            entity.Property(e => e.NtTitle)
                .HasMaxLength(200)
                .HasColumnName("ntTitle");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
