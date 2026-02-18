using Dtos.Wallet;
using FluentValidation;

namespace minhaApi.Users.Validation
{

    public class WithDrawWalletValidation : AbstractValidator<WithDrawWalletDto>
    {

        public WithDrawWalletValidation()
        {
            RuleFor(x => x.Amount)
              .NotEmpty().WithMessage("O valor do saque é obrigatorio ")
              .GreaterThan(0).WithMessage("O valor do saque precisa ser maior que 0");
        }
    }
}
