function saveCart(cartData) {
    localStorage.setItem('sockCart', cartData);
}

function loadCart() {
    return localStorage.getItem('sockCart');
}