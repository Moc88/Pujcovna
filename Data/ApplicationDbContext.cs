using Eshop.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pujcovna.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace pujcovna.Data
{
    public class ApplicationDbContext : IdentityDbContext //možná by tu mělo být ještě <ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>().Property(x => x.PriceWithDph).HasColumnType("decimal(10,1)");
            builder.Entity<Product>().Property(x => x.PriceWithoutDph).HasColumnType("decimal(10,1)");

            // určení, které sloupce budou v tabulce CategoryProduct sloužit jako klíče
            builder.Entity<CategoryProduct>().HasKey(cp => new
            {
                cp.CategoryId,
                cp.ProductId
            }
                                                    );

            // nastavení One-To-Many vazby pro obě entity
            builder.Entity<CategoryProduct>()
                   .HasOne(cp => cp.Category)
                   .WithMany(c => c.CategoryProducts)
                   .HasForeignKey(cp => cp.CategoryId);

            builder.Entity<CategoryProduct>()
                   .HasOne(cp => cp.Product)
                   .WithMany(p => p.CategoryProducts)
                   .HasForeignKey(cp => cp.ProductId);

            //testovací data pro kategorie - po nastavení relací mezi kategoriemi
            builder.Entity<Category>().HasData
                (
                //kategorie bez podkategorií
                new Category() { CategoryId = 1, Title = "Vrtací a bourací kladiva", Url = "vrtaci-a-bouraci-kladiva", OrderNo = 1, Hidden = false },
                //tahle má pár podkategorií - viz kat. Id 4-7
                new Category() { CategoryId = 2, Title = "Řezání a ohýbání", Url = "rezani-a-ohybani", OrderNo = 2, Hidden = false },
                //další rodičovská - pokdat. id 8-9, Order No má až 7!
                new Category() { CategoryId = 3, Title = "Lešení a bednění", Url = "leseni-a-bedneni", OrderNo = 7, Hidden = false },

                //podkategorie k řezání
                new Category() { CategoryId = 4, ParentCategoryId = 2, Title = "Pily", Url = "pily", OrderNo = 3, Hidden = false },
                new Category() { CategoryId = 5, ParentCategoryId = 2, Title = "Profilovačky, falcovačky", Url = "profilovacky-falcovacky", OrderNo = 4, Hidden = false },
                new Category() { CategoryId = 6, ParentCategoryId = 2, Title = "Řezačky", Url = "rezacky", OrderNo = 5, Hidden = false },
                new Category() { CategoryId = 7, ParentCategoryId = 2, Title = "Úhlové brusky", Url = "uhlove-brusky", OrderNo = 6, Hidden = false },
                //podkategorie k lešení-order No navazuje na orderNo rodičovské kat., parentCatId je 3
                new Category() { CategoryId = 8, ParentCategoryId = 3, Title = "Bednění a stavební podpěry", Url = "bedneni-a-stavebni-podpery", OrderNo = 8, Hidden = false },
                new Category() { CategoryId = 9, ParentCategoryId = 3, Title = "Lešení a lešenové věže", Url = "leseni-a-lesenove-veze", OrderNo = 9, Hidden = false }


                );
        }
        
    }
}
