using System;

public class Bus
{
	private int _licenseNumber;
	private DateTime _startActivity;
	private int _mileage;
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

	public bool ValidInput(string liscenseNumber, string startActivity)
    {
        int result;
		if (!int.TryParse(liscenseNumber, out result))
		{
			Console.WriteLine("Erorr: c'not convert to int");
			return false;
		}
		DateTime date = Convert.ToDateTime(startActivity);
		if (date == null)
			return false;

		if (date.Year < 2018 &&
				result < MAX_DIGITS_FOR_BUS_BEFORE_2018)
			return true;
		if (date.Year >= 2018 && 
			result < MAX_DIGITS_FOR_BUS_AFTER_2018)
			return true;
		return false;
	}

	public int LiscenseNumber
    {
		get => _licenseNumber;
		set => _licenseNumber = value;
    }

	public int Mileage
    {
		get => _mileage; 
		/*אם הקילומטרז + הנסיעה החדשה קטן מ1200 
		 * וגם הוא תידלק
		 * וגם הוא לא צריך טיפול (כי עברה שנה או 20000 קלימטר)
		 * אז הוא יכול לקבל את הנסיעה החדשה
		 */
		set 
		{ 
			if (IsFueled && !this.NeedsTreatment())
				_mileage = value; 
		} 
    }

	public bool IsFueled
    {
		get => _isFueled;
		set => _isFueled = value;
    }

	public bool NeedRefueling(int kilomters)
    {
		return _mileage + kilomters >= MAX_KILOMETER_AFTER_REFUELING;
	}

	public void Treatment()
    {
		_lastTreatment = DateTime.Now;
    }

	public bool NeedsTreatment()
    {
		return (this._lastTreatment - DateTime.Now.Date).Days >= 365 ||
			_mileage >= KILOMETER_BEFORE_TREATMENT;
	}



}
