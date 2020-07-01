using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace TheDevTrack.ObjectToCsv
{
    public class DateTable<T>
    {
        public DateTable(T[] rows)
        {
            Columns = typeof(T).GetProperties().Length;
            Rows = rows;
        }
        public int Columns { get; private set; }
        public T[] Rows { get; private set; }

        CsvConfiguration csvConfiguration;
        public CsvConfiguration CsvConfiguration
        {
            get
            {
                if (csvConfiguration == null)
                {
                    CsvConfiguration = new CsvConfiguration(CultureInfo.CurrentCulture)
                    {
                        Delimiter = ";"
                    };
                }
                return csvConfiguration;
            }
            set
            {
                csvConfiguration = value;
            }
        }

        /// <summary>
        /// The csv is created based on [Column("name")] atrributtes
        /// </summary>
        /// <returns></returns>
        public string ToStringCsv()
        {
            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(writer, CsvConfiguration))
            {
                var columns = GetCsvFields();

                foreach (var field in columns)
                    csvWriter.WriteField(field.GetCustomAttribute<ColumnAttribute>().Name);

                csvWriter.NextRecord();

                foreach (var row in this.Rows)
                {
                    foreach (var field in columns)
                        csvWriter.WriteField(GetColumnPropertyInfo(row, field).GetValue(row));
                    csvWriter.NextRecord();
                }

                writer.Flush();
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        private PropertyInfo[] GetCsvFields() => this.Rows[0].GetType().GetProperties();

        private PropertyInfo GetColumnPropertyInfo(T row, PropertyInfo field) => row.GetType().GetProperty(field.Name);

        /// <summary>
        /// The csv byte array is created based on [Column("name")] atrributtes
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArrayCsv() => Encoding.ASCII.GetBytes(ToStringCsv());
    }
}
