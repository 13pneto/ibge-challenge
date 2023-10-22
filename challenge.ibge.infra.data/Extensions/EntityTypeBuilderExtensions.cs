using challenge.ibge.infra.data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace challenge.ibge.infra.data.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static void ApplyDefaultEntityContextConfig<T>(this EntityTypeBuilder<T> entity) where T : BaseEntity
    {
        var entityProperties = entity.Metadata.ClrType.GetProperties();
        foreach (var entityPropertyInfo in entityProperties)
        {
            entity.Property(entityPropertyInfo.Name)
                .IsRequired();
        }
        
        entity.Property(x => x.UpdatedAt)
            .IsRequired(false);
    }
}