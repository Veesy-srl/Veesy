using System.Collections.Generic;
using Veesy.Service.Dtos;

namespace Veesy.Presentation.Model.Admin;

public class CreatorsListViewModel
{
    public List<FrelancerDto> FreelancerDtos { get; set; }
    public string ApplicationUrl { get; set; }
}