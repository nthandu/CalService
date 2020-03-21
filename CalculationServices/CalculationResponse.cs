namespace CalculationServices
{
    /// <summary>
    /// Holds the response from a calculation.
    /// </summary>
    public class CalculationResponse
    {
        /// <summary>
        /// Returns true when there is an error in performing the calculation.
        /// </summary>
        public bool HasError { get; set; }

        /// <summary>
        /// Returns the error message when there is an error occurred and a message is set.
        /// </summary>
        public string ErrorMessage { get; set; }
        
        /// <summary>
        /// Gets/sets the result of the calculation.
        /// </summary>
        public string Result { get; set; }
    }
}