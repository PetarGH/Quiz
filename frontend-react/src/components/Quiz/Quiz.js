// Import statements
import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import NavBar from "../Navigation";
import Footer from "../Footer";
import axios from "axios";
import "../../Style/Quiz.css";

const Quiz = ({ user }) => {
  const [quizzes, setQuizzes] = useState([]);
  const [filter, setFilter] = useState("");

  useEffect(() => {
    fetchQuizzes();
  }, []);

  const fetchQuizzes = async () => {
    try {
      const response = await axios.get("https://localhost:7267/api/Quiz");
      const quizzesWithCategories = await Promise.all(
        response.data.map(async (quiz) => {
          const category = await fetchQuizCategory(quiz.categoryId);
          return { ...quiz, categoryName: category.name };
        })
      );
      setQuizzes(quizzesWithCategories);
    } catch (error) {
      console.error("Error fetching quizzes:", error);
    }
  };

  const fetchQuizCategory = async (categoryId) => {
    try {
      const response = await axios.get(`https://localhost:7267/api/Category/${categoryId}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching category with ID ${categoryId}:`, error);
      return { name: "Unknown Category" };
    }
  };

  const handleFilterChange = (e) => {
    setFilter(e.target.value);
  };

  const handleDeleteQuiz = async (quizId) => {
    try {
      await axios.delete(`https://localhost:7267/api/Quiz/DeleteQuiz/${quizId}`);
      fetchQuizzes(); 
    } catch (error) {
      console.error(`Error deleting quiz with ID ${quizId}:`, error);
    }
  };

  const filteredQuizzes = quizzes.filter((quiz) =>
    quiz.title.toLowerCase().includes(filter.toLowerCase())
  );

  return (
    <>
      <NavBar user={user} />
      <div className="container mt-4">
        <style>{'body { background-color: #1b1b1b; }'}</style>
        <h2 className="mb-4" style={{ color: "white" }}>
          All Quizzes
        </h2>
        <div className="mb-3">
          <input
            type="text"
            placeholder="Filter quizzes by title"
            className="form-control"
            value={filter}
            onChange={handleFilterChange}
          />
        </div>
        <ul className="list-group">
          {filteredQuizzes.map((quiz) => (
            <li key={quiz.id} className="list-group-item quiz-item">
              {user.userType && (
                <div className="button-group">
                  <Link to={`/EditQuiz/${quiz.id}`}>
                    <button data-testid="editquiz-link" style={{ position: 'absolute', bottom: 20, right: 100 }} className="btn btn-warning mr-2">
                      Edit
                    </button>
                  </Link>
                  <button
                    style={{ position: 'absolute', marginLeft: '60px', bottom: 20, right: 10 }}
                    className="btn btn-danger"
                    onClick={() => handleDeleteQuiz(quiz.id)}
                  >
                    Delete
                  </button>
                </div>
              )}
              <h4 className="mb-2">{quiz.title}</h4>
              <p>{quiz.description}</p>
              <p style={{ position: "absolute", top: 0, right: 15 }}>
                Category: {quiz.categoryName}
              </p>
              <Link to={`/SelectedQuiz/${quiz.id}`}>
                <button className="btn btn-primary">Select</button>
              </Link>
            </li>
          ))}
        </ul>
      </div>
      <hr style={{ color: "white" }} />
      <Footer />
    </>
  );
};

export default Quiz;
