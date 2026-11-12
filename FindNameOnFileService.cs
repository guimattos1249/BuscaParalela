namespace BuscaParalela;
public class FindNameOnFileService
{
    private readonly IEnumerable<string> _files;
    private readonly string _name;

    public FindNameOnFileService (IEnumerable<string> files, string name)
    {
        _files = files;
        _name = name;
    }

    public async Task ExecuteAsync(int maxDegreeOfParalelism, CancellationToken ct)
    {
        if (maxDegreeOfParalelism < 1)
            throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParalelism), "O Paralelismo não pode ser menor que 1.");

        var parallelOptions = new ParallelOptions
        { 
            MaxDegreeOfParallelism = maxDegreeOfParalelism, 
            CancellationToken = ct 
        };

        await Parallel.ForEachAsync (
            _files, parallelOptions, 
            async (file, cancellationToken) =>
            {
                string fileName = Path.GetFileName(file);

                Console.WriteLine($"Processando arquivo: {fileName}");

                await Task.Delay(1500, cancellationToken);
                
                using StreamReader leitor = new(file);

                string? rowContent;
                int rowNumber = 0;
                int occurrences = 0;

                while ((rowContent = await leitor.ReadLineAsync(cancellationToken)) != null)
                {
                    rowNumber++;

                    if(rowContent.Contains(_name, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Linha: {rowNumber} - {rowContent}");
                        occurrences++;
                    }
                }

                if (occurrences > 0)
                    Console.WriteLine($"O nome Aparece {occurrences} vezes no arquivo {fileName}.");
                else 
                    Console.WriteLine($"Nome Não encontrado no arquivo {fileName}");

                Console.WriteLine("\n=============================");
            }
        );
    }
}