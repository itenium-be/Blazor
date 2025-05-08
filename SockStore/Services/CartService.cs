using Microsoft.JSInterop;
using SockStore.Models;
using System.Text.Json;

namespace SockStore.Services;

public interface ICartService {
    event Action? OnCartChanged;
    void AddSock(Sock product);
    List<Sock> GetCartSocks();
    int GetSockCount();
    decimal GetTotalPrice();
    void RemoveSock(int sockId);
    void ClearCart();
    Task InitializeAsync();
}

public class CartSock(Sock Sock, int Quantity) {
    public Sock Sock { get; } = Sock;
    public int Quantity { get; set; } = Quantity;
}

public class CartService(IJSRuntime jsRuntime) : ICartService {
    private readonly IJSRuntime? _jsRuntime = jsRuntime;
    private List<CartSock> _socks = [];
    private const string CartStorageKey = "sockCart"; // Voor Oefening 6

    public event Action? OnCartChanged;

    public async Task InitializeAsync() {
        if (_jsRuntime != null) {
            var jsonCartData = await _jsRuntime.InvokeAsync<string?>("loadCart");
            if (!string.IsNullOrEmpty(jsonCartData)) {
                try {
                    var loadedItems = JsonSerializer.Deserialize<List<CartSock>>(jsonCartData);
                    if (loadedItems != null) {
                        _socks = loadedItems;
                        NotifyStateChanged();
                    }
                }
                catch (JsonException ex) {
                    Console.WriteLine($"Error deserializing cart data: {ex.Message}");
                    await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", CartStorageKey);
                }
            }
        }
    }

    public void AddSock(Sock product) {
        var existingSock = _socks.FirstOrDefault(i => i.Sock.Id == product.Id);
        if (existingSock == null) // Voorkom duplicaten, of beheer aantal
        {
            _socks.Add(new CartSock(product, 1));
        }
        else {
            existingSock.Quantity++;
        }
        NotifyStateChanged(); 
        SaveChangesAndNotify();
    }

    public List<Sock> GetCartSocks() {
        return _socks.Select(s => s.Sock).ToList();
    }

    public int GetSockCount() {
        return _socks.Select(s => s.Quantity).Sum(); 
    }

    public decimal GetTotalPrice() {
        return _socks.Select(s => s.Sock.Price * s.Quantity).Sum();
    }
 
    public void RemoveSock(int productId) {
        var itemToRemove = _socks.FirstOrDefault(i => i.Sock.Id == productId);
        if (itemToRemove == null) return;
        _socks.Remove(itemToRemove);
        NotifyStateChanged();
        SaveChangesAndNotify();
    }

    public void ClearCart() {
        _socks.Clear();
        SaveChangesAndNotify();
    }

    private void NotifyStateChanged() => OnCartChanged?.Invoke();
    
    private async void SaveChangesAndNotify() {
        try {
            if (_jsRuntime != null) {
                string jsonCartData = JsonSerializer.Serialize(_socks);
                await _jsRuntime.InvokeVoidAsync("saveCart", jsonCartData);
            }
            NotifyStateChanged();
        }
        catch (Exception e) {
            throw;
        }
    }
}
