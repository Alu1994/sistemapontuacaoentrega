using DatabaseAccess.DbSet;
using SistemaPontuacaoEntrega;
using System.Text;

namespace DatabaseAccess.Queries
{
    public class BuscarPontuacao
    {
        private string schemaDB = "[dbo]";

        public BuscarPontuacao()
        {

        }

        public BuscarPontuacao(string schemaDB)
        {
            this.schemaDB = schemaDB;
        }

        public PontuacaoDeslocamentoDescricao RetornarPontoDistanciaPercorrida(string cepinicio, string cepfim)
        {
            using (var x = new DbAccess<Zona>())
            {
                var zonaInicial = x.GetByQuery<Zona>(MontarBuscaZonaID(cepinicio))?[0]?.zonaID;
                var zonaFinal = x.GetByQuery<Zona>(MontarBuscaZonaID(cepfim))?[0]?.zonaID;

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("SELECT ");
                stringBuilder.AppendLine("	PTD.idPontuacaoDeslocamento, ");
                stringBuilder.AppendLine("	ZInicio.zonaID as idZonaDistritoCepInicio, ");
                stringBuilder.AppendLine("	ZInicio.descricao as zonaDistritoCepInicioDescricao, ");
                stringBuilder.AppendLine("	ZFinal.zonaID as idZonaDistritoCepFinal, ");
                stringBuilder.AppendLine("	ZFinal.descricao as zonaDistritoCepFinalDescricao, ");
                stringBuilder.AppendLine("	PTD.ponto ");
                stringBuilder.AppendLine("FROM ");
                stringBuilder.AppendLine($"	{schemaDB}.PONTUACAODESLOCAMENTO PTD WITH(NOLOCK) ");
                stringBuilder.AppendLine("	INNER JOIN ");
                stringBuilder.AppendLine($"		{schemaDB}.ZONA ZInicio WITH(NOLOCK) ON ");
                stringBuilder.AppendLine("		PTD.idZonaDistritoCepInicio = ZInicio.zonaID ");
                stringBuilder.AppendLine("	INNER JOIN ");
                stringBuilder.AppendLine($"		{schemaDB}.ZONA ZFinal WITH(NOLOCK) ON ");
                stringBuilder.AppendLine("		PTD.idZonaDistritoCepFinal = ZFinal.zonaID ");
                stringBuilder.AppendLine("WHERE ");
                stringBuilder.AppendLine($"	ZInicio.zonaID = {zonaInicial.Value} ");
                stringBuilder.AppendLine("	AND ");
                stringBuilder.AppendLine($"	ZFinal.zonaID = {zonaFinal.Value} ");

                return x.GetByQuery<PontuacaoDeslocamentoDescricao>(stringBuilder.ToString())?[0];
            }
        }

        private string MontarBuscaZonaID(string cep)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("SELECT ");
            stringBuilder.AppendLine("	zonaID, '' as descricao ");
            stringBuilder.AppendLine("FROM ");
            stringBuilder.AppendLine($"	{schemaDB}.ZONADISTRITOCEP WITH(NOLOCK) ");
            stringBuilder.AppendLine("WHERE ");
            stringBuilder.AppendLine($"	INICIALDISTRITOCEPID = (select INICIALDISTRITOCEPID FROM {schemaDB}.INICIALCEPDISTRITO WHERE codigoInicialCEP = '{cep}') ");

            return stringBuilder.ToString();
        }
    }
}
