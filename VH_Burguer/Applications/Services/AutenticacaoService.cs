using VH_Burguer.Applications.Autenticacao;
using VH_Burguer.Domains;
using VH_Burguer.DTOs.AutenticacaoDto;
using VH_Burguer.Exceptions;
using VH_Burguer.Interfaces;

namespace VH_Burguer.Applications.Services
{
    public class AutenticacaoService
    {
        private readonly IUsuarioRepository _repository;
        private readonly GeradorTokenJwt _tokenJwt;

        public AutenticacaoService(IUsuarioRepository repository, GeradorTokenJwt tokenJwt)
        {
            _repository = repository;
            _tokenJwt = tokenJwt;
        }

        // compara a hash SHA256 para validar a senha
        private static bool VerificarSenha(string senhaDigitada, byte[] senhaHashBanco) // Parâmetros: senha digitada com senha armazenada no banco
        {
            // Transformar a senha digitada em hash
            using var sha = System.Security.Cryptography.SHA256.Create(); 
            var hashDigitado = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senhaDigitada));

            // Comparar com a senha do banco
            return hashDigitado.SequenceEqual(senhaHashBanco);
        }

        public TokenDto Login(LoginDto loginDto)
        {
            // Verfificar o E-mail digitado com o E-mail armazenado no banco
            Usuario usuario = _repository.ObterPorEmail(loginDto.Email);
            
            if(usuario == null)
            {
                throw new DomainException("E-mail ou senha inválidos.");
            }

            // Comparar a senha digitada com a senha armazenada
            if(!VerificarSenha(loginDto.Senha, usuario.Senha))
            {
                throw new DomainException("E-mail ou senha inválidos.");
            }

            // Gerando o token (caso esteja tudo certo)
            var token = _tokenJwt.GerarToken(usuario);

            TokenDto novoToken = new TokenDto {  Token = token };

            return novoToken;
        }
    }
}
