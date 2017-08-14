using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using NSubstitute;
using VeiculosWebApi.Models;
using VeiculosWebApi.Controllers;

namespace VeiculosWebApiTests.Controllers
{
    public class CategoriaControllerTest : ControllersTestBase<Categoria>
    {
        private readonly CategoriaController categoriaController;

        public CategoriaControllerTest()
        {
            categoriaController = new CategoriaController();
        }

        // [Fact]
        // public async Task InverterStatusAtivo()
        // {
        //     // Arrange
        //     await CreateAndCommit("categorias/TestSwitchActive", false, null, null, "Test Switch Active");

        //     // Act
        //     await categoriaController.InverterActiveStatus("TestSwitchActive");
        //     var categoriaResult = await serviceBase.FindAsync("categorias/TestSwitchActive");

        //     // Assert
        //     Assert.True(categoriaResult.Active);
        // }

        [Fact]
        public async Task PorNome_DeveRetornarUmaCategoria()
        {
            // Arrange
            await CreateAndCommit("categorias/TestPorNome", true, null, null, "Categoria Por Nome");

            // Act
            var result = await categoriaController.PorNome("Categoria Por Nome");
            var categoriaResult = result.Value as Categoria;

            // Assert
            Assert.Equal("Categoria Por Nome", categoriaResult.Name);
        }

        [Fact]
        public async Task Active_ShouldReturnOnlyDocActiveEqualTrue()
        {
            // Arrange
            await CreateAndCommit("categorias/TestActive", true, null, null, "Test Ativos");

            // Act
            var result = await categoriaController.Active();
            var categoriasResult = result.Value as List<Categoria>;

            // Assert
            foreach (var categoriayResult in categoriasResult)
                Assert.True(categoriayResult.Active);
        }

        [Fact]
        public async Task Add_ShouldAddNewCategoryInDatabase()
        {
            // Arrange

            var categoria = new Categoria()
            {
                Id = "categorias/TestAdd",
                Name = "Add Test"
            };

            // Act
            await categoriaController.AddOrUpdate(categoria);
            var categoriaResult = await serviceBase.FindAsync(categoria.Id);

            // Assert
            Assert.Equal(categoria.Id, categoriaResult.Id);
        }

        [Fact]
        public async Task Update_ShouldUpdateExistenceCategory()
        {
            // Arrange
            await CreateAndCommit("categorias/TestUpdate", true, $"{DateTime.Now:dddd-MM/dd/yyyy-HH:mm:ss}", null, "Update Test");

            var result = await serviceBase.FindAsync("categorias/TestUpdate");

            // Act
            result.ModificationTimeStamp = $"{DateTime.Now:dddd-MM/dd/yyyy-HH:mm:ss}";
            await categoriaController.AddOrUpdate(result);

            var categoriaResult = await serviceBase.FindAsync(result.Id);

            // Assert
            Assert.Equal(result.ModificationTimeStamp, categoriaResult.ModificationTimeStamp);
        }

        [Fact]
        public async Task Get_ShouldReturnCategory()
        {
            // Arrange
            await CreateAndCommit("categorias/TestGet", true, null, null, "Test Get");

            // Act
            var result = await categoriaController.Find("TestGet");
            var categoriaResult = result.Value as Categoria;

            // Assert
            Assert.Equal("categorias/TestGet", categoriaResult.Id);
        }

        [Fact]
        public async Task List_ShouldListAllCategory()
        {
            // Arrange
            for (int i = 0; i < 4; i++)
                await CreateAndCommit($"categorias/TestDelete{i}", true, null, null, $"Test Delete {i}");

            // Act
            var result = await categoriaController.List();
            var categoriasResult = result.Value as List<Categoria>;

            // Assert
            Assert.InRange(categoriasResult.Count, 3, 9999);
        }

        [Fact]
        public async Task Delete_ShouldSetActiveEqualFalse()
        {
            // Arrange
            await CreateAndCommit("categorias/TestDelete", true, null, null, "Delete Test");

            // Act
            await categoriaController.Delete("TestDelete");
            var result = await serviceBase.FindAsync("categorias/TestDelete");

            // Assert
            Assert.False(result.Active);
        }

        [Fact]
        public async Task ConfirmDelete_ShouldDeleteCategory()
        {
            // Arrange
            await CreateAndCommit("categorias/TestConfirmDelete", true, null, null, "Confirm Delete Test");

            // Act
            await categoriaController.ConfirmDelete("TestConfirmDelete");
            var result = await serviceBase.FindAsync("categorias/TestConfirmDelete");

            // Assert
            Assert.Null(result);
        }

        private async Task CreateAndCommit(string id, bool active, string createTimeStamp, string modificationTimeStamp, string name)
        {
            var categoria = new Categoria()
            {
                Id = id,
                Active = active,
                CreateTimeStamp = createTimeStamp,
                ModificationTimeStamp = modificationTimeStamp,
                Name = name
            };
            await AddAndCommit(categoria);
        }
    }
}