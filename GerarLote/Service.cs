using System.Text.RegularExpressions;

namespace GerarLote
{
    public class Service
    {
        public List<Lote> _dados { get; set; }

        public Service(List<Lote> dados)
        {
            _dados = dados;
        }

        public string NovoIdentificador(long Id, DateTime? day = null)
        {
            var user = "UserId";

            DateTime hoje = DateTime.Now;
            if (day != null)
                hoje = (DateTime)day;

            Int16 periodo;
            if (hoje.Hour < 12)
                periodo = (short)PeriodoEnum.Manha;
            else if (hoje.Hour < 18)
                periodo = (short)PeriodoEnum.Tarde;
            else
                periodo = (short)PeriodoEnum.Noite;

            string[] alphabet = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            var letra = alphabet[0];


            if (Id > 0)
            {
                var IdLoteAnterior = Id.ToString();
                var loteExistente = _dados.Where(d => d.Id == Id).FirstOrDefault();
                var identificadorExistente = loteExistente.Identificador;

                user = identificadorExistente.Split(".")[0];
                var LetraAtual = identificadorExistente.Split(".")[3];

                var regex = new Regex(@"[a-zA-Z]");
                var pegou = regex.IsMatch(LetraAtual);
                string apenasLetra = letra;
                if (pegou)
                    apenasLetra = regex.Match(LetraAtual).Value;

                var index = Array.BinarySearch(alphabet, apenasLetra);
                var novaLetra = (alphabet[++index])?? "A";

                //var dataDoLote = (string)identificadorExistente.Split(".")[1];
                //var anoDoLote = dataDoLote.Substring( 0, 4);
                //var mesDoLote = dataDoLote.Substring(4, 2);
                //var diaDoLote = dataDoLote.Substring(6, 2);
                //var dataLote = new DateTime(Int32.Parse(anoDoLote), Int32.Parse(mesDoLote), Int32.Parse(diaDoLote));

                var novoIdentificador = user + "." + String.Format("{0:yyyyMMdd}", hoje) + "." + periodo.ToString() + "." + novaLetra + IdLoteAnterior;
                return novoIdentificador;
            }
            else
            {
                var identificador = user + "." + String.Format("{0:yyyyMMdd}", hoje) + "." + periodo.ToString() + "." + letra + "0"; 
                return identificador;
            }
        }
    }
}
