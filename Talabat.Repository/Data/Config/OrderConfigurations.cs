using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Configure the relationship between Order and Address as owned entity
            // (هنا الادرس هيكون ضمن جدول الاوردر لزلك عملنا OwnsOne)

            builder.OwnsOne(o => o.ShippingAddress , a =>       // [1:1]
            {
                a.WithOwner();
            });


            // Configure the relationship between Order and Status as owned entity
            builder.Property(o => o.Status)
                .HasConversion(
                    s => s.ToString(),                                        // تتخزن في لبيانات ك string
                    s => (OrderStatus)Enum.Parse(typeof(OrderStatus), s));    // Convert string back to enum


            // Configure the relationship between Order and DeliveryMethod [1:1]
            builder.HasOne(o => o.DeliveryMethod)
                   .WithMany()
                   .HasForeignKey(o => o.DeliveryMethodId);

            // Configure the subtotal property to have a precision of 18 and scale of 2
            builder.Property(o => o.Subtotal)
                .HasColumnType("decimal(18,2)");



        }
    }
}
