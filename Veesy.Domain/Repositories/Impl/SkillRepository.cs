﻿using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;
using Veesy.Domain.Repositories.Impl;

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
            .Where(s => s.MyUserId == user.Id && s.Type == type);
    }
}