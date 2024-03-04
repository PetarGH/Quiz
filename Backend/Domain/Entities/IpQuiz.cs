using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class IpQuiz
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int CreatedBy { get; set; }

    public int Categoryid { get; set; }

    public virtual IpCategory Category { get; set; } = null!;

    public virtual IpUser CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<IpQuestion> IpQuestions { get; set; } = new List<IpQuestion>();
    public IpQuiz()
    {

    }

    public IpQuiz(int id, string title, string description, int createdBy, int categoryid)
    {
        Id = id;
        Title = title;
        Description = description;
        CreatedBy = createdBy;
        Categoryid = categoryid;
    }

    public IpQuiz(string title, string description, int createdBy, int categoryid)
    {
        Title = title;
        Description = description;
        CreatedBy = createdBy;
        Categoryid = categoryid;
    }
}
