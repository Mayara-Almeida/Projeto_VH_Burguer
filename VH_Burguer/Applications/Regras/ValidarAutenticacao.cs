using VH_Burguer.Applications.Services;
using VH_Burguer.Exceptions;

namespace VH_Burguer.Applications.Regras
{
    public class ValidarAutenticacao
    {
        public static void ValidarAutenticacaoLogin(AutenticacaoService validarAutenticacao)
        {
            if(validarAutenticacao == null)
            {
                throw new DomainException("Usuário não autenticado.");
            }
        }
    }
}
