using System.Data.Common;
using backendnet.Data;
using backendnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace backendnet.Controllers;

[Route("api/[controller]")]
[ApiController]

public class PeliculasController : Controller{
    private readonly DataContext _context;


    public PeliculasController(DataContext context)
    {
        _context = context;

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pelicula>>> GetPeliculas(){

        return await _context.Pelicula.Include(i=> i.Categorias).AsNoTracking().ToListAsync();

    }

    [HttpGet("{id}")]
    public async  Task<ActionResult<Pelicula>> GetPelicula (int id){

        var pelicula = await _context.Pelicula.Include(i => i.Categorias).AsTracking().FirstOrDefaultAsync(s => s.PeliculaId == id);

        if (pelicula == null)
        {
            return NotFound();

        }
        return pelicula;

    }

    [HttpPost]
    public async Task<ActionResult<Pelicula>> PostPelicula(PeliculaDTO peliculaDTO){
        Pelicula pelicula = new(){
            Titulo = peliculaDTO.Titulo,
            Sinopsis = peliculaDTO.Sinopsis,
            Anio = peliculaDTO.Anio,
            Poster = peliculaDTO.Poster,
            Categorias = []
        };

        if(peliculaDTO.Categorias != null){
            foreach(var categoriaId in peliculaDTO.Categorias){
                Categoria? categoria = await _context.Categoria.FindAsync(categoriaId);
                if (categoria !=null)
                    pelicula.Categorias.Add(categoria);
            }
        }

        _context.Pelicula.Add(pelicula);
        await _context.SaveChangesAsync();

        return CreatedAtAction (nameof(GetPelicula), new {id=pelicula.PeliculaId}, pelicula);

    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPelicula (int id, PeliculaDTO peliculaDTO){
        if(id != peliculaDTO.PeliculaId){
            return BadRequest();
        }

        var pelicula = await _context.Pelicula.Include(i => i.Categorias).FirstOrDefaultAsync(s =>  s.PeliculaId == id);
        if(pelicula == null){
            return NotFound();
        }
        pelicula.Titulo = peliculaDTO.Titulo;
        pelicula.Sinopsis = peliculaDTO.Sinopsis;
        pelicula.Anio = peliculaDTO.Anio;
        pelicula.Poster = peliculaDTO.Poster;
        pelicula.Categorias = [];

        if (peliculaDTO.Categorias !=null){
            foreach(var categoriaId in peliculaDTO.Categorias){
                Categoria? categoria = await _context.Categoria.FindAsync(categoriaId);
                if (categoria != null)
                    pelicula.Categorias.Add(categoria);
                
            }
        }

        try {
            await _context.SaveChangesAsync();
        }
        catch(DbException ex){
            Console.WriteLine(ex.Message);
            return BadRequest();
        }
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePelicula(int id){
        var pelicula = await _context.Pelicula.FindAsync(id);
        if(pelicula==null){
            return NotFound();
        }

        _context.Pelicula.Remove(pelicula);
        await _context.SaveChangesAsync();

        return NoContent();


    }
}