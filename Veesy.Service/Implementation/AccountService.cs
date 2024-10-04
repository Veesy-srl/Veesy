using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Constants;
using Veesy.Domain.Exceptions;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories;
using Veesy.Service.Dtos;
using Veesy.Service.Interfaces;

namespace Veesy.Service.Implementation;

public class AccountService : IAccountService
{
    private readonly IVeesyUoW _uoW;

    public AccountService(IVeesyUoW uoW)
    {
        _uoW = uoW;
    }

    public async Task<ResultDto> UpdateUserProfile(MyUser user)
    {
        _uoW.MyUserRepository.Update(user);
        await _uoW.CommitAsync(user);
        return new ResultDto(true, "");
    }

    public List<UsedSoftware> GetUsedSoftware()
    {
        return _uoW.UsedSoftwareRepository.FindAll().ToList();
    }

    public List<UsedSoftware> GetUsedSoftwareWithUser(MyUser user)
    {
        return _uoW.UsedSoftwareRepository.FindAll().Include(s => s.MyUserUsedSoftwares.Where(s => s.MyUserId == user.Id)).OrderBy(s => s.Name).ToList();
    }

    public List<MyUserUsedSoftware> GetUsedSoftwaresByUser(MyUser user)
    {
        return _uoW.UsedSoftwareRepository.GetUsedSoftwaresByUser(user);
    }

