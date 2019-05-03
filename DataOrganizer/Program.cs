using DataOrganizer.Models;
using System;
using System.Collections.Generic;

namespace DataOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello!");
            Console.Write("Enter path to data folder:");
            var path = Console.ReadLine();
            var reader = new DataReader();
            var result = reader.Read(path);

            if (result.Success && result.Data != null)
            {
                ProcessData(path, result.Data);
            }
            else
            {
                Console.WriteLine(result.Message);
            }

            Console.ReadKey();
        }

        private static void ProcessData(string path, List<DataModel> data)
        {
            var processor = new DataProcessor();
            var processedDataResult = processor.Process(data);

            if (processedDataResult.Success && processedDataResult.Data != null)
            {
                DataWriter.WriteReport(path, processedDataResult.Data);
                Console.WriteLine($"Report generated and saved to: {path}");
            }
            else
            {
                Console.WriteLine(processedDataResult.Message);
            }
        }
    }
}
