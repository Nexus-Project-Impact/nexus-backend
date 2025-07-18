using System;

public class TravelPackage
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Destination { get; set; }
	public int Duration { get; set; }
	public List<DateTime> AvailableDates { get; set; }
    public decimal Value { get; set; }
    public List<string> ImagesDestination { get; set; }
	public string HotelName { get; set; }
	public string HotelAddress { get; set; }
    public TimeSpan TimeFlight { get; set; }
    public string Airline { get; set; }
	public int HotelRating { get; set; }
	public string DepartureAirport { get; set; }
	public string ArrivalAirport { get; set; }
    public List<string> ImagesHotel { get; set; }

    //public List<string> VideosHotel { get; set; }
    //public List<string> VideosDestination { get; set; }



}
