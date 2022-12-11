using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SKU_MetaverseApi.Entities;
using SKU_MetaverseApi.DTO;
using System;
using System.Data.Entity;
using System.Net;
using System.Linq;

namespace SKU_MetaverseApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly AttendanceContext attendanceContext;

        public AttendanceController(AttendanceContext attendanceContext)
        {
            this.attendanceContext = attendanceContext;
        }

        [HttpPost("AttendUser")]
        public async Task<HttpStatusCode> AttendUser([FromForm] AttendanceDTO attendanceDto)
        {
            var entity = new Attendance()
            {
                aStudentId = attendanceDto.aStudentId,
                aStudentName = attendanceDto.aStudentName,
                aAttendTime = attendanceDto.aAttendTime
            };
            attendanceContext.attendance.Add(entity);
            await attendanceContext.SaveChangesAsync();
            return HttpStatusCode.Created;
        }
    }
}
