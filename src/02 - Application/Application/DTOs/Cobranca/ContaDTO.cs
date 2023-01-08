using System;

namespace Application.DTOs.Cobranca
{

    public class ContaGetDTO
    {
        public string Nome { get; set; }
        public decimal ValorOriginal { get; set; }
        public DateTime DataPagamento { get; set; }
        public int QuantidadeDiasAtraso { get; set; }
        public decimal ValorCorrigido { get; set; }
    }

    public class ContaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal ValorOriginal { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataPagamento { get; set; }
        public int QuantidadeDiasAtraso { get; set; }
        public decimal ValorCorrigido { get; set; }
        public decimal Multa { get; set; }
        public decimal JurosDia { get; set; }
        public string UsuarioId { get; set; }
    }

    public class ContaFilterDTO
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public DateTime? Vencimento { get; set; }
        public DateTime? Pagamento { get; set; }
        public int? QuantidadeDiasAtraso { get; set; }

        public DateTime? VencimentoInicial { get; set; }
        public DateTime? VencimentoFinal { get; set; }
    }
}
