using Veesy.Domain.Models;
using Veesy.Presentation.Model.Portfolio;

namespace Veesy.Presentation.Helper;

public class PortfolioHelper
{
    public PortfolioSettingsViewModel GetPortfolioSettingsViewModel(MyUser userInfo)
    {
        return new PortfolioSettingsViewModel();
    }
}