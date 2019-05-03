namespace DataOrganizer
{
    public static class Constants
    {
        public static readonly string[] Headers = { "Band", "PCL", "TX Power", "Target Power", "MIN Power", "MAX Power", "Check Result" };
        public static readonly string[] ReportHeaders = { "BAND", "PCL", "TxPower < Min (avg.)", "TxPower in range (avg.)", "TxPower > Max (avg.)", "PASS Count", "FAIL Count" };
    }
}
