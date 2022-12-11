using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SKU_MetaverseApi.Entities
{
    public partial class Attendance
    {
        [Key]
        public int Id { get; set; }

        public string aStudentId { get; set; } = null!;

        public string aStudentName { get; set; } = null!;
        public string aAttendTime { get; set; } = null!;
    }
}