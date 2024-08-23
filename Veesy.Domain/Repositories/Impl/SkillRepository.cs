using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;

namespace Veesy.Domain.Repositories.Impl;

public class SkillRepository : RepositoryBase<Skill>, ISkillRepository 
{
    public SkillRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
    {
    }

    public void DeleteMyUserSkills(List<MyUserSkill> skillToDelete)
    {
        _applicationDbContext.MyUserSkills.RemoveRange(skillToDelete);
    }

    public async Task AddMyUserSkills(List<MyUserSkill> skillToAdd)
    {
        await _applicationDbContext.MyUserSkills.AddRangeAsync(skillToAdd);
    }

    public IEnumerable<MyUserSkill> GetSkillsByUserAndType(MyUser user, char type)
    {
        return _applicationDbContext.MyUserSkills
            .Include(s => s.Skill)
            .Where(s => s.MyUserId == user.Id && s.Type == type)
            .OrderBy(s => s.Skill);
    }
}