import React, { useState, useEffect } from "react";
import NavBar from "../Navigation";
import axios from "axios";
import Footer from "../Footer";
import { useNavigate } from "react-router-dom";

const CreateQuiz = ({ user }) => {
  const [quizData, setQuizData] = useState({
    title: "",
    description: "",
    category: "",
    questions: [{ text: "", answers: [{ text: "", isCorrect: false }] }],
  });
  const [categories, setCategories] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    try {
      const response = await axios.get("https://localhost:7267/api/Category");
      setCategories(response.data);
    } catch (error) {
      console.error("Error fetching categories:", error);
    }
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setQuizData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleQuestionChange = (index, e) => {
    const { value } = e.target;
    setQuizData((prevData) => {
      const updatedQuestions = [...prevData.questions];
      updatedQuestions[index] = { ...updatedQuestions[index], text: value };
      return { ...prevData, questions: updatedQuestions };
    });
  };

  const handleAnswerChange = (qIndex, aIndex, e) => {
    const { name, value, checked, type } = e.target;
    setQuizData((prevData) => {
      const updatedQuestions = [...prevData.questions];
      const updatedAnswers = [...updatedQuestions[qIndex].answers];

      if (type === "text") {
        updatedAnswers[aIndex] = {
          ...updatedAnswers[aIndex],
          text: value,
        };
      } else if (type === "checkbox") {
        updatedAnswers.forEach((answer, index) => {
          // Uncheck all other checkboxes in the same question
          if (index !== aIndex) {
            answer.isCorrect = false;
          } else {
            // Check the selected checkbox
            updatedAnswers[aIndex] = {
              ...updatedAnswers[aIndex],
              isCorrect: checked,
            };
          }
        });
      }

      updatedQuestions[qIndex] = {
        ...updatedQuestions[qIndex],
        answers: updatedAnswers,
      };
      return { ...prevData, questions: updatedQuestions };
    });
  };


  const handleAddQuestion = () => {
    setQuizData((prevData) => ({
      ...prevData,
      questions: [
        ...prevData.questions,
        { text: "", answers: [{ text: "", isCorrect: false }] },
      ],
    }));
  };

  const handleAddAnswer = (qIndex) => {
    const maxAnswers = 4;
  
    setQuizData((prevData) => {
      const updatedQuestions = [...prevData.questions];
      const currentAnswers = updatedQuestions[qIndex].answers;
  
      if (currentAnswers.length < maxAnswers) {
        const newAnswer = { text: "", isCorrect: false };
        updatedQuestions[qIndex] = {
          ...updatedQuestions[qIndex],
          answers: [...currentAnswers, newAnswer],
        };
      }
  
      return { ...prevData, questions: updatedQuestions };
    });
  };
  
  const handleCreateQuiz = async (e) => {
    e.preventDefault();
    const { title, description, category, questions } = quizData;

    const formattedQuestions = questions.map(({ text, answers }) => ({
      text,
      answers: answers.map(({ text, isCorrect }) => ({ text, isCorrect })),
    }));
  
    const quizPayload = {
      title,
      description,
      categoryId: category,
      CreatedBy: user.id,
      questions: formattedQuestions,
    };
  
    try {
      await axios.post("https://localhost:7267/api/Quiz/CreateQuiz", quizPayload);
      navigate("/MyQuizzes");
    } catch (error) {
      console.error("Error creating quiz:", error);
    }
  };
  return (
    <>
      <NavBar user={user} />
      <div className="container mt-4">
        <style>{'body { background-color: #1b1b1b; }'}</style>
        <h2 style={{ color: "white" }}>Create a New Quiz</h2>
        <form onSubmit={handleCreateQuiz}>
          {/* Title */}
          <div className="form-group">
            <label htmlFor="title" style={{ color: "white" }}>
              Title
            </label>
            <input
              type="text"
              className="form-control"
              id="title"
              name="title"
              value={quizData.title}
              onChange={handleInputChange}
              required
              data-testid="quiz-name-input"
            />
          </div>

          {/* Description */}
          <div className="form-group">
            <label htmlFor="description" style={{ color: "white" }}>
              Description
            </label>
            <textarea
              className="form-control"
              id="description"
              name="description"
              value={quizData.description}
              onChange={handleInputChange}
              required
              data-testid="quiz-description-input"
            />
          </div>

          {/* Category */}
          <div className="form-group">
            <label htmlFor="category" style={{ color: "white" }}>
              Category
            </label>
            <select
              className="form-control"
              id="category"
              name="category"
              value={quizData.category}
              onChange={handleInputChange}
              required
              data-testid="category-select"
            >
              <option value="" disabled>
                Select a category
              </option>
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.name}
                </option>
              ))}
            </select>
          </div>

          {/* Questions and Answers */}
          <div className="form-group mt-4">
          <h3 style={{ color: "white" }}>Questions</h3>
          {quizData.questions.map((question, qIndex) => (
            <div key={qIndex} className="mb-3">
              <label
                htmlFor={`question-${qIndex + 1}`}
                style={{ color: "white" }}
              >
                Question {qIndex + 1}
              </label>
              <textarea
                className="form-control"
                id={`question-${qIndex + 1}`}
                name={`question-${qIndex + 1}`}
                value={question.text}
                onChange={(e) => handleQuestionChange(qIndex, e)}
                required
                data-testid={`question-text-input-${qIndex}`}
              />
              <h4 className="mt-3" style={{ color: "white" }}>
                Answers
              </h4>
              {question.answers.map((answer, aIndex) => (
                <div key={aIndex} className="form-check">
                  <input
                    type="text"
                    className="form-control"
                    id={`answer-${qIndex + 1}-${aIndex + 1}`}
                    name={`answer-${qIndex + 1}-${aIndex + 1}`}
                    value={answer.text}
                    onChange={(e) => handleAnswerChange(qIndex, aIndex, e)}
                    required
                    data-testid={`answer-text-input-${qIndex}-${aIndex}`}
                  />
                  <input
                    type="checkbox"
                    className="form-check-input ml-2"
                    id={`correct-${qIndex + 1}-${aIndex + 1}`}
                    name={`correct-${qIndex + 1}-${aIndex + 1}`}
                    checked={answer.isCorrect}
                    onChange={(e) => handleAnswerChange(qIndex, aIndex, e)}
                    data-testid={`correct-answer-checkbox-${qIndex}-${aIndex}`}
                  />
                  <label
                    style={{ color: "white" }}
                    className="form-check-label ml-1"
                    htmlFor={`correct-${qIndex + 1}-${aIndex + 1}`}
                  >
                    Correct
                  </label>
                </div>
              ))}
              <button
                type="button"
                className="btn btn-secondary mt-2"
                onClick={() => handleAddAnswer(qIndex)}
                data-testid={`add-answer-button-${qIndex}`}
              >
                Add Answer
              </button>
            </div>
          ))}
          <button
            type="button"
            className="btn btn-secondary"
            onClick={handleAddQuestion}
            data-testid="add-question-button"
          >
            Add Question
          </button>
        </div>

        <div className="text-center">
          {/* Submit Button */}
          <button type="submit" data-testid="create-quiz-button" className="btn btn-primary mt-4 ">
            Create Quiz
          </button>
        </div> <br />
      </form>
      </div>
      <Footer />
    </>
  );
};

export default CreateQuiz;