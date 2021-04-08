using System.ComponentModel;
using Caliburn.Micro;

namespace TRMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<string> _products;
        private BindingList<string> _cart;
        private int _itemQuantity;

        public BindingList<string> Products
        {
            get => _products;
            set
            {
                _products = value;
                NotifyOfPropertyChange(nameof(Products));
            }
        }

        public BindingList<string> Cart
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
            }
        }

        public string SubTotal
        {
            get
            {
                // TODO: replace with calculation
                return $"0.00";
            }
        }

        public string Tax
        {
            get
            {
                // TODO: replace with calculation
                return $"0.00";
            }
        }

        public string Total
        {
            get
            {
                // TODO: replace with calculation
                return $"0.00";
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                // Make sure something is selected
                // Make sure there is an item quantity

                return output;
            }
        }

        public void AddToCart()
        {

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
    }
}