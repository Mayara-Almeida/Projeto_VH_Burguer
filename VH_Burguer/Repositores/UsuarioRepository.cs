using VH_Burguer.Contexts;
using VH_Burguer.Domains;
using VH_Burguer.Interfaces;

namespace VH_Burguer.Repositores
{
    //Repository foi criado para garantir a persistência dos dados, garantindo dua implementação
    public class UsuarioRepository : IUsuarioRepository 
    {
        private readonly VH_BurguerContext _context;

        public UsuarioRepository(VH_BurguerContext context)
        {
            _context = context;
        }

        public List<Usuario> Listar() //Listar usuários registrados no banco
        {
            return _context.Usuario.ToList();
        }

        public Usuario? ObterPorId(int id)
        {
            return _context.Usuario.Find(id); //Find perfoma melhor com a chave primária
        }

        public Usuario? ObterPorEmail(string email)
        {                                        //auxiliar
            return _context.Usuario.FirstOrDefault(usuario => usuario.Email == email); 
        }

        public bool EmailExiste(string email)
        {
            return _context.Usuario.Any(usuario => usuario.Email == email); // Any -> returna true ou false, validando se existe ALGUM usuário com esse email
        }

        public void Adicionar(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            _context.SaveChanges();
        }

        public void Atualizar(Usuario usuario)
        {
            //Verificar se o usuário existe dentro do banco, para poder atualizar ele
            Usuario? usuarioBanco = _context.Usuario.FirstOrDefault(usuarioAux => usuarioAux.UsuarioID == usuario.UsuarioID);

            if(usuarioBanco == null)
            {
                return;
            }

            usuarioBanco.Nome = usuario.Nome;
            usuarioBanco.Email = usuario.Email;
            usuarioBanco.Senha = usuario.Senha;

            _context.SaveChanges();
        }

        public void Remover(int id)
        {
            Usuario? usuario = _context.Usuario.FirstOrDefault(usuarioAux => usuarioAux.UsuarioID == id);

            if(usuario == null)
            {
                return;
            }

            _context.Usuario.Remove(usuario);
            _context.SaveChanges();

        }

    }
}
