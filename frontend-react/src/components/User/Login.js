import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import axios from 'axios';

function Login() {
  const navigate = useNavigate();

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  const handleEmailChange = (value) => {
    setEmail(value);
    setError('');
  };

  const handlePasswordChange = (value) => {
    setPassword(value);
    setError('');
  };

  async function save(event) {
    event.preventDefault();

    if (!email || !password) {
      setError('Email and password are required');
      return;
    }

    try {
      await axios.post(
        'https://localhost:7267/api/User/Login',
        {
          Email: email,
          Password: password,
        },
        {
          withCredentials: true,
        }
      );
      setEmail('');
      setPassword('');
      setError('');
      navigate('/Home');
    } catch (err) {
      setError('Invalid email or password');
    }
  }

  return (
    <div className="login template d-flex justify-content-center align-items-center 100-w vh-100 bg-primary">
      <div className="40-w p-5 rounded bg-white">
        <form data-testid="login-form">
          <h3 className="text-center">Sign in</h3>
          <div className="mb-2">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              placeholder="Enter email"
              onChange={(e) => handleEmailChange(e.target.value)}
              className="form-control"
              data-testid="email"
            />
          </div>
          <div className="mb-2">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              placeholder="Enter Password"
              onChange={(e) => handlePasswordChange(e.target.value)}
              className="form-control"
              data-testid="password"
            />
          </div>
          {error && <p style={{ color: 'red' }} data-testid="error-message">{error}</p>}
          <div className="d-grid">
            <button onClick={save} className="btn btn-primary" data-testid="login-button">
              Sign in
            </button>
          </div>
          <p>
            <Link to="/signup" data-testid="signup-link">Signup</Link>
          </p>
        </form>
      </div>
    </div>
  );
}

export default Login;