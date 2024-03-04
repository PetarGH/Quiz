using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("ip_users")]
public partial class IpUser
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("Name")]
    public string Name { get; set; } = null!;

    [Column("Age")]
    public int Age { get; set; }

    [EmailAddress]
    [Column("Email")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [Column("Password")]
    public string Password { get; set; } = null!;

    [Column("Address")]
    public string Address { get; set; } = null!;

    [Column("user_type")]
    public bool UserType { get; set; }

    [Column("IsFrozen")]
    public bool IsFrozen { get; set; }

    [Column("Salt")]
    public string Salt { get; set; } = null!;

    public virtual ICollection<IpQuiz> IpQuizzes { get; set; } = new List<IpQuiz>();

    public IpUser()
    {

    }

    public IpUser(int id, string name, int age, string email, string password, string address, bool userType, bool isFrozen)
    {
        Id = id;
        Name = name;
        Age = age;
        Email = email;
        Password = password;
        Address = address;
        UserType = userType;
        IsFrozen = isFrozen;
    }

    public IpUser(string name, int age, string email, string password, string address, bool userType, bool isFrozen)
    {
        Name = name;
        Age = age;
        Email = email;
        Password = password;
        Address = address;
        UserType = userType;
        IsFrozen = isFrozen;
    }
}
