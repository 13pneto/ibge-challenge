using challenge.ibge.core.Entities;
using challenge.ibge.infra.data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace challenge.ibge.infra.data.Mappings;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("tb_user");
        builder.ApplyDefaultEntityContextConfig();
    }
}