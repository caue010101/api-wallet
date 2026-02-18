using FluentValidation;
using Dtos.User;

namespace minhaApi.Users.Validation
{

    public class CreateUserValidation : AbstractValidator<CreateUserDto>
    {

        public CreateUserValidation()
        {
            RuleFor(x => x.Name)
              .Cascade(CascadeMode.Stop)
              .NotEmpty()
              .WithMessage("Seu nome nao pode estar vazio ")
              .MinimumLength(2).WithMessage("Seu nome deve ter no minimo 2 caracteres ")
              .MaximumLength(25).WithMessage("Seu nome deve ter no maximo 25 caracteres ");

            RuleFor(x => x.Email)
              .Cascade(CascadeMode.Stop)
                .NotEmpty()
              .EmailAddress().WithMessage("Email é obrigatorio ")
              .MaximumLength(255).WithMessage("O email nao pode ultrapassar 255 caracteres ");

            RuleFor(x => x.Password)
              .Cascade(CascadeMode.Stop)
              .NotEmpty()
              .WithMessage("Digite a senha ")
              .MinimumLength(8).WithMessage("A senha precisa ter no minimo 8 caracteres ")
              .MaximumLength(100).WithMessage("A senha pode ter no maximo 100 caracteres ")

              .Matches("[A-Z]").WithMessage("A senha precisa ter pelo menos uma letra maiuscula ")
              .Matches("[a-z]").WithMessage("A senha precisa ter pelo menos uma letra minuscula ")
              .Matches("[0-9]").WithMessage("A senha precisa ter pelo menos um numero ")
              .Matches("[^a-zA-Z0-9]").WithMessage("A senha precisa ter pelo menos um simbulo ");
        }
    }
}
