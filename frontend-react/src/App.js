import React, {useEffect, useState } from 'react';
import { checkTokenAndFetchUser  } from './components/User/CheckForToken';
import {BrowserRouter, Routes, Route, Navigate, useNavigate} from 'react-router-dom';
import 'bootstrap/dist/css/bootstrap.min.css';
import "react-toastify/dist/ReactToastify.css";
import FirstPage from './FirstPage';
import Login from './components/User/Login';
import Signup from './components/User/Signup';
import Home from './components/Home/Home';
import Profile from './components/User/profile';
import Quiz from './components/Quiz/Quiz';
import CreateQuiz from './components/Quiz/CreateQuiz';
import Users from './components/User/Users';
import CrudCategories from './components/Category/CrudCategories';
import SelectedQuiz from './components/Quiz/SelectedQuiz';
import MyQuizzes from './components/Quiz/MyQuizzes';
import EditQuiz from './components/Quiz/EditQuiz';

function App() {
  const navigate = useNavigate();
  const [user, setUser] = useState(null);

  useEffect(() => {
    const fetchUser = async () => {
      // Perform token check only if the user is not already logged in
      if (!user) {
        const fetchedUser = await checkTokenAndFetchUser();
        setUser(fetchedUser);

        if (fetchedUser) {
          navigate('/home');
        }
      }
    };
  
    fetchUser();
  }, [user, navigate]);

  return (
      <Routes>
        <Route index element={<FirstPage />}></Route>
        <Route path='/login' element={<Login />}></Route>
        <Route path='/signup' element={<Signup />}></Route>
        <Route path='/home' element={user ? <Home user={user}/> : <Navigate to='/' />}></Route>
        <Route path='/profile' element={user ? <Profile user={user}/> : <Navigate to='/' />}></Route>
        <Route path='/quiz' element={user ? <Quiz user={user}/> : <Navigate to='/' />}></Route>
        <Route path='/myquizzes' element={user ? <MyQuizzes user={user}/> : <Navigate to='/' />}></Route>
        <Route path='/createquiz' element={user ? <CreateQuiz user={user}/> : <Navigate to='/' />}></Route>
        <Route path='/users' element={user && user.userType === true ? <Users user={user}/> : <Navigate to='/' />}></Route>
        <Route path='/crudcategories' element={user && user.userType === true ? <CrudCategories user={user}/> : <Navigate to='/' />}></Route>
        <Route path='/SelectedQuiz/:quizId' element={user ? <SelectedQuiz user={user}/> : <Navigate to='/' />}></Route>
        <Route path='/EditQuiz/:quizId' element={user ? <EditQuiz user={user}/> : <Navigate to='/' />}></Route>
      </Routes>
  )
}

export default App;
