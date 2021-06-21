using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Helpers
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id= "d245debe-022b-46a5-88e3-bb0b9252d026",
                    ConcurrencyStamp= "5770a7e8-f2f9-4e3e-b3b1-ac24e58b1a0c",
                    Name ="User",
                    NormalizedName="USER"
                },
                new IdentityRole
                {
                    Id = "f9e479f8-1549-4119-8b3b-4f5c07d0705a",
                    ConcurrencyStamp = "36cebdec-0e65-44c7-890f-5d43cc8a680e",
                    Name ="Administrator",
                    NormalizedName="ADMINISTRATOR"
                }
                );
        }
    }
}
