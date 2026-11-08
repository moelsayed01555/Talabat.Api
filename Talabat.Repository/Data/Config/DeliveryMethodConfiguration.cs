using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    internal class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            // Configure the cost property to have a precision of 18 and scale of 2
            builder.Property(dm => dm.Cost)
                .HasColumnType("decimal(18,2)");


        }
    }
}
