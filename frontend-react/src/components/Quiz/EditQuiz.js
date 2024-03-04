import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import NavBar from "../Navigation";
import axios from "axios";
import Footer from "../Footer";
import "../../Style/EditQuiz.css";

const EditQuiz = ({ user }) => {
  const { quizId } = useParams();
  const navigate = useNavigate();
  const [selectedQuiz, setSelectedQuiz] = useState({
    title: "",
    description: "",
    categoryid: null,
    ipQuestions: [],
  });

  const [categories, setCategories] = useState([]);
  const [validationErrors, setValidationErrors] = useState({});

  useEffect(() => {
    fetchSelectedQuiz();
    fetchCategories();
  }, [quizId]);

  const fetchSelectedQuiz = async () => {
    try {
      const response = await axios.get(`https://localhost:7267/api/Quiz/GetQuizQandA/${quizId}`);
      setSelectedQuiz(response.data);
    } catch (error) {
      console.error("Error fetching selected quiz:", error);
    }
  };

  const fetchCategories = async () => {
    try {
      const response = await axios.get("https://localhost:7267/api/Category");
      setCategories(response.data);
    } catch (error) {
      console.error("Error fetching categories:", error);
    }
  };

  const handleDeleteQuestion = async (questionId) => {
    try {
      await axios.delete(`https://localhost:7267/api/Question/DeleteQuestion/${questionId}`);
      fetchSelectedQuiz();
    } catch (error) {
      console.error(`Error deleting question with ID ${questionId}:`, error);
    }
  };

  const handleDeleteAnswer = async (answerId) => {
    try {
      await axios.delete(`https://localhost:7267/api/Answer/DeleteAnswer/${answerId}`);
      fetchSelectedQuiz();
    } catch (error) {
      console.error(`Error deleting answer with ID ${answerId}:`, error);
    }
  };

  const handleUpdateQuiz = async () => {
    if (validateForm()) {
      try {
        const updatedQuiz = {
          Title: selectedQuiz.title,
          Description: selectedQuiz.description,
          CategoryId: selectedQuiz.categoryid,
          Questions: selectedQuiz.ipQuestions.map((question) => ({
            Text: question.text,
            Answers: question.ipAnswers.map((answer) => ({
              Text: answer.text,
              IsCorrect: answer.isCorrect,
            })),
          })),
        };

        await axios.put(`https://localhost:7267/api/Quiz/Update/${quizId}`, updatedQuiz);
        fetchSelectedQuiz();
        navigate("/MyQuizzes");
      } catch (error) {
        console.error(`Error updating quiz with ID ${quizId}:`, error);
      }
    }
  };

  const validateForm = () => {
    const errors = {};
    if (!selectedQuiz.title.trim()) {
      errors.title = "Title is required";
    }
    if (!selectedQuiz.description.trim()) {
      errors.description = "Description is required";
    }
    if (!selectedQuiz.categoryid) {
      errors.category = "Category is required";
    }

    setValidationErrors(errors);

    return Object.keys(errors).length === 0;
  };

  const handleTitleChange = (e) => {
    setSelectedQuiz((prev) => ({ ...prev, title: e.target.value }));
  };

  const handleDescriptionChange = (e) => {
    setSelectedQuiz((prev) => ({ ...prev, description: e.target.value }));
  };

  const handleCategoryChange = (e) => {
    setSelectedQuiz((prev) => ({ ...prev, categoryid: e.target.value }));
  };

  const handleQuestionTextChange = (questionId, newText) => {
    setSelectedQuiz((prev) => ({
      ...prev,
      ipQuestions: prev.ipQuestions.map((question) =>
        question.id === questionId ? { ...question, text: newText } : question
      ),
    }));
  };

  const handleAnswerTextChange = (questionId, answerId, newText) => {
    setSelectedQuiz((prev) => ({
      ...prev,
      ipQuestions: prev.ipQuestions.map((question) =>
        question.id === questionId
          ? {
              ...question,
              ipAnswers: question.ipAnswers.map((answer) =>
                answer.id === answerId ? { ...answer, text: newText } : answer
              ),
            }
          : question
      ),
    }));
  };

  const handleIsCorrectChange = (questionId, answerId, newIsCorrect) => {
    newIsCorrect = newIsCorrect === 'true';

    setSelectedQuiz((prev) => ({
      ...prev,
      ipQuestions: prev.ipQuestions.map((question) =>
        question.id === questionId
          ? {
              ...question,
              ipAnswers: question.ipAnswers.map((answer) =>
                answer.id === answerId
                  ? { ...answer, isCorrect: newIsCorrect }
                  : { ...answer, isCorrect: false } // Uncheck other answers
              ),
            }
          : question
      ),
    }));
  };
  return (
    <>
      <style>{'body { background-color: #1b1b1b; }'}</style>
      <NavBar user={user} />
      <div className="container mt-4">
        {selectedQuiz ? (
          <>
            <h3 className="headers-info">Title</h3>
            <input
              type="text"
              value={selectedQuiz.title}
              onChange={handleTitleChange}
              className="edit-input"
              data-testid="quiz-title-input"
            />
            {validationErrors.title && (
              <p className="validation-error">{validationErrors.title}</p>
            )}
            <br /> <br />
            <h3 className="headers-info">Description</h3>
            <textarea
              value={selectedQuiz.description}
              onChange={handleDescriptionChange}
              className="edit-input"
            />
            {validationErrors.description && (
              <p className="validation-error">{validationErrors.description}</p>
            )}
            <br /> <br />
            <h3 className="headers-info">Category</h3>
            <select
              value={selectedQuiz.categoryid || ""}
              onChange={handleCategoryChange}
              className="edit-input"
            >
              <option value="">Select a category</option>
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.name}
                </option>
              ))}
            </select>
            {validationErrors.category && (
              <p className="validation-error">{validationErrors.category}</p>
            )} <br /> <br />

            {/* Rest of your JSX */}
            <h3 className="headers-info">Questions</h3>
            {selectedQuiz.ipQuestions && selectedQuiz.ipQuestions.length > 0 ? (
              <ul style={{ color: "white" }} className="question-list">
                {selectedQuiz.ipQuestions.map((question) => (
                  <li key={question.id} className="question-item">
                    <input
                      type="text"
                      value={question.text}
                      onChange={(e) => handleQuestionTextChange(question.id, e.target.value)}
                      className="edit-input"
                    />
                    <h3 className="answer-heading">Answers:</h3>
                    <ul className="answer-list">
                      {question.ipAnswers && question.ipAnswers.length > 0 ? (
                        question.ipAnswers.map((answer) => (
                          <li key={answer.id} className="answer-item">
                            <input
                              type="text"
                              value={answer.text}
                              onChange={(e) =>
                                handleAnswerTextChange(question.id, answer.id, e.target.value)
                              }
                              className="edit-input"
                            />
                            <select style={{marginLeft: "10px"}}
                              value={answer.isCorrect}
                              onChange={(e) =>
                                handleIsCorrectChange(question.id, answer.id, e.target.value)
                              }
                              className="edit-input"
                            >
                              <option value={true}>Correct</option>
                              <option value={false}>Incorrect</option>
                            </select>
                            <button style={{marginLeft: "10px"}}
                              className="btn btn-danger ml-2"
                              onClick={() => handleDeleteAnswer(answer.id)}
                            >
                              Delete Answer
                            </button>
                          </li>
                        ))
                      ) : (
                        <p className="no-answer">No answers for this question.</p>
                      )}
                    </ul>
                    <button
                      className="btn btn-danger mt-2"
                      onClick={() => handleDeleteQuestion(question.id)}
                    >
                      Delete Question
                    </button>
                  </li>
                ))}
              </ul>
            ) : (
              <p className="no-question">No questions found for this quiz.</p>
            )}

            <div className="text-center">
              {/* Button to update the entire quiz */}
              <button data-testid="update-quiz-button" className="btn btn-primary mt-4" onClick={handleUpdateQuiz}>
                Update Quiz
              </button>
            </div>
          </>
        ) : (
          <p className="loading">Loading...</p>
        )}
      </div>
      <br />
      <Footer />
    </>
  );
};

export default EditQuiz;
