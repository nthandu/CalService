namespace CalculationServices
{
    /// <summary>
    /// Contains methods
    /// </summary>
    public interface ICalculator
    {
        /// <summary>
        /// Performs the calculation on the user input.
        /// </summary>
        /// <param name="input"></param>
        CalculationResponse Calculate(string input);
    }
}