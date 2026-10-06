using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entites;

namespace Talabat.Repository.Data.Config
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .IsRequired();

            builder.Property(p => p.PictureUrl)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            // reltionships
            builder.HasOne(p => p.Brand)
                .WithMany(B => B.Products)
                .HasForeignKey(p => p.BrandId);

            builder.HasOne(p => p.Category)
                 .WithMany(C => C.Products)
                 .HasForeignKey(c => c.CategoryId);

        }
    }
}
