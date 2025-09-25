using System;
using System.Collections.Generic;

namespace Cookbook.SharedModels.Entities;

public partial class Review
{
    public int RecipeId { get; set; }

    public int ReviewerId { get; set; }

    public short Rating { get; set; }

    public string? Impression { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual User Reviewer { get; set; } = null!;
}
