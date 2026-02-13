namespace VH_Burguer.DTOs.UsuarioDto
{
    public class LerUsuarioDTO
    {
        public int UsuarioID { get; set; }

        public string Nome { get; set; } = null!;

        public string Email { get; set; } = null!;
        
        public bool StatusUsuario { get; set; }
    }
}
