namespace OnDemandTutor.ModelViews.AuthModelViews
{
    public class PaymentInfo
    {
        public Guid AccountId { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountName { get; set; }
        public string Bank { get; set; }
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public string OrderType { get; set; }
        public string TxnRef { get; set; }
    }
}