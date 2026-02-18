using FluentValidation;
using Dtos.Wallet;

namespace minhaApi.Wallet.Validation
{

    public class TransferValidation : AbstractValidator<TransferWalletDto>
    {

        public TransferValidation()
        {

            RuleFor(x => x.FromUserId)
              .NotEmpty();

            RuleFor(x => x.ToUserId)
              .NotEmpty()
              .NotEqual(x => x.FromUserId)
              .WithMessage("Nao é possivel transferir para si mesmo ");

            RuleFor(x => x.Amount)
              .NotEmpty().WithMessage("O valor da transferencia é obrigatorio ")
              .GreaterThan(0).WithMessage("O valor da transferencia deve ser maior que 0");
        }
    }
}
