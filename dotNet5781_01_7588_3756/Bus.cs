using System;

public class Bus
{
    private DateTime? _startActivity; // the date of the stert activity
	private int _kilometers; // the total kilometers
    private DateTime? _lastTreatment; //the date of the last treatment
	private const int KILOMETER_BEFORE_TREATMENT = 20000; 
	private const int MAX_KILOMETER_AFTER_REFUELING = 1200;

	// constractor
	public Bus(string licenseNumber, DateTime? startActivity)
	{
		LiscenseNumber = licenseNumber;
		_startActivity = startActivity;
		KilometersAfterFueling = 0;
	}

    public string LiscenseNumber { get; set; }

    public int Kilometers
	{
		get => _kilometers; 
		set 
		{
			_kilometers += value;
			KilometersAfterTreatment += value;
			KilometersAfterFueling += value;
		} 
    }

    public int KilometersAfterFueling { get; set; }

    public int KilometersAfterTreatment { get; set; }

    public bool NeedRefueling(int newKilomters)
    {
		return KilometersAfterFueling + newKilomters >= MAX_KILOMETER_AFTER_REFUELING;
	}

	public void Treatment()
    {
		_lastTreatment = DateTime.Now;
		KilometersAfterTreatment = 0;
    }
	/*
	 * Checks if a year has passed since the last treatment
	 * or 20,000 kilometers since the last treatment
	 */
	public bool NeedsTreatment()
    {
		return (_lastTreatment - DateTime.Now.Date)?.Days >= 365 ||
			KilometersAfterTreatment >= KILOMETER_BEFORE_TREATMENT;
	}

	public override string ToString()
    {
		string formatLiscenseNumber = LiscenseNumber;
		if (_startActivity?.Year < 2018)
        { // 00-000-00
			formatLiscenseNumber = formatLiscenseNumber.Insert(2, "-");
			formatLiscenseNumber = formatLiscenseNumber.Insert(6, "-");
		}
		else
		{ // 000-00-000
			formatLiscenseNumber = formatLiscenseNumber.Insert(3, "-");
			formatLiscenseNumber = formatLiscenseNumber.Insert(6, "-");
		}
		return String.Format("liscense number: {0}\n" +
			"Kilometers: {1}\n" +
			"Kilometers since last treatment: {2}",
			formatLiscenseNumber, Kilometers,
			KilometersAfterTreatment);
	}
}
