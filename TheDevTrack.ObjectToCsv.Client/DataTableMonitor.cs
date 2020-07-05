using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace TheDevTrack.ObjectToCsv.Client
{
    [MemoryDiagnoser]
    public class DataTableMonitor
    {
        [Benchmark]
        public bool TestCreatingCsvString()
        {
               Car[] cars = new Car[10]
               {
                new Car(1, "52-XX-53", "red", 2020),
                new Car(1, "25-RR-66", "blue", 2015),
                new Car(1, "11-FF-98", "orange", 2014),
                new Car(1, "57-HH-35", "black", 2015),
                new Car(1, "85-NN-00", "orange", 2019),
                new Car(1, "35-VV-98", "red", 2010),
                new Car(1, "09-UU-85", "blue", 2019),
                new Car(1, "56-QQ-23", "blue", 2016),
                new Car(1, "00-DD-11", "red", 2012),
                new Car(1, "11-MM-25", "black", 2017)
               };

            var dataTable = new DateTable<Car>(cars);
            var result = dataTable.ToStringCsv();
            return true;
        }
    }
}
