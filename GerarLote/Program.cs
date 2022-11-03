using GerarLote;
using System.Globalization;

//https://balta.io/blog/datetime-csharp-dotnet

Console.WriteLine("Gerador de Lote");
Console.WriteLine($"Agora { DateTime.Now }");
Console.WriteLine($"PT-BR { DateTime.UtcNow.ToString(CultureInfo.CreateSpecificCulture("pt-BR")) }");
Console.WriteLine($"TimeZone { System.TimeZoneInfo.Local } ");


var LoteId = 0;

var Banco = new LotesNoBanco();
foreach (var lote in Banco.getLotesDb())
{
    Console.WriteLine("================");
    Console.WriteLine($"ID: {lote.Id}");
    Console.WriteLine($"Nº: {lote.Identificador}");
    Console.WriteLine("================");
}

var service = new Service(Banco.getLotesDb());

var identidicador = service.NovoIdentificador(LoteId);
Console.WriteLine($"Identificador 1º Lote ===> { identidicador }");

var LoteTres = new Lote()
{
    Id = 3,
    Identificador = identidicador
};

Banco.SetLoteDb(LoteTres);
service = new Service(Banco.getLotesDb());

var identidicadorSequencial = service.NovoIdentificador(3);
Console.WriteLine($"Identificador Subsequente do 1º Lote ===> { identidicadorSequencial }");

var LoteQuatro = new Lote()
{
    Id = 4,
    Identificador = identidicadorSequencial
};

Banco.SetLoteDb(LoteQuatro);
service = new Service(Banco.getLotesDb());

var identidicadorSequencialProximoDia = service.NovoIdentificador(4, DateTime.Today.AddDays(+1));
Console.WriteLine($"Identificador Proximo Dia 1º Lote ===> { identidicadorSequencialProximoDia }");






