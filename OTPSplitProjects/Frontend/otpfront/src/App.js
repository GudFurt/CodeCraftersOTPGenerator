import logo from './logo.svg';
import './App.css';
import Button from '@mui/material/Button';
import axios from 'axios';
import React, { useState, useEffect } from 'react';


function App() {
    const [otp, setOtp] = useState('');
    const [timeLeft, setTimeLeft] = useState(60);
    const [buttonDisabled, setButtonDisabled] = useState(false);
    

    const handleOTPGeneration = async () => {
        try {
            const response = await axios.post('https://localhost:7231/api/generate-otp/generation');
            console.log(response.data);

            setOtp(response.data.value);

            setTimeLeft(60);
            setButtonDisabled(true);

            const timer = setInterval(() => {
                setTimeLeft(prevTime => prevTime - 1);
            }, 1000);

            setTimeout(() => {
                clearInterval(timer);
                setButtonDisabled(false);
                setOtp('');
            }, 60000);


        } catch (error) {
            console.error('Error generating otp: ', error);
        }
    }


    const sendTimeLeftToBackend = async (timeLeft) => {
        try {
            
            const response = await axios.post('https://localhost:7231/api/update-otp-validity', { timeLeft });
            console.log('Time left sent to backend:', timeLeft);
        } catch (error) {
            console.error('Error sending time left to backend: ', error);
        }
    }




  return (
      <div className="App-header">
          <h1>OTP Generation</h1>
          <div className="digitContainers">
              {otp && otp.split('').map((digit, index) => (
                  <div key={index} className="Key">{digit}</div>
              ))}
          </div>


          <Button onClick={handleOTPGeneration} className="GenButton" variant="outlined" disabled={buttonDisabled}>Generate New</Button>


          <p className="TimeText">Time Left To Use: {timeLeft} seconds</p>
      </div>
  );
}

export default App;
