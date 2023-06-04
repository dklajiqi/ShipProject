namespace ShipProject.Models;

public class Ship
{
     // The ship name
    public string Name { get; set; } = string.Empty;

    // The length of the ship
    public double Length { get; set; } = 0.0;

    // The width of the ship
    public double Width { get; set; } = 0.0;

    // The ship code
    public string Code {get; set;} = string.Empty;
    
}