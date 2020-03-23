using NUnit.Framework;
using Services.Core;

namespace Services.Tests
{
    public class CalculatorTests
    {
        [Test]
        public void CalculateReturnsInvalidResponseWhenInputIsNull()
        {
            var calculator = new Calculator();
            var response = calculator.Calculate("");
            Assert.IsTrue(response.HasError);
        }

        [Test]
        public void CalculateResponseShouldReturnErrorMessageWhenWhenInputIsNull()
        {
            var calculator = new Calculator();
            var response = calculator.Calculate("");
            Assert.IsTrue(response.HasError);
            Assert.IsNotNull(response.ErrorMessage);
        }

        [Test]
        public void CalculateReturnsValidResponseWhenInputIsNotNull()
        {
            var calculator = new Calculator();
            var response = calculator.Calculate("1234");
            Assert.IsFalse(response.HasError);
        }

        [Test]
        public void CalculateResponseShouldSetHasErrorToTrueWhenInputContainsAlphabets()
        {
            var calculator = new Calculator();
            var response = calculator.Calculate("abc123");
            Assert.IsTrue(response.HasError);
            Assert.NotNull(response.ErrorMessage);
        }

        [Test]
        public void CalculateShouldAllowAcceptableOperatorsAlongWithIntegers()
        {
            var calculator = new Calculator();
            var response = calculator.Calculate("1-23");
            Assert.IsFalse(response.HasError);
            Assert.Null(response.ErrorMessage);
        }

        [Test]
        public void CalculateShouldNotAllowNotAcceptableOperatorsAlongWithIntegers()
        {
            var calculator = new Calculator();
            var response = calculator.Calculate("1%23");
            Assert.IsTrue(response.HasError);
            Assert.NotNull(response.ErrorMessage);
        }

        [TestCase("4*5", "20")]
        [TestCase("4*5*2", "40")]
        [TestCase("4*5*3*2", "120")]
        [TestCase("4*5*3*2/2", "60")]
        [TestCase("4/2*5*3*2", "60")]
        [TestCase("4/2*5/2*3*2", "30")]
        [TestCase("5/2", "2.5")]
        [TestCase("4/2*3", "6")]
        [TestCase("5/2*3", "7.5")]
        [TestCase("5/2*3", "7.5")]
        [TestCase("4+5*2","14")]
        [TestCase("4+5/2", "6.5")]
        [TestCase("4+5/2-1", "5.5")]
        public void CalculateShouldReturnResponseAsExpected(string input, string expected)
        {
            var calculator = new Calculator();
            var response = calculator.Calculate(input);
            Assert.IsFalse(response.HasError);
            Assert.AreEqual(expected, response.Result);
        }

    }
}
