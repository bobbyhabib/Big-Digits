using System.ComponentModel.DataAnnotations;

namespace Dijits.Authentication.Models
{
    public class AudienceModel
    {
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
    }
}