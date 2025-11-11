using System.Linq.Expressions;
using LinqKit;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;

namespace SpotRent.Services.Spaces;

public static class AttributeHelper
{
    public static Func<IQueryable<Space>, IOrderedQueryable<Space>> BuildOrderByDelegate(string sort)
    {
        if (string.IsNullOrEmpty(sort))
        {
            return q => q.OrderBy(s => s.Id);
        }

        Func<IQueryable<Space>, IOrderedQueryable<Space>> orderBy = sort switch
        {
            "price-asc" => q => q.OrderBy(s => s.HourlyRate),
            "price-desc" => q => q.OrderByDescending(s => s.HourlyRate),
            "newest" => q => q.OrderByDescending(s => s.CreatedAt),
            _ => q =>
            {
                if (sort == "price_asc")
                {
                    return q.OrderBy(s => s.HourlyRate);
                }

                if (sort == "price_desc")
                {
                    return q.OrderByDescending(s => s.HourlyRate);
                }

                return q.OrderBy(s => s.Id);
            }
        };

        return orderBy;
    }

    public static ExpressionStarter<Space> BuildPredicate(
        string filter, SpaceType spaceType, decimal? minPrice = null, decimal? maxPrice = null)
    {
        var predicate = PredicateBuilder.New<Space>(true);
        predicate = AddCategories();
        predicate = AddPriceRange();

        if (string.IsNullOrEmpty(filter))
        {
            return predicate;
        }

        var decodedFilter = System.Net.WebUtility.UrlDecode(filter)?.Trim();
        if (string.IsNullOrEmpty(decodedFilter))
        {
            return predicate;
        }

        var splitBySemicolons = decodedFilter.Split(";");

        foreach (var str in splitBySemicolons)
        {
            predicate = AddParsedAttributeValuesFilters(str);
        }

        return predicate;

        ExpressionStarter<Space> AddCategories()
        {
            if (spaceType != SpaceType.None)
            {
                predicate = predicate.And(s => (s.SpaceType & spaceType) != SpaceType.None);
            }

            return predicate;
        }

        ExpressionStarter<Space> AddPriceRange()
        {
            if (minPrice.HasValue)
            {
                predicate = predicate.And(s => s.HourlyRate >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                predicate = predicate.And(s => s.HourlyRate <= maxPrice.Value);
            }

            return predicate;
        }

        ExpressionStarter<Space> AddParsedAttributeValuesFilters(string str)
        {
            var parts = str.Split(':', 2);
            if (parts.Length < 2 || !int.TryParse(parts[0].Trim(), out int attrId))
            {
                return predicate;
            }

            var values = parts[1]
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .Where(v => v.Length > 0)
                .ToList();

            if (values.Count == 0)
            {
                return predicate;
            }

            var allNumeric = values.All(v => double.TryParse(
                v,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out _));

            var attrFilter = allNumeric
                ? AddNumericAttribute(attrId, values)
                : AddStringAttribute(attrId, values);

            predicate = predicate.And(attrFilter);

            return predicate;
        }
    }

    private static Expression<Func<Space, bool>> BuildAttributePredicate(int attrId, IEnumerable<string> values)
    {
        var pParam = Expression.Parameter(typeof(Space), "p");
        var avParam = Expression.Parameter(typeof(AttributeValue), "av");

        var attrProp = Expression.Property(avParam, nameof(AttributeValue.AttributeId));
        var attrConst = Expression.Constant(attrId);
        var attrEqual = Expression.Equal(attrProp, attrConst);

        var valueProp = Expression.Property(avParam, nameof(AttributeValue.Value));
        Expression orChain = null;
        foreach (var val in values)
        {
            var valConst = Expression.Constant(val);
            var eq = Expression.Equal(valueProp, valConst);
            orChain = orChain == null ? eq : Expression.OrElse(orChain, eq);
        }

        var innerCondition = orChain ?? Expression.Constant(false);
        var innerAnd = Expression.AndAlso(attrEqual, innerCondition);
        var innerLambda = Expression.Lambda<Func<AttributeValue, bool>>(innerAnd, avParam);

        var avProp = Expression.Property(pParam, nameof(Space.AttributeValues));
        var anyMethod = typeof(Enumerable).GetMethods()
            .First(m => m.Name == "Any" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(AttributeValue));
        var anyCall = Expression.Call(anyMethod, avProp, innerLambda);

        return Expression.Lambda<Func<Space, bool>>(anyCall, pParam);
    }

    private static Expression<Func<Space, bool>> AddNumericAttribute(int attrId, List<string> values)
    {
        var normalizedValues = new HashSet<string>();
        foreach (var val in values)
        {
            if (double.TryParse(val, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out double numericVal))
            {
                normalizedValues.Add(val);
                normalizedValues.Add(numericVal.ToString(System.Globalization.CultureInfo.InvariantCulture));
                if (numericVal == Math.Floor(numericVal))
                {
                    normalizedValues.Add(((int)numericVal).ToString());
                }
            }
            else
            {
                normalizedValues.Add(val);
            }
        }

        return BuildAttributePredicate(attrId, normalizedValues);
    }

    private static Expression<Func<Space, bool>> AddStringAttribute(int attrId, List<string> values)
    {
        return BuildAttributePredicate(attrId, values);
    }
}
