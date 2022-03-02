using IMDb.Api.Extension;
using IMDb.Api.Models;
using IMDbApi.Context;
using IMDbApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMDb.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly dbContext _context;

        public MoviesController(dbContext context)
        {
            _context = context;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("Cadastro")]
        [Authorize(Roles = "isAdmin")]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        [Route("Voto")]
        public async Task<ActionResult<Movie>> RateMovie(Rating rate)
        {
            _context.ratings.Add(rate);
            await _context.SaveChangesAsync();

            return Ok("Votado");
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("GetMovies")]
        public ActionResult<PagedCollectionResponse<Movie>> Get([FromQuery] FilterModel filter)
        {
            //Filter  
            Func<FilterModel, IEnumerable<Movie>> filterData = (filterModel) =>
            {
                return _context.movies.Where(m => m.Name.StartsWith(filterModel.Name ??
                  String.Empty, StringComparison.InvariantCultureIgnoreCase)
                || m.Director.StartsWith(filterModel.Director ??
                  String.Empty, StringComparison.InvariantCultureIgnoreCase)
                || m.Genres.Contains(filterModel.MovieGenre)
                || m.Actors.Contains(filterModel.MovieActor))
                .Skip((filterModel.Page-1) * filter.Limit)
                .Take(filterModel.Limit)
                .OrderBy(n => n.Name)
                .AsNoTracking()
                .AsEnumerable();
            };


            //Get the data for the current page  
            var result = new PagedCollectionResponse<Movie>();
            result.Data = filterData(filter);

            //Get next page URL string  
            FilterModel nextFilter = new FilterModel();
            nextFilter.Page += 1;
            String nextUrl = filterData(nextFilter).Count() <= 0 ? null :
                this.Url.Action("Get", null, nextFilter, Request.Scheme);

            //Get previous page URL string  
            FilterModel previousFilter = new FilterModel();
            previousFilter.Page -= 1;
            String previousUrl = previousFilter.Page <= 0 ? null :
                this.Url.Action("Get", null, previousFilter, Request.Scheme);

            result.NextPage = !String.IsNullOrWhiteSpace(nextUrl) ? new Uri(nextUrl) : null;
            result.PreviousPage = !String.IsNullOrWhiteSpace(previousUrl) ? new Uri(previousUrl) : null;

            return result;
        }

    }
}
