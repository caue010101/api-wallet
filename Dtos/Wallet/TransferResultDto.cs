namespace Dtos.Wallet
{

    public class TransferResultDto
    {

        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
