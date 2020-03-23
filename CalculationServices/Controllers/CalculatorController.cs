using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Core;

namespace CalculationServices.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        #region Members

        private readonly ILogger<CalculatorController> _logger;
        private readonly ICalculator _calculator;

        #endregion

        #region Controller

        public CalculatorController(ILogger<CalculatorController> logger,
            ICalculator calculator)
        {
            _logger = logger;
            _calculator = calculator;
        }

        #endregion


        [HttpGet]
        public IActionResult Get([FromQuery]string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return BadRequest("Invalid input");

            var response = _calculator.Calculate(input);

            if (response.HasError == false &&
                !string.IsNullOrWhiteSpace(response.Result))
            {
                return Ok(response.Result);
            }

            return BadRequest(response.ErrorMessage);
        }
    }
}