using System.ComponentModel.DataAnnotations.Schema;

namespace TheDevTrack.ObjectToCsv.Test.Entities
{
    public class Car
    {
        public Car(int id, string licensePlate, string color, short year)
        {
            Id = id;
            LicensePlate = licensePlate;
            Color = color;
            Year = year;
        }

        [ColumnAttribute("Car code")]
        public int Id { get; set; }
        [ColumnAttribute("License plate")]
        public string LicensePlate { get; set; }
        public string teste { get; set; } = "wasdasdad";
        [ColumnAttribute("Color")]
        public string Color { get; set; }
        [ColumnAttribute("Year")]
        public short Year { get; set; }
    }
}
