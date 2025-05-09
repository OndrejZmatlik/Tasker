﻿using System.ComponentModel.DataAnnotations;

namespace Tasker.Web.Data.Entities
{
    public class Group
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
