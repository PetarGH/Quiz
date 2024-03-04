import React, { useEffect, useState, useRef } from "react";
import Navigation from "../Navigation";
import Footer from "../Footer";
import * as signalR from "@microsoft/signalr";
import FunFacts from "./FunFacts";
import "../../Style/Quiz.css";
import axios from "axios";
import { Link } from "react-router-dom";

const Home = ({ user }) => {
  const [messages, setMessages] = useState([]);
  const [inputMessage, setInputMessage] = useState("");
  const [messageCount, setMessageCount] = useState(0);
  const [isChatDisabled, setIsChatDisabled] = useState(false);
  const intervalRef = useRef(null);
  const [newestQuiz, setNewestQuiz] = useState(null);

  const hubConnection = useRef(
    new signalR.HubConnectionBuilder().withUrl("https://localhost:7267/chatHub").build()
  );

  const fetchNewestQuiz = async () => {
    try {
      const response = await axios.get("https://localhost:7267/api/Quiz/GetNewestQuiz");
      const quiz = response.data;

      const category = await fetchQuizCategory(quiz.categoryId);

      const quizWithCategory = { ...quiz, categoryName: category.name };

      setNewestQuiz(quizWithCategory);
    } catch (error) {
      console.error("Error fetching newest quiz:", error);
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

  useEffect(() => {
    const startHubConnection = async () => {
      try {
        fetchNewestQuiz();
        if (hubConnection.current.state === signalR.HubConnectionState.Disconnected) {
          await hubConnection.current.start();
          console.log("Connected to SignalR hub");

          hubConnection.current.on("ReceiveMessage", (userName, message) => {
            const newMessage = `${userName}: ${message}`;
            setMessages((prevMessages) => [...prevMessages, newMessage]);
          });
        }
      } catch (error) {
        console.error("Error connecting to SignalR hub:", error);
      }
    };

    startHubConnection();

    return () => {
      if (
        hubConnection.current &&
        hubConnection.current.state === signalR.HubConnectionState.Connected
      ) {
        hubConnection.current.stop();
      }

      clearInterval(intervalRef.current);
    };
  }, []);

  const handleInputChange = (event) => {
    setInputMessage(event.target.value);
  };

  const sendMessage = async () => {
    if (isChatDisabled) {
      return;
    }

    if (inputMessage.trim() !== "") {
      try {
        await hubConnection.current.invoke("SendMessage", user.name, inputMessage);
        setInputMessage("");
        setMessageCount(messageCount + 1);

        if (messageCount >= 4) {
          setIsChatDisabled(true);
          setTimeout(() => {
            setIsChatDisabled(false);
            setMessageCount(0);
          }, 10000);
        }
      } catch (error) {
        console.error("Error sending message:", error);
      }
    }
  };

  useEffect(() => {
    intervalRef.current = setInterval(() => {
      setMessageCount(0);
    }, 7500);

    return () => {
      clearInterval(intervalRef.current);
    };
  }, []);

  return (
    <div style={{ display: "flex", flexDirection: "column", height: "100vh" }}>
      <style>
        {`
          body {
            background-color: #1b1b1b;
            margin: 0;
            overflow: hidden;
          }
          .content-container {
            display: flex;
            flex: 1;
            padding: 20px;
            marginBottom: 50px;
            overflow: hidden;
          }
          .chat-box {
            background-color: white;
            border-radius: 10px;
            padding: 10px;
            width: 300px;
            max-height: 70vh;
            overflow-y: scroll;
            margin-right: 20px;
          }
          .input-container {
            width: 300px;
            margin-left: 20px;
            margin-right: 20px;
            display: flex;
            flex-direction: column;
          }
          input {
            width: 100%;
            padding: 8px;
            margin-bottom: 5px;
          }
          button {
            width: 100%;
            padding: 8px;
          }
        `}
      </style>
      <Navigation user={user} />
      <br />
      <h3 style={{ color: "white", textAlign: "center" }}>
        Welcome to QuizzTurn {user.name}.
      </h3>
      <div className="content-container">
        <div className="chat-box">
          {messages.map((message, index) => (
            <div key={index} style={{ marginBottom: "10px" }}>
              {message}
            </div>
          ))}
        </div>
        <div style={{ marginLeft: "15%", marginTop: "3%"}}>
          <FunFacts />
        </div>
        <div className="container mt-4">
          <h3 style={{ marginLeft: "20%", color: "white"}}>Newest quiz</h3>
          <ul className="list-group" style={{ marginLeft: "20%"}}>
            {newestQuiz && (
              <li key={newestQuiz.id} className="list-group-item quiz-item">
                <h4 className="mb-2">{newestQuiz.title}</h4>
                <p>{newestQuiz.description}</p>
                <p style={{ position: "absolute", top: 0, right: 15 }}>
                  Category: {newestQuiz.categoryName}
                </p><br/>
                <Link to={`/SelectedQuiz/${newestQuiz.id}`}>
                  <button className="btn btn-primary" style={{width: "90px"}}>Select</button>
                </Link>
              </li>
            )}
          </ul>
        </div>
      </div>
      <div className="input-container">
        <input
          type="text"
          value={inputMessage}
          onChange={handleInputChange}
          onKeyPress={(event) => {
            if (event.key === "Enter") {
              sendMessage();
            }
          }}
          placeholder={isChatDisabled ? "Chat disabled" : "Type your message..."}
          disabled={isChatDisabled}
        />
        <button onClick={sendMessage} disabled={isChatDisabled}>
          Send
        </button>
      </div>
      <Footer />
    </div>
  );
};

export default Home;
