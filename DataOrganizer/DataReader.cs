using DataOrganizer.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DataOrganizer
{
    public class DataReader
    {
        public ProcessResult<List<DataModel>> Read(string path)
        {
            try
            {
                if (!Directory.Exists(path)) return new ProcessResult<List<DataModel>> { Success = true, Data = null, Message = "Given path does not exits" };

                var list = new ConcurrentBag<DataModel>();

                Task.WaitAll(GetDirectories(path).Select(dir => Task.Run(() =>
                    {
                        foreach (var item in ReadFiles(dir))
                        {
                            list.Add(item);
                        }
                    })).ToArray());

                return new ProcessResult<List<DataModel>> { Success = true, Data = list.ToList() };
            }
            catch (Exception e)
            {
                return new ProcessResult<List<DataModel>>
                {
                    Success = false,
                    Data = null,
                    Message = e.Message
                };
            }
        }

        private IEnumerable<string> GetDirectories(string path)
        {
            var directories = Directory.GetDirectories(path);

            if (!directories.Any())
            {
                return new List<string> { path };
            }
            else if (directories.Count() == 1)
            {
                return GetDirectories(directories[0]);
            }

            return directories;
        }

        private IEnumerable<DataModel> ReadFiles(string dir)
        {
            var files = Directory.GetFiles(dir);
            var list = new List<DataModel>();

            foreach (var file in files)
            {
                list.AddRange(ReadFile(file));
            }

            return list;
        }

        private IEnumerable<DataModel> ReadFile(string file)
        {
            var list = new List<DataModel>();
            var headerFound = false;
            var columnSequence = Constants.Headers;

            foreach (var line in File.ReadAllLines(file))
            {
                if (IsHeaderLine(line))
                {
                    headerFound = true;
                    columnSequence = Array.ConvertAll(line.Split(','), p => p.Trim());
                }
                else if (headerFound)
                {
                    var dataColumns = Array.ConvertAll(line.Split(','), p => p.Trim());

                    if (dataColumns.Length != columnSequence.Length) continue;

                    var model = new DataModel
                    {
                        Band = dataColumns[Array.IndexOf(columnSequence, "Band")],
                        PCL = int.Parse(dataColumns[Array.IndexOf(columnSequence, "PCL")]),
                        TXPower = float.Parse(dataColumns[Array.IndexOf(columnSequence, "TX Power")]),
                        TargetPower = float.Parse(dataColumns[Array.IndexOf(columnSequence, "Target Power")]),
                        MINPower = float.Parse(dataColumns[Array.IndexOf(columnSequence, "MIN Power")]),
                        MAXPower = float.Parse(dataColumns[Array.IndexOf(columnSequence, "MAX Power")]),
                        CheckResult = dataColumns[Array.IndexOf(columnSequence, "Check Result")] == "PASS"
                    };

                    list.Add(model);
                }
            }

            return list;
        }

        private bool IsHeaderLine(string line)
        {
            var columns = Array.ConvertAll(line.Split(','), p => p.Trim());

            if (Constants.Headers.Length == columns.Length)
            {
                foreach (var column in columns)
                {
                    if (!Constants.Headers.Contains(column))
                        return false;
                }

                return true;
            }

            return false;
        }
    }
}
