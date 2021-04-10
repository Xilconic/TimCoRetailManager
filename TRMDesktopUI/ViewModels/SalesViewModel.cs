using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductEndpoint _productEndpoint;
        private readonly IConfigHelper _configHelper;
        private BindingList<ProductModel> _products;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private int _itemQuantity = 1;
        private ProductModel _selectedProduct;

        public SalesViewModel(
            IProductEndpoint productEndpoint,
            IConfigHelper configHelper)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
        }

        protected override async void OnViewLoaded(object view)
        {
            // TODO: Concerned about async void, which is typically discouraged as it cannot ever be awaited for. Going along with course...
            base.OnViewLoaded(view);
            await LoadProducts();
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAllAsync();
            Products = new BindingList<ProductModel>(productList);
        }

        public BindingList<ProductModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(nameof(Products));
            }
        }

        public ProductModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(nameof(SelectedProduct));
                NotifyOfPropertyChange(nameof(CanAddToCart));
            }
        }

        public BindingList<CartItemModel> Cart
        {
            get => _cart;
            set
            {
                _cart = value;
                NotifyOfPropertyChange(nameof(Cart));
            }
        }

        public int ItemQuantity
        {
            get => _itemQuantity;
            set
            {
                _itemQuantity = value;
                NotifyOfPropertyChange(nameof(ItemQuantity));
                NotifyOfPropertyChange(nameof(CanAddToCart));
            }
        }

        public string SubTotal => CalculateSubTotal().ToString("C");

        public string Tax => CalculateTax().ToString("C");

        public string Total
        {
            get
            {
                decimal total = CalculateSubTotal() + CalculateTax();
                return total.ToString("C");
            }
        }

        public bool CanAddToCart
        {
            get
            {
                // Make sure something is selected
                // Make sure there is an item quantity
                return ItemQuantity > 0 &&
                    SelectedProduct?.QuantityInStock >= ItemQuantity;
            }
        }

        public void AddToCart()
        {
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
                // HACK - There should be a better way of refreshing the cart display (Bas: which is making CartItemModel a ViewModel with notify property changes)
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(nameof(SubTotal));
            NotifyOfPropertyChange(nameof(Tax));
            NotifyOfPropertyChange(nameof(Total));
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                // Make sure something is selected

                return output;
            }
        }

        public void RemoveFromCart()
        {
            // TODO: DRY: repeated in AddToCart; But going along with course...
            NotifyOfPropertyChange(nameof(SubTotal));
            NotifyOfPropertyChange(nameof(Tax));
            NotifyOfPropertyChange(nameof(Total));
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                // Make sure something is the cart

                return output;
            }
        }

        public void CheckOut()
        {

        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            foreach (var cartItemModel in Cart)
            {
                subTotal += cartItemModel.Product.RetailPrice * cartItemModel.QuantityInCart;
            }

            return subTotal;
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate() / 100; // TODO: The devision by 100 should happen in ConfigHelper.GetTaxRate, but going along with course...
            // TODO: DRY, as this copies code from CalculateSubTotal and then modifies that result. But going along with course...
            taxAmount = Cart
                .Where(item => item.Product.IsTaxable)
                .Sum(item => item.Product.RetailPrice * item.QuantityInCart * taxRate);

            return taxAmount;
        }
    }
}