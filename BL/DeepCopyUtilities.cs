using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public static class DeepCopyUtilities
    {
        public static T CopyPropertiesTo<T, S>(this S from, T to)
        {
            foreach (PropertyInfo propTo in to.GetType().GetProperties())
            {
                PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                if (propFrom == null)
                    continue;
                var value = propFrom.GetValue(from, null);
                if (value is ValueType || value is string)
                    propTo.SetValue(to, value);
            }
            return to;
        }

        public static object CopyPropertiesToNew<S>(this S from, Type type)
        {
            object to = Activator.CreateInstance(type); // new object of Type
            from.CopyPropertiesTo(to);
            return to;
        }
        public static string FormatLiscenseNumber(this BO.Bus bus)
        {
            string formatNumber = bus.LicenseNum.ToString();
            if (bus.FromDate.Year < 2018)
            { // 00-000-00
                int numOfZeros = Math.Abs(formatNumber.Length - 7);
                for (int j = 0; j < numOfZeros; j++)
                    formatNumber = formatNumber.Insert(0, "0"); // add zeros to the bigenig 
                formatNumber = formatNumber.Insert(2, "-");
                formatNumber = formatNumber.Insert(6, "-");
            }
            else
            { // 000-00-000
                int numOfZeros = Math.Abs(formatNumber.Length - 8);
                for (int j = 0; j < numOfZeros; j++)
                    formatNumber = formatNumber.Insert(0, "0"); // add zeros to the bigenig 
                formatNumber = formatNumber.Insert(3, "-");
                formatNumber = formatNumber.Insert(6, "-");
            }
            return formatNumber;
        }
    }
}
