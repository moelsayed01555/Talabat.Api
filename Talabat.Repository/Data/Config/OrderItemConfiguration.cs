using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            // Configure the relationship between OrderItem and ProductItemOrdered as owned entity
            builder.OwnsOne(oi => oi.Product, p =>
            {
                p.WithOwner();
            });

            // Configure the price property to have a precision of 18 and scale of 2
            builder.Property(oi => oi.Price)
                .HasColumnType("decimal(18,2)");

        }
    }
}
