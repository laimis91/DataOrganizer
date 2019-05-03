﻿using DataOrganizer.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataOrganizer
{
    public class DataProcessor
    {
        public ProcessResult<List<TxStatisticModel>> Process(List<DataModel> data)
        {
            try
            {
                var list = new List<TxStatisticModel>();
                var groupedByBand = data.GroupBy(d => d.Band);
                list.AddRange(ProcessData(groupedByBand));
                return new ProcessResult<List<TxStatisticModel>>
                {
                    Success = true,
                    Data = list
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

        private List<TxStatisticModel> ProcessData(IEnumerable<IGrouping<string, DataModel>> data)
        {
            var list = new List<TxStatisticModel>();
            foreach (var item in data)
            {
                list.AddRange(Calculate(item.Key, item.GroupBy(i => i.PCL)));
            }

            return list;
        }

        private IEnumerable<TxStatisticModel> Calculate(string band, IEnumerable<IGrouping<int, DataModel>> data)
        {
            var list = new List<TxStatisticModel>();
            foreach (var item in data)
            {
                var txPowerLessThanRangeList = item.Where(i => i.TXPower < i.MINPower).Select(i => i.TXPower);
                var txPowerInRangeList = item.Where(i => i.TXPower >= i.MINPower && i.TXPower <= i.MAXPower).Select(i => i.TXPower);
                var txPowerMoreThanRangeList = item.Where(i => i.TXPower > i.MAXPower).Select(i => i.TXPower);

                var powerLessThanRangeList = txPowerLessThanRangeList as float[] ?? txPowerLessThanRangeList.ToArray();
                var powerInRangeList = txPowerInRangeList as float[] ?? txPowerInRangeList.ToArray();
                var powerMoreThanRangeList = txPowerMoreThanRangeList as float[] ?? txPowerMoreThanRangeList.ToArray();
                
                list.Add(new TxStatisticModel
                {
                    Band = band,
                    PCL = item.Key,
                    TxPowerLessThanRangeAvg = powerLessThanRangeList.Any() ? powerLessThanRangeList.Average() : 0,
                    TxPowerInRangeAvg = powerInRangeList.Any() ? powerInRangeList.Average() : 0,
                    TxPowerGraterThanRangeAvg = powerMoreThanRangeList.Any() ? powerMoreThanRangeList.Average() : 0,
                    PassCount = item.Count(i => i.CheckResult),
                    FailCount = item.Count(i => !i.CheckResult)
                });
            }
            return list;
        }
    }
}