import React,{} from "react";
import { Navbar, Nav } from "react-bootstrap";
import profilePic from "../Pictures/profile.png";
import logoutPic from "../Pictures/logout.png";
import { Link, useNavigate } from "react-router-dom";
import "../style.css";
import axios from "axios";

const Navigation = ({user}) => {

    const navigate = useNavigate();


    const logout = async () => {
        try {
            await axios.post('https://localhost:7267/api/User/Logout', {}, {
              withCredentials: true,
            });
            
            navigate("/");

          } catch (error) {
            console.log('Error:', error);
          }
    };


    return(
        <Navbar className="navbar" bg="dark" expand="lg">
            <Navbar.Toggle aria-controls="basic-navbar-nav"/>
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="rd">
                        <Link className="a title d-inline p-2 bg-dark text-white" to="/Home">
                            QuizzTurn
                        </Link>
                        <Link data-testid="quiz-link" className="a d-inline p-2 bg-dark text-white" to="/Quiz">
                            Quiz
                        </Link>
                        <Link className="a d-inline p-2 bg-dark text-white" to="/MyQuizzes">
                            MyQuizzes
                        </Link>
                        <Link data-testid="createQuiz-link" className="a d-inline p-2 bg-dark text-white" to="/CreateQuiz">
                            CreateQuiz
                        </Link>
                        {user.userType === true && (
                            <Link data-testid="users-link" className="a d-inline p-2 bg-dark text-white" to="/Users">
                                Users
                            </Link>
                        )} {user.userType === true && ( 
                            <Link data-testid="category-link" className="a d-inline p-2 bg-dark text-white" to="/CrudCategories">
                                Categories
                            </Link>
                        )}
                        <Link className="a d-inline p-2 bg-dark text-white" to="/profile">
                            <img src={profilePic} title="profile" width={25} height={25} />
                        </Link>
                        <Link className="a d-inline p-2 bg-dark text-white" to="/" onClick={logout}>
                            <img src={logoutPic} title="logout" width={25} height={25} style={{float: "right"}}/>
                        </Link>
                    </Nav>
                </Navbar.Collapse>
        </Navbar>
    )
}

export default Navigation;