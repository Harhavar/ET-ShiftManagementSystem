using Moq;
using ET_ShiftManagementSystem.Controllers;
using ET_ShiftManagementSystem.Models;
using ET_ShiftManagementSystem.Services;
using ET_ShiftManagementSystem.Entities;
using ET_ShiftManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using ET_ShiftManagementSystem.Servises;
using Pipelines.Sockets.Unofficial.Arenas;
using ET_ShiftManagementSystem.Models.ProjectsModel;

namespace ProjectUnitTest
{
    public class UnitTestController
    {
        private readonly Mock<IProjectServices> productService;
        private readonly Mock<IUserRepository> User;
        private readonly Mock<IShiftServices> shiftServices;
        private readonly Mock<ShiftManagementDbContext> _dbContext;
        private readonly ShiftManagementDbContext dbContext;

        public UnitTestController()
        {
            productService = new Mock<IProjectServices>();
            User = new Mock<IUserRepository>();
            shiftServices = new Mock<IShiftServices>();
            _dbContext = new Mock<ShiftManagementDbContext>();
        }
        public UnitTestController(ShiftManagementDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [Fact]
        public void GetProductList_ProductList()
        {
            //arrange
            var productList = GetProductsData();
            productService.Setup(x => x.GetProjectsData())
                .Returns(productList);
            var productController = new ProjectsController(productService.Object, User.Object, shiftServices.Object);

            //act
            var productResult = productController.GetProjectsData();

            //assert
            Assert.NotNull(productResult);
           // Assert.Equal(GetProductsData().Count(), productResult.Count());
            Assert.Equal(GetProductsData().ToString(), productResult.ToString());
            Assert.True(productList.Equals(productResult));
        }

        [Fact]
        public void GetProductByID_Product()
        {
            //arrange
            var productList = ProjectById(Guid.Parse("A1A3C9D6 - 743F - 433F - 87A9 - 928DC30ADBFB"));
            productService.Setup(x => x.GetProjectById(Guid.Parse("A1A3C9D6 - 743F - 433F - 87A9 - 928DC30ADBFB")))
                .Returns(productList);
            var productController = new ProjectsController(productService.Object, User.Object, shiftServices.Object);

            //act
            var productResult = productController.GetProjectById(Guid.Parse("A1A3C9D6 - 743F - 433F - 87A9 - 928DC30ADBFB"));

            //assert
            Assert.NotNull(productResult);
            //Assert.Equal(productList.ProductId, productResult.ProductId);
            //Assert.True(productList[1].ProductId == productResult.ProductId);
        }

        //[Theory]
        //[InlineData("IPhone")]
        //public void CheckProductExistOrNotByProductName_Product(string productName)
        //{
        //    //arrange
        //    var productList = GetProductsData();
        //    productService.Setup(x => x.GetProductList())
        //        .Returns(productList);
        //    var productController = new ProductController(productService.Object);

        //    //act
        //    var productResult = productController.ProductList();
        //    var expectedProductName = productResult.ToList()[0].ProductName;

        //    //assert
        //    Assert.Equal(productName, expectedProductName);
        //}


        //[Fact]
        //public void AddProduct_Product()
        //{
        //    //arrange
        //    var productList = GetProductsData();
        //    productService.Setup(x => x.AddProject(Guid.NewGuid(),new Projects()))
        //        .Returns(productList);
        //    var productController = new ProjectsController(productService.Object, User.Object, shiftServices.Object);

        //    //act
        //    var productResult = productController.AddProject(Guid.NewGuid(), new AddProjectRequest());

        //    //assert
        //    Assert.NotNull(productResult);
        //    Assert.Equal(productList[2].ProductId, productResult.ProductId);
        //    Assert.True(productList[2].ProductId == productResult.ProductId);
        //}


        private async Task<IEnumerable<Projects>> GetProductsData()
        {
            return await dbContext.Projects.ToListAsync();
        }
        public async Task<Projects> ProjectById(Guid projectId)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(x => x.ProjectId == projectId);

            if (project == null)
            {
                return null;
            }
            return project;
        }
        public async Task<Projects> AddProject(Guid tenantId, Projects project)
        {
            project.ProjectId = Guid.NewGuid();
            project.TenentId = tenantId;
            project.CreatedDate = DateTime.Now;
            project.LastModifiedDate = DateTime.Now;
            project.Status = "active";
            await dbContext.Projects.AddAsync(project);
            await dbContext.SaveChangesAsync();
            return project;

        }

    }
}