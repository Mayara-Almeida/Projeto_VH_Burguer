using VH_Burguer.Contexts;
using VH_Burguer.Domains;
using VH_Burguer.Interfaces;

namespace VH_Burguer.Repositores
{
    public class LogAlteracaoProdutoRepository : ILogAlteracaoProdutoRepository
    {
        private readonly VH_BurguerContext _context;

        public LogAlteracaoProdutoRepository(VH_BurguerContext context)
        {
            _context = context;
        }

        public List<Log_AlteracaoProduto> Listar()
        {
            // OrderByDescending -> Ordena por data decrescente(dos últimos lançamentos até os primeiros)
            List<Log_AlteracaoProduto> log =
                _context.Log_AlteracaoProduto.OrderByDescending(l => l.DataAlteracao).ToList();

            return log; // Retorna a lista de todas as alterações de todos os produtos encontrados no banco 
        }

        public List<Log_AlteracaoProduto> ListarPorProduto(int produtoId)
        {
            List<Log_AlteracaoProduto> AlteracoesProduto =
                _context.Log_AlteracaoProduto
                .Where(log => log.ProdutoID == produtoId) // Procura o produto onde o id do banco seja igual ao id passado
                .OrderByDescending(log => log.DataAlteracao) // Ordena
                .ToList();

            return AlteracoesProduto; // Retorna a lista de log por produto
        }
    }
}
