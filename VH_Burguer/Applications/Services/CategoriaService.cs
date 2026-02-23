using VH_Burguer.Domains;
using VH_Burguer.DTOs.CategoriaDto;
using VH_Burguer.Exceptions;
using VH_Burguer.Interfaces;

namespace VH_Burguer.Applications.Services
{
    public class CategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public List<LerCategoriaDto> Listar()
        {
            List<Categoria> categorias = _repository.Listar();

            // Converte cada categoria para LerCategoriaDto
            List<LerCategoriaDto> categoriaDto = categorias.Select(categoria => new LerCategoriaDto{
                CategoriaId = categoria.CategoriaID,
                Nome = categoria.Nome
            }).ToList();

            // Retorna a lista já convertida em Dto
            return categoriaDto;
        }

        public LerCategoriaDto ObterPorId(int id)
        {
            Categoria categoria = _repository.ObterPorId(id);

            if(categoria == null)
            {
                throw new DomainException("Categoria não encontrada.");
            }

            LerCategoriaDto categoriaDto = new LerCategoriaDto
            {
                CategoriaId = categoria.CategoriaID,
                Nome = categoria.Nome
            };

            return categoriaDto;  
        }

        private static void ValidarNome(string nome) // Valida se o campo nome foi preenchido
        {
            if(string.IsNullOrWhiteSpace(nome))
            {
                throw new DomainException("Nome é obrigatório");
            }
        }

        public void Adicionar(CriarCategoriaDto criarDto)
        {
            ValidarNome(criarDto.Nome);

            if (_repository.NomeExiste(criarDto.Nome))
            {
                throw new DomainException("Categoria já existe.");
            }

            Categoria categoria = new Categoria
            {
                Nome = criarDto.Nome,
            };

            _repository.Adicionar(categoria);
        }

        public void Atualizar(int id, CriarCategoriaDto criarDto)
        {
            ValidarNome(criarDto.Nome);

            Categoria categoriaBanco = _repository.ObterPorId(id);

            if(categoriaBanco == null)
            {
                throw new DomainException("Categoria não encontrada.");
            }

            if(_repository.NomeExiste(criarDto.Nome, categoriaIdAtual: id)) // categoriaIdAtual: id -> categoriaIdAtual  recebe o ID
            {
                throw new DomainException("Já existe outra categoria com esse nome.");
            }

            categoriaBanco.Nome = criarDto.Nome;
            _repository.Atualizar(categoriaBanco);
        }

        public void Remover(int id)
        {
            Categoria categoriaBanco = _repository.ObterPorId(id);

            if(categoriaBanco == null)
            {
                throw new DomainException("Categoria não encontrada.");
            }

            // Caso ele encontre a categoria no banco, ele remove pelo id
            _repository.Remover(id);
        }
    }
}
