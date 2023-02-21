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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById",  new {id = celestialObject.Id}, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var oldCelestialObject = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();
            if (oldCelestialObject != null)
            {
                oldCelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
                oldCelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
                oldCelestialObject.Name= celestialObject.Name;

                _context.Update(oldCelestialObject);
                _context.SaveChanges();
                return NoContent();
            }

            else return NotFound();

        }

        [HttpPatch("{id/name}")]
        public IActionResult RenameObject(int id, string Name)
        {
            var oldCelestialObject = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();
            if (oldCelestialObject != null)
            {
                oldCelestialObject.Name = Name;

                _context.Update(oldCelestialObject);
                _context.SaveChanges();
                return NoContent();
            }

            else return NotFound();

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var oldCelestialObject = _context.CelestialObjects.Where(x => x.Id == id).FirstOrDefault();
            if (oldCelestialObject != null)
            {
                _context.RemoveRange(oldCelestialObject);
                _context.SaveChanges();
                return NoContent();
            }

            else return NotFound();

        }

    }
}
