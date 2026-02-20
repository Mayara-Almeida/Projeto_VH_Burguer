using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using VH_Burguer.Applications.Services;
using VH_Burguer.DTOs.Produtodto;
using VH_Burguer.Exceptions;

namespace VH_Burguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;

        public ProdutoController(ProdutoService service)
        {
            _service = service;
        }

        // Autenticação do usuário[
        private int ObterUsuarioIdLogado()
        {
            // Busca no token/claims o valor armazenado como id do usuário
            // ClaimTypes.NameIdentifier -> geralmente guarda o ID do usuário no JWT
            string? idTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(idTexto))
            {
                throw new DomainException("Usuário não autenticado");
            }

            // Converte o ID que veio com o texto para inteiro, pois o usuárioID no sistema está armazenado como int
            // As Claims(informações do usuário dentro do texto) sempre são armazenadas como texto.
            return int.Parse(idTexto);
        }

        [HttpGet]
        public ActionResult<List<LerProdutoDto>> Listar()
        {
            List<LerProdutoDto> produtos = _service.Listar();

            // return StatusCode(200, produtos);
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public ActionResult<LerProdutoDto> ObterPorId(int id)
        {
            LerProdutoDto produto = _service.ObterPorId(id);

            if (produto == null)
            {
                // return StatusCode(404);
                return NotFound();
            }

            return Ok(produto);
        }

        // GET -> caminho da imagem: api/produto/5/imagem (o número é o id da imagem)
        [HttpGet("{id}/imagem")]
        public ActionResult ObterImagem(int id)
        {
            try
            {
                var imagem = _service.ObterImagem(id);

                // Retorna o arquivo para o navegador
                // "image/jpeg" informa o tipo da imagem (MIME type)
                // O navgador entende que deve renderizar como imagem
                return File(imagem, "image/jpeg");
            }

            catch (DomainException ex)
            {
                return NotFound(ex.Message); // Não encontrou a imagem
            }
        }

        [HttpPost]
        [Consumes("multipart/form-data")] // Indica que recebe dados no formato multipart/form-data, necessário quando enviamos arquivos (ex: imagem do produto)
        [Authorize] // Exige login para adicionar produtos

        // [FromForm] -> diz que os dados vem do formulário da requisição (multipart/form-data)
        public ActionResult Adicionar([FromForm] CriarProdutoDto produtoDto)
        {
            try
            {
                int usuarioId = ObterUsuarioIdLogado();

                // o cadastro fica associado ao usuário logado
                _service.Adicionar(produtoDto, usuarioId);

                return StatusCode(201); // Created
            }

            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [Authorize]

        public ActionResult Atualizar(int id, [FromForm] AtualizarProdutoDto produtoDto)
        {
            try
            {
                _service.Atualizar(id, produtoDto);
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
                return BadRequest(ex.Message);
            }
        }
    }
}
