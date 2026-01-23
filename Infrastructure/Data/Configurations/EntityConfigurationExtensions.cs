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
    /// Configures an enum property with string conversion.
    /// </summary>
    public static PropertyBuilder<TEnum?> ConfigureEnum<TEnum>(
        this PropertyBuilder<TEnum?> builder,
        bool isRequired = false)
        where TEnum : struct, Enum
    {
        var propertyBuilder = builder
            .HasConversion(
                enumValue => enumValue.HasValue ? enumValue.Value.ToString() : null,
                stringValue => !string.IsNullOrEmpty(stringValue) 
                    ? (TEnum?)Enum.Parse(typeof(TEnum), stringValue) 
                    : null);

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a non-nullable enum property with string conversion.
    /// </summary>
    public static PropertyBuilder<TEnum> ConfigureEnum<TEnum>(
        this PropertyBuilder<TEnum> builder,
        bool isRequired = true)
        where TEnum : struct, Enum
    {
        var propertyBuilder = builder
            .HasConversion(
                enumValue => enumValue.ToString(),
                stringValue => (TEnum)Enum.Parse(typeof(TEnum), stringValue));

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a string property with max length and required/optional clarity.
    /// </summary>
    public static PropertyBuilder<string?> ConfigureString(
        this PropertyBuilder<string?> builder,
        int maxLength,
        bool isRequired = false)
    {
        var propertyBuilder = builder
            .HasMaxLength(maxLength);

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a decimal property with precision and scale.
    /// </summary>
    public static PropertyBuilder<decimal?> ConfigureDecimal(
        this PropertyBuilder<decimal?> builder,
        int precision = 18,
        int scale = 2,
        bool isRequired = false)
    {
        var propertyBuilder = builder
            .HasColumnType($"decimal({precision},{scale})");

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a non-nullable decimal property with precision and scale.
    /// </summary>
    public static PropertyBuilder<decimal> ConfigureDecimal(
        this PropertyBuilder<decimal> builder,
        int precision = 18,
        int scale = 2,
        bool isRequired = true)
    {
        var propertyBuilder = builder
            .HasColumnType($"decimal({precision},{scale})");

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a timestamp property (DateTimeOffset) with default value.
    /// </summary>
    public static PropertyBuilder<DateTimeOffset?> ConfigureTimestamp(
        this PropertyBuilder<DateTimeOffset?> builder,
        bool hasDefaultValue = true,
        bool isRequired = false)
    {
        var propertyBuilder = builder;

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
    /// Configures a non-nullable timestamp property (DateTimeOffset) with default value.
    /// </summary>
    public static PropertyBuilder<DateTimeOffset> ConfigureTimestamp(
        this PropertyBuilder<DateTimeOffset> builder,
        bool hasDefaultValue = true,
        bool isRequired = true)
    {
        var propertyBuilder = builder;

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
    /// Configures a boolean property with default value.
    /// </summary>
    public static PropertyBuilder<bool> ConfigureBoolean(
        this PropertyBuilder<bool> builder,
        bool defaultValue = false,
        bool isRequired = true)
    {
        var propertyBuilder = builder
            .HasDefaultValue(defaultValue);

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a nullable boolean property with default value.
    /// </summary>
    public static PropertyBuilder<bool?> ConfigureBoolean(
        this PropertyBuilder<bool?> builder,
        bool? defaultValue = null,
        bool isRequired = false)
    {
        var propertyBuilder = builder;

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
    /// Configures a GUID property.
    /// </summary>
    public static PropertyBuilder<Guid> ConfigureGuid(
        this PropertyBuilder<Guid> builder,
        bool isRequired = true)
    {
        var propertyBuilder = builder;

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a nullable GUID property.
    /// </summary>
    public static PropertyBuilder<Guid?> ConfigureGuid(
        this PropertyBuilder<Guid?> builder,
        bool isRequired = false)
    {
        var propertyBuilder = builder;

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures an integer property.
    /// </summary>
    public static PropertyBuilder<int> ConfigureInteger(
        this PropertyBuilder<int> builder,
        bool isRequired = true)
    {
        var propertyBuilder = builder;

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }

    /// <summary>
    /// Configures a nullable integer property.
    /// </summary>
    public static PropertyBuilder<int?> ConfigureInteger(
        this PropertyBuilder<int?> builder,
        bool isRequired = false)
    {
        var propertyBuilder = builder;

        if (isRequired)
        {
            propertyBuilder.IsRequired();
        }

        return propertyBuilder;
    }
}

