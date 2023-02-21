using System.Linq;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [ApiController]
    [Route("")]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        [ActionName("GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();
            if (celestialObject != null)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();
                return Ok(celestialObject);
            }

            else return NotFound();

        }

        [HttpGet("{name}")]
        [ActionName("GetByName")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (celestialObjects.Count > 0)
            {
                foreach (var celestialObject in celestialObjects)
                {
                    celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialObject.Id).ToList();
                }
                return Ok(celestialObjects);
            }

            else return NotFound();

        }

        [HttpGet("{name}")]
        [ActionName("GetAll")]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach(var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(celestialObjects);
            
        }
    }
}
