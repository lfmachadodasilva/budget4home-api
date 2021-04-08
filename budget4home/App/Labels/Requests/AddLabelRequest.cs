using budget4home.App.Groups;
using System.ComponentModel.DataAnnotations;

namespace budget4home.App.Labels.Requests
{
    public class AddLabelRequest
    {
        [Required]
        public string Name { get; set; }

        [GroupValidation]
        public long GroupId { get; set; }
    }
}