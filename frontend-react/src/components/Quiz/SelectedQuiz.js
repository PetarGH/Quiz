import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import NavBar from "../Navigation";
import axios from "axios";
import { FaCheck, FaTimes } from "react-icons/fa";
import "../../Style/SelectedQuiz.css";

const SelectedQuiz = ({ user }) => {
  const { quizId } = useParams();
  const [selectedQuiz, setSelectedQuiz] = useState({
    title: "",
    description: "",
    categoryid: null,
    ipQuestions: [],
  });
  const [submitted, setSubmitted] = useState(false);
  const [userAnswers, setUserAnswers] = useState([]);
  const [correctAnswers, setCorrectAnswers] = useState([]);

  useEffect(() => {
    fetchSelectedQuiz();
  }, [quizId]);

  const fetchSelectedQuiz = async () => {
    try {
      const response = await axios.get(`https://localhost:7267/api/Quiz/GetQuizQandA/${quizId}`);
      setSelectedQuiz(response.data);
      setUserAnswers(new Array(response.data.ipQuestions.length).fill(""));
    } catch (error) {
      console.error("Error fetching selected quiz:", error);
    }
  };

  const handleSubmit = async () => {
    try {
      const response = await axios.get(`https://localhost:7267/api/Answer/GetAllRightQuizAnswers/${quizId}`);
      setCorrectAnswers(response.data);
      setSubmitted(true);
    } catch (error) {
      console.error("Error fetching correct answers:", error);
    }
  };

  const handleOptionChange = (questionIndex, answerText) => {
    setUserAnswers((prev) =>
      prev.map((answer, index) => (index === questionIndex ? answerText : answer))
    );
  };

  // Function to check if all questions are answered
  const allQuestionsAnswered = () => {
    return userAnswers.every((answer) => answer !== "");
  };

  const countCorrectAnswers = () => {
    return userAnswers.filter((answer, index) => answer === correctAnswers[index].text).length;
  };

  return (
    <>
      <NavBar user={user} />
      <div className="container mt-4">
        {selectedQuiz ? (
          <>
            <h2 className="headers-info align-text">{selectedQuiz.title}</h2>
            <p className="headers-info align-text">{selectedQuiz.description}</p>

            {/* Display questions and options */}
            {selectedQuiz.ipQuestions && selectedQuiz.ipQuestions.length > 0 ? (
              selectedQuiz.ipQuestions.map((question, questionIndex) => (
                <div key={question.id}>
                  <h4 className="question-info">{`Question ${questionIndex + 1}: ${question.text}`}</h4>
                  <ul className="headers-info">
                    {question.ipAnswers && question.ipAnswers.length > 0 ? (
                      question.ipAnswers.map((answer) => (
                        <li key={answer.id}>
                          <label>
                            <input
                              type="radio"
                              name={`question_${question.id}`}
                              value={answer.text}
                              checked={userAnswers[questionIndex] === answer.text}
                              onChange={() => handleOptionChange(questionIndex, answer.text)}
                              disabled={submitted}
                            />
                            {answer.text}
                          </label>
                        </li>
                      ))
                    ) : (
                      <p className="no-info">No options available for this question.</p>
                    )}
                  </ul>
                </div>
              ))
            ) : (
              <p className="no-info">No questions available for this quiz.</p>
            )}

            {/* Submit button */}
            {!submitted && (
              <button className="submit-button" onClick={handleSubmit} disabled={!allQuestionsAnswered()}>
                Submit
              </button>
            )}

            {/* Display user answers and correct answers after submission */}
            {submitted && userAnswers.length > 0 && correctAnswers.length > 0 ? (
  <div>
    <h3 className="answer-info">Your Answers: {countCorrectAnswers()}/{userAnswers.length}</h3>
    <ul className="headers-info">
      {userAnswers.map((answer, index) => (
        <li key={index}>
          <strong>{`Question ${index + 1} (${
            userAnswers[index] === correctAnswers[index].text ? "Correct" : "Incorrect"
          }):`}</strong>{" "}
          {answer}
          {userAnswers[index] === correctAnswers[index].text && (
            <FaCheck style={{ color: "green", marginLeft: "5px" }} />
          )}
          {userAnswers[index] !== correctAnswers[index].text && (
            <FaTimes style={{ color: "red", marginLeft: "5px" }} />
          )}
          {userAnswers[index] !== correctAnswers[index].text && (
            <>
              <strong className="answer-info" style={{ paddingLeft: '10px' }}>Correct Answer:</strong>{" "}
              {correctAnswers[index].text}
            </>
          )}
        </li>
      ))}
    </ul>
              </div>
            ) : (
              <p className="no-info">Please submit your answers</p>
            )}
          </>
        ) : (
          <p>Loading...</p>
        )}
      </div>
    </>
  );
};

export default SelectedQuiz;