﻿using CSharpFunctionalExtensions;
using FluentValidation;
using StudentManagement.Domain.Utils;

namespace StudentManagement.Api.Utils;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
     this IRuleBuilder<T, TElement> ruleBuilder,
     Func<TElement, Result<TValueObject, Error>> factoryMethod)
     where TValueObject : ValueObject
    {
        return MustBeDomainObject(ruleBuilder, factoryMethod);
    }

    public static IRuleBuilderOptions<T, TElement> MustBeEntity<T, TElement, TEntity>(
     this IRuleBuilder<T, TElement> ruleBuilder,
     Func<TElement, Result<TEntity, Error>> factoryMethod)
     where TEntity : Entity
    {
        return MustBeDomainObject(ruleBuilder, factoryMethod);
    }
    
    private static IRuleBuilderOptions<T, TElement> MustBeDomainObject<T, TElement, TDomainObject>(
        IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TDomainObject, Error>> factoryMethod)
        where TDomainObject : class
    {
        return (IRuleBuilderOptions<T, TElement>)ruleBuilder.Custom((value, context) =>
        {
            Result<TDomainObject, Error> result = factoryMethod(value);

            if (result.IsFailure)
            {
                context.AddFailure(result.Error.Serialize());
            }
        });
    }
}