using System.ComponentModel.DataAnnotations;

namespace SKU_MetaverseApi.DTO
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public string? studentId { get; set; }
        public string? Pw { get; set; }
        public string? studentName { get; set; }
        public string? studentMail { get; set; }
        [DataType(DataType.Date)]
        public DateTime connectTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime exitTime { get; set; }
    }
}
