using Domain.Interfaces.Application;
using Domain.Utilities;
using System.Collections.Generic;

namespace Application.Utility
{
    public class Notificador : INotificador
    {
        public List<Notificacao> ListNotificacoes { get; } = new List<Notificacao>();

        public void Add(Notificacao notificacao)
        {
            ListNotificacoes.Add(notificacao);
        }

        public void Clear()
        {
            ListNotificacoes.Clear();
        }
    }   
}
