# dotNet5781_7588_3756


ex2 design:

/***************** File 1 ****************/
Class Coordinate{
	Flids:
		flout _x;
		flout _y;

	Constractor(flout x, flout y);

	Mathods:
		get, set
		flout DistanceBetweenCoord(Coordinate other); // result >= 0

}


Class BusStation inheritor Coordinate{
	
	Filds:
		Int _busStationKey;
		string _addresBusStation;
	
	Constractor(int key, flout latitude, flout  longitude);

	Mathods:
		get, set for all
		string override ToString(); // Bus Station Code: 765432, 31.234567°N 34.56789°E
}
/***************** File 1 ****************/

/***************** File 2 ****************/
Class BusLineStation inheritor BusStation {
	Filds:
		flout _distanceFromPrevStation;
		TimeSpan _timeSpanFromPrevStation;

	Constreactor(BusLineStation prevStation);

	Mathod:
		get, set for all
		double DistanceBetweenStations(BusLineStation other);
		TimeSpan TimeBetweenStations(BusLineStation other);
}
/***************** File 2 ****************/

/***************** File 3 ****************/
Class BusLine{
	
	Filds:
		int _busLine
		BusLineStation _firstStation;
		BusLineStation _lastStation;
		Enum _area = {General, North, South, Center, Jerusalem, Ayosh};
		List<BusLineStation> _stations;

	Constractor();

	Mathods:
		string override ToString();
		bool RemoveStation();
		bool StationInTheRoute();
		double DistanceBetweenStations(BusLineStation other);
		TimeSpan TimeBetweenStations(BusLineStation other);
		BusLine SubRouteBeteenStation(BusLineStation first, BusLineStation second);
		bool FastRoute(BusLine other); // IComperble
}
/***************** File 3 ****************/

/***************** File 4 ****************/

Class BusLineCollection{
	
}

/***************** File 4 ****************/

/***************** File 5 ****************/
Main...
			

