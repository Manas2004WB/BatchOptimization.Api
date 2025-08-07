using System;
using System.Collections.Generic;
using BatchOptimization.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BatchOptimization.Api.Data;

public partial class BatchDbContext : DbContext
{
    public BatchDbContext()
    {
    }

    public BatchDbContext(DbContextOptions<BatchDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BatchStatuses> BatchStatuses { get; set; }

    public virtual DbSet<Batches> Batches { get; set; }

    public virtual DbSet<CalibrationData> CalibrationData { get; set; }

    public virtual DbSet<CalibrationStatus> CalibrationStatus { get; set; }

    public virtual DbSet<CalibrationType> CalibrationType { get; set; }

    public virtual DbSet<ColorimeterInstruments> ColorimeterInstruments { get; set; }

    public virtual DbSet<CommentHistory> CommentHistory { get; set; }

    public virtual DbSet<Pages> Pages { get; set; }

    public virtual DbSet<Plants> Plants { get; set; }

    public virtual DbSet<Predictions> Predictions { get; set; }

    public virtual DbSet<ProductTypes> ProductTypes { get; set; }

    public virtual DbSet<RolePageAccesses> RolePageAccesses { get; set; }

    public virtual DbSet<ShotMeasurements> ShotMeasurements { get; set; }

    public virtual DbSet<ShotTinters> ShotTinters { get; set; }

    public virtual DbSet<Shots> Shots { get; set; }

    public virtual DbSet<SkuVersionMeasurements> SkuVersionMeasurements { get; set; }

    public virtual DbSet<SkuVersions> SkuVersions { get; set; }

    public virtual DbSet<Skus> Skus { get; set; }

    public virtual DbSet<StandardRecipes> StandardRecipes { get; set; }

    public virtual DbSet<TinterBatchMeasurements> TinterBatchMeasurements { get; set; }

    public virtual DbSet<TinterBatches> TinterBatches { get; set; }

    public virtual DbSet<Tinters> Tinters { get; set; }

    public virtual DbSet<UserRoles> UserRoles { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    public virtual DbSet<WeightPredictions> WeightPredictions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-GNMTP4S;Database=BatchOPT;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BatchStatuses>(entity =>
        {
            entity.HasKey(e => e.BatchStatusId).HasName("PK__batch_st__65BA5F367163A8C4");

            entity.ToTable("batch_statuses");

            entity.HasIndex(e => e.StatusName, "UQ__batch_st__501B37531E00AD41").IsUnique();

            entity.Property(e => e.BatchStatusId).HasColumnName("batch_status_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BatchStatusesCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_batch_statuses_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.BatchStatusesUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_batch_statuses_updated_by");
        });

        modelBuilder.Entity<Batches>(entity =>
        {
            entity.HasKey(e => e.BatchId).HasName("PK__batches__DBFC04318BE1736F");

            entity.ToTable("batches");

            entity.HasIndex(e => new { e.SkuVersionId, e.BatchCode }, "uq_batches_sku_version_batch_code").IsUnique();

            entity.Property(e => e.BatchId).HasColumnName("batch_id");
            entity.Property(e => e.BatchCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("batch_code");
            entity.Property(e => e.BatchSize).HasColumnName("batch_size");
            entity.Property(e => e.BatchStatusId)
                .HasDefaultValue(1)
                .HasColumnName("batch_status_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.SkuVersionId).HasColumnName("sku_version_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.BatchStatus).WithMany(p => p.Batches)
                .HasForeignKey(d => d.BatchStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_batches_batch_status_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BatchesCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_batches_created_by");

            entity.HasOne(d => d.SkuVersion).WithMany(p => p.Batches)
                .HasForeignKey(d => d.SkuVersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_batches_sku_version_id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.BatchesUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_batches_updated_by");
        });

        modelBuilder.Entity<CalibrationData>(entity =>
        {
            entity.HasKey(e => e.CalibrationId).HasName("PK__calibrat__3DB44D47C0AC9B4C");

            entity.ToTable("calibration_data");

            entity.Property(e => e.CalibrationId).HasColumnName("calibration_id");
            entity.Property(e => e.AbsoluteCalibrationA).HasColumnName("absolute_calibration_a");
            entity.Property(e => e.AbsoluteCalibrationB).HasColumnName("absolute_calibration_b");
            entity.Property(e => e.AbsoluteCalibrationL).HasColumnName("absolute_calibration_l");
            entity.Property(e => e.AutoModulation).HasColumnName("auto_modulation");
            entity.Property(e => e.AutoModulationStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("auto_modulation_status");
            entity.Property(e => e.CalibrationDatetime).HasColumnName("calibration_datetime");
            entity.Property(e => e.CalibrationSourceUsed)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("calibration_source_used");
            entity.Property(e => e.CalibrationTypeId).HasColumnName("calibration_type_id");
            entity.Property(e => e.Comments)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("comments");
            entity.Property(e => e.DailyCalibrationA).HasColumnName("daily_calibration_a");
            entity.Property(e => e.DailyCalibrationB).HasColumnName("daily_calibration_b");
            entity.Property(e => e.DailyCalibrationL).HasColumnName("daily_calibration_l");
            entity.Property(e => e.DcSource)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dc_source");
            entity.Property(e => e.DeltaE).HasColumnName("delta_e");
            entity.Property(e => e.DishCleanCheckA).HasColumnName("dish_clean_check_a");
            entity.Property(e => e.DishCleanCheckB).HasColumnName("dish_clean_check_b");
            entity.Property(e => e.DishCleanCheckL).HasColumnName("dish_clean_check_l");
            entity.Property(e => e.DishCleanCheckSource)
                .HasMaxLength(50)
                .HasColumnName("dish_clean_check_source");
            entity.Property(e => e.Readjustment).HasColumnName("readjustment");
            entity.Property(e => e.SensorId)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("sensor_id");
            entity.Property(e => e.WhiteReference).HasColumnName("white_reference");
            entity.Property(e => e.WhiteReferenceStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("white_reference_status");

            entity.HasOne(d => d.CalibrationType).WithMany(p => p.CalibrationData)
                .HasForeignKey(d => d.CalibrationTypeId)
                .HasConstraintName("fk_calibration_type_id");
        });

        modelBuilder.Entity<CalibrationStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__calibrat__3683B5319C538F5A");

            entity.ToTable("calibration_status");

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.AmFlag).HasColumnName("am_flag");
            entity.Property(e => e.AutoModulationStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("auto_modulation_status");
            entity.Property(e => e.SensorId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("sensor_id");
            entity.Property(e => e.WhiteReferenceStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("white_reference_status");
            entity.Property(e => e.WrFlag).HasColumnName("wr_flag");
        });

        modelBuilder.Entity<CalibrationType>(entity =>
        {
            entity.HasKey(e => e.CalibrationTypeId).HasName("PK__calibrat__791B65C23E51AA49");

            entity.ToTable("calibration_type");

            entity.HasIndex(e => e.CalibrationTypeName, "UQ__calibrat__A7A26FB4DB8AE8B1").IsUnique();

            entity.Property(e => e.CalibrationTypeId).HasColumnName("calibration_type_id");
            entity.Property(e => e.CalibrationTypeName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("calibration_type_name");
        });

        modelBuilder.Entity<ColorimeterInstruments>(entity =>
        {
            entity.HasKey(e => e.ColorimeterInstrumentId).HasName("PK__colorime__66B4B51C6242B6FD");

            entity.ToTable("colorimeter_instruments");

            entity.Property(e => e.ColorimeterInstrumentId).HasColumnName("colorimeter_instrument_id");
            entity.Property(e => e.ColorimeterInstrument)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("colorimeter_instrument");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ColorimeterInstrumentsCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_colorimeter_instruments_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ColorimeterInstrumentsUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_colorimeter_instruments_updated_by");
        });

        modelBuilder.Entity<CommentHistory>(entity =>
        {
            entity.HasKey(e => e.CommentHistoryId).HasName("PK__comment___73D5F4FC1821AD91");

            entity.ToTable("comment_history");

            entity.Property(e => e.CommentHistoryId).HasColumnName("comment_history_id");
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("changed_at");
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
            entity.Property(e => e.ColumnName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("comments")
                .HasColumnName("column_name");
            entity.Property(e => e.NewComment)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("new_comment");
            entity.Property(e => e.OldComment)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("old_comment");
            entity.Property(e => e.RecordId).HasColumnName("record_id");
            entity.Property(e => e.TableName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("table_name");
        });

        modelBuilder.Entity<Pages>(entity =>
        {
            entity.HasKey(e => e.PageId).HasName("PK__pages__637F305A3F83ACC5");

            entity.ToTable("pages");

            entity.HasIndex(e => e.PageName, "UQ__pages__C6DD7EDC2DF411D6").IsUnique();

            entity.Property(e => e.PageId).HasColumnName("page_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PageName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("page_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PagesCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_pages_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PagesUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_pages_updated_by");
        });

        modelBuilder.Entity<Plants>(entity =>
        {
            entity.HasKey(e => e.PlantId).HasName("PK__plants__A576B3B432A59C1A");

            entity.ToTable("plants");

            entity.HasIndex(e => e.PlantName, "UQ__plants__2D64245370354214").IsUnique();

            entity.Property(e => e.PlantId).HasColumnName("plant_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PlantName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("plant_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PlantsCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_plants_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PlantsUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_plants_updated_by");
        });

        modelBuilder.Entity<Predictions>(entity =>
        {
            entity.HasKey(e => e.PredictionId).HasName("PK__predicti__F1AE77BF88179FB9");

            entity.ToTable("predictions");

            entity.HasIndex(e => e.ShotId, "UQ__predicti__A4803B973429980F").IsUnique();

            entity.Property(e => e.PredictionId).HasColumnName("prediction_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DaMax).HasColumnName("da_max");
            entity.Property(e => e.DaMin).HasColumnName("da_min");
            entity.Property(e => e.DbMax).HasColumnName("db_max");
            entity.Property(e => e.DbMin).HasColumnName("db_min");
            entity.Property(e => e.DeMax).HasColumnName("de_max");
            entity.Property(e => e.DeMin).HasColumnName("de_min");
            entity.Property(e => e.DlMax).HasColumnName("dl_max");
            entity.Property(e => e.DlMin).HasColumnName("dl_min");
            entity.Property(e => e.ShotId).HasColumnName("shot_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PredictionsCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_predictions_created_by");

            entity.HasOne(d => d.Shot).WithOne(p => p.Predictions)
                .HasForeignKey<Predictions>(d => d.ShotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_predictions_shot_id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PredictionsUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_predictions_updated_by");
        });

        modelBuilder.Entity<ProductTypes>(entity =>
        {
            entity.HasKey(e => e.ProductTypeId).HasName("PK__product___6EED3ED69CF1FD57");

            entity.ToTable("product_types");

            entity.Property(e => e.ProductTypeId).HasColumnName("product_type_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.ProductName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("product_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ProductTypesCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_product_types_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ProductTypesUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_product_types_updated_by");
        });

        modelBuilder.Entity<RolePageAccesses>(entity =>
        {
            entity.HasKey(e => e.RolePageAccessId).HasName("PK__role_pag__D9542AF613043771");

            entity.ToTable("role_page_accesses");

            entity.HasIndex(e => new { e.UserRoleId, e.PageId }, "uq_role_page_accesses_role_page").IsUnique();

            entity.Property(e => e.RolePageAccessId).HasColumnName("role_page_access_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.PageId).HasColumnName("page_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RolePageAccessesCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_role_page_accesses_created_by");

            entity.HasOne(d => d.Page).WithMany(p => p.RolePageAccesses)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_role_page_accesses_page_id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.RolePageAccessesUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_role_page_accesses_updated_by");

            entity.HasOne(d => d.UserRole).WithMany(p => p.RolePageAccesses)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_role_page_accesses_user_role_id");
        });

        modelBuilder.Entity<ShotMeasurements>(entity =>
        {
            entity.HasKey(e => e.ShotMeasurementId).HasName("PK__shot_mea__EC64674063307AEF");

            entity.ToTable("shot_measurements");

            entity.Property(e => e.ShotMeasurementId).HasColumnName("shot_measurement_id");
            entity.Property(e => e.MeasurementType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("measurement_type");
            entity.Property(e => e.MeasurementValue).HasColumnName("measurement_value");
            entity.Property(e => e.ShotId).HasColumnName("shot_id");

            entity.HasOne(d => d.Shot).WithMany(p => p.ShotMeasurements)
                .HasForeignKey(d => d.ShotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shot_measurements_shot_id");
        });

        modelBuilder.Entity<ShotTinters>(entity =>
        {
            entity.HasKey(e => e.ShotTinterId).HasName("PK__shot_tin__77533A03F61887C6");

            entity.ToTable("shot_tinters");

            entity.Property(e => e.ShotTinterId).HasColumnName("shot_tinter_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.ShotId).HasColumnName("shot_id");
            entity.Property(e => e.TinterBatchId).HasColumnName("tinter_batch_id");
            entity.Property(e => e.TinterWeight).HasColumnName("tinter_weight");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ShotTintersCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shot_tinters_created_by");

            entity.HasOne(d => d.Shot).WithMany(p => p.ShotTinters)
                .HasForeignKey(d => d.ShotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shot_tinters_shot_id");

            entity.HasOne(d => d.TinterBatch).WithMany(p => p.ShotTinters)
                .HasForeignKey(d => d.TinterBatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shot_tinters_tinter_batch_id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ShotTintersUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_shot_tinters_updated_by");
        });

        modelBuilder.Entity<Shots>(entity =>
        {
            entity.HasKey(e => e.ShotId).HasName("PK__shots__A4803B96462D2A67");

            entity.ToTable("shots");

            entity.HasIndex(e => new { e.BatchId, e.ShotNumber }, "uq_shots_batch_id_shot_number").IsUnique();

            entity.Property(e => e.ShotId).HasColumnName("shot_id");
            entity.Property(e => e.BatchId).HasColumnName("batch_id");
            entity.Property(e => e.Comments)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("comments");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.ShotNumber).HasColumnName("shot_number");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Batch).WithMany(p => p.Shots)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shots_batch_id");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ShotsCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shots_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ShotsUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shots_updated_by");
        });

        modelBuilder.Entity<SkuVersionMeasurements>(entity =>
        {
            entity.HasKey(e => e.SkuVersionMeasurementId).HasName("PK__sku_vers__DE4DC1DC1AB4E208");

            entity.ToTable("sku_version_measurements");

            entity.Property(e => e.SkuVersionMeasurementId).HasColumnName("sku_version_measurement_id");
            entity.Property(e => e.MeasurementType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("measurement_type");
            entity.Property(e => e.MeasurementValue).HasColumnName("measurement_value");
            entity.Property(e => e.SkuVersionId).HasColumnName("sku_version_id");

            entity.HasOne(d => d.SkuVersion).WithMany(p => p.SkuVersionMeasurements)
                .HasForeignKey(d => d.SkuVersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sku_version_measurements_sku_version_id");
        });

        modelBuilder.Entity<SkuVersions>(entity =>
        {
            entity.HasKey(e => e.SkuVersionId).HasName("PK__sku_vers__4DCA7851371A963D");

            entity.ToTable("sku_versions");

            entity.Property(e => e.SkuVersionId).HasColumnName("sku_version_id");
            entity.Property(e => e.ColorimeterInstrumentId).HasColumnName("colorimeter_instrument_id");
            entity.Property(e => e.Comments)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("comments");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.IsDefault).HasColumnName("is_default");
            entity.Property(e => e.ProductTypeId).HasColumnName("product_type_id");
            entity.Property(e => e.SkuId).HasColumnName("sku_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.VersionName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("version_name");
            entity.Property(e => e.VersionNumber).HasColumnName("version_number");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SkuVersionsCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sku_versions_created_by");

            entity.HasOne(d => d.Sku).WithMany(p => p.SkuVersions)
                .HasForeignKey(d => d.SkuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sku_versions_sku_id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SkuVersionsUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sku_versions_updated_by");
        });

        modelBuilder.Entity<Skus>(entity =>
        {
            entity.HasKey(e => e.SkuId).HasName("PK__skus__EAC95375FE4B1BA8");

            entity.ToTable("skus");

            entity.HasIndex(e => new { e.PlantId, e.SkuName }, "uq_skus_plant_id_sku_name").IsUnique();

            entity.Property(e => e.SkuId).HasColumnName("sku_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PlantId).HasColumnName("plant_id");
            entity.Property(e => e.SkuName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("sku_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SkusCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_skus_created_by");

            entity.HasOne(d => d.Plant).WithMany(p => p.Skus)
                .HasForeignKey(d => d.PlantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_skus_plant_id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SkusUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_skus_updated_by");
        });

        modelBuilder.Entity<StandardRecipes>(entity =>
        {
            entity.HasKey(e => e.StandardRecipeId).HasName("PK__standard__990C7354BFC1D3FE");

            entity.ToTable("standard_recipes");

            entity.Property(e => e.StandardRecipeId).HasColumnName("standard_recipe_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.SkuVersionId).HasColumnName("sku_version_id");
            entity.Property(e => e.TinterId).HasColumnName("tinter_id");
            entity.Property(e => e.UpdateNumber).HasColumnName("update_number");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.StandardRecipesCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_standard_recipes_created_by");

            entity.HasOne(d => d.SkuVersion).WithMany(p => p.StandardRecipes)
                .HasForeignKey(d => d.SkuVersionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_standard_recipes_sku_version_id");

            entity.HasOne(d => d.Tinter).WithMany(p => p.StandardRecipes)
                .HasForeignKey(d => d.TinterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_standard_recipes_tinter_id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.StandardRecipesUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_standard_recipes_updated_by");
        });

        modelBuilder.Entity<TinterBatchMeasurements>(entity =>
        {
            entity.HasKey(e => e.TinterBatchMeasurementId).HasName("PK__tinter_b__ABD243CF6F7F63A2");

            entity.ToTable("tinter_batch_measurements");

            entity.Property(e => e.TinterBatchMeasurementId).HasColumnName("tinter_batch_measurement_id");
            entity.Property(e => e.MeasurementType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("measurement_type");
            entity.Property(e => e.MeasurementValue).HasColumnName("measurement_value");
            entity.Property(e => e.TinterBatchId).HasColumnName("tinter_batch_id");

            entity.HasOne(d => d.TinterBatch).WithMany(p => p.TinterBatchMeasurements)
                .HasForeignKey(d => d.TinterBatchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tinter_batch_measurements_tinter_batch_id");
        });

        modelBuilder.Entity<TinterBatches>(entity =>
        {
            entity.HasKey(e => e.TinterBatchId).HasName("PK__tinter_b__41A5B92FAE9076F9");

            entity.ToTable("tinter_batches");

            entity.HasIndex(e => new { e.TinterId, e.TinterBatchCode }, "uq_tinter_batches_tinter_id_tinter_batch_code").IsUnique();

            entity.Property(e => e.TinterBatchId).HasColumnName("tinter_batch_id");
            entity.Property(e => e.BatchTinterName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("batch_tinter_name");
            entity.Property(e => e.Comments)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("comments");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Strength).HasColumnName("strength");
            entity.Property(e => e.TinterBatchCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tinter_batch_code");
            entity.Property(e => e.TinterId).HasColumnName("tinter_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TinterBatchesCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tinter_batches_created_by");

            entity.HasOne(d => d.Tinter).WithMany(p => p.TinterBatches)
                .HasForeignKey(d => d.TinterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tinter_batches_tinter_id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TinterBatchesUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tinter_batches_updated_by");
        });

        modelBuilder.Entity<Tinters>(entity =>
        {
            entity.HasKey(e => e.TinterId).HasName("PK__tinters__B873D1FC7E24597B");

            entity.ToTable("tinters");

            entity.HasIndex(e => new { e.PlantId, e.TinterCode }, "uq_tinters_plant_id_tinter_code").IsUnique();

            entity.Property(e => e.TinterId).HasColumnName("tinter_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PlantId).HasColumnName("plant_id");
            entity.Property(e => e.TinterCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tinter_code");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TintersCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tinters_created_by");

            entity.HasOne(d => d.Plant).WithMany(p => p.Tinters)
                .HasForeignKey(d => d.PlantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_tinters_plant_id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TintersUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_tinters_updated_by");
        });

        modelBuilder.Entity<UserRoles>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__user_rol__B8D9ABA2C02A5D99");

            entity.ToTable("user_roles");

            entity.HasIndex(e => e.RoleName, "UQ__user_rol__783254B183FC47D5").IsUnique();

            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role_name");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370F82B0D51B");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164463FB87B").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC572328B87DD").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_users_user_role_id");
        });

        modelBuilder.Entity<WeightPredictions>(entity =>
        {
            entity.HasKey(e => e.WeightPredictionId).HasName("PK__weight_p__0A45A215EDCB87A2");

            entity.ToTable("weight_predictions");

            entity.Property(e => e.WeightPredictionId).HasColumnName("weight_prediction_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.PredictedWeight).HasColumnName("predicted_weight");
            entity.Property(e => e.PredictionId).HasColumnName("prediction_id");
            entity.Property(e => e.TinterId).HasColumnName("tinter_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.WeightPredictionsCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_weight_predictions_created_by");

            entity.HasOne(d => d.Prediction).WithMany(p => p.WeightPredictions)
                .HasForeignKey(d => d.PredictionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_weight_predictions_prediction_id");

            entity.HasOne(d => d.Tinter).WithMany(p => p.WeightPredictions)
                .HasForeignKey(d => d.TinterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_weight_predictions_tinter_id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.WeightPredictionsUpdatedByNavigation)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_weight_predictions_updated_by");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
