using Xunit;
using System.Threading.Tasks;
using NSubstitute;
using WebCrawler.Tools;
using System.Collections.Generic;
using VeiculosWebApi.Models;
using System.Linq;

public class ExtractorToolsTests
{
    // Extract data from web request
    [Fact]
    public async Task ExtraiMarcas_ShouldExtractJsonFromUrlWhereFilterMatchRegex()
    {
        // Arrange
        var categoria = "moto";
        var _extractMarcas = Substitute.For<ExtractorTools>();
        var marcas = new List<Marca>();

        //Act
        _extractMarcas.Categoria = categoria;
        await _extractMarcas.ExtractMarcas();

        // Assert
        Assert.NotNull(_extractMarcas.Marcas);
        Assert.InRange(_extractMarcas.Marcas.Count, 1, 99999);
        foreach (var marca in _extractMarcas.Marcas)
            Assert.Matches(marca.Categoria, "categorias/"+categoria);
    }

    // Deve extrair os modelos de uma marca de cada categoria
    [Fact]
    public async Task ExtrairModelos_ExtraiTodosModelosAtivosDaMarcaPorCategoria()
    {
        // Arrange
        var categoria = "carro";
        var extractor = Substitute.For<ExtractorTools>();

        extractor.Categoria = categoria;
        await extractor.ExtractMarcas();
        var marca = extractor.Marcas.FirstOrDefault();

        // Act
        await extractor.ExtractModelos(marca.Name);

        // Assert
        Assert.NotNull(extractor.Modelos);
        Assert.InRange(extractor.Modelos.Count, 1, 999999);
        foreach(var modelo in extractor.Modelos)
            Assert.Matches(modelo.Categoria, "categories/"+categoria);
    }
}