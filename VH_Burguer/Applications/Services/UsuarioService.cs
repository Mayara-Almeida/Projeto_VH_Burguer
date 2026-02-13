using System.Data;
using System.Security.Cryptography;
using System.Text;
using VH_Burguer.Domains;
using VH_Burguer.DTOs.UsuarioDto;
using VH_Burguer.Exceptions;
using VH_Burguer.Interfaces;

namespace VH_Burguer.Applications.Services
{

    // Service concentra o "como fazer" todas as ações
    public class UsuarioService
    {
        private readonly IUsuarioRepository _repository; //_repository é o canal para acessar os dados.

        //Injeção de dependências
        // Implementa o repositório e o service só depende da interface
        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        // Utilizamos a private pois o método não é regra de negócio e não faz sentido existir fora do usuarioService
        private static LerUsuarioDTO LerDto(Usuario usuario) // Pega a entidade usuario e gera um DTO
        {
            LerUsuarioDTO lerUsuario = new LerUsuarioDTO // Instanciando objeto para cria-ló a partir do molde
            {
                UsuarioID = usuario.UsuarioID,
                Nome = usuario.Nome,
                Email = usuario.Email,
                StatusUsuario = usuario.StatusUsuario ?? true // Se não tiver status no banco, deia como true
            };

            return lerUsuario;
        }

        public List<LerUsuarioDTO> Listar()
        {
            List<Usuario> usuarios = _repository.Listar(); // Puxa a lisa usuarios do banco 

            List<LerUsuarioDTO> usuarioDto = usuarios //salva a lista de usuarios na DTO
                .Select(usuarioBanco => LerDto(usuarioBanco)) // SELECT que percorre cada usuário e LerDto(usuario)
                .ToList(); //ToList() -> devolve uma lista de DTOs
            return usuarioDto;
        }

        private static void ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new DomainException("Email inválido.");
            }
        }

        private static byte[] HashSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha)) // Garante que a senha não esteja vazia
            {
                throw new DomainException("Senha é obrigatória.");
            }

            using var sha256 = SHA256.Create();// Gera um hash SHA256 e devolve em byte[]
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
        }

        public LerUsuarioDTO ObterPorId(int id) // Busca usuário por Id
        {
            Usuario? usuario = _repository.ObterPorId(id);

            if (usuario == null)
            {
                throw new DomainException("Usuário não existe.");
            }

            return LerDto(usuario); // Se existe usuário, converte para DTO e devove o usuário
        }

        public LerUsuarioDTO ObterPorEmail(string email) // Busca usuário por email
        {
            Usuario? usuario = _repository.ObterPorEmail(email);

            if (usuario == null)
            {
                throw new DomainException("Usuário não existe.");
            }

            return LerDto(usuario); // Se existe usuário, converte para DTO e devove o usuário
        }

        public LerUsuarioDTO Adicionar(CriarUsuarioDTO usuarioDto)
        {
            ValidarEmail(usuarioDto.Email);

            if (_repository.EmailExiste(usuarioDto.Email))
            {
                throw new DomainException("Já existe um uuário com esse e-mail");
            }

            Usuario usuario = new Usuario // Criando entidade usuario
            {
                Nome = usuarioDto.Nome,
                Email = usuarioDto.Email,
                Senha = HashSenha(usuarioDto.Senha),
                StatusUsuario = true
            };

            _repository.Adicionar(usuario); // Lá dentro ele irá adicionar o usuario e salvar
            return LerDto(usuario); // Retorna LerDto para não retornar objeto com a senha.
        }

        public LerUsuarioDTO Atualizar(int id, CriarUsuarioDTO usuarioDto)
        {
            Usuario usuarioBanco = _repository.ObterPorId(id);

            if (usuarioBanco == null)
            {
                throw new DomainException("Usuário não encontrado.");
            }

            ValidarEmail(usuarioDto.Email);

            Usuario usuarioComMesmoEmail = _repository.ObterPorEmail(usuarioDto.Email);

            if (usuarioComMesmoEmail != null && usuarioComMesmoEmail.UsuarioID != id) // Verificando se já existe um usuário com aquele email
            {
                throw new DomainException("Já existe um usuário com esre e-mail.");
            }

            // As informações que estão no banco(usuarioBanco) vão receber as novas alterações passadas, vindas de usuarioDto
            usuarioBanco.Nome = usuarioDto.Nome;
            usuarioBanco.Email = usuarioDto.Email;
            usuarioBanco.Senha = HashSenha(usuarioDto.Senha);

            _repository.Atualizar(usuarioBanco);

            return LerDto(usuarioBanco);
        }

        public void Remover(int id)
        {
            Usuario usuario = _repository.ObterPorId(id);

            if(usuario == null) // Caso não encontre o usuário
            {
                throw new DomainException("Usuário não encontrado");
            }

            _repository.Remover(id); // Se for enontrado, passa para o repository remover
        }
    }
}
