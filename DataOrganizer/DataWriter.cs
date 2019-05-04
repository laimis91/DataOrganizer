using DataOrganizer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataOrganizer
{
    public class DataWriter
    {
        /// <summary>
        /// Generate report
        /// </summary>
        /// <param name="path">Path to save report</param>
        /// <param name="data">Calculated data to generate report</param>
        /// <returns>Response to identify if report was successfully generated and saved</returns>
        public ProcessResult<object> WriteReport(string path, IEnumerable<TxStatisticModel> data)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine(string.Join(',', Constants.ReportHeaders));

                foreach (var item in data)
                {
                    sb.AppendLine($"{item.Band}, {item.PCL}, {item.TxPowerLessThanRangeAvg}, {item.TxPowerInRangeAvg}, {item.TxPowerGraterThanRangeAvg}, {item.PassCount}, {item.FailCount}");
                }

                var filePath = Path.Combine(path, $"report-{DateTime.Now:yyyy-MM-dd_hhmmss}.csv");
                File.WriteAllText(filePath, sb.ToString());

                return new ProcessResult<object> { Success = true };
            }
            catch (Exception e)
            {
                return new ProcessResult<object> { Success = false, Message = e.Message };
            }
        }
    }
}
