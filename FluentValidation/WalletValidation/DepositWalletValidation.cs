using FluentValidation;
using Dtos.Wallet;

namespace minhaApi.Wallet.Validation
{
    public class WalletValidation : AbstractValidator<DepositWalletDto>
    {

        public WalletValidation()
        {
            RuleFor(x => x.Amount)
              .NotEmpty().WithMessage("O valor do deposito é obrigatorio ")
              .GreaterThan(0).WithMessage("O valor do deposito precisa ser maior que 0 ");
        }
    }
}
