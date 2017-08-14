using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using VeiculosWebApi.Models;
using VeiculosWebApi.Controllers;

namespace VeiculosWebApiTests.Controllers
{
    public class MarcaControllerTest : ControllersTestBase<Marca>
    {
        private readonly MarcaController marcaController;

        public MarcaControllerTest()
        {
            marcaController = new MarcaController();
        }

        [Fact]
        public async Task PorCategoria_DeveRetornarAsMarcasDaCategoria()
        {
            // Arrange
            await CreateAndCommit("marcas/TestPorCategoria1", true, null, "categorias/1", "Marca Por Categoria1 Test");

            // Act
            var result = await marcaController.PorCategoria("1");
            var marcasResult = result.Value as List<Marca>;

            // Assert
            foreach (var marcaResult in marcasResult)
                Assert.Equal("categorias/1", marcaResult.Categoria);
        }

        [Fact]
        public async Task NEGATIVE_PorCategoria_DeveRetornarAsMarcasDaCategoria()
        {
            // Arrange
            await CreateAndCommit("marcas/TestPorCategoria1NEGATIVO", true, null, "categorias/1", "Marca Por Categoria1 Test NEGATIVO");

            // Act
            var result = await marcaController.PorCategoria("4");

            // Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task PorNome_DeveRetornarUmaMarcaPorNomeECategoria()
        {
            // Arrange
            await CreateAndCommit("marcas/TestPorNome", true, null, "categorias/1", "TEST POR NOME");

            // Act
            var result = await marcaController.PorNome("1", "TEST POR NOME");
            var marcaResult = result.Value as Marca;

            // Assert
            Assert.Equal("TEST POR NOME", marcaResult.Name);
        }

        [Fact]
        public async Task NEGATIVE_MarcaPorNome_DeveRetornarUmaMarcaPorNomeECategoria()
        {
            // Arrange
            await CreateAndCommit("marcas/TestPorNomeNEGATIVO", true, null, null, "Teste Por Nome NEGATIVO");

            // Act
            var result = await marcaController.PorNome("1", "NEGATIVO");

            // Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Add_ShouldAddMarcaInDatabase()
        {
            // Arrange
            var marca = new Marca()
            {
                Id = "marcas/TestAdd",
                Name = "ADD TEST",
            };

            // Act
            await marcaController.AddUpdateAsync(marca);

            var marcaResult = await serviceBase.FindAsync(marca.Id);

            // Assert
            Assert.Equal(marca.Id, marcaResult.Id);
        }

        [Fact]
        public async Task Update_ShouldUpdateModelInDatabase()
        {
            // Arrange
            await CreateAndCommit("marcas/TestUpdate", true, null, null, "Update Test");

            var marcaResult = await serviceBase.FindAsync("marcas/TestUpdate");

            // Act
            marcaResult.ModificationTimeStamp = $"{DateTime.Now:dddd-dd/MM/yyyy-HH:mm:ss}";
            await marcaController.AddUpdateAsync(marcaResult);

            var _marcaResult = await serviceBase.FindAsync("marcas/TestUpdate");

            // Assert
            Assert.Equal(marcaResult.ModificationTimeStamp, _marcaResult.ModificationTimeStamp);
        }

        [Fact]
        public async Task Find_ShouldGetMarcaInDatabase()
        {
            // Arrange
            await CreateAndCommit("marcas/TestGet", true, null, null, "Test Get");

            // Act
            var result = await marcaController.Find("TestGet");
            var marcaResult = result.Value as Marca;

            // Assert
            Assert.Equal("marcas/TestGet", marcaResult.Id);
        }

        [Fact]
        public async Task List_ShouldReturnAllMarcaFromDatabase()
        {
            // Arrange
            for (int i = 1; i < 4; i++)
                await CreateAndCommit($"marcas/TestList{i}", true, null, null, $"Test List{i}");

            // Act
            var result = await marcaController.List();
            var marcasResult = result.Value as List<Marca>;

            // Assert
            Assert.InRange(marcasResult.Count, 3, 99999);
        }

        [Fact]
        public async Task Active_ShouldReturnOnlyMarcaWithPropActiveEqualTrue()
        {
            // Arrange
            await CreateAndCommit("marcas/TestActive", true, null, null, "Test Active");

            // Act
            var result = await marcaController.Active();
            var marcasResult = result.Value as List<Marca>;

            // Assert
            foreach (var resultMarcas in marcasResult)
                Assert.True(resultMarcas.Active);
        }

        [Fact]
        public async Task Delete_ShouldSetActiveEqualFalse()
        {
            // Arrange
            await CreateAndCommit("marcas/TestDelete", true, null, null, "Delete Test");

            // Act
            await marcaController.Delete("TestDelete");

            var marcaResult = await serviceBase.FindAsync("marcas/TestDelete");

            // Assert
            Assert.False(marcaResult.Active);
        }

        [Fact]
        public async Task ConfirmDelete_ShouldDeleteInDatabase()
        {
            // Arrange
            await CreateAndCommit("marcas/TestConfirmDelete", true, null, null, "Confirm Delete");

            // Act
            await marcaController.ConfirmDelete("TestConfirmDelete");

            var marcaResult = await serviceBase.FindAsync("marcas/TestConfirmDelete");

            // Assert
            Assert.Null(marcaResult);
        }

        private async Task CreateAndCommit(string id, bool active, string modificationTimeStamp, string categoria,string nome)
        {
            var marca = new Marca()
            {
                Id = id,
                Active = active,
                CreateTimeStamp = $"{DateTime.Now:dddd-MM/dd/yyyy-HH:mm:ss}",
                ModificationTimeStamp = modificationTimeStamp,
                Categoria = categoria,
                Principal = true,
                Name = nome
            };

            await AddAndCommit(marca);
        }
    }
}