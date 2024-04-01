using System.Data.Common;
using backendnet.Data;
using backendnet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendnet.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CategoriasController:Controller{
    private readonly DataContext _context;

    public CategoriasController(DataContext context){
        _context = context;

    }


//GET: api/categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias(){
        return await _context.Categoria.AsNoTracking().ToListAsync();

    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> GetCategoria(int id){
        var categoria = await _context.Categoria.FindAsync(id);
        if(categoria==null){
            return NotFound();
        }
        return categoria;


    }

    [HttpPost]
    public async Task<ActionResult<Categoria>> PostCategoria(CategoriaDTO categoriaDTO){
        Categoria categoria = new(){
            Nombre = categoriaDTO.Nombre
        };

        _context.Categoria.Add(categoria);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategoria), new {id = categoria.CategoriaId}, categoria);

    }
    [HttpPut("{id}")]
     public async Task<IActionResult> PutCategoria(int id, CategoriaDTO categoriaDTO){
        if(id != categoriaDTO.CategoriaId){
            return BadRequest();
        }
        var categoria = await _context.Categoria.FindAsync(id);
        if(categoria == null){
            return NotFound();
        }

        categoria.Nombre = categoriaDTO.Nombre;

        try{
            await _context.SaveChangesAsync();

        }
        catch (DbException ex){
            Console.WriteLine(ex.Message);
            return BadRequest();

        }

        return NoContent();
        
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoria(int id){

        var categoria = await _context.Categoria.FindAsync(id);
        if(categoria == null){
            return NotFound();
        }

        if(categoria.Protegida){
            return BadRequest();
        }

        _context.Categoria.Remove(categoria);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

