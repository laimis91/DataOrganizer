using DataOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataOrganizer
{
    public class DataProcessor
    {
        private List<TxStatisticModel> _data;

        public DataProcessor()
        {
            _data = new List<TxStatisticModel>();
        }
        /// <summary>
        /// Calculate data for report
        /// </summary>
        /// <param name="data">Data from files</param>
        /// <returns>Calculated data</returns>
        public ProcessResult<List<TxStatisticModel>> Process(List<DataModel> data)
        {
            try
            {
                var groupedByBand = data.GroupBy(d => d.Band);
                ProcessData(groupedByBand);
                return new ProcessResult<List<TxStatisticModel>>
                {
                    Success = true,
                    Data = _data
                };
            }
            catch (Exception e)
            {
                return new ProcessResult<List<TxStatisticModel>>
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        private void ProcessData(IEnumerable<IGrouping<string, DataModel>> data)
        {
            foreach (var item in data)
            {
                Calculate(item.Key, item.GroupBy(i => i.PCL));
            }
        }

        private void Calculate(string band, IEnumerable<IGrouping<int, DataModel>> data)
        {
            foreach (var item in data)
            {
                var txPowerLessThanRangeList = item.Where(i => i.TXPower < i.MINPower).Select(i => i.TXPower);
                var txPowerInRangeList = item.Where(i => i.TXPower >= i.MINPower && i.TXPower <= i.MAXPower).Select(i => i.TXPower);
                var txPowerMoreThanRangeList = item.Where(i => i.TXPower > i.MAXPower).Select(i => i.TXPower);

                _data.Add(new TxStatisticModel
                {
                    Band = band,
                    PCL = item.Key,
                    TxPowerLessThanRangeAvg = txPowerLessThanRangeList.Any() ? txPowerLessThanRangeList.Average() : 0,
                    TxPowerInRangeAvg = txPowerInRangeList.Any() ? txPowerInRangeList.Average() : 0,
                    TxPowerGraterThanRangeAvg = txPowerMoreThanRangeList.Any() ? txPowerMoreThanRangeList.Average() : 0,
                    PassCount = item.Count(i => i.CheckResult),
                    FailCount = item.Count(i => !i.CheckResult)
                });
            }
        }
    }
}
