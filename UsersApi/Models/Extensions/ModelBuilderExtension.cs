using Microsoft.EntityFrameworkCore;

namespace UsersApi.Models.Extensions;

public static class ModelBuilderExtension
{
    public static void AddPostgreSqlRules(this ModelBuilder modelBuilder)
    {
        // Prefix column names with table name
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (entity.BaseType == null)
            {
                entity.SetTableName(entity.GetTableName().ToSnakeCase());
            }

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.Name.ToSnakeCase());
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(key.GetName().ToSnakeCase());
            }

            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName().ToSnakeCase());
            }
        }
    }
}