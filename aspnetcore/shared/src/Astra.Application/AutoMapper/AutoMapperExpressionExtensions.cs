using System.Linq.Expressions;
using Astra.AutoMapper.Converters;
using Astra.Localization;
using AutoMapper;

namespace Astra.AutoMapper;

public static class AutoMapperExpressionExtensions
{
    public static IMappingExpression<TSource, TDestination> ToUrl<TSource, TDestination>(
        this IMappingExpression<TSource, TDestination> mappingExpression,
        Expression<Func<TDestination, string?>> destinationMember,
        Expression<Func<TSource, string?>> sourceMember)
        where TDestination : class
        where TSource : class
    {
        return mappingExpression.ForMember(
            destinationMember,
            memberOptions =>
                memberOptions.ConvertUsing<FileUrlValueConverter, string?>(sourceMember));
    }

    public static IMappingExpression<TSource, TDestination>
        ToValue<TSource, TDestination, TSourceMember, TDestinationMember, TValueConverter>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            Expression<Func<TDestination, TDestinationMember>> destinationMember,
            Expression<Func<TSource, TSourceMember>> sourceMember)
        where TValueConverter : IValueConverter<TSourceMember, TDestinationMember>
        where TDestination : class, new()
        where TDestinationMember : class
    {
        return mappingExpression.ForMember(
            destinationMember,
            memberOptions =>
                memberOptions.ConvertUsing<TValueConverter, TSourceMember>(sourceMember));
    }
    
    public static IMappingExpression<TSource, TDestination> ToLocal<TSource, TDestination, TSourceMember, TDestinationMember>(
        this IMappingExpression<TSource, TDestination> mappingExpression,
        Expression<Func<TDestination, TDestinationMember>> destinationMember,
        Expression<Func<TSource, TSourceMember>> sourceMember)
        where TSource : IHasLocalizable
        where TDestination : class, new()
    {
        return mappingExpression.ForMember(
            destinationMember,
            opt =>
                opt.MapFrom(s => s.GetLocalizableValue(sourceMember)));
    }
}