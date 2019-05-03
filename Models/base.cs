using System;
using System.ComponentModel.DataAnnotations;

namespace DeliverTheThing.Models
{
    public abstract class Base
    {
        [Key]
        public int Id { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}