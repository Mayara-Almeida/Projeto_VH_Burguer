using Microsoft.EntityFrameworkCore;
using VH_Burguer.Contexts;
using VH_Burguer.Domains;
using VH_Burguer.Interfaces;

namespace VH_Burguer.Repositores
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly VH_BurguerContext _context;

        public ProdutoRepository(VH_BurguerContext context)
        {
            _context = context;
        }

        public List<Produto> listar()
        {
            List<Produto> produtos = _context.Produto
                .Include(produto => produto.Categoria) // Busca produtos e para cada produto traz as suas categorias
                .Include(PRODUTO => PRODUTO.Usuario) // Busca produtos e para cada produto traz as suas categorias
                .ToList();

            return produtos;
        }

        public Produto ObterPorId(int id)
        {
            Produto? produto = _context.Produto
                .Include(produtoDb => produtoDb.Categoria)
                .Include(produtoDb => produtoDb.Usuario)
                .FirstOrDefault(produtoDb => produtoDb.ProdutoID == id); // Procura no banco e retorna o produto com o id que requisitamos

            return produto; 
        }

        public bool NomeExiste(string nome, int? produtoIdAtual = null)
        {
            // AsQueryable -> Monta a consulta para executar passo a passo
            // Monta a consulta na tabela produto, porém não executa nada no banco ainda(fica segurando a consulta)
            var produtoConsultado = _context.Produto.AsQueryable();

            // se o produto atual tiver valor, então atualizamos o produto pelo id
            if (produtoIdAtual.HasValue)
            {
                produtoConsultado = produtoConsultado.Where(produto => produto.ProdutoID != produtoIdAtual.Value);
            }

            return produtoConsultado.Any(produto => produto.Nome== nome);
        }

        public byte[] ObterImagem(int id)
        {
            var produto = _context.Produto // O var é utilizado devido ao retorno do FirstorDefault
                .Where(produto => produto.ProdutoID == id)
                .Select(produto => produto.Imagem)
                .FirstOrDefault();

            return produto; // Retorna a imagem relacionada aquele produto por Id
        } 

        public void Adicionar(Produto produto, List<int> categoriaIds) // Para cada produto adicionado é necessário passar uma categoria também
        {
            List<Categoria> categorias = _context.Categoria
                .Where(categoria => categoriaIds.Contains(categoria.CategoriaID))
                .ToList(); // Contains -> retorna true se houver registro

            produto.Categoria = categorias; // Adiciona as categorias incluídas ao produto

            _context.Produto.Add(produto);
            _context.SaveChanges();
        }

        public void Atualizar(Produto produto, List<int> categoriaIds) // List<int> -> Traz somente uma lista de Ids
        {
            Produto? produtoBanco = _context.Produto
                .Include(produto => produto.Categoria)
                .FirstOrDefault(produtoAux => produtoAux.ProdutoID == produto.ProdutoID);

            if (produtoBanco == null)
            {
                return; // Se não existir um produto, ele não vai atualizar
            }

            // Atualização os produtos que estão no banco
            produtoBanco.Nome = produto.Nome;
            produtoBanco.Preco = produto.Preco;
            produtoBanco.Descricao = produto.Descricao;

            // Verificação para trocar a imagem apenas caso a nova seja diferente da que já estiver no banco
            if (produto.Imagem != null && produto.Imagem.Length > 0) // Coloca maior que zaro caso o campo esteja vazio ou em branco
            {
                produtoBanco.Imagem = produto.Imagem;
            }

            if(produto.StatusProduto.HasValue)
            {
                produtoBanco.StatusProduto = produto.StatusProduto;
            }

            // Busca todas as categorias no banco com o id igual das categorias que vieram da requisição/front
            var categorias = _context.Categoria
                .Where(categoria => categoriaIds.Contains(categoria.CategoriaID))
                .ToList();


            // Remove as ligações atuais entre o produto e as categorias
            // Ele não apaga as categorias do banco, apenas remove o vínculo com a tabela ProdutoCategoria
            produtoBanco.Categoria.Clear(); 

                        // auxiliar
            foreach (var categoria in categorias)
            {
                produtoBanco.Categoria.Add(categoria);
            }

            _context.SaveChanges();
        }

        public void Remover(int id)
        {
            Produto? produto = _context.Produto.FirstOrDefault(produto => produto.ProdutoID == id);

            if (produto == null)
            {
                return;
            }

            _context.Produto.Remove(produto);
            _context.SaveChanges();
        }
    }
}
