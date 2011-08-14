using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(Product product, int quantity){
            //find passed product in colltion and return line
            CartLine line = lineCollection.Where(l => l.Product.ProductID == product.ProductID).FirstOrDefault();

            //if find update quantity by adding passed quantity
            if (line == null)
            {
                lineCollection.Add(new CartLine { Quantity = quantity, Product = product});
            } 
            else {
                line.Quantity += quantity;
            }         

        }
        //clear cart
        public void Clear()
        {
            lineCollection.Clear();
        }

        public void RemoveLine(Product product) {
            lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);
        }

        public decimal ComputeTotalValue(){
            return lineCollection.Sum(l => l.Product.Price * l.Quantity);
        }

        public IEnumerable<CartLine> Lines {
            get { return lineCollection; }
        }
        //add item
        //total value
        //remove line
        //lines
    }

    public class CartLine {
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}
