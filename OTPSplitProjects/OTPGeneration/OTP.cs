using System;

public class OTP
{

    public char[] otpValue = new char[5];


    public bool isValid;


    public OTP(string otp)
    {
        


        otp.CopyTo(otpValue);


        isValid = true;
    }

    public string OTPValue
    {
        get { return new string(otpValue); }
        set
        {
            value.CopyTo(otpValue);
            isValid = true;
        }
    }

    public bool IsValid
    {
        get { return isValid; }
    }

    public bool IsOTPValid(int secondsLeft)
    {
        if(secondsLeft > 0)
        {
            return true;
        }
        return false;
    }
}
