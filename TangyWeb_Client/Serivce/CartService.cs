﻿using Blazored.LocalStorage;
using Tangy_Common;
using TangyWeb_Client.Serivce.IService;
using TangyWeb_Client.ViewModels;

namespace TangyWeb_Client.Serivce
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        public event Action OnChange;

        public CartService(ILocalStorageService localStorageService)
        {
            _localStorage = localStorageService;
        }

        public async Task IncrementCart(ShoppingCart shoppingCart)
        {
            var cart = await _localStorage.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);
            bool itemInCart = false;

            if (cart == null)
            {
                cart = new List<ShoppingCart>();
            }

            foreach (var obj in cart)
            {
                if (obj.ProductId == shoppingCart.ProductId && obj.ProductPriceId == shoppingCart.ProductPriceId)
                {
                    itemInCart = true;
                    obj.Count += shoppingCart.Count;
                }
            }

            if (!itemInCart)
            {
                cart.Add(new ShoppingCart()
                {
                    ProductId = shoppingCart.ProductId,
                    ProductPriceId = shoppingCart.ProductPriceId,
                    Count = shoppingCart.Count
                });
            }

            await _localStorage.SetItemAsync(SD.ShoppingCart, cart);
            OnChange.Invoke();
        }

        public async Task DecrementCart(ShoppingCart shoppingCart)
        {
            var cart = await _localStorage.GetItemAsync<List<ShoppingCart>>(SD.ShoppingCart);

            //if count is 0 or 1 then we remove the item
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].ProductId == shoppingCart.ProductId && cart[i].ProductPriceId == shoppingCart.ProductPriceId)
                {
                    if (cart[i].Count == 1 || shoppingCart.Count == 0)
                    {
                        cart.Remove(cart[i]);
                    }
                    else
                    {
                        cart[i].Count -= shoppingCart.Count;
                    }
                }
            }

            await _localStorage.SetItemAsync(SD.ShoppingCart, cart);
            OnChange.Invoke();
        }
    }
}
