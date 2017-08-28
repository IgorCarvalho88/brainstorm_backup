using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brainstorm.Models;
using Brainstorm.ViewModel;
using Brainstorm.Repository.Database;

namespace Brainstorm.Repository
{
    interface BrainstormRepository
    {
        DataRow guardarReuniao(BrainstormViewModel model);
        DataRow guardarTema(Tema tema, int id);

        DataRow guardarEstado(string estado, string data, int id, string utilizador);

        ReuniaoBrainstorm getReuniaoBrainstorm(int id);
        List<Tema> getBrainstormTemas(int id);

        DataRow alterarReuniao(BrainstormViewModel mode, int id);
        void alterarTema(Tema tema, int id);
        DataRow guardarInterveniente(Interveniente interveniente, int idBrainstorm);
        // DataRow alterarEstado(string estado, int id);

        List<GestaoBrainstorm> getReunioesBrainstorm();

        string getWorkflow(int id);

        void eliminarBrainstorm(int id);

        void deleteTemas(int? id);

        DataRow gerarActividade(Tema tema, string idTabelaAux, string ut, string moduloID, string componenteID, int idReuniao);

        string getTabelaId();
        string getComponente();
        string getModulo();

        DataRow guardarBrainstorm_anexo(int id_brainstorm, string path, string nome, string length);

        List<Anexo> getBrainstormAnexos(int id);
        Anexo getBrainstormAnexoByID(int id);

        string Verifica_GestInov_Configurado();
        void UpdateActividadeIdeia(string[] infoAtividade);

        IHttpActionResult CriarActividade(ReuniaoBrainstorm brainstorm, Tema tema, string responsavel);
        DataTable Get_Eq_Agendamento();

        DataTable getDescritivoPorCodigo(string cod_ut);

        void guardarChatMensagens(string mensagem, int idTema, int idReuniao, string ut, string data);

        List<ChatMensagem> getBrainstormChatMensagens(int idTema, int idReuniao);
    }
}
