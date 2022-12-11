using Microsoft.AspNetCore.Http;
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
    public class MemberController : ControllerBase
    {
        private readonly MemberContext memberContext;

        public MemberController(MemberContext memberContext)
        {
            this.memberContext = memberContext;
        }
        
        //전부 불러보기
        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<MemberDTO>>> Get()
        {
            var list = await memberContext.member.Select(
                s => new MemberDTO
                {
                    Id = s.Id,
                    studentId = s.StudentId,
                    Pw = s.Pw,
                    studentName = s.StudentName,
                    studentMail = s.StudentMail
                }
            ).ToListAsync();
            if (list.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return list;
            }
        }

        //Id 번호로 호출하기
        [HttpGet("GetUserById")]
        public async Task<ActionResult<MemberDTO>> GetUserById([FromForm] int Id)
        {
            MemberDTO memberDto = await memberContext.member.Select(
                s => new MemberDTO
                {
                    Id = s.Id,
                    studentId = s.StudentId,
                    Pw = s.Pw,
                    studentName=s.StudentName,
                    studentMail=s.StudentMail
                }
            ).FirstOrDefaultAsync(s => s.Id == Id);
            if(memberDto == null)
            {
                return NotFound();
            }
            else
            {
                return memberDto;
            }
        }

        //회원가입에 사용한 가능한 메서드
        [HttpPost("InsertUser")]
        public async Task<HttpStatusCode> InsertUser([FromForm] MemberDTO memberDto)
        {
            var entity = new Member()
            {
                StudentId = memberDto.studentId,
                Pw = memberDto.Pw,
                StudentName = memberDto.studentName,
                StudentMail = memberDto.studentMail,
            };
            memberContext.member.Add(entity);
            await memberContext.SaveChangesAsync();
            return HttpStatusCode.Created;
        }

        //유저를 ID로 추적해서 삭제
        [HttpDelete("DeleteUser/{Id}")]
        public async Task<HttpStatusCode> DeleteUser(int Id)
        {
            var entity = new Member()
            {
                Id = Id
            };
            memberContext.member.Attach(entity);
            memberContext.member.Remove(entity);
            await memberContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpPost("LoginUser")]
        public async Task<ActionResult<MemberDTO>> LoginUser([FromForm] MemberDTO loginDTO)
        {
            //entityFrameworkCore를 이용해 우선 모든 데이터 로드
            using (var context = new MemberContext())
            {
                var members = context.member.ToList();
                foreach(var items in members)
                {
                    if(items.StudentId == loginDTO.studentId && items.Pw == loginDTO.Pw)
                    {
                        return Ok();
                        break;
                    }
                }
                return NotFound();
            }
            
        }
    }
}
