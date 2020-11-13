using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

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
    }
}
