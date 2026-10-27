using System;
using System.Collections.Generic;
using System.Text;

namespace Talabat.Core.Entites
{
    // this basket will deal with redis db

    public class CustomerBasket
    {
        public string Id { get; set; }   // string because frontend will send it from grid

        public List<BasketItem> Items { get; set; }
    }
}
