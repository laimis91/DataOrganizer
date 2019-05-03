﻿using DataOrganizer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataOrganizer
{
    public class DataWriter
    {
        public static void WriteReport(string path, IEnumerable<TxStatisticModel> data)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Join(',', Constants.ReportHeaders));

            foreach (var item in data)
            {
                sb.AppendLine($"{item.Band}, {item.PCL}, {item.TxPowerLessThanRangeAvg}, {item.TxPowerInRangeAvg}, {item.TxPowerGraterThanRangeAvg}, {item.PassCount}, {item.FailCount}");
            }

            var filePath = Path.Combine(path, $"report-{DateTime.Now:yyyy-MM-dd_hhmmss}.csv");
            File.WriteAllText(filePath, sb.ToString());
        }
    }
}