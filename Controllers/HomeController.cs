using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShipProject.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ShipProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private string jsonPath = "shiprepository.json";

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var json = LoadJson(jsonPath);

        List<Ship> ?ships = JsonConvert.DeserializeObject<List<Ship>>(json);

        return View("Index", ships);
    }

    [Route("edit/{code}")]
    public IActionResult Edit(string code)
    {
        Ship result = new Ship();

        var json = LoadJson(jsonPath);

        List<Ship> ?ships = JsonConvert.DeserializeObject<List<Ship>>(json);

        if(ships != null)
        {
            foreach(var ship in ships)
            {
                if(ship.Code == code)
                {
                    result = ship;
                    break;
                }
            }    
        }

        return View("Edit", result);
    }

    [Route("add")]
    public IActionResult Add()
    {
        return View("Add");
    }

    [Route("AddShip")]
    public IActionResult AddShip(Ship ship)
    {
        var guid = Guid.NewGuid();

        Console.WriteLine("Guid:" + guid);

        if (ship != null)
        {
            var regex = @"^[A-Za-z]{4}-\d{4}-[A-Za-z]\d$";

            var match = Regex.Match(ship.Code, regex, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                return NotFound();
            }


            var json = LoadJson(jsonPath);

            List<Ship> ?ships = JsonConvert.DeserializeObject<List<Ship>>(json);

            ships?.Add(ship);

            var jsonToOutput = JsonConvert.SerializeObject(ships, Formatting.Indented);

            WriteJson(jsonPath, jsonToOutput);

            return View("Index", ships);
        }
        else
        {
            return NotFound();
        }
    }


    
    [Route("EditShip")]
    public ActionResult Update(string code, Ship entry)
    {
        
        var json = LoadJson(jsonPath);

        List<Ship>? ships = JsonConvert.DeserializeObject<List<Ship>>(json);

        if(ships != null)
        {
            var regex = @"^[A-Za-z]{4}-\d{4}-[A-Za-z]\d$";

            var match = Regex.Match(entry.Code, regex, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                return NotFound();
            }

            foreach(var ship in ships)
            {
                if(ship.Code == code)
                {
                    ship.Code = entry.Code;
                    ship.Name = entry.Name;
                    ship.Length = entry.Length;
                    ship.Width = entry.Width;
                    break;
                }
                
            }
            var jsonToOutput = JsonConvert.SerializeObject(ships, Formatting.Indented);

            WriteJson(jsonPath, jsonToOutput);
        }

        return View();
    }
    


    [Route("delete/{code}")]
    public ActionResult Delete([FromRoute] string code)
    {
        var json = LoadJson(jsonPath);

        List<Ship> ?ships = JsonConvert.DeserializeObject<List<Ship>>(json);

        if(ships != null)
        {
            foreach(var ship in ships)
            {
                if(ship.Code == code)
                {
                    ships.Remove(ship);
                    break;
                }
            }

            var jsonToOutput = JsonConvert.SerializeObject(ships, Formatting.Indented);

            WriteJson(jsonPath, jsonToOutput);
        }

        return View("Index", ships);
    }



    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {});
    }

    public string LoadJson(string path)
    {
        string json = "";

        using (StreamReader r = new StreamReader(path))
        {
            json = r.ReadToEnd();
        }

        return json;
    }

    public void WriteJson(string path, string output)
    {
        using (StreamWriter wr = new StreamWriter(path))
        {
            wr.Write(output);
        }
    }
}
