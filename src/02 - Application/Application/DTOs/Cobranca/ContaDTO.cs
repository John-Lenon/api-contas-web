using Domain.Interfaces;
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
        public int Id { get; set; } = 0;
        public string Nome { get; set; }
        public decimal ValorOriginal { get; set; }
        public DateTime DataVencimento { get; set; } = DateTime.MinValue;
        public DateTime DataPagamento { get; set; } = DateTime.MinValue;
        public int QuantidadeDiasAtraso { get; set; } = 0;
        public decimal ValorCorrigido { get; set; } = 0;
        public decimal Multa { get; set; } = 0;
        public decimal JurosDia { get; set; } = 0;
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
