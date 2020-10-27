using System;

public class Bus
{
	private string _licenseNumber; // the license number of the bus
	private DateTime? _startActivity; // the date of the stert activity
	private int _kilometers; // the total kilometers
	private int _prevKilometers; // the previes kilometers before a travel
	private int _kilometersAfterTreatment; // kilometers after treatment
	private DateTime? _lastTreatment; //the date of the last treatment
	private bool _isFueled; // flag to know if the bus is fueled
	private const int KILOMETER_BEFORE_TREATMENT = 20000; 
	private const int MAX_KILOMETER_AFTER_REFUELING = 1200;

	// constractor
	public Bus(string licenseNumber, DateTime? startActivity)
	{
		_licenseNumber = licenseNumber;
		_startActivity = startActivity;
		_isFueled = true;
	}


	public string LiscenseNumber
    {
		get => _licenseNumber;
		set => _licenseNumber = value;
    }

	public int Kilometers
	{
		get => _kilometers; 
		set 
		{
			_prevKilometers = _kilometers;
			//update only if the bus is fueled and dont need a treatment
			if (IsFueled && !this.NeedsTreatment())
				_kilometers += value; 
			
			_kilometersAfterTreatment = _kilometers;
			//If the bus has traveled a greater distance than 12000 kilometers
			//it needs refueling 
			if ((_kilometers - _prevKilometers) >=
				MAX_KILOMETER_AFTER_REFUELING)
				IsFueled = false;
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
		_kilometersAfterTreatment = 0;
    }
	/*
	 * Checks if a year has passed since the last treatment
	 * or 20,000 kilometers since the last treatment
	 */
	public bool NeedsTreatment()
    {
		return (_lastTreatment - DateTime.Now.Date)?.Days >= 365 ||
			_kilometersAfterTreatment >= KILOMETER_BEFORE_TREATMENT;
	}

	public string toString()
    {
		string strLiscenseNumber = _licenseNumber;
		if (_startActivity?.Year < 2018)
        { // 00-000-00
			strLiscenseNumber = strLiscenseNumber.Insert(2, "-");
			strLiscenseNumber = strLiscenseNumber.Insert(6, "-");
		}
		else
		{ // 000-00-000
			strLiscenseNumber = strLiscenseNumber.Insert(3, "-");
			strLiscenseNumber = strLiscenseNumber.Insert(6, "-");
		}
		return String.Format("liscense number: {0}\n" +
			"Kilometers: {1}\n" +
			"date of last treatment: {2}",
			strLiscenseNumber, Kilometers,
			_lastTreatment?.ToShortDateString());
	}
}
