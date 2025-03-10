using FluentValidation;

namespace ChatMessagesApp.Core.Application.Features;

public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
{
    public SendMessageCommandValidator()
    {
        RuleFor(v => v.DemandId)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} is required.");
        RuleFor(v => v.SenderId)
            .NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(v => v.Content)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(1000).WithMessage("{PropertyName} must not exceed 500 characters.");
    }
}