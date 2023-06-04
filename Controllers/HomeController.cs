using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShipProject.Models;
using Newtonsoft.Json;

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

    
    [HttpPost]
    public ActionResult Create(Ship entry)
    {
        var json = LoadJson(jsonPath);

        List<Ship> ?ships = JsonConvert.DeserializeObject<List<Ship>>(json);

        ships?.Add(entry);

        var jsonToOutput = JsonConvert.SerializeObject(ships, Formatting.Indented);

        WriteJson(jsonPath, jsonToOutput);

        return View();
    }

    [HttpGet]
    public ActionResult Read(string code)
    {   
        Ship result;

        var json = LoadJson(jsonPath);

        List<Ship>? ships = JsonConvert.DeserializeObject<List<Ship>>(json);

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

        return View();
    }

    [HttpPut]
    public ActionResult Update(string code, Ship entry)
    {
        
        var json = LoadJson(jsonPath);

        List<Ship>? ships = JsonConvert.DeserializeObject<List<Ship>>(json);

        if(ships != null)
        {
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

    [HttpDelete]
    public ActionResult Delete(string code)
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

        return View();
    }



    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
