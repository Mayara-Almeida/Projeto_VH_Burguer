using VH_Burguer.Applications.Conversoes;
using VH_Burguer.Applications.Regras;
using VH_Burguer.Domains;
using VH_Burguer.DTOs.Produtodto;
using VH_Burguer.Exceptions;
using VH_Burguer.Interfaces;

namespace VH_Burguer.Applications.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _repository;

        public ProdutoService(IProdutoRepository repository)
        {
            _repository = repository;
        }

        // Para cada produto que veio do banco
        // Cria um DTO só com o que a requisição/front precisa exibir
        public List<LerProdutoDto> Listar()
        {
            List<Produto> produtos = _repository.listar(); // Chamando método de listar produtos 

            // Conversão da lista de produtos para Dto
            // SELECT percorre cada produto e transforma em DTO -> LerProdutoDto
            List<LerProdutoDto> produtosDto =
                produtos.Select(ProdutoParaDto.ConverterParaDto).ToList();

            return produtosDto;
        }

        public LerProdutoDto ObterPorId(int id)
        {
            Produto produto = _repository.ObterPorId(id);

            if(produto == null)
            {
                throw new DomainException("Produto não encontrado");
            }

            // Converte o produto para DTO e devolve
            return ProdutoParaDto.ConverterParaDto(produto);
        }

        // Criando validações de cadastro e retornando
        private static void ValidarCadastro(CriarProdutoDto produtoDto)
        {
            if(string.IsNullOrWhiteSpace(produtoDto.Nome)) // Caso o nome esteja vazio ou em branco
            {
                throw new DomainException("Nome é obrigatório.");
            }

            if(produtoDto.Preco < 0)
            {
                throw new DomainException("Preço deve ser maior ue zero.");
            }

            if (string.IsNullOrWhiteSpace(produtoDto.Descricao))
            {
                throw new DomainException("Descrição é obrigatória.");
            }

            if (produtoDto.Imagem == null || produtoDto.Imagem.Length == 0)
            {
                throw new DomainException("Imagem é obrigatória.");
            }

            if(produtoDto.CategoriaIds == null || produtoDto.CategoriaIds.Count == 0)
            {
                throw new DomainException("Produto deve estar vinculado a no mínimo uma categoria.");
            }
        }

        public byte[] ObterImagem(int id)
        {
            byte[] imagem = _repository.ObterImagem(id); // Retorna a imagem que está no banco

            if(imagem == null || imagem.Length == 0)
            {
                throw new DomainException("Imagem não encontrada.");
            }

            return imagem;
        }

        // Retorar o método adicomar como leitura
        public LerProdutoDto Adicionar(CriarProdutoDto produtoDto, int usuarioId)
        {
            ValidarCadastro(produtoDto); // Antes de adicionar o validar cadastro tem que ser chamado

            if(_repository.NomeExiste(produtoDto.Nome))
            {
                throw new DomainException("Produto já existente.");
            }

            Produto produto = new Produto
            {
                Nome = produtoDto.Nome,
                Preco = produtoDto.Preco,
                Descricao = produtoDto.Descricao,
                Imagem = ImagemParaBytes.ConverterImagem(produtoDto.Imagem),
                StatusProduto = true,
                UsuarioID = usuarioId // mesmo usuarioId que ele recebe no método adicionar
            };

            _repository.Adicionar(produto, produtoDto.CategoriaIds); // Adiciona o produto e as categorias

            return ProdutoParaDto.ConverterParaDto(produto); // Conversão do produto para Dto
        }

        public LerProdutoDto Atualizar(int id, AtualizarProdutoDto produtoDto)
        {
            HorarioAlteracaoProduto.ValidarHorario(); // Verifica se está dentro do horário de atualização do cardápio(estaelecimento fechado)

            Produto produtoBanco = _repository.ObterPorId(id); // Busca o produto no banco pelo id

            if(produtoBanco == null)
            {
                throw new DomainException("Produto não enconrado.");
            }

            // produtoIdAtual: -> dois pontos serve para passar o valor do parâmetro
            if (_repository.NomeExiste(produtoDto.Nome, produtoIdAtual: id))
            {
                throw new DomainException("Já existe outro produto com esse nome.");
            }

            if (produtoDto.CategoriaIds == null || produtoDto.CategoriaIds.Count == 0)
            {
                throw new DomainException("Produto deve estar vinculado a no mínimo uma categoria.");
            }

            if(produtoDto.Preco < 0)
            {
                throw new DomainException("Preço deve ser maior ue zero.");
            }

            produtoBanco.Nome = produtoDto.Nome;
            produtoBanco.Preco = produtoDto.Preco;
            produtoBanco.Descricao = produtoDto.Descricao;

            if(produtoDto.Imagem != null && produtoDto.Imagem.Length > 0)
            {
                produtoBanco.Imagem = ImagemParaBytes.ConverterImagem(produtoDto.Imagem);
            }

            if(produtoDto.StatusProduto.HasValue)
            {
                produtoBanco.StatusProduto = produtoDto.StatusProduto.Value;
            }

            _repository.Atualizar(produtoBanco, produtoDto.CategoriaIds);
            return ProdutoParaDto.ConverterParaDto(produtoBanco); // Retorna a visualização do produto convertido
        }

        public void Remover(int id)
        {
            HorarioAlteracaoProduto.ValidarHorario();

            Produto produto = _repository.ObterPorId(id);

            if(produto == null)
            {
                throw new DomainException("Produto não encontrado.");
            }

            _repository.Remover(id);
        }
    }
}
