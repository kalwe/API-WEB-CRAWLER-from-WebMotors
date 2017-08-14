using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VeiculosWebApi.DbContext;
using VeiculosWebApi.Interfaces;
using VeiculosWebApi.Interfaces.Repositories;
using VeiculosWebApi.Interfaces.Services;
using VeiculosWebApi.Models;
using VeiculosWebApi.Repositories;
using VeiculosWebApi.Services;
using WebCrawler.Tools;

namespace WebCrawler.Services
{
    public class ExtractorService : IDisposable
    {
        private static readonly IVeiculosDbContext db = new VeiculosDbContext();
        private static readonly IRepositoryBase<Marca> repositoryMarca = new RepositoryBase<Marca>(db);
        private static readonly IRepositoryBase<Modelo> repositoryModelo = new RepositoryBase<Modelo>(db);

        private readonly ICategoriaRepository categoriaController = new CategoriaRepository(db);
        private readonly IMarcaService marcaService = new MarcaService(repositoryMarca);
        private readonly IModeloService modeloService = new ModeloService(repositoryModelo);

        private ExtractorTools extractor = new ExtractorTools();

        public string Categoria;

        public async Task CriarCategoria(string categoria)
        {
            // Pesquisa e retorna as marcas por categoria
            await categoriaController.AddOrUpdateAsync(new Categoria()
            {
                Id = $"categorias/{categoria}",
                //Id = null,
                Active = true,
                CreateTimeStamp = $"{DateTime.Now:dddd-MM/dd/yyyy-HH:mm:ss}",
                ModificationTimeStamp = null,
                Name = char.ToUpper(categoria[0]) + categoria.Substring(1)
            });
        }

        public async Task<IEnumerable<Marca>> MarcasPorCategoria(string categoria)
        {
            var marcas = await marcaService.PorCategoria(categoria);
            return marcas.ToList();
        }

        // Extrai e grava no database todos os modelos de uma marca
        public async Task ExtractAndRecordModelosDeMarca(string marca)
        {
            // Extrai modelos da marca
            await extractor.ExtractModelos(marca);

            var marcaResult = await marcaService.PorNome(Categoria, marca);

            // Variaveis de controle
            int gravados = 0;
            int alterados = 0;

            // Retorna os modelos do database
            // Caso o modelo ja exista sera atualizada se nao sera gravada
            var modelosDatabase = await modeloService.PorMarca(Categoria, marca);

            if (modelosDatabase.Count() > 0)
            {
                // Se a entidade ja estiver cadastra atualiza
                foreach (var modeloDatabase in modelosDatabase)
                {
                    foreach (var modeloExtraida in extractor.Modelos)
                    {
                        // Compara se a marca extraida ja esta no database
                        if (modeloExtraida.Name == modeloDatabase.Name)
                        {
                            // Atualiza marca
                            modeloDatabase.ModificationTimeStamp = $"{DateTime.Now:dddd-MM/dd/yyyy-HH:mm:ss}";

                            modeloService.Add(modeloDatabase);

                            // Imprime no console
                            Console.WriteLine($"Modelo: {modeloDatabase.Name} (alterado)");
                            alterados++;
                        }
                    }
                }
            }
            else
            {
                // Adiciona na lista par ao commit
                foreach (var modeloExtraido in extractor.Modelos)
                {
                    modeloExtraido.Marca = marcaResult.Id;

                    modeloService.Add(modeloExtraido);

                    // Imprime no console
                    Console.WriteLine($"Modelo: \"{modeloExtraido.Name}\" (gravado)");
                    gravados++;
                }
            }

            // Commita alteracoes no database
            await modeloService.CommitAsync();

            // Imprime o resultado com o total de marcas gravas e atualizadas
            Console.WriteLine($"\nTotal de {extractor.Modelos.Count} modelos da {Categoria} da marca {marca}\n {gravados} gravados\n {alterados} alterados");
            Console.WriteLine("\n-------------------------------------------------\n");
            Thread.Sleep(500);
        }

        // Extrai todas as marcas das categorias 
        public async Task ExtractAndRecordMarcas(string categoria)
        {
            await CriarCategoria(categoria);

            // Seta a categoria e extrai as marcas
            extractor.Categoria = categoria;
            await extractor.ExtractMarcas();

            // Variaveis de controle
            int gravados = 0;
            int alterados = 0;

            // Retorna as marcas do database
            var marcasDatabase = await marcaService.PorCategoria(categoria);
            // Caso a marca ja exista sera atualizada se nao sera gravada
            if (marcasDatabase.Count() > 0)
            {
                // Se a entidade ja estiver cadastra atualiza
                foreach (var marcaDatabase in marcasDatabase)
                {
                    foreach (var marcaExtraida in extractor.Marcas)
                    {
                        // Compara se a marca extraida ja esta no database
                        if (marcaExtraida.Name == marcaDatabase.Name)
                        {
                            // Atualiza marca
                            marcaDatabase.ModificationTimeStamp = $"{DateTime.Now:dddd-MM/dd/yyyy-HH:mm:ss}";
                            marcaService.Add(marcaDatabase);

                            // Imprime no console
                            Console.WriteLine($"Marca: \"{marcaDatabase.Name}\" (alterada)");
                            alterados++;
                        }
                    }
                }
            }
            else
            {
                // Adiciona na lista par ao commit
                foreach (var marcaExtraida in extractor.Marcas)
                {
                    marcaService.Add(marcaExtraida);
                    // Imprime no console
                    Console.WriteLine($"Marca: {marcaExtraida.Name} (gravada)");
                    gravados++;
                }
            }

            // Commita alteracoes no database
            await marcaService.CommitAsync();

            // Imprime o resultado com o total de marcas gravas e atualizadas
            Console.WriteLine($"\nTotal de {extractor.Marcas.Count} de marcas de {categoria}\n {gravados} gravadas\n {alterados} alteradas\n");
            //Console.WriteLine("\nAperte [ENTER] para continuar >>>");
            //Console.ReadLine();
            Thread.Sleep(5000);
        }

        public void Dispose()
        {
            Dispose();
        }
    }
}