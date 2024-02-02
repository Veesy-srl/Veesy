using Veesy.Domain.Models;

namespace Veesy.Service.Dtos;

public class MediaOverviewDto
{
    public int NumberMedia { get; set; }
    public float MediaSize { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}

public class ChangeSubscriptionDto
{
    public string SubscriptionName { get; set; }
    public string MyUserId { get; set; }
}

public class CreatorOverviewDto
{
    public int NumberCreator { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}

public class LastMediaUpload
{
    public float MediaSize { get; set; }
    public string Filename { get; set; }
    public string Username { get; set; }
    public DateTime Date { get; set; }
}

public class FrelancerDto
{
    public string Fullname => FirstName +  " " + LastName;
    public string CreateDate { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string SubscriptionPlan { get; set; }
    public string Code { get; set; }
    public Guid MainPortfolioCode { get; set; }
    public List<string> Software { get; set; }
    public string Category { get; set; }
    public List<string> SoftSkill { get; set; }
    public List<string> Fields { get; set; }
}

public class FrelancerInfoDto
{
    public string Fullname => FirstName +  " " + LastName;
    public string CreateDate { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string SubscriptionPlan { get; set; }
    public string Image { get; set; }
}

public static class MapAdminDto{
    
    public static List<FrelancerDto> MapCreatorDtos(List<MyUser> users)
    {
        var frelancerDtos = new List<FrelancerDto>();
        users.ForEach(user => frelancerDtos.Add(new FrelancerDto()
        {
            MainPortfolioCode = user.Portfolios == null || user.Portfolios.Count == 0 ? Guid.Empty : user.Portfolios[0].Id,
            Code = user.Id,
            FirstName = user.Name,
            LastName = user.Surname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            SubscriptionPlan = user.MyUserSubscriptionPlans.LastOrDefault().SubscriptionPlan.Name,
            Category = user.Category,
            Fields = user.MyUserSectors.Select(s => s.Sector.Name).ToList(),
            Software = user.MyUserUsedSoftwares.Select(s => s.UsedSoftware.Name).ToList(),
            SoftSkill = user.MyUserSkills.Select(s => s.Skill.Name).ToList(),
            CreateDate = user.CreateDate.ToString("dd/MM/yy hh:mm")
        }));
        return frelancerDtos;
    }
    
    public static List<LastMediaUpload> MapLastMediaUploadDtos(List<Media> medias)
    {
        var mediasUploaded = new List<LastMediaUpload>();
        medias.ForEach(media => mediasUploaded.Add(new LastMediaUpload()
        {
            Date = media.CreateRecordDate,
            MediaSize = media.Size / (1024 * 1024),
            Filename = media.OriginalFileName,
            Username = media.MyUser.UserName
        }));
        return mediasUploaded;
    }
}