import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";

function Signup() {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [age, setAge] = useState("");
  const [address, setAddress] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const navigate = useNavigate();

  const handleNameChange = (value) => {
    setName(value);
    setError("");
  };

  const handleAddressChange = (value) => {
    setAddress(value);
    setError("");
  };

  const handleAgeChange = (value) => {
    // Allow only numeric input for the age field
    const numericValue = value.replace(/\D/, "");
    setAge(numericValue);
    setError("");
  };

  const handleEmailChange = (value) => {
    setEmail(value);
    setError("");
  };

  const handlePasswordChange = (value) => {
    setPassword(value);
    setError("");
  };

  const axiosInstance = axios.create({
    withCredentials: true,
  });

  async function save(event) {
    event.preventDefault();

    // Basic validation
    if (!email || !name || !age || !address || !password) {
      setError("All fields are required");
      return;
    }

    // Additional validation for age
    const ageNumber = parseInt(age, 10);
    if (isNaN(ageNumber) || ageNumber <= 0) {
      setError("Age must be a positive number");
      return;
    }

    // Additional validation to prevent sending JSON strings
    if (/[{}[\]:,",]/.test(age)) {
      setError("Invalid characters in the age field.");
      return;
    }

    try {
      await axiosInstance.post(
        "https://localhost:7267/api/User/Register",
        {
          Name: name,
          Age: ageNumber,
          Email: email,
          Address: address,
          Password: password,
        }
      );
      alert("Registration successful.");
      setName("");
      setEmail("");
      setAge("");
      setPassword("");
      setAddress("");
      setError("");
      navigate("/Login");
    } catch (err) {
      setError("Registration failed. Please check your information.");
    }
  }

  return (
    <div className="login template d-flex justify-content-center align-items-center 100-w vh-100 bg-primary">
      <div className="40-w p-5 rounded bg-white">
        <form>
          <h3 className="text-center">Sign up</h3>
          <div className="mb-2">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              id="txtEmail"
              placeholder="Enter email"
              onChange={(e) => handleEmailChange(e.target.value)}
              className="form-control"
            />
          </div>
          <div className="mb-2">
            <label htmlFor="name">Name</label>
            <input
              type="text"
              id="txtName"
              placeholder="Enter name"
              onChange={(e) => handleNameChange(e.target.value)}
              className="form-control"
            />
          </div>
          <div className="mb-2">
            <label htmlFor="age">Age</label>
            <input
              type="text"
              id="txtAge"
              placeholder="Enter age"
              onChange={(e) => handleAgeChange(e.target.value)}
              className="form-control"
            />
          </div>
          <div className="mb-2">
            <label htmlFor="address">Address</label>
            <input
              type="text"
              id="txtAddress"
              placeholder="Enter address"
              onChange={(e) => handleAddressChange(e.target.value)}
              className="form-control"
            />
          </div>
          <div className="mb-2">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="txtPassword"
              placeholder="Enter Password"
              onChange={(e) => handlePasswordChange(e.target.value)}
              className="form-control"
            />
          </div>
          {error && <p style={{ color: "red" }}>{error}</p>}
          <div className="d-grid">
            <button onClick={save} className="btn btn-primary">
              Sign up
            </button>
          </div>
          <p>
            Already have an account? <Link to="/login">Login</Link>
          </p>
        </form>
      </div>
    </div>
  );
}

export default Signup;