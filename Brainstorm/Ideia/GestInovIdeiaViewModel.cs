using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Brainstorm.Ideia
{
    public class IdeiaViewModel
    {
        public Guid ID { get; set; }

        public string UtilizadorID { get; set; }

        public string UtilizadorDescritivo { get; set; }

        public int tipoIdeiaID { get; set; }

        public string titulo { get; set; }

        public string descricao { get; set; }

        public int origemID { get; set; }

        public string problemas { get; set; }

        public string riscos { get; set; }

        public string resultadosEsperados { get; set; }

        public string tecnologias { get; set; }

        public virtual List<AutorIdeiaViewModel> autores { get; set; }

        public string[] autoresID { get; set; }

        public virtual string acessoID { get; set; }

        public string observacoes { get; set; }

        public virtual EstadoIdeiaViewModel estado { get; set; }

        public DateTime dataRegisto { get; set; }

    }

    public class AutorIdeiaViewModel
    {
        public int ID { get; set; }

        public Guid ideiaID { get; set; }

        public virtual IdeiaViewModel ideia { get; set; }

        public string userID { get; set; }

        public string convidadoID { get; set; }
    }

    public class WorkflowEstadosIdeiaViewModel
    {
        public int ID { get; set; }

        public virtual string estado { get; set; }

        public Guid ideiaID { get; set; }

        public virtual IdeiaViewModel ideia { get; set; }

        public DateTime data { get; set; }

        public string UtilizadorID { get; set; }

        public string UtilizadorDescritivo { get; set; }
        public int? avaliacaoID { get; set; }
    }

    public class ComentarioViewModel
    {
        public int ID { get; set; }

        public string UtilizadorID { get; set; }

        public string AutorEmail { get; set; }

        public string ComentarioTexto { get; set; }

        public DateTime Data { get; set; }

        public Guid ideiaID { get; set; }

    }

    public class ClassificacaoIdeiaViewModel
    {
        public int ID { get; set; }

        public string utilizadorID { get; set; }

        public string utilizadorEmail { get; set; }

        public Guid ideiaID { get; set; }

        public virtual List<ClassificacaoTopicoViewModel> classificacoesTopicos { get; set; }
    }

    public class ClassificacaoTopicoViewModel
    {
        public int ID { get; set; }

        public string utilizadorID { get; set; }

        public string utilizadorEmail { get; set; }

        public int classificacaoIdeiaID { get; set; }

        public int classificacao { get; set; }

        public int topicoAvaliacaoID { get; set; }
    }


    public class TipoIdeiaViewModel
    {
        public int ID { get; set; }

        public string tipo { get; set; }

        public virtual List<TopicoClassificacaoViewModel> topicosAvaliacao { get; set; }

    }

    public class TopicoClassificacaoViewModel
    {
        public int ID { get; set; }

        public string topico { get; set; }

        public string legendaValorMinimo { get; set; }

        public string legendaValorMaximo { get; set; }

        public int tipoIdeiaID { get; set; }

        public int valorDefeito { get; set; }
    }

    public enum EstadoIdeiaViewModel
    {
        Pendente, Registada, Classificada, Aprovada, Rejeitada, PendenteNovaAvaliacao, EmExecucao, Implementada, Terminada
    }
    public enum EstadoAvaliacaoIdeiaViewModel
    {
        Pendente, Finalizada
    }
    public enum TipoDocumentoDestinoViewModel
    {
        Campanha, Projeto, Atividade
    }

    public class AvaliacaoIdeiaViewModel
    {
        public int ID { get; set; }

        public string[] participantesSelectList { get; set; }

        public virtual List<ParticipanteReuniaoIdeiaViewModel> participantes { get; set; }

        public DateTime dataInicio { get; set; }

        public DateTime dataFim { get; set; }

        public virtual List<AvaliacaoIdeiaIndividual> ideiasAvaliadas { get; set; }

        public int ataID { get; set; }

        public virtual EstadoAvaliacaoIdeiaViewModel estado { get; set; }
    }

    public class AvaliacaoIdeiaIndividual
    {
        public int ID { get; set; }

        public string responsavel { get; set; }

        public DateTime deadline { get; set; }

        public string deadlineString { get; set; }

        public string parecer { get; set; }

        public int avaliacaoID { get; set; }

        public Guid ideiaID { get; set; }

        public string observacoes { get; set; }

        public virtual TipoDocumentoDestinoViewModel documentoDestino { get; set; }

        public virtual EstadoIdeiaViewModel novoEstado { get; set; }

        public bool isAvaliada { get; set; }

    }

    public class ParticipanteReuniaoIdeiaViewModel
    {
        public int ID { get; set; }

        public int avaliacaoID { get; set; }

        public string userID { get; set; }

    }
}