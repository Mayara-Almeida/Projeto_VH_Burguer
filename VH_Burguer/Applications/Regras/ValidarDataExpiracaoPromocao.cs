using VH_Burguer.Exceptions;

namespace VH_Burguer.Applications.Regras
{
    public class ValidarDataExpiracaoPromocao
    {
        public static void ValidarDataExpiracao(DateTime dataExpiracao)
        {
            if(dataExpiracao <= DateTime.Now)
            {
                throw new DomainException("Data de Expiração deve ser futura.");
            }
        }
    }
}
