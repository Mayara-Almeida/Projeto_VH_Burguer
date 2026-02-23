using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VH_Burguer.Applications.Services;
using VH_Burguer.DTOs.CategoriaDto;
using VH_Burguer.Exceptions;

namespace VH_Burguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _service;

        public CategoriaController(CategoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerCategoriaDto>> Listar()
        {
            List<LerCategoriaDto> categorias = _service.Listar();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public ActionResult<LerCategoriaDto> ObterPorId(int id)
        {
            LerCategoriaDto categoria = _service.ObterPorId(id);

            if(categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Adicionar(CriarCategoriaDto criarDto)
        {
            try
            {
                _service.Adicionar(criarDto);
                return StatusCode(201); // Created
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

       [HttpPut("{id}")]
       [Authorize]
       public ActionResult Atualizar(int id, CriarCategoriaDto criarDto)
        {
            try
            {
                _service.Atualizar(id, criarDto);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Remover(int id)
        {
            try
            {
                _service.Remover(id);
                return NoContent();
            }
            catch (DomainException ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
