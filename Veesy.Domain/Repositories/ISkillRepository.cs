using Veesy.Domain.Data;
using Veesy.Domain.Models;
using Veesy.Domain.Repositories.Base;

namespace Veesy.Domain.Repositories;

public interface ISkillRepository : IRepositoryBase<Skill>
{
    void DeleteMyUserSkills(List<MyUserSkill> skillToDelete);
    Task AddMyUserSkills(List<MyUserSkill> skillToAdd);
    
    IEnumerable<MyUserSkill> GetSkillsByUserAndType(MyUser user, char type);
}