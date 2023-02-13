namespace GerarLote
{
    public class LotesNoBanco
    {
        public readonly List<Lote> _lotes = new List<Lote>();

        public LotesNoBanco()
        {
            var loteUm = new Lote()
            {
                Id = 1,
                Identificador = "20221030-1-A"
            };
            _lotes.Add(loteUm);

            var loteDois = new Lote()
            {
                Id = 2,
                Identificador = "20221103-1-A"
            };
            _lotes .Add(loteDois);
        }

        public List<Lote> getLotesDb()
        {
            return _lotes;
        }

        public void SetLoteDb(Lote lote)
        {
            _lotes.Add(lote);
        }
    }
}
