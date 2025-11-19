using FantasyNameGenerator.Lib.Domain.Model;
using FantasyNameGenerator.Lib.Domain.Services;

namespace FantasyNameGenerator.WPF.ViewModels
{
    public class EditCultureViewModel
    {
        public required NameCulture Culture { get; set; }

        public EditCultureViewModel(INameCultureProvider cultureProvider)
        {

        }
    }
}
