namespace Veesy.Domain.Models;

public class CategoryWork
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public virtual List<MyUserCategoryWork> MyUserCategoriesWork { get; set; }
}