using Microsoft.AspNetCore.Mvc;
using dii_MovieCatalogSvc.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace dii_MovieCatalogSvc.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieCatalogSvcContext _context;

        public MoviesController(MovieCatalogSvcContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetMovies()
        {
            return _context.Movies.Values;
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public ActionResult<Movie> GetMovie(Guid id)
        {
            if (_context.Movies.TryGetValue(id, out Movie movie))
            {
                return movie;
            }
            return NotFound();
        }
    }
}