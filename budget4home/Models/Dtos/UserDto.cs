using System.ComponentModel.DataAnnotations.Schema;

namespace budget4home.Models.Dtos
{
    public class UserDto : BaseDtoString
    {
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
    }
}