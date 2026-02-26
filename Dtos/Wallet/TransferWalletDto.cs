namespace Dtos.Wallet
{

    public class TransferWalletDto
    {
        public Guid ToUserId { get; set; }
        public decimal Amount { get; set; }
    }
}
