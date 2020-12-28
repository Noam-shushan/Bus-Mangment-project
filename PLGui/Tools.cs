using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLGui
{
    public class Tools
    {
		public static PO.Bus CopyBusBOToPO(BO.Bus busBo)
		{
			PO.Bus busPo = new PO.Bus()
			{
				FromDate = busBo.FromDate,
				LicenseNum = formatLiscenseNumber(busBo.LicenseNum, busBo.FromDate),
				FuelRemain = busBo.FuelRemain,
				Status = busBo.Status,
				TotalTrip = busBo.TotalTrip,				
		    };
			return busPo;
        }

		private static string formatLiscenseNumber(int liscenseNumber, DateTime fromDate)
		{
			string formatNumber = liscenseNumber.ToString();
			if (fromDate.Year < 2018)
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
