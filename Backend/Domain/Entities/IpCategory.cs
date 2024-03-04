using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class IpCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ParentId { get; set; }

    public virtual ICollection<IpCategory> InverseParent { get; set; } = new List<IpCategory>();

    public virtual ICollection<IpQuiz> IpQuizzes { get; set; } = new List<IpQuiz>();

    public virtual IpCategory? Parent { get; set; }

    public IpCategory()
    {

    }

    public IpCategory(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public IpCategory(string name)
    {
        Name = name;
    }

    public IpCategory(string name, int? parentId)
    {
        Name = name;
        ParentId = parentId;
    }
}
