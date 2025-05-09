﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasker.Web.Data.Entities
{
    public class Subject
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string ShortName { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [ForeignKey(nameof(Group))]
        public Guid GroupId { get; set; } = Guid.Empty;

        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; } = null!;

        [NotMapped]
        public string Title => $"{ShortName}{(GroupId != Guid.Empty ? $" {Group.Name}" : string.Empty)} - {FullName}";

        [NotMapped]
        public string TitleShort => $"{ShortName}{(GroupId != Guid.Empty ? $" {Group.Name}" : string.Empty)}";

        public override bool Equals(object? obj) => obj is Subject subject && Id == subject.Id;

        public override int GetHashCode() => Id.GetHashCode();

        override public string ToString() => Title;
    }
}
