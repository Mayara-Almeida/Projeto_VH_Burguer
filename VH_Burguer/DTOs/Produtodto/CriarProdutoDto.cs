namespace VH_Burguer.DTOs.Produtodto
{
    public class CriarProdutoDto
    {
        public string Nome { get; set; } = null!;

        public decimal Preco { get; set; }

        public string Descricao { get; set; } = null!;

        public IFormFile Imagem { get; set; } = null!; // IFormFile -> a imagem vem no formato multipart/form-data, ideal para upload de arquivo
        public List<int> CategoriaIds { get; set; } = new();
    }
}
