using VH_Burguer.Contexts;
using VH_Burguer.Domains;
using VH_Burguer.Interfaces;

namespace VH_Burguer.Repositores
{
    public class PromocaoRepository : IPromocaoRepository
    {
        private readonly VH_BurguerContext _context;

        public PromocaoRepository(VH_BurguerContext context)
        {
            _context = context;
        }

        public List<Promocao> Listar()
        {
            return _context.Promocao.ToList();
        }

        public Promocao ObterPorId(int id) // Retorna apenas uma promoção obtida pelo id
        {
            Promocao promocao = _context.Promocao.FirstOrDefault(p => p.PromocaoID == id);
            return promocao;
        }

        public bool NomeExiste(string nome, int? promocaoIdAtual = null)
        {
            var consulta = _context.Promocao.AsQueryable();

            if (promocaoIdAtual.HasValue) // Verifica se já existe valor com esse id
            {
                consulta = consulta.Where(p => p.PromocaoID != promocaoIdAtual.Value); //
            }

            return consulta.Any(p => p.Nome == nome);
        }

        public void Adicionar(Promocao promocao)
        {
            _context.Promocao.Add(promocao);
            _context.SaveChanges();
        }

        public void Atualizar(Promocao promocao)
        {
            Promocao? promocaoBanco = _context.Promocao.FirstOrDefault(p => p.PromocaoID == promocao.PromocaoID);

            if(promocaoBanco == null)
            {
                return; 
            }

            promocaoBanco.Nome = promocao.Nome;
            promocaoBanco.DataExpiracao = promocao.DataExpiracao;
            promocaoBanco.StatusPromocao = promocao.StatusPromocao;

            _context.SaveChanges();
        }

        public void Remover(int id)
        {
            Promocao promocao = _context.Promocao.FirstOrDefault(p => p.PromocaoID == id);

            if(promocao == null)
            {
                return; 
            }

            _context.Promocao.Remove(promocao);
            _context.SaveChanges();
        }
    }
}
