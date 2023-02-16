namespace Relatorio.Utilitarios
{
    public class Cabecalho
    {
        public Cabecalho()
        {
            Parametros = new List<Dictionary<string, string>>();
        }

        public string PathLogomarca { get; set; }
        public string NomeRelatorio { get; set; }

        public List<Dictionary<String, String>> Parametros { get; set; }
    }
}
