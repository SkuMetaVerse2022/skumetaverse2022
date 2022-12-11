using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SKU_MetaverseApi.Entities
{
    public partial class Member
    {
        [Key]
        public int Id { get; set; }

        public string StudentId { get; set; } = null!;

        public string StudentName { get; set; } = null!;

        public string Pw { get; set; } = null!;

        public string StudentMail { get; set; } = null!;
    }
}
