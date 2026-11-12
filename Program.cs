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

List<string> files =
    [.. Directory.EnumerateFiles(folder, "*.txt")];

if (files.Count == 0)
{
    Console.WriteLine("Diretório vazio.");
    return;
}

Console.WriteLine("Digite o Nome a ser procurado:");
string? name;

do
{
    Console.Write("Digite o nome que procura: ");
    name = Console.ReadLine();
} while (string.IsNullOrWhiteSpace(name));

using var cts = new CancellationTokenSource();
CancellationToken token = cts.Token;

var service = new FindNameOnFileService(files, name.Trim());

await service.ExecuteAsync(2, token);
