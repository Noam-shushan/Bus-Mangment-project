using System;
using System.Text;

public class Bus
{
	private int _licenseNumber;
	private DateTime _startActivity;
	private int _kilometers;
	private DateTime _lastTreatment;
	private bool _isFueled;
	private const int KILOMETER_BEFORE_TREATMENT = 20000;
	private const int MAX_KILOMETER_AFTER_REFUELING = 1200;
	private const int MAX_DIGITS_FOR_BUS_BEFORE_2018 = 1000000;
	private const int MAX_DIGITS_FOR_BUS_AFTER_2018 = 10000000;


	public Bus(int licenseNumber, DateTime startActivity)
	{
		_licenseNumber = licenseNumber;
		_startActivity = startActivity;
		_isFueled = true;
	}

	public bool ValidInput(int liscenseNumber, string startActivity)
    {

		DateTime date = Convert.ToDateTime(startActivity);
		if (date == null)
			return false;
		if (date.Year < 2018 &&
				liscenseNumber < MAX_DIGITS_FOR_BUS_BEFORE_2018)
			return true;
		if (date.Year >= 2018 &&
			liscenseNumber < MAX_DIGITS_FOR_BUS_AFTER_2018)
			return true;
		return false;
	}

	public int LiscenseNumber
    {
		get => _licenseNumber;
		set => _licenseNumber = value;
    }

	public int Kilometers
	{
		get => _kilometers; 
		/*אם הקילומטרז + הנסיעה החדשה קטן מ1200 
		 * וגם הוא תידלק
		 * וגם הוא לא צריך טיפול (כי עברה שנה או 20000 קלימטר)
		 * אז הוא יכול לקבל את הנסיעה החדשה
		 */
		set 
		{ 
			if (IsFueled && !this.NeedsTreatment())
				_kilometers = value; 
		} 
    }

	public bool IsFueled
    {
		get => _isFueled;
		set => _isFueled = value;
    }

	public bool NeedRefueling(int kilomters)
    {
		return kilomters >= MAX_KILOMETER_AFTER_REFUELING;
	}

	public void Treatment()
    {
		_lastTreatment = DateTime.Now;
    }

	public bool NeedsTreatment()
    {
		return (this._lastTreatment - DateTime.Now.Date).Days >= 365 ||
			_kilometers >= KILOMETER_BEFORE_TREATMENT;
	}

	public string toString()
    {
		string strLiscenseNumber = "";
		if (_startActivity.Year < 2018)
        {
			strLiscenseNumber = AddZeros(LiscenseNumber,
				MAX_DIGITS_FOR_BUS_BEFORE_2018);

			strLiscenseNumber.Insert(2, "-");
			strLiscenseNumber.Insert(6, "-");
		}
		else
		{
			strLiscenseNumber = AddZeros(LiscenseNumber,
				MAX_DIGITS_FOR_BUS_AFTER_2018);

			strLiscenseNumber.Insert(3, "-");
			strLiscenseNumber.Insert(6, "-");
		}
		return String.Format("liscense number : {0}\nKilometers: {1}", strLiscenseNumber, Kilometers);
	}
	
	private string AddZeros(int liscenseNumber, int yaerDefintion)
    {
		var sb = new StringBuilder(liscenseNumber.ToString());
		for (int x = yaerDefintion; x > 0; x /= 10)
		{
			if (liscenseNumber < x)
				sb.Insert(0, "0");
		}
		return sb.ToString();
	}



}
