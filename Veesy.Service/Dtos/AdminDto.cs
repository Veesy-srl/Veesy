using System;
using System.Collections.Generic;
using System.Linq;
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
public class MapOverviewDto
{
    public int Number { get; set; }
    public string City { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}

public class ChangeSubscriptionDto
{
    public string SubscriptionName { get; set; }
    public string? MyUserId { get; set; }
}

public class UpdateUserVisibilityDto
{
    public bool Visibility { get; set; }
    public string? MyUserId { get; set; }
}

public class ReferralLinkDto
{
    public string Endpoint { get; set; }
    public string? RedirectUrl { get; set; }
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
    public string FullnameForUrl => (FirstName + "-" + LastName).ToLower().Replace(" ", "-");
    public string PortfolionameForUrl => PortfolioName == null ? "" : PortfolioName.ToLower().Replace(" ", "-");
    public string CreateDate { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string SubscriptionPlan { get; set; }
    public string Code { get; set; }
    public Guid MainPortfolioCode { get; set; }
    public string PortfolioName { get; set; }
    public int PortfoliosCount { get; set; }
    public int PublicPortfoliosCount { get; set; }
    public int MediasCount { get; set; }
    public List<string> Software { get; set; }
    public string Category { get; set; }
    public List<string> SoftSkill { get; set; }
    public List<string> Fields { get; set; }
    public bool VisibleCreatorPage { get; set; }
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

public class CreatorTrackingFormDto
{
    public string SenderName { get; set; }
    public string SenderEmail { get; set; }
    public string RecipientName { get; set; }
    public string RecipientId { get; set; }
    public DateTime DateTime { get; set; }
}

public static class MapAdminDto{
    
    public static List<FrelancerDto> MapCreatorDtos(List<MyUser> users)
    {
        var frelancerDtos = new List<FrelancerDto>();
        var us = users.ToArray();
        foreach (var user in us)
        {
            frelancerDtos.Add(new FrelancerDto()
            {
                Code = user.Id,
                FirstName = user.Name,
                LastName = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                SubscriptionPlan = user.MyUserSubscriptionPlans.Count == 0 ? "Free" : user.MyUserSubscriptionPlans.LastOrDefault().SubscriptionPlan.Name,
                Category = user.Category,
                VisibleCreatorPage = user.VisibleInCreatorPage,
                Fields = user.MyUserSectors == null ? null : user.MyUserSectors.Select(s => s.Sector.Name).ToList(),
                Software = user.MyUserUsedSoftwares == null ? null : user.MyUserUsedSoftwares.Select(s => s.UsedSoftware.Name).ToList(),
                SoftSkill = user.MyUserSkills == null ? null : user.MyUserSkills.Select(s => s.Skill.Name).ToList(),
                CreateDate = user.CreateDate.ToString("dd/MM/yy hh:mm")
            });
        }
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

    public static List<CreatorTrackingFormDto> MapCreatorTrackingFormDtos(List<TrackingForm> trackingForms)
    {
        var creatorTrackingForms = new List<CreatorTrackingFormDto>();
        foreach (var form in trackingForms)
        {
            creatorTrackingForms.Add(new CreatorTrackingFormDto
            {
                SenderName = form.NameSender,
                SenderEmail = form.EmailSender,
                RecipientName = form.MyUser.Fullname,
                RecipientId = form.MyUserId,
                DateTime = form.CreateRecordDate
            });
        }
        return creatorTrackingForms;
    }
}