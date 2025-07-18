using System;
using Nexus.Domain.Entities;


public class Evaluation
{
	public int Id { get; set; }
	public double Score { get; set; }
	public string Comment { get; set; }
	public DateTime Data { get; set; } = DateTime.UtcNow;
	public int UserId { get; set; }
	public int TravelPackageId { get; set; }
	public User User { get; set; }
	public TravelPackage TravelPackage { get; set; }


}