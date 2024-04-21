using Microsoft.AspNetCore.Mvc;


namespace OTPGeneration.Controllers
{

    [ApiController]
    [Route("api/generate-otp")]

    public class OTPGenerationController : ControllerBase
    {

        public OTP otp;

        [HttpPost]
        public IActionResult GenerateOTP()
        {
            otp = GenOTPValue();
            string value = new string(otp.otpValue);
            return Ok(new { value });
        }

        [HttpPost("validity")]
        public IActionResult UpdateOTPValidity([FromBody] int timeLeft)
        {
            if (otp != null)
            {
                bool status = otp.IsOTPValid(timeLeft);
                otp.isValid = status;
                Console.WriteLine(otp.isValid);
                return Ok();
            }
            return NotFound("OTP object not found. Generate OTP first");
        }


        private OTP GenOTPValue()
        {
            Random random = new Random();
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            OTP otp = new OTP("");

            // Random 50/50 coin flip for the first value, either letter or digit
            int firstValueFlip = random.Next(2);
            if (firstValueFlip == 0)
            {
                // First value is a letter
                otp.otpValue[0] = letters[random.Next(letters.Length)];
            }
            else // firstValueFlip == 1
            {
                // First value is a digit
                otp.otpValue[0] = (char)('0' + random.Next(10));
            }

            // Generate rest of the values based on previous ones
            for (int i = 1; i < 5; i++)
            {
                if (char.IsLetter(otp.otpValue[i - 1])) // If previous value is a letter
                {
                    if (i == 4) // If the last character
                    {
                        // Calculate last digit based on the sum of digits
                        int sumOfDigits = 0;
                        foreach (char c in otp.otpValue)
                        {
                            if (char.IsDigit(c))
                            {
                                sumOfDigits += (c - '0');
                            }
                        }
                        // Calculate the last digit using formula
                        otp.otpValue[i] = (char)('0' + Math.Abs(1 - sumOfDigits) * 2 % 7);
                    }
                    else
                    {
                        // Generate random letter
                        otp.otpValue[i] = letters[random.Next(letters.Length)];
                    }
                }
                else // Previous character is a digit
                {
                    // 50/50 to decide whether to generate a letter or a digit
                    if (random.Next(2) == 0)
                    {
                        // Generate a random letter
                        otp.otpValue[i] = letters[random.Next(letters.Length)];
                    }
                    else
                    {
                        // Generate a random digit
                        otp.otpValue[i] = (char)('0' + random.Next(10));
                    }
                }
            }
            return otp;
        }
    }
}
