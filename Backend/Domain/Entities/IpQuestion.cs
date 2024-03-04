using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class IpQuestion
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int QuizId { get; set; }

    public virtual ICollection<IpAnswer> IpAnswers { get; set; } = new List<IpAnswer>();

    public virtual IpQuiz Quiz { get; set; } = null!;

    public IpQuestion()
    {

    }

    public IpQuestion(int id, string text, int quizId)
    {
        Id = id;
        Text = text;
        QuizId = quizId;
    }

    public IpQuestion(string text, int quizId)
    {
        Text = text;
        QuizId = quizId;
    }
}
