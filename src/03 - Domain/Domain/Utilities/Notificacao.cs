namespace Domain.Utilities
{
    public class Notificacao
    {
        public EnumTipoNotificacao Tipo { get; set; }
        public string Mensagem { get; set; }

        public Notificacao(EnumTipoNotificacao tipo, string mensagem)
        {
            Tipo = tipo;
            Mensagem = mensagem;
        }

        public Notificacao()
        {
        }
    }

    public enum EnumTipoNotificacao
    {
        Informacao = 1,
        Error = 2,
    }
}
