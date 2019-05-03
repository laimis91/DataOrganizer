namespace DataOrganizer.Models
{
    public class TxStatisticModel
    {
        public string Band { get; set; }
        public int PCL { get; set; }
        public float TxPowerLessThanRangeAvg { get; set; }
        public float TxPowerInRangeAvg { get; set; }
        public float TxPowerGraterThanRangeAvg { get; set; }
        public int PassCount { get; set; }
        public int FailCount { get; set; }
    }
}
