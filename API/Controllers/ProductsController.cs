

using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        public IGenericRepository<ProductType> ProductTypesRepo { get; }
        public IGenericRepository<ProductBrand> ProductBrandsRepo { get; }
        public IGenericRepository<Product> ProductsRepo { get; }
        public IMapper Mapper { get; }
        
        public ProductsController(IGenericRepository<Product> productsRepo, IGenericRepository<ProductBrand> productBrandsRepo,
            IGenericRepository<ProductType> productTypesRepo, IMapper mapper) 
        {
            this.Mapper = mapper;
            this.ProductsRepo = productsRepo;
            this.ProductBrandsRepo = productBrandsRepo;
            this.ProductTypesRepo = productTypesRepo;
                       
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();

            var products = await ProductsRepo.ListAsync(spec);
            
            return Ok(Mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        
        }
        

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product =  await ProductsRepo.GetEntityWithSpec(spec);

            return Mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<Product>>> GetBrands()
        {
            var brands = await ProductBrandsRepo.ListAllAsync();
            return Ok(brands);
        }

         [HttpGet("types")]
        public async Task<ActionResult<List<Product>>> GetTypes()
        {
            var types = await ProductTypesRepo.ListAllAsync();            
            return Ok(types);
        }
    }
}