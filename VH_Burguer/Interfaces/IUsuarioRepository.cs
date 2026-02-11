using VH_Burguer.Domains;

namespace VH_Burguer.Interfaces
{
    public interface IUsuarioRepository
    {
        List<Usuario> Listar();

        Usuario? ObterPorId(int id); // Pode ser que não venha nenhum usuário na busca, portanto coloca-se o ponto de interrogação, podendo ser nulo

        Usuario? ObterPorEmail(string email);

        bool EmailExiste(string email); // Verificar se o email existe

        void Adicionar(Usuario usuario);

        void Atualizar(Usuario usuario);

        void Remover(int id); //Remove pelo id do usuário
    }
}
