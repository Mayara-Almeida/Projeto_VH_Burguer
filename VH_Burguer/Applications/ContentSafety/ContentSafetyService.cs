using Google.GenAI;

namespace VH_Burguer.Applications.ContentSafety
{
    public class ContentSafetyService : IContentSafetyRepository
    {
        private readonly string _apiKey;

        public ContentSafetyService(IConfiguration configuration) // Construtor que vai trazer algumas infos da chave da api
        {
            // Validações:
            _apiKey = configuration["Gemini:ApiKey"] ?? // Verifica na appsetting.json se o ApiKey existe
                                                        // Caso vc tenha confifurado a variável de ambiente na sua máquina:
                                                        //Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? // Verifica se existe na variável de ambiente
                throw new Exception("API Key não configurada"); // Erro
        }

        // Herdar método da repository
        public async Task<(bool aprovado, string msg)> ValidarConteudo(string texto)
        {
            if(string.IsNullOrEmpty(_apiKey)) // Verificar se a api está vindo vazia 
            {
                return (false, "API Key não configurada"); // false -> nem vai cadastrar o produto se a api para validação não esiver configurada
            }

            try
            {
                // Cliente responsável pela comunicação com o Gemini
                Client client = new Client(apiKey: _apiKey);

                // Definir prompt
                string prompt = $@"Você é um moderador de conteúdo extremamente rigoroso para uma plataforma pública.

                    Analise o TEXTO abaixo considerando as regras:

                    - NÃO é permitido:
                      - palavrões, xingamentos ou linguagem vulgar (ex: ""caralho"", ""porra"", ""merda"", etc.)
                      - conteúdo ofensivo, agressivo ou desrespeitoso
                      - conteúdo com duplo sentido ou conotação sexual
                      - qualquer linguagem inadequada para ambiente profissional ou educacional
                      - conteúdo ilegal (drogas, armas, etc.)

                    - Mesmo que esteja em tom informal ou ""brincadeira"", ainda deve ser considerado INSEGURO.

                    - Seja extremamente conservador: na dúvida, classifique como INSEGURO.

                    Responda APENAS com:

                    SEGURO ou INSEGURO: [breve motivo em português]

                    TEXTO:{texto}";

                // Envia o texto para análise da IA
                var response = await client.Models.GenerateContentAsync(
                    model: "gemini-2.5-flash-lite", // Modelo do Gemini usado
                    contents: prompt // Conteúdo passado é o prompt
                );

                string result = response.Text?.Trim().ToUpper() ?? "";// response.Text? -> Vai pegar somente o texto da resposta (que pode vim vazia) e .Trim() -> vai ignorar os espaços vazios

                if (result.StartsWith("INSEGURO")){ // Se o resultado (a resposta) da IA iniciar com INSEGURO
                    return (false, result);
                } 
                 
                return (true, "Textos seguros!");
                
            }
            catch (Exception ex)
            {
                return(false, "Erro na IA " + ex.Message);
            }
        }
    }
}
