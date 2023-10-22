using challenge.ibge.infra.data.Entities;
using challenge.ibge.infra.data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace challenge.ibge.infra.data.Mappings;

public class LocalityMap : IEntityTypeConfiguration<Locality>
{
    public void Configure(EntityTypeBuilder<Locality> builder)
    {
        builder.ToTable("tb_locality");
        builder.ApplyDefaultEntityContextConfig();
        
        builder
            .HasIndex(x => x.IbgeCode)
            .IsUnique();
    }
}