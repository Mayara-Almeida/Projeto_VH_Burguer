using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VH_Burguer.Domains;
using VH_Burguer.Exceptions;

namespace VH_Burguer.Applications.Autenticacao
{
    public class GeradorTokenJwt
    {
        private readonly IConfiguration _config;

        // Recebe as configurações do appsettings.json
        public GeradorTokenJwt(IConfiguration config)
        {
            _config = config;
        }

        public string GerarToken(Usuario usuario) // Passa as informações do usuário ao gerar o token
        {
            // KEY -> chave secreta usada para assinar o token
            // Garante que o token não foi alterado
            var chave = _config["Jwt:Key"]!;

            // ISSUER -> quem gerou o token (nome da API / sistema que gerou)
            // a API valida se o token veio do emissor correto
            var issuer = _config["Jwt:Issuer"]!;

            // AUDIENCE -> para quem o token foi criado
            // define qual sistema pod eusar o token
            var audience = _config["Jwt:Audience"]!;

            //TEMPO DE EXPIRAÇÂO -> define quanto minutos o token será válido
            // depois disso, o usuário precisa logar novamente
            var expiraEmMinutos = int.Parse(_config["Jwt:ExpiraEmMinutos"]!);

            // Converte a chave para bytes(necessária para criar a assinatura)
            var keyBytes = Encoding.UTF8.GetBytes(chave); 

            // Segurança: exige uma chave com pelo menos 32 caracteres (256 bits)
           if(keyBytes.Length < 32)
           {
                throw new DomainException("Jwt: Key precisa ter pelo menos 32 caracteres (256 biits).");
           }

           // Cria a chave de segurança usada para assinar o token
           var securityKey = new SymmetricSecurityKey(keyBytes);

           // define o algoritmo de assinatura do token
           var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Claims -> informações dp usuário que vão dentro do token
            // essas informações podem ser recuperadas na API para identificar quem está logado
            var claims = new List<Claim>
           {
               // ID do usuário (para saber quem fez a ação)
               new Claim (ClaimTypes.NameIdentifier, usuario.UsuarioID.ToString()),

               // Nome do usuário
               new Claim(ClaimTypes.Name, usuario.Nome),

               // Email do usuário
               new Claim(ClaimTypes.Email, usuario.Email)
           };
            
           // Cria o token Jwt com todas as informações 
           var token = new JwtSecurityToken(
               issuer: issuer,                                         // Quem gerou o token
               audience: audience,                                     // Quem pode usar o token
               claims: claims,                                         // Dados do usuário
               expires: DateTime.Now.AddMinutes(expiraEmMinutos),      // Validade do token
               signingCredentials: credentials                         // Assinatura de segurança
           );

            // Converte o token para string e essa string é enviada para o cliente
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
