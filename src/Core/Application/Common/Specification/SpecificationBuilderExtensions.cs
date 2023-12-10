using System.Linq.Expressions;

namespace Squares.Application.Common.Specification;

public static class SpecificationBuilderExtensions
{
    // TODO: Change logic, search also password fields?
    public static IOrderedSpecificationBuilder<T> SearchBy<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Filter? filter)
    {
        if (filter?.Filters?.Count() > 0)
        {
            var parameter = Expression.Parameter(typeof(T));
            Expression binaryExpresioFilter;

            if (!string.IsNullOrEmpty(filter.Logic))
            {
                if (filter.Filters is null)
                {
                    throw new CustomException("The Filters attribute is required when declaring a logic");
                }

                binaryExpresioFilter = CreateFilterExpression(filter.Logic, filter.Filters, parameter);
            }
            else
            {
                var filterValid = GetValidFilter(filter);
                binaryExpresioFilter = CreateFilterExpression(filterValid.Field!, filterValid.Operator!, filterValid.Value, parameter);
            }

            ((List<WhereExpressionInfo<T>>)specificationBuilder.Specification.WhereExpressions)
                .Add(new WhereExpressionInfo<T>(Expression.Lambda<Func<T, bool>>(binaryExpresioFilter, parameter)));
        }

        return new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);
    }

    private static Expression CreateFilterExpression(
        string logic,
        IEnumerable<Filter> filters,
        ParameterExpression parameter)
    {
        Expression filterExpression = default!;

        foreach (var filter in filters)
        {
            Expression bExpresionFilter;

            if (!string.IsNullOrEmpty(filter.Logic))
            {
                if (filter.Filters is null)
                {
                    throw new CustomException("The Filters attribute is required when declaring a logic");
                }

                bExpresionFilter = CreateFilterExpression(filter.Logic, filter.Filters, parameter);
            }
            else
            {
                var filterValid = GetValidFilter(filter);
                bExpresionFilter = CreateFilterExpression(filterValid.Field!, filterValid.Operator!, filterValid.Value, parameter);
            }

            filterExpression = filterExpression is null ? bExpresionFilter : CombineFilter(logic, filterExpression, bExpresionFilter);
        }

        return filterExpression;
    }

    private static Expression CreateFilterExpression(string field, string @operator, object? value, ParameterExpression parameter)
    {
        var expression = GetPropertyExpression(field, parameter);
        var expressionValue = Expression.Constant(GetValue(value, expression.Type), expression.Type);
        return CreateFilterExpression(expression, expressionValue, @operator);
    }

    private static Expression CreateFilterExpression(
        Expression memberExpression,
        Expression constantExpression,
        string filterOperator)
    {
        if (memberExpression.Type == typeof(string))
        {
            constantExpression = Expression.Call(constantExpression, "ToLower", null);
            memberExpression = Expression.Call(memberExpression, "ToLower", null);
        }

        return filterOperator switch
        {
            FilterOperator.EQ => Expression.Equal(memberExpression, constantExpression),
            FilterOperator.NEQ => Expression.NotEqual(memberExpression, constantExpression),
            FilterOperator.LT => Expression.LessThan(memberExpression, constantExpression),
            FilterOperator.LTE => Expression.LessThanOrEqual(memberExpression, constantExpression),
            FilterOperator.GT => Expression.GreaterThan(memberExpression, constantExpression),
            FilterOperator.GTE => Expression.GreaterThanOrEqual(memberExpression, constantExpression),
            FilterOperator.CONTAINS => Expression.Call(memberExpression, "Contains", null, constantExpression),
            FilterOperator.STARTSWITH => Expression.Call(memberExpression, "StartsWith", null, constantExpression),
            FilterOperator.ENDSWITH => Expression.Call(memberExpression, "EndsWith", null, constantExpression),
            _ => throw new CustomException("Filter Operator is not valid."),
        };
    }

    private static Expression CombineFilter(
        string filterOperator,
        Expression bExpresionBase,
        Expression bExpresion) => filterOperator switch
        {
            FilterLogic.AND => Expression.And(bExpresionBase, bExpresion),
            FilterLogic.OR => Expression.Or(bExpresionBase, bExpresion),
            FilterLogic.XOR => Expression.ExclusiveOr(bExpresionBase, bExpresion),
            _ => throw new ArgumentException("FilterLogic is not valid."),
        };

    private static MemberExpression GetPropertyExpression(
        string propertyName,
        ParameterExpression parameter)
    {
        Expression propertyExpression = parameter;
        foreach (string member in propertyName.Split('.'))
        {
            propertyExpression = Expression.PropertyOrField(propertyExpression, member);
        }

        return (MemberExpression)propertyExpression;
    }

    public static object? GetValue(object? value, Type type)
    {
        if (value != null)
        {
            string stringValue = value.ToString()!;

            if (type.IsEnum)
            {
                return ChangeType(Enum.Parse(type, stringValue, true), type);
            }

            if (type == typeof(Guid))
            {
                return ChangeType(Guid.Parse(stringValue), type);
            }

            if (type == typeof(string))
            {
                return stringValue;
            }

            if (type == typeof(DateOnly) || type == typeof(DateOnly?))
            {
                return ChangeType(DateOnly.Parse(stringValue), type);
            }

            return ChangeType(stringValue, type);
        }

        return null;
    }

    public static object? ChangeType(object value, Type conversion)
    {
        var type = conversion;

        if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        {
            if (value == null)
            {
                return null;
            }

            type = Nullable.GetUnderlyingType(type);
        }

        return Convert.ChangeType(value, type!);
    }

    private static Filter GetValidFilter(Filter filter)
    {
        return string.IsNullOrEmpty(filter.Field)
            ? throw new CustomException("The field attribute is required when declaring a filter")
            : string.IsNullOrEmpty(filter.Operator)
            ? throw new CustomException("The Operator attribute is required when declaring a filter")
            : filter;
    }

    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        List<string>? orderByFields,
        Dictionary<string, Expression<Func<T, object?>>>? expressions = null)
    {
        if (orderByFields is not null)
        {
            foreach (var field in ParseOrderBy(orderByFields))
            {
                if (field.Key.StartsWith("@"))
                {
                    if (expressions?.TryGetValue(field.Key, out var expression) == true)
                    {
                        ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions)
                            .Add(new OrderExpressionInfo<T>(expression, field.Value));
                    }
                }
                else
                {
                    var paramExpr = Expression.Parameter(typeof(T));

                    Expression propertyExpr = paramExpr;
                    foreach (string member in field.Key.Split('.'))
                    {
                        propertyExpr = Expression.PropertyOrField(propertyExpr, member);
                    }

                    var keySelector = Expression.Lambda<Func<T, object?>>(
                        Expression.Convert(propertyExpr, typeof(object)),
                        paramExpr);

                    ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions)
                        .Add(new OrderExpressionInfo<T>(keySelector, field.Value));
                }
            }
        }

        return new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);
    }

    private static Dictionary<string, OrderTypeEnum> ParseOrderBy(List<string> orderByFields) =>
        new(orderByFields.Select((orderByfield, index) =>
        {
            string[] fieldParts = orderByfield.Split(' ');
            string field = fieldParts[0];
            bool descending = fieldParts.Length > 1 && fieldParts[1].StartsWith("Desc", StringComparison.OrdinalIgnoreCase);
            var orderBy = index == 0 ? descending ?
                OrderTypeEnum.OrderByDescending : OrderTypeEnum.OrderBy : descending ?
                OrderTypeEnum.ThenByDescending : OrderTypeEnum.ThenBy;

            return new KeyValuePair<string, OrderTypeEnum>(field, orderBy);
        }));
}