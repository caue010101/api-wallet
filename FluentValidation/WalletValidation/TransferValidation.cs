using FluentValidation;
using Dtos.Wallet;

namespace minhaApi.Wallet.Validation
{

    public class TransferValidation : AbstractValidator<TransferWalletDto>
    {

        public TransferValidation()
        {

            RuleFor(x => x.ToUserId)
              .NotEmpty()
              .WithMessage("O destino da transferencia é obrigatorio ");

            RuleFor(x => x.Amount)
              .NotEmpty().WithMessage("O valor da transferencia é obrigatorio ")
              .GreaterThan(0).WithMessage("O valor da transferencia deve ser maior que 0");
        }
    }
}
