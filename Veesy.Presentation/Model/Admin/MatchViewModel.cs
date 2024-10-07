using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Admin;

public class MatchViewModel
{
    public List<CreatorTrackingFormDto> TrackingFormDtos { get; set; }
    public List<(string CreatorName, int FormCount)> FormCount { get; set; }
}