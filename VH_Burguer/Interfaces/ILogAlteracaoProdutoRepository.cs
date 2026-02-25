using VH_Burguer.Domains;

namespace VH_Burguer.Interfaces
{
    public interface ILogAlteracaoProdutoRepository
    {
        List<Log_AlteracaoProduto> Listar(); // Lista de todos os logs/alterações
        List<Log_AlteracaoProduto> ListarPorProduto(int produtoId); // Lista de todos os logs/alterações de um produto específico
    }
}
