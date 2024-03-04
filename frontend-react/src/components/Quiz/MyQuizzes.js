import React, { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import NavBar from "../Navigation";
import axios from "axios";
import "../../Style/MyQuiz.css"
import Footer from "../Footer";


const MyQuizzes = ({ user }) => {
  const [userQuizzes, setUserQuizzes] = useState([]);

  useEffect(() => {
    fetchUserQuizzes();
  }, [user.id]);

  const fetchUserQuizzes = async () => {
    try {
      const response = await axios.get(`https://localhost:7267/api/Quiz/GetUserQuizzes/${user.id}`);
      const quizzesWithCategories = await Promise.all(
        response.data.map(async (quiz) => {
          const category = await fetchQuizCategory(quiz.categoryId);
          return { ...quiz, categoryName: category.name };
        })
      );
      setUserQuizzes(quizzesWithCategories);
    } catch (error) {
      console.error("Error fetching quizzes:", error);
    }
  };

  const fetchQuizCategory = async (categoryId) => {
    try {
      const response = await axios.get(`https://localhost:7267/api/Category/${categoryId}`);
      return response.data; // Change from response.data.value to response.data
    } catch (error) {
      console.error(`Error fetching category with ID ${categoryId}:`, error);
      return { name: "Unknown Category" };
    }
  };

  const handleDeleteQuiz = async (quizId) => {
    try {
      await axios.delete(`https://localhost:7267/api/Quiz/DeleteQuiz/${quizId}`);
      // Refresh the quizzes after deletion
      fetchUserQuizzes();
    } catch (error) {
      console.error(`Error deleting quiz with ID ${quizId}:`, error);
    }
  };
  

  return (
    <>
      <NavBar user={user} />
      <div className="container mt-4">
      <style>{'body { background-color: #1b1b1b; }'}</style>
        <h2 style={{color: "white"}}>My Quizzes</h2>
        {userQuizzes.length === 0 ? (
          <p style={{color: "white"}}>No quizzes found for this user.</p>
        ) : (
          <ul className="list-group">
            {userQuizzes.map((quiz) => (
              <li key={quiz.id} className="list-group-item">
                <h4>{quiz.title}</h4>
                <p>{quiz.description}</p>
                <p style={{position: "absolute", top: 0, right: 15}}>Category: {quiz.categoryName}</p>
                <Link data-testid={`edit-quiz-button-${quiz.id}`} to={`/EditQuiz/${quiz.id}`} className="btn btn-primary">
                  Edit
                </Link>
                <Link style={{ marginLeft: '10px' }} to={`/SelectedQuiz/${quiz.id}`} className="btn btn-success ml-2">
                  Test
                </Link>
                <button data-testid={`delete-quiz-button-${quiz.id}`} style={{position: "absolute", right: 15 }} className="btn btn-danger ml-2" onClick={() => handleDeleteQuiz(quiz.id)}>
                  Delete
                </button>
              </li>
            ))}
          </ul>
        )}
      </div>
      <hr style={{color: 'white'}}/>
      <Footer />
    </>
  );
};

export default MyQuizzes;