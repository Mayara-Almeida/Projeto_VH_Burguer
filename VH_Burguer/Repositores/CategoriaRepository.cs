using VH_Burguer.Contexts;
using VH_Burguer.Domains;
using VH_Burguer.Interfaces;

namespace VH_Burguer.Repositores
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly VH_BurguerContext _context;

        public CategoriaRepository(VH_BurguerContext context)
        {
            _context = context;
        }

        public List<Categoria> Listar()
        {
          return _context.Categoria.ToList();
        }

        public Categoria ObterPorId (int id)
        {
            Categoria categoria = _context.Categoria.FirstOrDefault(c => c.CategoriaID == id);

            return categoria;
        }

        public bool NomeExiste(string nome, int? categoriaIdAtual = null)
        {
            // AsQueryable -> Cria a consulta inicial na tabela Categoria, mas ainda não executa nada no banco
            var consulta = _context.Categoria.AsQueryable();

            // Se for informado um ID atual
            // Significa que estamos editando um categoria existente
            // Então vamos ignorar essa própria categoria na verificação
            if(categoriaIdAtual.HasValue) // Verificando se existe um valor
            {
                // Remove da busca a categoria com esse mesmo Id
                // Evita que o sistema considere o próprio registro como duplicado
                // exemplo: SELECT * FROM CategoriaID != 5
                consulta = consulta.Where(categoria => categoria.CategoriaID != categoriaIdAtual.Value);
            }

            // Verifica se existe alguma categoria com esse mesmo nome, retorna true ou false
            return consulta.Any(c => c.Nome == nome);
        }

        public void Adicionar(Categoria categoria)
        {
            _context.Categoria.Add(categoria);
            _context.SaveChanges();
        }

        public void Atualizar(Categoria categoria)
        {
            Categoria categoriaBanco = _context.Categoria.FirstOrDefault(c => c.CategoriaID == categoria.CategoriaID);

            if(categoriaBanco == null)
            {
                return;
            }

            // Caso tenha achado a CategoriaID no banco:
            categoriaBanco.Nome = categoria.Nome;
            _context.SaveChanges();
        }

        public void Remover (int id)
        {
            Categoria categoriaBanco = _context.Categoria.FirstOrDefault(c => c.CategoriaID == id);

            if(categoriaBanco == null)
            {
                return; 
            }

            _context.Categoria.Remove(categoriaBanco);
            _context.SaveChanges();
        }
    }
}
