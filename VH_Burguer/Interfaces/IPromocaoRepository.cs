using VH_Burguer.Domains;

namespace VH_Burguer.Interfaces
{
    public interface IPromocaoRepository
    {
        List<Promocao> Listar();
        Promocao ObterPorId(int id);

        bool NomeExiste(string nome, int? promocaoAtual = null);
        void Adicionar(Promocao promocao);
        void Atualizar(Promocao promocao);
        void Remover(int id);
    }
}
