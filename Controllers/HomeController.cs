using Microsoft.AspNetCore.Mvc;
using KartApplication.Models;
using System.Diagnostics;

namespace KartApplication.Controllers
{

    public class HomeController : Controller
{
    // If you need database connection, you can use the following lines:
    // private readonly ApplicationDbContext _context;

    //public HomeController(ApplicationDbContext context)
    //{
    //    _context = context;
    //}

    private static AreaChange lastChange = null;

    // GET: Display the form to create a new case (OpprettSak)
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // Privacy page (you can skip this or update as per your needs)
    public IActionResult Privacy()
    {
        return View();
    }

    // Handle error page
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // GET: Bruker/Index formunu görüntüle
    [HttpGet]
    public IActionResult OpprettSak()
    {
        return View();
    }

    // POST: Process the GeoJSON, description, address, and selected map type from the form
    [HttpPost]
    public IActionResult OpprettSak(string geoJson, string description, string address, string selectedMapType)
    {
        // Generate a new 8-digit numeric ID
        string newId = IdGenerator.GenerateNumericIdFromGuid();

        // Populate the AreaChange object
        lastChange = new AreaChange
        {
            Id = newId,
            GeoJson = geoJson,
            Description = description,
            Address = address,
            Dato = DateTime.Now, // Timestamp for the submission
            SelectedMapType = selectedMapType // Save the selected map type (fargekart, gratonekart, turkart, sjokart)
        };

        // Redirect to the 'Beskrivelse' page after submission
        return RedirectToAction("Beskrivelse");
    }

    // GET: Show the Beskrivelse page with the data submitted from OpprettSak
    [HttpGet]
    public IActionResult Beskrivelse()
    {
        if (lastChange != null)
        {
            return View(lastChange); // Pass the lastChange object to the view
        }

        return RedirectToAction("OpprettSak"); // If no data, redirect to the form
    }

    // POST: Handle the description submission and redirect to the 'Oversikt' page
    [HttpPost]
    public IActionResult Beskrivelse(string description)
    {
        if (lastChange != null)
        {
            lastChange.Description = description; // Update the description
        }
        return RedirectToAction("Oversikt");
    }

    // GET: Show the Oversikt page to display a summary of the submitted data
    [HttpGet]
    public IActionResult Oversikt()
    {
        if (lastChange != null)
        {
            return View(lastChange); // Pass the data to the view
        }

        return RedirectToAction("OpprettSak"); // Redirect to form if no data exists
    }

    // GET: Display the confirmation page (Kvittering) with a reference number
    [HttpGet]
    public IActionResult Kvittering(string id)
    {
        if (lastChange != null && lastChange.Id == id)
        {
            return View(lastChange); // Display the saved data
        }

        return NotFound(); // Return 404 if no data is found
    }

    // POST: Redirect to Kvittering page
    [HttpPost]
    public IActionResult Kvittering()
    {
        if (lastChange != null)
        {
            return View(lastChange); // Show the confirmation data
        }

        return NotFound(); // Return 404 if the session has no data
    }
}
}