    public async Task<ResultDto> UpdateMyUserUsedSoftware(List<MyUserUsedSoftware> usedSoftwareToDelete, List<MyUserUsedSoftware> usedSoftwareToAdd, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.UsedSoftwareRepository.DeleteMyUserUsedSoftwares(usedSoftwareToDelete);
                await _uoW.UsedSoftwareRepository.AddMyUserUsedSoftwares(usedSoftwareToAdd);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public IEnumerable<Skill> GetSkillsWithUserByType(MyUser userInfo, char type)
    {
        return _uoW.SkillRepository.FindAll().Include(s => s.MyUserSkills.Where(s => s.MyUserId == userInfo.Id && s.Type == type)).OrderBy(s => s.Name);
    }

    public IEnumerable<MyUserSkill> GetSkillsByUserAndType(MyUser user, char type)
    {
        return _uoW.SkillRepository.GetSkillsByUserAndType(user, type);
    }

    public async Task<ResultDto> UpdateMyUserSkills(List<MyUserSkill> skillToDelete, List<MyUserSkill> skillToAdd, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.SkillRepository.DeleteMyUserSkills(skillToDelete);
                await _uoW.SkillRepository.AddMyUserSkills(skillToAdd);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public List<CategoryWork> GetCategoriesWork()
    {
        return _uoW.MyUserRepository.GetCategoriesWork();
    }
    
    public List<Sector> GetSectors()
    {
        return _uoW.MyUserRepository.GetSectors();
    }

    public SubscriptionPlan GetSubscriptionPlanByName(string name)
    {
        return _uoW.MyUserRepository.GetSubscriptionPlanByName(name);
    }

    public List<InfoToShow> GetInfosToShow()
    {
        return _uoW.MyUserRepository.GetInfosToShow();
    }

    public List<CategoryWork> GetCategoriesWorkWithUser(string userId)
    {
        return _uoW.MyUserRepository.GetCategoriesWorkByUserId(userId);
    }
    
    public List<RoleWork> GetRolesWorkWithUser(string userId)
    {
        return _uoW.MyUserRepository.GetRolesWorkByUserId(userId);
    }
    
    public List<Sector> GetSectorsWithUser(string userId)
    {
        return _uoW.MyUserRepository.GetSectorsByUserId(userId);
    }

    public List<MyUserCategoryWork> GetCategoriesWorkByUser(MyUser userInfo)
    {
        return _uoW.MyUserRepository.GetCategoriesWorkByUser(userInfo);
    }
    
    public List<MyUserRoleWork> GetRolesWorkByUser(MyUser userInfo)
    {
        return _uoW.MyUserRepository.GetRolesWorkByUser(userInfo);
    }
    
    public List<MyUserSector> GetSectorsByUser(MyUser userInfo)
    {
        return _uoW.MyUserRepository.GetSectorsByUser(userInfo);
    }

    public async Task<ResultDto> UpdateMyUserCategoriesWork(List<MyUserCategoryWork> categoryWorksToDelete, List<MyUserCategoryWork> categoryWorksToAdd, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.MyUserRepository.DeleteMyUserCategoriesWork(categoryWorksToDelete);
                await _uoW.MyUserRepository.AddMyUserCategoriesWork(categoryWorksToAdd);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public async Task<ResultDto> UpdateMyUserRolesWork(List<MyUserRoleWork> rolesWorksToDelete, List<MyUserRoleWork> rolesWorksToAdd, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.MyUserRepository.DeleteMyUserRolesWork(rolesWorksToDelete);
                await _uoW.MyUserRepository.AddMyUserRolesWork(rolesWorksToAdd);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }
    
    public async Task<ResultDto> UpdateMyUserSectors(List<MyUserSector> sectorsToDelete, List<MyUserSector> sectorsToAdd, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.MyUserRepository.DeleteMyUserSectors(sectorsToDelete);
                await _uoW.MyUserRepository.AddMyUserSectors(sectorsToAdd);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public List<MyUserInfoToShow> GetInfosToShowByUser(MyUser userInfo)
    {
        return _uoW.MyUserRepository.GetInfosToShowByUser(userInfo);
    }

    public async Task<ResultDto> UpdateMyUserInfoToShow(List<MyUserInfoToShow> infoToShowToDelete, List<MyUserInfoToShow> infoToShowToAdd, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.MyUserRepository.DeleteMyUserInfoToShow(infoToShowToDelete);
                await _uoW.MyUserRepository.AddMyUserInfoToShow(infoToShowToAdd);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public List<MyUserLanguageSpoken> GetLanguageSpokenByUser(MyUser userInfo)
    {
        return _uoW.MyUserRepository.GetLanguageSpokenByUser(userInfo);
    }

    public async Task<ResultDto> UpdateMyUserLanguageSpoken(List<MyUserLanguageSpoken> languageSpokenToDelete, List<MyUserLanguageSpoken> languageSpokenToAdd, MyUser user)
    {
        using (var transaction = _uoW.DbContext.Database.BeginTransaction())
        {
            try
            {
                _uoW.MyUserRepository.DeleteMyUserLanguageSpoken(languageSpokenToDelete);
                await _uoW.MyUserRepository.AddMyUserLanguageSpoken(languageSpokenToAdd);
                await _uoW.CommitAsync(user);
                await transaction.CommitAsync();
                return new ResultDto(true, "");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }

    public List<LanguageSpoken> GetLanguagesSpokenWithUser(MyUser userInfo)
    {
        return _uoW.MyUserRepository.GetLanguagesSpokenByUserId(userInfo.Id);
    }

    public List<InfoToShow> GetInfosToShowWithUser(MyUser userInfo)
    {
        return _uoW.MyUserRepository.GetInfoToShowByUserId(userInfo.Id);
    }

    public IEnumerable<MyUser> GetAllVisibleCreators()
    {
        return _uoW.DbContext.MyUsers
            .Where(u => u.VisibleInCreatorPage && u.Portfolios.Any(p => p.IsMain && p.IsPublic && p.Status == 1))
            .Include(u => u.Portfolios.OrderByDescending(p => p.IsMain && p.IsPublic && p.Status == 1))
            .Include(t => t.MyUserCategoriesWork)
            .ThenInclude(g => g.CategoryWork);
    }

    public List<MyUser> GetFilteredCreatorsToShow(List<string> category)
    {
        return GetAllVisibleCreators().Where(u => category.All(category => u.MyUserCategoriesWork.Any(cw => cw.CategoryWork.Name == category))).ToList();
    }

    public int NumberRecordCompiled(MyUser userInfo)
    {
        var user = _uoW.MyUserRepository.FindByCondition(s => s.Id == userInfo.Id)
            .Include(s => s.MyUserSkills)
            .Include(s => s.MyUserUsedSoftwares)
            .Include(s => s.MyUserSectors)
            .Include(s => s.MyUserCategoriesWork)
            .Include(s => s.MyUserLanguagesSpoken)
            .Include(s => s.MyUserRolesWork)
            .SingleOrDefault();
        var count = 0;
        count = user.MyUserSkills.Count != 0 ? 2 : count;
        count = user.MyUserUsedSoftwares.Count != 0 ? count + 2 : count;
        count = user.MyUserSectors.Count != 0 ? count + 2 : count;
        count = user.MyUserCategoriesWork.Count != 0 ? count + 2 : count;
        count = user.MyUserLanguagesSpoken.Count != 0 ? count + 2 : count;
        count = user.MyUserRolesWork.Count != 0 ? count + 2 : count;
        count = user.Name != null ? count + 1 : count;
        count = user.Surname != null ? count + 1 : count;
        count = user.Email != null ? count + 1 : count;
        count = user.VATNumber != null ? count + 1 : count;
        count = user.UserName != null ? count + 1 : count;
        count = user.Biografy != null ? count + 2 : count;
        count = user.Category != null ? count + 2 : count;
        count = user.ProfileImageFileName != null ? count + 2 : count;
        count = user.PortfolioIntro != null ? count + 2 : count;
        count = user.PhoneNumber != null ? count + 1 : count;
        return count ;
    }

    public SubscriptionPlan GetUserSubscription(MyUser user)
    {
        return _uoW.DbContext.MyUserSubscriptionPlans
            .Include(s => s.SubscriptionPlan)
            .Where(s => s.MyUserId == user.Id)
            .OrderBy(s => s.CreateRecordDate)
            .LastOrDefault().SubscriptionPlan;
    }

    public List<FrelancerDto> GetCreators()
    {
         return _uoW.MyUserRepository.FindAll()
            .Select(user => new FrelancerDto
            {
                Code = user.Id,
                FirstName = user.Name,
                LastName = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Category = user.Category,
                VisibleCreatorPage = user.VisibleInCreatorPage,
                Fields = user.MyUserSectors.Select(us => us.Sector.Name).ToList(),
                Software = user.MyUserUsedSoftwares.Select(us => us.UsedSoftware.Name).ToList(),
                SoftSkill = user.MyUserSkills
                    .Where(us => us.Type == SkillConstants.SoftSkill)
                    .Select(us => us.Skill.Name).ToList(),
                SubscriptionPlan = user.MyUserSubscriptionPlans
                    .OrderByDescending(sp => sp.CreateRecordDate)
                    .Select(sp => sp.SubscriptionPlan.Name)
                    .FirstOrDefault() ?? "Free",
                CreateDate = user.CreateDate.ToString("dd/MM/yy hh:mm"),
                PortfoliosCount = user.Portfolios.Count,
                PublicPortfoliosCount = user.Portfolios.Count(s => s.Status == PortfolioContants.STATUS_PUBLISHED),
                PortfolioName = user.Portfolios.Where(s => s.IsMain)
                    .Select(s => s.Name)
                    .FirstOrDefault(),
                MainPortfolioCode = user.Portfolios.Where(s => s.IsMain)
                    .Select(s => s.Id)
                    .FirstOrDefault(),
                MediasCount = user.Medias.Count
            })
            .ToList();
    }

    public List<FrelancerDto> GetCreatorsSecondPage()
    {
         return _uoW.MyUserRepository.FindAll()
            .Select(user => new FrelancerDto
            {
                Code = user.Id,
                FirstName = user.Name,
                LastName = user.Surname,
                Fields = user.MyUserSectors.Select(us => us.Sector.Name).ToList(),
                Software = user.MyUserUsedSoftwares.Select(us => us.UsedSoftware.Name).ToList(),
                SoftSkill = user.MyUserSkills
                    .Where(us => us.Type == SkillConstants.SoftSkill)
                    .Select(us => us.Skill.Name).ToList(),
            })
            .ToList();
    }

    public List<FrelancerDto> GetCreatorsFirstPage()
    {
         return _uoW.MyUserRepository.FindAll()
            .Select(user => new FrelancerDto
            {
                Code = user.Id,
                FirstName = user.Name,
                LastName = user.Surname,
                Email = user.Email,
                SubscriptionPlan = user.MyUserSubscriptionPlans
                    .OrderByDescending(sp => sp.CreateRecordDate)
                    .Select(sp => sp.SubscriptionPlan.Name)
                    .FirstOrDefault() ?? "Free",
                CreateDate = user.CreateDate.ToString("dd/MM/yy hh:mm"),
                PortfoliosCount = user.Portfolios.Count,
                PublicPortfoliosCount = user.Portfolios.Count(s => s.Status == PortfolioContants.STATUS_PUBLISHED),
                MediasCount = user.Medias.Count,
                VisibleCreatorPage = user.VisibleInCreatorPage
            })
            .ToList();
    }

    public int GetCreatorsCount()
    {
        return _uoW.MyUserRepository.FindAll().ToList().Count;
    }

    public MyUser GetUserById(string id)
    {
        return _uoW.MyUserRepository.FindByCondition(s => s.Id == id)
            .Include(s => s.MyUserSubscriptionPlans.OrderBy(s => s.CreateRecordDate))
                .ThenInclude(s => s.SubscriptionPlan)
            .SingleOrDefault();
    }

    public List<string> GetUserSector(string userId)
    {
        return _uoW.DbContext.MyUserSectors
            .Include(s => s.Sector)
            .Where(s => s.MyUserId == userId)
            .Select(s => s.Sector.Name)
            .ToList();
    }
    public List<string> GetUserUsedSoftware(string userId)
    {
        return _uoW.DbContext.MyUserUsedSoftwares
            .Include(s => s.UsedSoftware)
            .Where(s => s.MyUserId == userId)
            .Select(s => s.UsedSoftware.Name)
            .ToList();
    } 
    public List<string> GetUserSoftSkill(string userId)
    {
        return _uoW.DbContext.MyUserSkills
            .Include(s => s.Skill)
            .Where(s => s.MyUserId == userId && s.Type == SkillConstants.SoftSkill)
            .Select(s => s.Skill.Name)
            .ToList();
    }
    public List<string> GetUserLanguageSpoken(string userId)
    {
        return _uoW.DbContext.MyUserLanguagesSpoken
            .Include(s => s.LanguageSpoken)
            .Where(s => s.MyUserId == userId)
            .Select(s => s.LanguageSpoken.Language)
            .ToList();
    }

    public List<CreatorOverviewDto> GetCreatorNumberByMonthGroupByDay(int month, int year)
    {
        var res = _uoW.MyUserRepository.FindByCondition(s => s.CreateDate.Month == month && s.CreateDate.Year == year)
            .GroupBy(s => s.CreateDate.Day)
            .Select(g => new CreatorOverviewDto
            {
                Day = g.Key,
                NumberCreator = g.Count(),
                Date = g.Select(s => s.CreateDate).FirstOrDefault()
            })
            .ToList();
        return res;
    }

    public List<MyUser> GetCreatorsPlus()
    {
        return _uoW.MyUserRepository.FindAll()
            .Include(s => s.MyUserSubscriptionPlans)
            .ThenInclude(s => s.SubscriptionPlan)
            .Include(s => s.Portfolios.Where(s => s.IsMain))
            .Where(s =>
                s.MyUserSubscriptionPlans.OrderBy(s => s.CreateRecordDate).LastOrDefault().SubscriptionPlan.Price > 0).ToList();
    }

    public int GetNumberPayingUsers()
    {
        var users = _uoW.MyUserRepository.FindAll()
            .Include(s => s.MyUserSubscriptionPlans)
                .ThenInclude(s => s.SubscriptionPlan)
            .Where(s => s.MyUserSubscriptionPlans.OrderBy(s => s.CreateRecordDate).LastOrDefault().SubscriptionPlan.Price > 0 );
        return users.Count();
    }

    public List<MyUser> GetLastFourLoginAttempt(int number)
    {
        return _uoW.MyUserRepository.FindByCondition(s => s.LastLoginTime != null)
            .OrderByDescending(s => s.LastLoginTime).Take(number).ToList();
    }

    public List<MyUser> GetLastFourCreatedUser(int number)
    {
        return _uoW.MyUserRepository.FindAll().OrderByDescending(s => s.CreateDate).Take(number).ToList();
    }

    public SubscriptionPlan GetUserSubscriptionPlan(string userId)
    {
        return _uoW.DbContext.MyUserSubscriptionPlans
            .Include(s => s.SubscriptionPlan)
            .OrderBy(s => s.CreateRecordDate)
            .LastOrDefault(s => s.MyUserId == userId).SubscriptionPlan;
    }

    public SubscriptionPlan GetSubscriptionPlanById(Guid id)
    {
        return _uoW.DbContext.SubscriptionPlans.SingleOrDefault(s => s.Id == id);
    }

    public async Task AddNewUserSubscription(string userId, Guid id, MyUser user)
    {
        await _uoW.DbContext.MyUserSubscriptionPlans.AddAsync(new MyUserSubscriptionPlan()
        {
            MyUserId = userId,
            SubscriptionPlanId = id
        });
        await _uoW.CommitAsync(user.Id);
    }

    public async Task DeleteUser(MyUser user)
    {
        var id = user.Id;
        _uoW.MyUserRepository.Delete(user);
        await _uoW.CommitAsync(id);
    }

    public List<MyUser> GetUserEmailNotConfirmed(int days)
    {
        return _uoW.MyUserRepository.FindByCondition(s => !s.EmailConfirmed && s.CreateDate.AddDays(days) < DateTime.Now).ToList();
    }

    public async Task DeleteUsers(List<MyUser> users)
    {
        _uoW.MyUserRepository.DeleteRange(users);
        await _uoW.CommitAsync(new MyUser());
    }

    public List<MyUser> GetUserToSendEmailPro()
    {
        return _uoW.MyUserRepository
            .FindByCondition(s => s.CreateDate.Date.AddDays(7) <= DateTime.Now.Date && s.EmailConfirmed)
            .Include(s => s.MyUserSubscriptionPlans)
            .ThenInclude(s => s.SubscriptionPlan)
            .ToList();
    }

    public async Task UpdateMyUsers(List<MyUser> usersToUpdate)
    {
        _uoW.MyUserRepository.UpdateRange(usersToUpdate);
        await _uoW.CommitAsync(new MyUser());
    }

    public MyUser? GetUserByDiscordId(string discordId)
    {
        return _uoW.MyUserRepository.FindByCondition(x => x.DiscordId == discordId).FirstOrDefault();
    }

    public class CreatorOverviewDto
    {
        public int NumberCreator { get; set; }
        public int Day { get; set; }
        public DateTime Date { get; set; }
    }
}