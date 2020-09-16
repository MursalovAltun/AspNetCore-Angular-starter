using System;
using System.Linq;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Internal;
using Xunit;

namespace UnitTests.Common.Asserts
{
    public static class FluentValidationAsserts
    {
        public static PropertyRule HasPropertyRule<T, TProperty>(
            this AbstractValidator<T> validator,
            Expression<Func<T, TProperty>> expression)
        {
            var propertyRule = validator
                .OfType<PropertyRule>()
                .SingleOrDefault(rule => rule.Member == expression.GetMember());

            var propertyName = expression.GetMember().Name;
            Assert.True(propertyRule != null, $"Expects a property rule for {propertyName}, but it is not defined");

            return propertyRule;
        }

        public static void HasValidator<TValidator>(this PropertyRule propertyRule)
        {
            var actual = propertyRule.Validators.SingleOrDefault();

            var memberName = propertyRule.Member.Name;
            var type = typeof(TValidator);
            var message =
                $"Expects the property rule for {memberName} has only validator of type {type}, but it doesn't have";

            Assert.True(actual is TValidator, message);
        }

        public static void HasValidator<TValidator1, TValidator2>(this PropertyRule propertyRule)
        {
            propertyRule.HasValidator(new[] {typeof(TValidator1), typeof(TValidator2)});
        }

        public static void HasValidator<TValidator1, TValidator2, TValidator3>(this PropertyRule propertyRule)
        {
            propertyRule.HasValidator(new[] {typeof(TValidator1), typeof(TValidator2), typeof(TValidator3)});
        }

        public static void HasValidator(this PropertyRule propertyRule, Type[] validators)
        {
            var actual = propertyRule.Validators.Select(validator => validator.GetType());
            var unexpected = actual.Except(validators).ToArray();

            var typeNameOfUnexpectedValidators = string.Join(", ", unexpected.Select(type => type.Name));
            Assert.True(0 == unexpected.Length, $"There are unexpected validators: {typeNameOfUnexpectedValidators}");

            var missed = validators.Except(validators).ToArray();
            var typeNamesOfMissedValidators = string.Join(", ", missed.Select(type => type.Name));
            Assert.True(0 == unexpected.Length, $"There are missed validators: {typeNamesOfMissedValidators}");
        }
    }
}