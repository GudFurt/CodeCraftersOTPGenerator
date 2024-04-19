using Microsoft.AspNetCore.Mvc;
using OTPGeneration.Controllers;
using Xunit;
using static System.Net.WebRequestMethods;



namespace OTPTesting
{
    public class OTPTests
    {
        [Fact]
        public void ConstructorSetValue()
        {
            string otpValue = "12345";

            OTP otp = new OTP(otpValue);

            Assert.Equal(otpValue, otp.OTPValue);
            Assert.True(otp.IsValid);
        }

        [Fact]
        public void OTPValueSetIsValidTrue()
        {

            string otpValue = "12345";
            OTP otp = new OTP("");

            otp.OTPValue = otpValue;

            Assert.Equal(otpValue, otp.OTPValue);
            Assert.True(otp.IsValid);
        }

        [Theory]
        [InlineData(10, true)]  // secondsLeft > 0
        [InlineData(0, false)]   // secondsLeft = 0
        [InlineData(-1, false)]  // secondsLeft < 0
        public void IsOTPValidTest(int secondsLeft, bool expectedResult)
        {

            OTP otp = new OTP("");


            bool result = otp.IsOTPValid(secondsLeft);


            Assert.Equal(expectedResult, result);
        }
    }

    public class OTPControllerTests
    {
        [Fact]
        public void GenerateOTPReturnTest()
        {

            var controller = new OTPGenerationController();


            var result = controller.GenerateOTP();

 
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            var value = okResult.Value;
            Assert.NotNull(value);

        }


        [Fact]
        public void UpdateOTPValidityNotFoundNull()
        {

            var controller = new OTPGenerationController();
            var timeLeft = 60;
            controller.otp = null;

            var result = controller.UpdateOTPValidity(timeLeft);

            Assert.IsType<NotFoundObjectResult>(result);

        }
    }

}
