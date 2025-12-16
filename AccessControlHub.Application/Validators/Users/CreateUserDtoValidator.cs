using AccessControlHub.Application.Dtos.Users;
using FluentValidation;
using AccessControlHub.Domain.Repositories;

namespace AccessControlHub.Application.Validators.Users;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    private readonly IUserRepository _userRepository;
    public CreateUserDtoValidator(IUserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters long");

        RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email format is invalid")
                .MustAsync(BeUniqueEmail).WithMessage("Email already exists");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var exists = await _userRepository.EmailExistsAsync(email);
        return !exists;
    }
}
