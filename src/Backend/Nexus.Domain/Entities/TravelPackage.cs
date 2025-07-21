using System;
using Nexus.Domain.Entities;

public class TravelPackage
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Destination { get; set; }
	public int Duration { get; set; }
	public DateTime DepartureDate { get; set; }
	public DateTime ReturnDate { get; set; }
    public decimal Value { get; set; }
	//public ICollection<Image> Images { get; set; } = new List<Image>();

}
