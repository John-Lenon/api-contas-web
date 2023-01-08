namespace Domain.Entities.Cobranca
{
    public class RegraDiaAtraso
    {
        public int Id { get; set; }
        public int DiasAtrasoMinimo { get; set; }
        public int? DiasAtrasoMaximo { get; set; }
        public decimal Multa { get; set; }
        public decimal JurosDia { get; set; }
    }
}
