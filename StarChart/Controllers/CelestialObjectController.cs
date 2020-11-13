using System;
using System.Linq;
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

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Where(o => o.Id == id).SingleOrDefault();

            if (celestialObject == null)
                return NotFound();

            var satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id)?.ToList();
            celestialObject.Satellites = satellites;

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(o => o.Name == name).ToList();

            if (celestialObjects.Count == 0)
                return NotFound();

            foreach (var obj in celestialObjects)
            {
                var satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == obj.Id)?.ToList();
                obj.Satellites = satellites;
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            foreach (var obj in celestialObjects)
            {
                var satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == obj.Id)?.ToList();
                obj.Satellites = satellites;
            }

            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject obj)
        {
            _context.CelestialObjects.Add(obj);

            _context.SaveChanges();

            return CreatedAtRoute("GetById",
                new { id = obj.Id },
                obj);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject obj)
        {
            var celestialObject = _context.CelestialObjects.Where(o => o.Id == id).SingleOrDefault();

            if (celestialObject == null)
                return NotFound();

            celestialObject.Name = obj.Name;
            celestialObject.OrbitalPeriod = obj.OrbitalPeriod;
            celestialObject.OrbitedObjectId = obj.OrbitedObjectId;

            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _context.CelestialObjects.Where(o => o.Id == id).SingleOrDefault();

            if (celestialObject == null)
                return NotFound();

            celestialObject.Name = name;

            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(o => o.Id == id || o.OrbitedObjectId == id).ToList();

            if (celestialObjects.Count == 0)
                return NotFound();

            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
