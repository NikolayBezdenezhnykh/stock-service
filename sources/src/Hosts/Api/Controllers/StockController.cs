using Api.Dtos;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/v{version:apiVersion}/stock/reserve")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly StockDbContext _dbContext;

        public StockController(StockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Reserve([FromBody] ProductsDto products)
        {
            var reserveProduct = new ReserveProduct()
            {
                DateReserve = DateTime.UtcNow,
                Status = (int)ReserveProductStatus.Approve
            };

            var productIds = products.Products.Select(p => p.ProductId).ToList();
            var availableProducts = await _dbContext.AvailableProducts.Where(pr => productIds.Contains(pr.ProductId)).ToListAsync();
            foreach (var product in products.Products)
            {
                var availableProduct = availableProducts.SingleOrDefault(it => it.ProductId == product.ProductId);
                if (availableProduct == null
                    || availableProduct.StockLevel < product.Quantity)
                {
                    return NotFound($"Продукта: '{product.ProductId}' нет в наличии.");
                }

                availableProduct.StockLevel -= product.Quantity;
                reserveProduct.Items.Add(new ReserveProductItem()
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                });
            }

            _dbContext.ReserveProducts.Add(reserveProduct);

            await _dbContext.SaveChangesAsync();

            return Ok(new { reserveProduct.Id });
        }

        [HttpPut("cancelled/{reserveId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Cancelled(int reserveId)
        {
            var reservedProduct = await _dbContext.ReserveProducts.Include(c => c.Items).SingleOrDefaultAsync(pr => pr.Id == reserveId);
            if (reservedProduct == null) return NotFound();

            var productIds = reservedProduct.Items.Select(p => p.ProductId).ToList();
            var availableProducts = await _dbContext.AvailableProducts.Where(pr => productIds.Contains(pr.ProductId)).ToListAsync();
            foreach (var product in availableProducts) 
            {
                var reservedProductItem = reservedProduct.Items.Single(p => p.ProductId == product.Id);
                product.StockLevel += reservedProductItem.Quantity;
            }

            reservedProduct.Status = (int)ReserveProductStatus.Cancelled;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}