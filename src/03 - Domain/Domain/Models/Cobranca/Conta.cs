using Domain.Entities.Usuarios;
using System;

namespace Domain.Entities.Cobranca
{
    public class Conta
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

        public bool ContaAtrasada => DataPagamento > DataVencimento;
        public Usuario Usuario { get; set; }
    }   
}
