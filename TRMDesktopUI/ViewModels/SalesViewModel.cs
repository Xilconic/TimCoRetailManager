using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using Caliburn.Micro;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Helpers;
using TRMDesktopUI.Library.Models;
using TRMDesktopUI.Models;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private readonly IProductEndpoint _productEndpoint;
        private readonly IConfigHelper _configHelper;
        private readonly ISaleEndpoint _saleEndpoint;
        private readonly IMapper _mapper;
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _window;

        private BindingList<ProductDisplayModel> _products;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        private int _itemQuantity = 1;
        private ProductDisplayModel _selectedProduct;
        private CartItemDisplayModel _selectedCartItem;

        public SalesViewModel(
            IProductEndpoint productEndpoint,
            IConfigHelper configHelper,
            ISaleEndpoint saleEndpoint,
            IMapper mapper,
            StatusInfoViewModel status,
            IWindowManager window)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
            _status = status;
            _window = window;
        }

        protected override async void OnViewLoaded(object view)
        {
            // TODO: Concerned about async void, which is typically discouraged as it cannot ever be awaited for. Going along with course...
            base.OnViewLoaded(view);
            try
            {
                await ResetSalesViewModelAsync();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject(); // TODO: _window.ShowDialog prototype indicated dictionary to be used; Going along with course...
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if (ex.Message == "Unauthorized") // TODO: Not a fan of the straight message compare, as it's likely going to break for different culture; Going along with course...
                {
                    _status.UpdateMessage("Unauthorized Access", "You do not have permission to interact with the Sales form.");
                    _window.ShowDialog(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    _window.ShowDialog(_status, null, settings);
                }

                TryClose();
            }
        }

        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAllAsync();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }

        public BindingList<ProductDisplayModel> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(nameof(Products));
            }
        }

        public ProductDisplayModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(nameof(SelectedProduct));
                NotifyOfPropertyChange(nameof(CanAddToCart));
            }
        }

        private async Task ResetSalesViewModelAsync()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            await LoadProducts();

            // TODO: The below is copied from AddToCart; Going along with course
            NotifyOfPropertyChange(nameof(SubTotal));
            NotifyOfPropertyChange(nameof(Tax));
            NotifyOfPropertyChange(nameof(Total));
            NotifyOfPropertyChange(nameof(CanCheckOut));
        }

        public CartItemDisplayModel SelectedCartItem
        {
            get => _selectedCartItem;
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(nameof(SelectedCartItem));
                NotifyOfPropertyChange(nameof(CanRemoveFromCart));
            }
        }

        public BindingList<CartItemDisplayModel> Cart
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
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
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
            NotifyOfPropertyChange(nameof(CanCheckOut));
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                // Make sure something is selected
                output = SelectedCartItem != null &&
                         SelectedCartItem?.QuantityInCart > 0; // TODO: Bug: AddToCart method changes QuantityInCart property, therefore should fire NotifyPropertyChange; Going along with course...

                return output;
            }
        }

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += 1;
            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }

            NotifyOfPropertyChange(nameof(CanAddToCart));
            // TODO: DRY: repeated in AddToCart; But going along with course...
            NotifyOfPropertyChange(nameof(SubTotal));
            NotifyOfPropertyChange(nameof(Tax));
            NotifyOfPropertyChange(nameof(Total));
            NotifyOfPropertyChange(nameof(CanCheckOut));
        }

        public bool CanCheckOut
        {
            get
            {
                bool output = false;

                // Make sure something is the cart
                if (Cart.Count > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public async Task CheckOut()
        {
            // Create a SaleModel and post to the API
            var sale = new SaleModel();
            foreach (var item in Cart)
            {
                sale.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart,
                });
            }

            await _saleEndpoint.PostSaleAsync(sale);

            await ResetSalesViewModelAsync();
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            foreach (var CartItemDisplayModel in Cart)
            {
                subTotal += CartItemDisplayModel.Product.RetailPrice * CartItemDisplayModel.QuantityInCart;
            }

            return subTotal;
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate() / 100; // TODO: The division by 100 should happen in ConfigHelper.GetTaxRate, but going along with course...
            // TODO: DRY, as this copies code from CalculateSubTotal and then modifies that result. But going along with course...
            taxAmount = Cart
                .Where(item => item.Product.IsTaxable)
                .Sum(item => item.Product.RetailPrice * item.QuantityInCart * taxRate);

            return taxAmount;
        }
    }
}