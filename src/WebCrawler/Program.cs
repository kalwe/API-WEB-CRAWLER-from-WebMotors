using System;
using System.Threading.Tasks;
using System.Text;
using WebCrawler.Services;

class Program
{
    public static void ConfigureConsole()
    {
        // Console title
        string title = "VeiculosApi WebCrawler Extractor";
        Console.Title = title;
        Console.OutputEncoding = Encoding.UTF8;
        Console.ForegroundColor = ConsoleColor.Red;
    }

    // Main
    public static void Main(string[] args)
    {
        // Call MainAsync passing the params and get result
        MainAsync(args).GetAwaiter().GetResult();
    }

    // MainAsync
    public static async Task MainAsync(string[] args)
    {
        ConfigureConsole();

        // Usage Help if args equal '-h'
        // if (args.Contains("-h") | args.Contains("--help"))
        // {
        //     Console.WriteLine("Usage: -t {carro/moto} --modelos {s/n:default=n}  --show{only output} -q{quiet mode}}");
        // }

        var extractorService = new ExtractorService();

        string[] categorias = { "moto", "carro" };
        foreach (var categoria in categorias)
        {
            await MarcaModelo(categoria);
        }

        async Task MarcaModelo(string categoria)
        {
            extractorService.Categoria = categoria;
            await extractorService.ExtractAndRecordMarcas(categoria);

            var marcas = await extractorService.MarcasPorCategoria(categoria);
            foreach (var marca in marcas)
            {
                await extractorService.ExtractAndRecordModelosDeMarca(marca.Name);
            }
        }
    }
}