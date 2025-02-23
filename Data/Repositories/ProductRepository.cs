using Data.Contexts;
using Data.Entities;

namespace Data.Repositories;

public class ProductRepository(DataContext context)
{
    private readonly DataContext _context = context;

    // CREATE
    public async Task<ProductEntity> AddProductAsync(ProductEntity product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    // READ
    public async Task<ProductEntity?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    // UPDATE
    public async Task<ProductEntity?> UpdateProductAsync(int id, ProductEntity updatedProduct)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return null;

        product.ProductName = updatedProduct.ProductName;

        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    // DELETE
    public async Task<bool> DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) 
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}