namespace NetKubernetes2._0.DTOs.InmuebleDtos
{
    public class InmuebleResponseDto
    {
        public int Id { get; set; } 
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }

        public decimal Precio { get; set; }

        public string? Picture { get; set; }
    }
}
