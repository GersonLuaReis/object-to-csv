using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TheDevTrack.ObjectToCsv
{
    public class DateTable<T>
    {
        public DateTable(T[] rows)
        {
            Rows = rows;
            Properties = typeof(T).GetProperties();
        }

        private readonly T[] Rows;
        private readonly PropertyInfo[] Properties;

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
            Tuple<int, string>[] result = new Tuple<int, string>[Properties.Length - 1];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Tuple.Create(Properties[i].MetadataToken, Properties[i].GetCustomAttribute<ColumnAttribute>().Name ?? Properties[i].Name);
            }
            return result;
        }

        private string GetValueFromRowAndColumn(T row, int columnId) => Properties.First(x => x.MetadataToken == columnId).GetValue(row).ToString();

    }
}
