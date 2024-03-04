import React, { useState } from "react";
import "../../Style/FunFacts.css"; // Create a CSS file for styling

const FunFacts = () => {
  const funFactsData = [
    "1: Quizzes can improve memory!",
    "2: The word 'quiz' originates from the Latin word 'quis', meaning 'who'.",
    "3: The longest quiz marathon lasted for 150 hours!",
    // Add more fun facts as needed
  ];

  const [currentFactIndex, setCurrentFactIndex] = useState(0);

  const handleFactChange = () => {
    setCurrentFactIndex((prevIndex) => (prevIndex + 1) % funFactsData.length);
  };

  return (
    <div className="fun-facts-container">
      <h3 className="fun-facts-text">Fun Facts</h3>
      <p className="fun-facts-text">{funFactsData[currentFactIndex]}</p>
      <button onClick={handleFactChange}>Next Fun Fact</button>
    </div>
  );
};

export default FunFacts;