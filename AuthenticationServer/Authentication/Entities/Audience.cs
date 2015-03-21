using System.ComponentModel.DataAnnotations;

namespace Dijits.Authentication.Entities
{
    /// <summary>
    /// Verifies which resource audiences.
    /// </summary>
    public class Audience
    {
        [Key]
        [MaxLength(32)]
        public string ClientId { get; set; }

        [MaxLength(80)]
        [Required]
        public string Base64Secret { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}