using budget4home.App.Groups;
using System.ComponentModel.DataAnnotations;

namespace budget4home.App.Labels.Requests
{
    public class GetFullLabelsRequest
    {
        [GroupValidation]
        public long Group { get; set; }

        [Range(1700, int.MaxValue)]
        public int Year { get; set; }

        [Range(1, 12)]
        public int Month { get; set; }
    }
}