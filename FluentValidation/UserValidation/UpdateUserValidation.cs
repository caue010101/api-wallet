using FluentValidation;
using Dtos.User;

namespace minhaApi.Users.Validation
{

    public class UpdateUserValidation : AbstractValidator<UpdateUserDto>
    {

        public UpdateUserValidation()
        {

            RuleFor(x => x.Name)
              .NotEmpty().WithMessage("O Nome do usuario é obrigatorio ")
              .MinimumLength(2).WithMessage("O nome do usuario deve ter no minimo 2 caracteres ")
              .MaximumLength(25).WithMessage("O nome do usuario deve ter no maximo 25 caracteres ");

            RuleFor(x => x.Email)
              .EmailAddress().WithMessage("O Email é obrigatorio ");
        }
    }
}



