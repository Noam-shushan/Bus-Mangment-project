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

	public Bus(int licenseNumber, DateTime startActivity)
	{
		_licenseNumber = licenseNumber;
		_startActivity = startActivity;
		_mileage = 0;
	}

	public int LiscenseNumber
    {
		get => _licenseNumber;
    }

	public int Mileage
    {
		get => _mileage;
		/*אם הקילומטרז + הנסיעה החדשה קטן מ1200 
		 * וגם הוא תידלק
		 * וגם הוא לא צריך טיפול (כי עברה שנה או 20000 קלימטר)
		 * אז הוא יכול לקבל את הנסיעה החדשה
		 */
		set { if (IsFueled && !this.NeedsTreatment())
				_mileage = value; } 
    }

	public bool IsFueled
    {
		get => _isFueled;
		set => _isFueled = value;
    }

	public bool NeedRefueling(int kilomters)
    {
		return _mileage + kilomters < MAX_KILOMETER_AFTER_REFUELING;

	}

	public void Treatment()
    {
		_lastTreatment = DateTime.Now;
    }

	private bool NeedsTreatment()
    {
		return (this._lastTreatment - DateTime.Now.Date).Days >= 365 ||
			_mileage >= KILOMETER_BEFORE_TREATMENT;
	}



}
