using System;
using Xunit;
using System.Threading.Tasks;
using VeiculosWebApi.Models;
using System.Collections.Generic;
using NSubstitute;
using VeiculosWebApi.Controllers;
using VeiculosWebApi.Interfaces.Services;
using VeiculosWebApi.Services;
using VeiculosWebApi.Repositories;
using VeiculosWebApi.Interfaces.Repositories;

namespace VeiculosWebApiTests.Controllers
{
    public class ModeloControllerTest : ControllersTestBase<Modelo>
    {
        private static readonly IModeloRepository modeloRepository = new ModeloRepository(db);
        private readonly IModeloService modeloService = new ModeloService(modeloRepository, switchActiveStatus);
        private readonly ModeloController modeloController;

        public ModeloControllerTest()
        {
            modeloController = new ModeloController(modeloService);
        }

        [Fact]
        public async Task PorMarca_DeveRetornarModelosPorMarca()
        {
            // Arrange
            await CreateAndCommit("modelos/TestPorMarca", true, null, "categorias/1", "marcas/19", null, "Test Por Marca");

            // Act
            var result = await modeloController.PorMarca("1", "19");
            var modelosResult = result.Value as List<Modelo>;

            // Assert
            foreach (var modeloResult in modelosResult)
                Assert.Equal("marcas/19", modeloResult.Marca);
        }

        [Fact]
        public async Task NEGATIVE_PorMarca_DeveRetornarModelosPorMarca()
        {
            // Arrange
            await CreateAndCommit("modelos/TestPorMarcaNEGATIVE", true, null, null, null, null, "Test Por Marca NEGATIVO");

            // Act
            var result = await modeloController.PorMarca("1", "9");

            // Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Active_DeveRetornarApenasOsModelosAtivos()
        {
            // Arrange
            await CreateAndCommit("modelos/TestAtivos", true, null, null, null, null, "Test Ativos");

            // Act
            var result = await modeloController.Ativos();
            var modelosResult = result.Value as List<Modelo>;

            // Assert
            foreach (var modelo in modelosResult)
                Assert.True(modelo.Active);
        }

        [Fact]
        public async Task Add_DeveAdicionarUmModeloNoDatabase()
        {
            // Arrange
            var modelo = new Modelo()
            {
                Id = "modelos/TestAdd",
                Name = "AddTest"
            };

            // Act
            await modeloController.AddUpdateAsync(modelo);
            var result = await serviceBase.FindAsync(modelo.Id);

            // Assert
            Assert.Equal(modelo.Id, result.Id);
        }

        [Fact]
        public async Task Find_DeveObterUmModeloDoDatabase()
        {
            // Arrange
            await CreateAndCommit("modelos/TestGet", true, null, null, null, null, "GetTest");

            // Act
            var result = await modeloController.Find("TestGet");
            var modeloResult = result.Value as Modelo;

            // Assert
            Assert.Equal("modelos/TestGet", modeloResult.Id);
        }

        [Fact]
        public async Task Update_DeveAtualizarUmModeloNoDatabase()
        {
            // Arrange
            await CreateAndCommit("modelos/TestUpdate", true, $"{DateTime.Now:dddd-dd/MM/yyyy-HH:mm:ss}", null, null, null, "UpdateTest");

            var modeloResult = await serviceBase.FindAsync("modelos/TestUpdate");

            // Act
            modeloResult.ModificationTimeStamp = $"{DateTime.Now:dddd-dd/MM/yyyy-HH:mm:ss}";
            await modeloController.AddUpdateAsync(modeloResult);

            var _modeloResult = await serviceBase.FindAsync(modeloResult.Id);

            // Assert
            Assert.Equal(modeloResult.Id, _modeloResult.Id);
            Assert.Equal(modeloResult.ModificationTimeStamp, _modeloResult.ModificationTimeStamp);
        }

        [Fact]
        public async Task List_DeveListarTodosOsModelosDoDatabase()
        {
            // Arrange
            for (int i = 1; i < 4; i++)
                await CreateAndCommit($"modelos/TestList{i}", true, null, null, null, null, $"ListTest{1}");

            // Act
            var result = await modeloController.List();
            var modelosResult = result.Value as List<Modelo>;

            // Assert
            Assert.InRange(modelosResult.Count, 3, 999999);
        }

        [Fact]
        public async Task Delete_DeveDeixarOModeloInativo()
        {
            // Arrange
            await CreateAndCommit("modelos/TestDelete", true, null, null, null, null, "DeleteTest");

            // Act
            await modeloController.Delete("TestDelete");
            var modeloResult = await serviceBase.FindAsync("modelos/TestDelete");

            // Assert
            Assert.False(modeloResult.Active);
        }

        [Fact]
        public async Task ConfirmDelete_DeveDeletarUmModeloDoDatabaseUmModelo()
        {
            // Arrange
            await CreateAndCommit("modelos/TestConfirmDelete", true, null, null, null, null, "ConfirmDeleteTest");

            // Act
            await modeloController.ConfirmDelete("TestConfirmDelete");
            var modeloResult = await serviceBase.FindAsync("modelos/TestConfirmDelete");

            // Assert
            Assert.Null(modeloResult);
        }

        private async Task CreateAndCommit(string id, bool active, string modificationTimeStamp, string categoria, string marca, IList<string> versao, string nome)
        {
            var modelo = new Modelo()
            {
                Id = id,
                Active = active,
                CreateTimeStamp = $"{DateTime.Now:dddd-dd/MM/yyyy-HH:mm:ss}",
                ModificationTimeStamp = modificationTimeStamp,
                Categoria = categoria,
                Marca = marca,
                Versao = versao,
                Name = nome
            };

            await AddAndCommit(modelo);
        }
    }
}