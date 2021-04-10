using System.ComponentModel.DataAnnotations;

namespace budget4home.App.Labels.Requests
{
    public class UpdateLabelRequest
    {
        [LabelValidation]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}