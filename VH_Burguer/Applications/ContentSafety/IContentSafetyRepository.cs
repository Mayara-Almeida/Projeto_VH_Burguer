namespace VH_Burguer.Applications.ContentSafety
{
    public interface IContentSafetyRepository
    {
        Task<(bool aprovado, string msg)> ValidarConteudo(string texto);
        // bool aprovado -> texto aprovado ou não
        // string msg -> aviso da recusa caso texto seja reprovado
    }
}
