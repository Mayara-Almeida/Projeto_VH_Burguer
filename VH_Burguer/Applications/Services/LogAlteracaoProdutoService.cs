using VH_Burguer.Domains;
using VH_Burguer.DTOs.LogProdutoDto;
using VH_Burguer.Interfaces;

namespace VH_Burguer.Applications.Services
{
    public class LogAlteracaoProdutoService
    {
        private readonly ILogAlteracaoProdutoRepository _repository;

        public LogAlteracaoProdutoService(ILogAlteracaoProdutoRepository repository)
        {
            _repository = repository;
        }

        public List<LerLogProdutoDto> Listar()
        {
            List<Log_AlteracaoProduto> logs = _repository.Listar();

            List<LerLogProdutoDto> listaLogProduto = logs.Select(log => new LerLogProdutoDto
            {
                LogId = log.Log_AlteracaoProdutoID,// log -> auxilixar que percorre o banco de dados
                ProdutoId = log.ProdutoID,
                NomeAnterior = log.NomeAnterior,
                PrecoAnterior = log.PrecoAnterior,
                DataAlteracao = log.DataAlteracao
            }).ToList();

            return listaLogProduto;
        }

        public List<LerLogProdutoDto> ListarPorProduto(int produtoId)
        {
            List<Log_AlteracaoProduto> logs = _repository.ListarPorProduto(produtoId);

            List<LerLogProdutoDto> listaLogproduto = logs.Select(log => new LerLogProdutoDto
            {
                LogId = log.Log_AlteracaoProdutoID, // log -> auxilixar que percorre o banco de dados
                ProdutoId = log.ProdutoID,
                NomeAnterior = log.NomeAnterior,
                PrecoAnterior = log.PrecoAnterior,
                DataAlteracao = log.DataAlteracao
            }).ToList();

            return listaLogproduto;
        }
    }
}
