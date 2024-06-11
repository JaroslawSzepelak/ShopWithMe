﻿using ShopWithMe.Models.Interfaces;
using ShopWithMe.Models.Products;

namespace ShopWithMe.Models.Cart
{
    public class CartLine : IEntity
    {
        public long Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}