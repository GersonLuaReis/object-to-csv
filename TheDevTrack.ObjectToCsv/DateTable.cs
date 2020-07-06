using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TheDevTrack.ObjectToCsv
{
    public class DateTable<T>
    {
        public DateTable(T[] rows)
        {
            Rows = rows;
            type = typeof(T);
            Properties = type.GetProperties();
        }

        public T[] Rows { get; private set; }
        public Type type { get; private set; }
        public PropertyInfo[] Properties { get; private set; }

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
        /// The csv byte array is created based on [Column("name")] atrributtes
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArrayCsv() => Encoding.ASCII.GetBytes(ToStringCsv());

        private const string nextLine = "\r\n";
        private const string nextColumn = ";";
        /// <summary>
        /// The csv is created based on [Column("name")] atrributtes
        /// </summary>
        /// <returns></returns>
        public string ToStringCsv()
        {
            var csv = new StringBuilder();

            var columns = GetCsvFields();

            for (int i = 0; i < columns.Length; i++)
            {
                csv.Append(columns[i].Item2);
                csv.Append(nextColumn);
            }

            csv.Append(nextLine);

            for (int i = 0; i < Rows.Length; i++)
            {
                for (int x = 0; x < columns.Length; x++)
                {
                    csv.Append(GetValueFromRowAndColumn(Rows[i], columns[x].Item1));
                    csv.Append(nextColumn);
                }
                csv.Append(nextLine);
            }

            return csv.ToString();
        }

        private Tuple<int, string>[] GetCsvFields()
        {
            return Properties
                        .Where(x => x.CustomAttributes.Any(y => y.AttributeType.Equals(typeof(ColumnAttribute))))
                        .Select(x => Tuple.Create(x.MetadataToken, x.GetCustomAttribute<ColumnAttribute>()?.Name))
                        .ToArray();
        }

        private string GetValueFromRowAndColumn(T row, int columnId) => Properties.First(x => x.MetadataToken == columnId).GetValue(row).ToString();

    }
}
