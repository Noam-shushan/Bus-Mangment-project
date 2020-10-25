using System;

public class Bus
{
	private int _licenseNumber;
	private DateTime _startActivity;
	private int _mileage;
	private DateTime _lastTreatment;
	private const int MAX_KILOMETER_AFTER_REFUELING = 1200

	public Bus(int licenseNumber, DateTime startActivity)
	{
		this._licenseNumber = licenseNumber;
		this._startActivity = startActivity;
		this._mileage = 0;
	}

	public int Mileage
    {
        get { return this._mileage }
		set { this._mileage = newMileage}
    }

	public void Treatment()
    {
		this._lastTreatment = DateTime.Now;
    }

	//private bool NeedsTreatment()
    //{
		//return this._lastTreatment

	//}



}
