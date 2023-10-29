using Cint.CodingChallenge.Model;
using Cint.CodingChallenge.Web.ViewModels;

namespace Cint.CodingChallenge.Web.Extensions
{
    // No AutoMapper needed for this simple mapping
    // https://cezarypiatek.github.io/post/why-i-dont-use-automapper/

    public static class MappingExtensions
    {
        public static SurveyViewModel ToView(this Survey model)
        {
            return new SurveyViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IncentiveEuros = model.IncentiveEuros,
                LengthMinutes = model.LengthMinutes
            };
        }
        public static Survey ToModel(this SurveyViewModel view)
        {
            return new Survey
            {
                Id = view.Id,
                Name = view.Name,
                Description = view.Description,
                IncentiveEuros = view.IncentiveEuros,
                LengthMinutes = view.LengthMinutes
            };
        }

        public static Survey ToModel(this SurveyCreateViewModel view, Guid newId)
        {
            return new Survey
            {
                Id = newId,
                Name = view.Name,
                Description = view.Description,
                IncentiveEuros = view.IncentiveEuros,
                LengthMinutes = view.LengthMinutes
            };
        }
    }
}
