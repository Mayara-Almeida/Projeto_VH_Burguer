using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using VH_Burguer.Applications.Services;
using VH_Burguer.DTOs.UsuarioDto;
using VH_Burguer.Exceptions;

namespace VH_Burguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        // GET -> Lista informações
        [HttpGet]
        public ActionResult<List<LerUsuarioDTO>> Listar()
        {
            List<LerUsuarioDTO> usuarios = _service.Listar();

            // Retorna a lista de usuários, a partir da DTO de Services
            return Ok(usuarios); // StatusCode OK - 200 - DEU CERTO
        }

        [HttpGet("{id}")] // recebe um parâmetro chamado id
        public ActionResult<LerUsuarioDTO> ObterPorId(int id)
        {
            LerUsuarioDTO usuario = _service.ObterPorId(id);
            if (usuario == null)
            {
                return NotFound(); // StatusCode - Não encontrado - 404
            }

            return Ok(usuario); // Caso ele ache por id
        }

        [HttpGet("email/{email}")] // Recebe um caminho, o segundo email retona o email da pessoa no caminho - não é correto fazer isso
        public ActionResult<LerUsuarioDTO> ObterPorEmail(string email)
        {
            LerUsuarioDTO usuario = _service.ObterPorEmail(email);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // POST - envia dados
        [HttpPost]
        public ActionResult<LerUsuarioDTO> Adicionar(CriarUsuarioDTO usuarioDTO)
        {
            try // Dentro do try é tentado executar algo, se der certo já retorna o status
            {
                LerUsuarioDTO usuarioCriado = _service.Adicionar(usuarioDTO);

                return StatusCode(201, usuarioCriado);
            }

            catch (DomainException ex) // Catch é chamado quando o try der erro
            {
                return BadRequest(ex.Message); // Message é o construtor do Exception criado
            }
        }

        // PUT - Fazer alterações de todos os dados
        [HttpPut("{id}")]
        public ActionResult<LerUsuarioDTO> Atualizar(int id, CriarUsuarioDTO usuarioDTO)
        {
            try
            {
                LerUsuarioDTO usuarioAtualizado = _service.Atualizar(id, usuarioDTO);

                return StatusCode(200, usuarioAtualizado);
            }

            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // DELETE - remove os dados
        // No nosso banco o delete vai apenas inativar o usuário, devido ao uso da trigger
        [HttpDelete("{id}")]
        public ActionResult Remover(int id)
        {
            try
            {
                _service.Remover(id);
                return NoContent(); //~StatusCode 204 - Não retorna nada
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

