﻿using CsvHelper;
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
        }

        public T[] Rows { get; set; }

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

                for (int i = 0; i < columns.Length; i++)
                    csvWriter.WriteField(columns[i]);

                csvWriter.NextRecord();

                for (int i = 0; i < Rows.Length; i++)
                {
                    for (int x = 0; x < columns.Length; x++)
                        csvWriter.WriteField(GetColumnPropertyInfo(Rows[i], columns[x]));
                    csvWriter.NextRecord();
                }

                writer.Flush();
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        private string[] GetCsvFields() => typeof(T)
                                            .GetProperties()
                                            .Where(x => x.CustomAttributes.Any(y => y.AttributeType.Equals(typeof(ColumnAttribute))))
                                            .Select(x => x.Name)
                                            .ToArray();

        private string GetColumnPropertyInfo(T row, string field) => (row.GetType().GetProperty(field)).GetValue(row).ToString();
    }
}
