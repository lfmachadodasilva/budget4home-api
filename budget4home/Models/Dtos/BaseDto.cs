namespace budget4home.Models
{
    public interface IDto<T>
    {
        T Id { get; set; }
        string Name { get; set; }
    }

    public abstract class BaseDto : IDto<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public abstract class BaseDtoString : IDto<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}