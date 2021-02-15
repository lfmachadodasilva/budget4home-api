using System.ComponentModel.DataAnnotations;

namespace budget4home.Models
{
    public interface IModel<T>
    {
        [Key]
        T Id { get; set; }

        [Required]
        string Name { get; set; }
    }

    public abstract class BaseModel : IModel<long>
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }

    public abstract class BaseModelString : IModel<string>
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}