using BuscaParalela;

string folder = Path.Combine(
    AppContext.BaseDirectory,
    "resources"
);

if (!Directory.Exists(folder))
{
    Console.WriteLine("Diretório não encontrado.");
    return;
}

IEnumerable<string> files =
    Directory.EnumerateFiles(folder, "*.txt");

Console.WriteLine("Digite o Nome a ser procurado:");
string name = Console.ReadLine()!;

if (!files.Any() || string.IsNullOrWhiteSpace(name))
{
    Console.WriteLine("Diretório vazio ou nome inválido.");
    return;
}

using var cts = new CancellationTokenSource();
CancellationToken token = cts.Token;

var service = new FindNameOnFileService(files, name);

await service.ExecuteAsync(2, token);
