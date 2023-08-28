using Veesy.Domain.Models;

namespace Veesy.Presentation.Model.Auth;

public class SignUpViewModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; } 
    public string ConfirmPassword { get; set; }
    public List<CategoryWork> CategoriesWork { get; set; }
    public List<Guid> SelectedCategoriesWork { get; set; }
}