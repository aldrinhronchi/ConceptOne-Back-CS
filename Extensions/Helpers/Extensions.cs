using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TMODELOBASET_WebAPI_CS.Extensions.Helpers
{
    public static class Extensions
    {
        /// <summary>
        /// Formata a string de acordo com a máscara especificada
        /// </summary>
        /// <param name=“input”>A string de entrada.</param>
        /// <param name=“mask”>A máscara para formatação. Como “A##-##-T-###Z”</param>
        /// <returns>A string formatada</returns>
        public static string FormatWithMask(this string input, string mask)
        {
            if (String.IsNullOrWhiteSpace(input)) return input;
            var output = string.Empty;
            var index = 0;
            foreach (var m in mask)
            {
                if (m == '#')
                {
                    if (index < input.Length)
                    {
                        output += input[index];
                        index++;
                    }
                }
                else
                    output += m;
            }
            return output;
        }

        /// <summary>
        /// Procura em todo o campo, seja no inicio, fim ou meio
        /// </summary>
        /// <param name="value">A string que esta o texto</param>
        /// <param name="search">O texto a ser procurado</param>
        /// <returns>Se tem ou não o texto na string</returns>
        public static bool Like(this string value, string search)
        {
            return value.Contains(search) || value.StartsWith(search) || value.EndsWith(search);
        }

        /// <summary>
        /// Retira tags html
        /// </summary>
        /// <param name="input">O texto a ter as tag html retirado</param>
        /// <returns>O texto sem as tag html</returns>
        public static string StripHtml(this string input)
        {
            var tagsExpression = new Regex(@"</?.+?>");
            return tagsExpression.Replace(input, " ");
        }

        /// <summary>
        /// Converte o Enumerable em um DataTable
        /// </summary>
        /// <typeparam name="T">o Tipo da lista</typeparam>
        /// <param name="varlist">o Enumerable</param>
        /// <returns>Um DataTable</returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = rec.GetType().GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if (colType.IsGenericType && colType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static IEnumerable<t> Randomize<t>(this IEnumerable<t> target)
        {
            Random r = new Random();

            return target.OrderBy(x => r.Next());
        }

        public static bool IsValidEmailAddress(this string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        public static bool IsWeekday(this DayOfWeek dow)
        {
            switch (dow)
            {
                case DayOfWeek.Sunday:
                case DayOfWeek.Saturday:
                    return false;

                default:
                    return true;
            }
        }

        public static bool IsWeekend(this DayOfWeek dow)
        {
            return !dow.IsWeekday();
        }

        public static DateTime AddWorkdays(this DateTime startDate, int days)
        {
            // start from a weekday
            while (startDate.DayOfWeek.IsWeekend())
            {
                startDate = startDate.AddDays(1.0);
            }
            for (int i = 0; i < days; ++i)
            {
                startDate = startDate.AddDays(1.0);

                while (startDate.DayOfWeek.IsWeekend())
                    startDate = startDate.AddDays(1.0);
            }
            return startDate;
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static bool IsPrime(this int number)
        {
            if (number % 2 == 0)
            {
                return number == 2;
            }
            int sqrt = (int)Math.Sqrt(number);
            for (int t = 3; t <= sqrt; t = t + 2)
            {
                if (number % t == 0)
                {
                    return false;
                }
            }
            return number != 1;
        }

        public static string SerializeToXml(this object obj)
        {
            XDocument doc = new XDocument();
            using (XmlWriter xmlWriter = doc.CreateWriter())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                xmlSerializer.Serialize(xmlWriter, obj);
                xmlWriter.Close();
            }
            return doc.ToString();
        }

        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        public static string ToCSV<T>(this IEnumerable<T> instance, char separator)
        {
            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.Each(value => csv.AppendFormat("{0}{1}", value, separator));
                return csv.ToString(0, csv.Length - 1);
            }
            return null;
        }

        public static string? ToCSV<T>(this IEnumerable<T> instance)
        {
            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.Each(v => csv.AppendFormat("{0},", v));
                return csv.ToString(0, csv.Length - 1);
            }
            return String.Empty;
        }

        public static IEnumerable<T> RemoveDuplicates<T>(this ICollection<T> list, Func<T, int> Predicate)
        {
            var dict = new Dictionary<int, T>();

            foreach (var item in list)
            {
                if (!dict.ContainsKey(Predicate(item)))
                {
                    dict.Add(Predicate(item), item);
                }
            }

            return dict.Values.AsEnumerable();
        }

        public static string GetMethodName(this MethodBase method)
        {
            string _methodName = method?.DeclaringType?.FullName ?? "Não Identificado";

            if (_methodName.Contains(">") || _methodName.Contains("<"))
            {
                _methodName = _methodName.Split('<', '>')[1];
            }
            else
            {
                _methodName = method?.Name ?? "Não Identificado";
            }

            return _methodName;
        }

        public static string GetClassName(this MethodBase method)
        {
            string className = method?.DeclaringType?.FullName ?? "Não Identificado";

            if (className.Contains(">") || className.Contains("<"))
            {
                className = className.Split('+')[0];
            }
            return className;
        }

        public static string GetUniqueFileName(this string OriginalName)
        {
            string Extension = Path.GetExtension(OriginalName).ToLower();
            Guid guid = Guid.NewGuid();
            return $"{TimeZoneManager.GetTimeNow().ToString("ddMMyyyy")}-{guid.ToString().Replace("-", "").Substring(0, 5)}{Extension}";
        }

        public static string RemoveNonNumericDigits(this string str)
        {
            return new string(str.Where(char.IsDigit).ToArray());
        }
    }
}