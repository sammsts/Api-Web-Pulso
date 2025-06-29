using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Punch
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("latitude")]
        public decimal? Latitude { get; set; }

        [Column("longitude")]
        public decimal? Longitude { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("manually_edited")]
        public bool ManuallyEdited { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public User User { get; set; }
    }

    public enum PunchType
    {
        In = 0,
        Out = 1
    }
}
