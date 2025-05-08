using SockStore.Models;
using System.Net.Http.Json;

namespace SockStore.Services;

public interface IProductService {
    Task<List<Sock>> GetProductsAsync();
    Task<Sock> GetProductByIdAsync(int id);
}

public class ProductService(HttpClient httpClient) : IProductService {
    private List<Sock>? _socks;

    public async Task<List<Sock>> GetProductsAsync() {
        if (_socks == null) {
            // Haal de JSON data op en deserialiseer deze naar een List<Sock>
            _socks = await httpClient.GetFromJsonAsync<List<Sock>>("socks.json");
        }
        return _socks ?? [];
    }
    
    public Task<Sock> GetProductByIdAsync(int id) {
        var product = _socks.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(product);
    }
}
