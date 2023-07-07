

using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using API.Helpers;

namespace API.Controllers
{
    
    public class ProductsController : BaseApiController
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
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);
            var totalItems = await ProductsRepo.CountAsync(countSpec);

            var products = await ProductsRepo.ListAsync(spec);

            var data = Mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products); 
            
            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        
        }
        

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product =  await ProductsRepo.GetEntityWithSpec(spec);

            if(product == null) return NotFound(new ApiResponse(404));

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