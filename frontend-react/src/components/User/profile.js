import React, { useState } from "react";
import NavBar from "../Navigation";
import axios from "axios";
import Footer from "../Footer";
import "../../Style/Profile.css";

const Profile = ({ user }) => {
  const [name, setName] = useState(user.name || "");
  const [age, setAge] = useState(user.age || "");
  const [address, setAddress] = useState(user.address || "");

  const handleUpdateProfile = async (event) => {
    event.preventDefault();

    // Basic validation
    if (!name || !address || !age) {
      alert("All fields are required.");
      return;
    }

    // Additional validation for age
    const ageNumber = parseInt(age, 10);
    if (isNaN(ageNumber) || ageNumber <= 0) {
      alert("Age must be a valid positive number.");
      return;
    }

    // Additional validation to prevent sending JSON strings
    if (/[{}[\]:,",]/.test(age)) {
      alert("Invalid characters in the age field.");
      return;
    }

    try {
      await axios.put(
        `https://localhost:7267/api/User/Update/${user.id}`,
        {
          name,
          age,
          address,
        },
        {
          withCredentials: true,
        }
      );

      alert("Updated successfully!");
    } catch (error) {
      alert("Error updating profile:", error);
    }
  };

  return (
    <div>
      <NavBar user={user} />
      <div className="profile-container">
        <style>{'body { background-color: #1b1b1b; }'}</style>
        <h2 className="profile-title">{user.name}'s profile</h2>
        <form className="profile-form" onSubmit={handleUpdateProfile}>
          <div className="profile-input">
            <div>
              <label>Name</label>
            </div>
            <input type="text" value={name} onChange={(e) => setName(e.target.value)} />
          </div>{" "}
          <br />
          <div className="profile-input">
            <div>
              <label>Address</label>
            </div>
            <input type="text" value={address} onChange={(e) => setAddress(e.target.value)} />
          </div>{" "}
          <br />
          <div className="profile-input">
            <div>
              <label>Age</label>
            </div>
            <input
              type="text"
              value={age}
              onChange={(e) => {
                // Allow only numeric input for the age field
                const numericValue = e.target.value.replace(/\D/, "");
                setAge(numericValue);
              }}
            />
          </div>{" "}
          <br />
          <button type="submit" className="profile-button">
            Update Profile
          </button>
        </form>
      </div>
      <hr style={{ color: "white" }} />
      <Footer />
    </div>
  );
};

export default Profile;