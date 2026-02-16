namespace Dtos.Wallet
{

    public class TransferWalletDto
    {

        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public decimal Amount { get; set; }
    }
}
