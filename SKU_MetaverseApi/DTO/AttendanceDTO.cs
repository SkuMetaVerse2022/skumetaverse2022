using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SKU_MetaverseApi.DTO
{
    public class AttendanceDTO
    {
        public int Id { get; set; }
        public string? aStudentId { get; set; }
        public string? aStudentName { get; set; }
        public string? aAttendTime { get; set; }
    }
}