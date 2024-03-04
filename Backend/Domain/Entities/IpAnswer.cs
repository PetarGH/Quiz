using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class IpAnswer
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public bool IsCorrect { get; set; }

    public int QuestionId { get; set; }

    public virtual IpQuestion Question { get; set; } = null!;

    public IpAnswer()
    {

    }

    public IpAnswer(int id, string text, bool isCorrect, int questionId)
    {
        Id = id;
        Text = text;
        IsCorrect = isCorrect;
        QuestionId = questionId;
    }

    public IpAnswer(string text, bool isCorrect, int questionId)
    {
        Text = text;
        IsCorrect = isCorrect;
        QuestionId = questionId;
    }
}
