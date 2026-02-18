using VH_Burguer.Exceptions;

namespace VH_Burguer.Applications.Regras
{
    public class HorarioAlteracaoProduto
    {
        public static void ValidarHorario()
        {
            var agora = DateTime.Now.TimeOfDay; // Pegando o horário atual
            var abertura = new TimeSpan(10, 0, 0); // Criando horário de abertura da loja para restrição - hora, minuto, segundo (vai abrir 16h) 
            var fechamento = new TimeSpan(23, 0, 0); // Criando horário de fechamento

            // Verificar se o estabelecimento está aberto
            // Retorna true ou false
            var estaAberto = agora >= abertura && agora <= fechamento; 

            if (estaAberto) // se retornar true
            {
                throw new DomainException("Produto só pode ser alterado fora do horário de funcionamento do estabeleimento");
            }
        }
    }
}
