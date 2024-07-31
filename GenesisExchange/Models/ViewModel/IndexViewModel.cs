namespace GenesisExchange.Models.ViewModel
{
    public class IndexViewModel
    {
        public IEnumerable<Transactions> Transactions { get; set; }
        public int TransactionCount {  get; set; }  
        public int PendingCount {  get; set; }  
        public decimal TransactionTotal { get; set; }

    }
}
