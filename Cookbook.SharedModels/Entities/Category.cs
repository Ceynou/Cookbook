using System;
using System.Collections.Generic;

namespace Cookbook.SharedModels.Entities;

public partial class Category
{
    public short CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
