namespace DatabaseAccess.DbSet
{
    public class PontuacaoDeslocamento
    {
        public int idPontuacaoDeslocamento { get; set; }
        public int idZonaDistritoCepInicio { get; set; }
        public int idZonaDistritoCepFinal { get; set; }
        public string Ponto { get; set; }
    }

    public class PontuacaoDeslocamentoDescricao
    {
        public int idPontuacaoDeslocamento { get; set; }
        public int idZonaDistritoCepInicio { get; set; }
        public string zonaDistritoCepInicioDescricao { get; set; }
        public int idZonaDistritoCepFinal { get; set; }
        public string zonaDistritoCepFinalDescricao { get; set; }
        public string Ponto { get; set; }
    }
}
