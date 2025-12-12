using System.Linq.Expressions;
using System.Text;
using Domain.Enums;

namespace Infrastructure.Data.Configurations;

/// <summary>
/// Extension methods for Entity Framework Core entity configurations to reduce duplication
/// and enforce consistent configuration patterns across all entities.
/// </summary>
public static class EntityConfigurationExtensions
{
    /// <summary>
    /// Configures an enum property with string conversion and snake_case column naming.
    /// </summary>
    public static PropertyBuilder<TEnum?> ConfigureEnum<TEnum>(
        this PropertyBuilder<TEnum?> builder,
        string columnName,
        bool isRequired = false)
        where TEnum : struct, Enum
    {
        var propertyBuilder = builder
            .HasConversion(
                enumValue => enumValue.HasValue ? enumValue.Value.ToString() : null,
                stringValue => !string.IsNullOrEmpty(stringValue) 
                    ? (TEnum?)Enum.Parse(typeof(TEnum), stringValue) 
                    : null)
            .HasColumnName(ToSnakeCase(columnName));

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a non-nullable enum property with string conversion and snake_case column naming.
    /// </summary>
    public static PropertyBuilder<TEnum> ConfigureEnum<TEnum>(
        this PropertyBuilder<TEnum> builder,
        string columnName,
        bool isRequired = true)
        where TEnum : struct, Enum
    {
        var propertyBuilder = builder
            .HasConversion(
                enumValue => enumValue.ToString(),
                stringValue => (TEnum)Enum.Parse(typeof(TEnum), stringValue))
            .HasColumnName(ToSnakeCase(columnName));

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a string property with max length, required/optional clarity, and snake_case column naming.
    /// </summary>
    public static PropertyBuilder<string?> ConfigureString(
        this PropertyBuilder<string?> builder,
        string columnName,
        int maxLength,
        bool isRequired = false)
    {
        var propertyBuilder = builder
            .HasMaxLength(maxLength)
            .HasColumnName(ToSnakeCase(columnName));

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a decimal property with precision, scale, and snake_case column naming.
    /// </summary>
    public static PropertyBuilder<decimal?> ConfigureDecimal(
        this PropertyBuilder<decimal?> builder,
        string columnName,
        int precision = 18,
        int scale = 2,
        bool isRequired = false)
    {
        var propertyBuilder = builder
            .HasColumnType($"decimal({precision},{scale})")
            .HasColumnName(ToSnakeCase(columnName));

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a non-nullable decimal property with precision, scale, and snake_case column naming.
    /// </summary>
    public static PropertyBuilder<decimal> ConfigureDecimal(
        this PropertyBuilder<decimal> builder,
        string columnName,
        int precision = 18,
        int scale = 2,
        bool isRequired = true)
    {
        var propertyBuilder = builder
            .HasColumnType($"decimal({precision},{scale})")
            .HasColumnName(ToSnakeCase(columnName));

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a timestamp property (DateTimeOffset) with default value and snake_case column naming.
    /// </summary>
    public static PropertyBuilder<DateTimeOffset?> ConfigureTimestamp(
        this PropertyBuilder<DateTimeOffset?> builder,
        string columnName,
        bool hasDefaultValue = true,
        bool isRequired = false)
    {
        var propertyBuilder = builder
            .HasColumnName(ToSnakeCase(columnName));

        if (hasDefaultValue)
        {
            propertyBuilder.HasDefaultValueSql("SYSDATETIMEOFFSET()");
        }

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a non-nullable timestamp property (DateTimeOffset) with default value and snake_case column naming.
    /// </summary>
    public static PropertyBuilder<DateTimeOffset> ConfigureTimestamp(
        this PropertyBuilder<DateTimeOffset> builder,
        string columnName,
        bool hasDefaultValue = true,
        bool isRequired = true)
    {
        var propertyBuilder = builder
            .HasColumnName(ToSnakeCase(columnName));

        if (hasDefaultValue)
        {
            propertyBuilder.HasDefaultValueSql("SYSDATETIMEOFFSET()");
        }

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a boolean property with default value and snake_case column naming.
    /// </summary>
    public static PropertyBuilder<bool> ConfigureBoolean(
        this PropertyBuilder<bool> builder,
        string columnName,
        bool defaultValue = false,
        bool isRequired = true)
    {
        var propertyBuilder = builder
            .HasDefaultValue(defaultValue)
            .HasColumnName(ToSnakeCase(columnName));

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a nullable boolean property with default value and snake_case column naming.
    /// </summary>
    public static PropertyBuilder<bool?> ConfigureBoolean(
        this PropertyBuilder<bool?> builder,
        string columnName,
        bool? defaultValue = null,
        bool isRequired = false)
    {
        var propertyBuilder = builder
            .HasColumnName(ToSnakeCase(columnName));

        if (defaultValue.HasValue)
        {
            propertyBuilder.HasDefaultValue(defaultValue.Value);
        }

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a GUID property with snake_case column naming.
    /// </summary>
    public static PropertyBuilder<Guid> ConfigureGuid(
        this PropertyBuilder<Guid> builder,
        string columnName,
        bool isRequired = true)
    {
        var propertyBuilder = builder
            .HasColumnName(ToSnakeCase(columnName));

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a nullable GUID property with snake_case column naming.
    /// </summary>
    public static PropertyBuilder<Guid?> ConfigureGuid(
        this PropertyBuilder<Guid?> builder,
        string columnName,
        bool isRequired = false)
    {
        var propertyBuilder = builder
            .HasColumnName(ToSnakeCase(columnName));

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures an integer property with snake_case column naming.
    /// </summary>
    public static PropertyBuilder<int> ConfigureInteger(
        this PropertyBuilder<int> builder,
        string columnName,
        bool isRequired = true)
    {
        var propertyBuilder = builder
            .HasColumnName(ToSnakeCase(columnName));

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a nullable integer property with snake_case column naming.
    /// </summary>
    public static PropertyBuilder<int?> ConfigureInteger(
        this PropertyBuilder<int?> builder,
        string columnName,
        bool isRequired = false)
    {
        var propertyBuilder = builder
            .HasColumnName(ToSnakeCase(columnName));

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }


    /// <summary>
    /// Converts PascalCase or camelCase to snake_case.
    /// </summary>
    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new StringBuilder();
        result.Append(char.ToLowerInvariant(input[0]));

        for (int i = 1; i < input.Length; i++)
        {
            if (char.IsUpper(input[i]))
            {
                result.Append('_');
                result.Append(char.ToLowerInvariant(input[i]));
            }
            else
            {
                result.Append(input[i]);
            }
        }

        return result.ToString();
    }
}

