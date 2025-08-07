using System;
using System.Collections.Generic;

namespace BatchOptimization.Api.Models;

public partial class CommentHistory
{
    public int CommentHistoryId { get; set; }

    public string TableName { get; set; } = null!;

    public int RecordId { get; set; }

    public string ColumnName { get; set; } = null!;

    public string? OldComment { get; set; }

    public string? NewComment { get; set; }

    public int ChangedBy { get; set; }

    public DateTime? ChangedAt { get; set; }
}
