namespace VH_Burguer.DTOs.Produtodto
{
    public class AtualizarProdutoDto
    {
        public string Nome { get; set; } = null!;

        public decimal Preco { get; set; }

        public string Descricao { get; set; } = null!;

        public IFormFile Imagem { get; set; } = null!; // IFormFile -> a imagem vem no formato multipart/form-data, ideal para upload de arquivo

        public List<int> CategoriaIds { get; set; } = new();

        public bool? StatusProduto {  get; set; } // Ponto de interrogação permite que ele não venha nada
    }
}
