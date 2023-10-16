using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovicesApi.Models;

namespace MovicesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private new List<string> _allowExtentions = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize =1048576;
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]MovieDto dto )
        {
            if (!_allowExtentions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
               return BadRequest("Only .png and .jpg images are allowed!");
            if(dto.Poster.Length>_maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1Mb!");
            var isValidGenre = await _context.Genres.AnyAsync(g=>g.Id==dto.GenreId); 
            if(!isValidGenre)
                return BadRequest("Invalid genre ID!");
            using var dataStream=new MemoryStream();    

            await dto.Poster.CopyToAsync(dataStream);
            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Title = dto.Title,  
                Poster=dataStream.ToArray(),
                Rate = dto.Rate,
                Storeline = dto.Storeline,
                Year = dto.Year,    
            };
           await _context.AddAsync(movie);
            _context.SaveChanges(); 
            return Ok(movie);

        }
    }
}
