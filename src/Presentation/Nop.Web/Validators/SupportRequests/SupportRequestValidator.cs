using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;
using Nop.Web.Models.SupportRequests;

namespace Nop.Web.Validators.SupportRequests;

public class SupportRequestValidator : BaseNopValidator<CreateSupportRequestModel>
{
    public SupportRequestValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.MessageText)
            .NotEmpty()
            .WithMessageAwait(localizationService.GetResourceAsync("SupportRequest.MessageText.Required"));
    }
}
