using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Admin;

public class MatchViewModel
{
    public List<FrelancerDto> FreelancerDtos { get; set; }
    public List<CreatorTrackingFormDto> TrackingFormDtos { get; set; }
    public List<(string CreatorName, int FormCount)> FormCount { get; set; }
}