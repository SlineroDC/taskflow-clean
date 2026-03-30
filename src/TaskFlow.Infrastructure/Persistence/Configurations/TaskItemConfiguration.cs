using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title).IsRequired().HasMaxLength(200);

        // Filtro global para el Soft Delete
        builder.HasQueryFilter(t => !t.IsDeleted);

        //Índice único compuesto para evitar órdenes duplicados por proyecto
        builder.HasIndex(t => new { t.ProjectId, t.Order });
    }
}
