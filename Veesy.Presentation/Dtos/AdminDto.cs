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
    
    public static List<FrelancerDto> MapFrelancerDtos(List<MyUser> users)
    {
        var frelancerDtos = new List<FrelancerDto>();
        users.ForEach(user => frelancerDtos.Add(new FrelancerDto()
        {
            Code = user.Id,
            FirstName = user.Name,
            LastName = user.Surname,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            SubscriptionPlan = user.SubscriptionPlan.Name,
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