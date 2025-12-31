using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using LinqKit;
using SpotRent.Domain.Entities;
using SpotRent.Domain.Enums;

namespace SpotRent.Services.Spaces;

public static class AttributeHelper
{
    public static Func<IQueryable<Space>, IOrderedQueryable<Space>> BuildOrderByDelegate(string sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return q => q.OrderBy(s => s.Id);
        }

        return sort switch
        {
            "price-asc" or "price_asc" => q => q.OrderBy(s => s.HourlyRate),
            "price-desc" or "price_desc" => q => q.OrderByDescending(s => s.HourlyRate),
            "newest" => q => q.OrderByDescending(s => s.CreatedAt),
            _ => q => q.OrderBy(s => s.Id)
        };
    }

    public static ExpressionStarter<Space> BuildPredicate(SpaceFilterCriteria criteria)
    {
        var predicate = PredicateBuilder.New<Space>(true);

        if (criteria.SpaceType.HasValue && criteria.SpaceType.Value != SpaceType.None)
        {
            predicate = predicate.And(s => (s.SpaceType & criteria.SpaceType.Value) != SpaceType.None);
        }

        if (criteria.MinCapacity.HasValue)
        {
            predicate = predicate.And(s => s.Capacity >= criteria.MinCapacity.Value);
        }

        if (criteria.MaxCapacity.HasValue)
        {
            predicate = predicate.And(s => s.Capacity <= criteria.MaxCapacity.Value);
        }

        if (criteria.MinAreaSqm.HasValue)
        {
            predicate = predicate.And(s => s.AreaSqm >= criteria.MinAreaSqm.Value);
        }

        if (criteria.MaxAreaSqm.HasValue)
        {
            predicate = predicate.And(s => s.AreaSqm <= criteria.MaxAreaSqm.Value);
        }

        if (criteria.MinHourlyRate.HasValue)
        {
            predicate = predicate.And(s => s.HourlyRate >= criteria.MinHourlyRate.Value);
        }

        if (criteria.MaxHourlyRate.HasValue)
        {
            predicate = predicate.And(s => s.HourlyRate <= criteria.MaxHourlyRate.Value);
        }

        if (!string.IsNullOrWhiteSpace(criteria.City))
        {
            predicate = predicate.And(s => s.Address != null && s.Address.City == criteria.City);
        }

        if (!string.IsNullOrWhiteSpace(criteria.Attributes))
        {
            predicate = AddParsedAttributeFilters(predicate, criteria.Attributes!);
        }

        return predicate;
    }

    private static ExpressionStarter<Space> AddParsedAttributeFilters(
        ExpressionStarter<Space> predicate,
        string rawFilter)
    {
        var decodedFilter = WebUtility.UrlDecode(rawFilter)?.Trim();
        if (string.IsNullOrEmpty(decodedFilter))
        {
            return predicate;
        }

        var segments = decodedFilter.Split(';', StringSplitOptions.RemoveEmptyEntries);
        foreach (var segment in segments)
        {
            var attrPredicate = BuildAttributeFilter(segment);
            if (attrPredicate != null)
            {
                predicate = predicate.And(attrPredicate);
            }
        }

        return predicate;
    }

    private static Expression<Func<Space, bool>> BuildAttributeFilter(string segment)
    {
        var parts = segment.Split(':', 2);
        if (parts.Length < 2 || !int.TryParse(parts[0].Trim(), out var attrId))
        {
            return null;
        }

        var values = parts[1]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(v => v.Trim())
            .Where(v => v.Length > 0)
            .ToList();

        if (values.Count == 0)
        {
            return null;
        }

        var numeric = values.All(v => double.TryParse(
            v,
            NumberStyles.Any,
            CultureInfo.InvariantCulture,
            out _));

        return numeric ? AddNumericAttribute(attrId, values) : AddStringAttribute(attrId, values);
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
            if (double.TryParse(val, NumberStyles.Any, CultureInfo.InvariantCulture, out var numericVal))
            {
                normalizedValues.Add(val);
                normalizedValues.Add(numericVal.ToString(CultureInfo.InvariantCulture));
                if (Math.Abs(numericVal - Math.Floor(numericVal)) < double.Epsilon)
                {
                    normalizedValues.Add(((int)numericVal).ToString(CultureInfo.InvariantCulture));
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
