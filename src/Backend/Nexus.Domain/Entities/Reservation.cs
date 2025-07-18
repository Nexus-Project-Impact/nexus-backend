using System;
using Nexus.Domain.Entities;


public class Class1
{
	public int Id { get; set; }
	public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
	public string Status { get; set; } 
	public int ReservationNumber { get; set; }
	public int UserId { get; set; }
	public int TravelPackageId { get; set; }
	public User User { get; set; }
	public TravelPackage TravelPackage { get; set; }

}
