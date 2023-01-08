namespace Application.DTOs.Cobranca
{
    public class RegraDiaAtrasoDTO
    {
        public int Id { get; set; }
        public int DiasAtrasoMinimo { get; set; }
        public int DiasAtrasoMaximo { get; set; }
        public decimal Multa { get; set; }
        public decimal JurosDia { get; set; }
    }

    public class RegraDiaAtrasoFilterDTO
    {
        public int? Id { get; set; }
        public int? DiasAtrasoMinimo { get; set; }
        public int? DiasAtrasoMaximo { get; set; }
    }
}
