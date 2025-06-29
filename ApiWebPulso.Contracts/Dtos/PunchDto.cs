using ApiWebPulso.Contracts.Enums;

namespace ApiWebPulso.Dtos
{
    public class PunchDto
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public PunchType Type { get; set; }
        public DateTime? Timestamp { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Address { get; set; } = "";
        public bool ManuallyEdited { get; set; }
    }
}
