import React from 'react';
import "../Style/Footer.css";

const Footer = () => {
  return (
    <footer className="footer-container">
      <div className="contact-info">
        <h3 className='header-info'>Contact Information</h3>
        <p>Email: info@quizzturn.com</p>
        <p>Phone: +31 (0)12 345 6789</p>
        <p>Address: 123 Main Street, Cityville</p>
      </div>
      <div className="quizzturn-info quizz-turn">
        <h3>QuizzTurn</h3>
        <p>&copy; 2023 QuizzTurn. All rights reserved.</p>
      </div>
    </footer>
  );
};

export default Footer;