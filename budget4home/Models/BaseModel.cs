using System.ComponentModel.DataAnnotations;

namespace budget4home.Models
{
    public abstract class BaseModel
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}