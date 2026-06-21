using System;
using Eligibility.Api.Domain.Rules;
using FluentAssertions;
using Xunit;

namespace Eligibility.Api.Tests.Domain;

public class RuleTests
{
    private static LoanApplicationData CreateData(
        decimal income = 30000m,
        decimal amount = 100000m,
        int tenure = 12,
        decimal emi = 10000m)
    {
        return new LoanApplicationData(Guid.NewGuid(), Guid.NewGuid(), amount, tenure, income, emi);
    }

    [Theory]
    [InlineData(25000, true)]
    [InlineData(30000, true)]
    [InlineData(24999.99, false)]
    public void MinimumIncomeRule_ShouldEvaluateCorrectly(decimal income, bool expectedPassed)
    {
        var rule = new MinimumIncomeRule();
        var data = CreateData(income: income);

        var result = rule.Evaluate(data);

        result.Passed.Should().Be(expectedPassed);
    }

    [Theory]
    [InlineData(10000, 5000, true)] // 50%
    [InlineData(10000, 5001, false)] // > 50%
    [InlineData(10000, 0, true)] // 0%
    public void DebtToIncomeRule_ShouldEvaluateCorrectly(decimal income, decimal emi, bool expectedPassed)
    {
        var rule = new DebtToIncomeRule();
        var data = CreateData(income: income, emi: emi);

        var result = rule.Evaluate(data);

        result.Passed.Should().Be(expectedPassed);
    }

    [Theory]
    [InlineData(10000, 200000, true)] // exactly 20x
    [InlineData(10000, 150000, true)] // < 20x
    [InlineData(10000, 200001, false)] // > 20x
    public void MaximumAmountRule_ShouldEvaluateCorrectly(decimal income, decimal amount, bool expectedPassed)
    {
        var rule = new MaximumAmountRule();
        var data = CreateData(income: income, amount: amount);

        var result = rule.Evaluate(data);

        result.Passed.Should().Be(expectedPassed);
    }

    [Theory]
    [InlineData(6, true)]
    [InlineData(84, true)]
    [InlineData(5, false)]
    [InlineData(85, false)]
    [InlineData(24, true)]
    public void TenureRule_ShouldEvaluateCorrectly(int tenure, bool expectedPassed)
    {
        var rule = new TenureRule();
        var data = CreateData(tenure: tenure);

        var result = rule.Evaluate(data);

        result.Passed.Should().Be(expectedPassed);
    }

    [Theory]
    [InlineData(10000, 10000, true)] // Equal
    [InlineData(10000, 5000, true)] // Less
    [InlineData(10000, 10001, false)] // Greater
    public void EmiObligationRule_ShouldEvaluateCorrectly(decimal income, decimal emi, bool expectedPassed)
    {
        var rule = new EmiObligationRule();
        var data = CreateData(income: income, emi: emi);

        var result = rule.Evaluate(data);

        result.Passed.Should().Be(expectedPassed);
    }
}
