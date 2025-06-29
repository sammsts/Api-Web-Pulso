using ApiWebPulso.Contracts.Enums;
namespace ApiWebPulso.Contracts.Dtos
{
    public class ReportFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PunchType? Type { get; set; }
    }

}
