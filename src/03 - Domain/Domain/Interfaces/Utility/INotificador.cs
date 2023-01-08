using Domain.Utilities;
using System.Collections.Generic;

namespace Domain.Interfaces.Application
{
    public interface INotificador
    {
        List<Notificacao> ListNotificacoes { get; }
        void Add(Notificacao notificacao);
        void Clear();
    }
}
