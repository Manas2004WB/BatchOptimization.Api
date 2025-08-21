using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class Users
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Email { get; set; } = null!;

    public int UserRoleId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual ICollection<BatchStatuses> BatchStatusesCreatedByNavigation { get; set; } = new List<BatchStatuses>();

    public virtual ICollection<BatchStatuses> BatchStatusesUpdatedByNavigation { get; set; } = new List<BatchStatuses>();

    public virtual ICollection<Batches> BatchesCreatedByNavigation { get; set; } = new List<Batches>();

    public virtual ICollection<Batches> BatchesUpdatedByNavigation { get; set; } = new List<Batches>();

    public virtual ICollection<ColorimeterInstruments> ColorimeterInstrumentsCreatedByNavigation { get; set; } = new List<ColorimeterInstruments>();

    public virtual ICollection<ColorimeterInstruments> ColorimeterInstrumentsUpdatedByNavigation { get; set; } = new List<ColorimeterInstruments>();

    public virtual ICollection<Pages> PagesCreatedByNavigation { get; set; } = new List<Pages>();

    public virtual ICollection<Pages> PagesUpdatedByNavigation { get; set; } = new List<Pages>();

    public virtual ICollection<Plants> PlantsCreatedByNavigation { get; set; } = new List<Plants>();

    public virtual ICollection<Plants> PlantsUpdatedByNavigation { get; set; } = new List<Plants>();

    public virtual ICollection<Predictions> PredictionsCreatedByNavigation { get; set; } = new List<Predictions>();

    public virtual ICollection<Predictions> PredictionsUpdatedByNavigation { get; set; } = new List<Predictions>();

    public virtual ICollection<ProductTypes> ProductTypesCreatedByNavigation { get; set; } = new List<ProductTypes>();

    public virtual ICollection<ProductTypes> ProductTypesUpdatedByNavigation { get; set; } = new List<ProductTypes>();

    public virtual ICollection<RolePageAccesses> RolePageAccessesCreatedByNavigation { get; set; } = new List<RolePageAccesses>();

    public virtual ICollection<RolePageAccesses> RolePageAccessesUpdatedByNavigation { get; set; } = new List<RolePageAccesses>();

    public virtual ICollection<ShotTinters> ShotTintersCreatedByNavigation { get; set; } = new List<ShotTinters>();

    public virtual ICollection<ShotTinters> ShotTintersUpdatedByNavigation { get; set; } = new List<ShotTinters>();

    public virtual ICollection<Shots> ShotsCreatedByNavigation { get; set; } = new List<Shots>();

    public virtual ICollection<Shots> ShotsUpdatedByNavigation { get; set; } = new List<Shots>();

    public virtual ICollection<SkuVersions> SkuVersionsCreatedByNavigation { get; set; } = new List<SkuVersions>();

    public virtual ICollection<SkuVersions> SkuVersionsUpdatedByNavigation { get; set; } = new List<SkuVersions>();

    public virtual ICollection<Skus> SkusCreatedByNavigation { get; set; } = new List<Skus>();

    public virtual ICollection<Skus> SkusUpdatedByNavigation { get; set; } = new List<Skus>();

    public virtual ICollection<StandardRecipes> StandardRecipesCreatedByNavigation { get; set; } = new List<StandardRecipes>();

    public virtual ICollection<StandardRecipes> StandardRecipesUpdatedByNavigation { get; set; } = new List<StandardRecipes>();

    public virtual ICollection<TinterBatches> TinterBatchesCreatedByNavigation { get; set; } = new List<TinterBatches>();

    public virtual ICollection<TinterBatches> TinterBatchesUpdatedByNavigation { get; set; } = new List<TinterBatches>();

    public virtual ICollection<Tinters> TintersCreatedByNavigation { get; set; } = new List<Tinters>();

    public virtual ICollection<Tinters> TintersUpdatedByNavigation { get; set; } = new List<Tinters>();

    public virtual UserRoles UserRole { get; set; } = null!;

    public virtual ICollection<WeightPredictions> WeightPredictionsCreatedByNavigation { get; set; } = new List<WeightPredictions>();

    public virtual ICollection<WeightPredictions> WeightPredictionsUpdatedByNavigation { get; set; } = new List<WeightPredictions>();
}
