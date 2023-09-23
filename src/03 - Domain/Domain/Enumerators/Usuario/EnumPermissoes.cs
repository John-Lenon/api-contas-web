using System.ComponentModel;

namespace Domain.Enumerators.Usuario
{
    public enum EnumPermissoes
    {
        #region Permissoes para Contas
        /// <summary>
        /// Visualizar Contas
        /// </summary>
        [Description("Visualizar Contas")]
        USU_000001 = 1,

        /// <summary>
        /// Criar Contas
        /// </summary>
        [Description("Criar Contas")]
        USU_000002 = 2,

        /// <summary>
        /// Deletar Contas
        /// </summary>
        [Description("Deletar Contas")]
        USU_000003 = 3,

        /// <summary>
        /// Atualizar Contas
        /// </summary>
        [Description("Atualizar Contas")]
        USU_000004 = 4,
        #endregion

        #region Permissoes Administrativas

        /// <summary>
        /// Criar Novo Usuário
        /// </summary>
        [Description("Criar Novo Usuário")]
        USU_000005 = 5,

        /// <summary>
        /// Deletar Usuário
        /// </summary>
        [Description("Deletar Usuário")]
        USU_000006 = 6,

        /// <summary>
        /// Dar Permissoes
        /// </summary>
        [Description("Dar Permissoes a Usuários")]
        USU_000007 = 7,

        /// <summary>
        /// Atualizar Outros Usuários
        /// </summary>
        [Description("Atualizar Outros Usuários")]
        USU_000008 = 8
        #endregion
    }
}