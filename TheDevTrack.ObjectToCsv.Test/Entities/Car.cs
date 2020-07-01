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

        [Column("Car code")]
        public int Id { get; set; }
        [Column("License plate")]
        public string LicensePlate { get; set; }
        [Column("Color")]
        public string Color { get; set; }
        [Column("Year")]
        public short Year { get; set; }
    }
}
