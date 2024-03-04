import React, { useEffect} from 'react';
import { Link, useNavigate} from 'react-router-dom';
import Footer from './components/Footer';
import axios from 'axios';

export default function FirstPage() {

    const navigate = useNavigate();

    useEffect(() => {
      const checkJwtToken = async () => {
        try {
          const response = await axios.get('https://localhost:7267/api/User/CheckJwtToken', {
            withCredentials: true,
          });
  
          if (response.data.hasToken) {
            // Token is present, redirect to Home
            navigate('/Home');
          } else {
            // Token is not present, you can handle this case if needed
          }
        } catch (error) {
          console.error('Error checking JWT token:', error);
        }
      };
  
      checkJwtToken();
    }, [navigate]);


  return (
    <div>
      <style>{'body { background-color: #1b1b1b; margin: 0; padding: 0; }'}</style>
      <div className='text-center'>
        <h3 style={{ color: 'white', marginBottom: 0, justifyContent: 'space-between', padding: 10 }}>
          QuizzTurn <Link to='/login' style={{ float: 'right' }}>
            Sign in
          </Link>{' '}
        </h3>
      </div>
      <hr style={{ color: 'white' }} />
      <div className='grid-container'>
        <div className='grid-item grid-item-1'>
          <p className='text-center'>
            "QuizzTurn" is an engaging and interactive online platform designed to cater<br />
            to the quiz enthusiasts and knowledge seekers of all ages. With a commitment<br />
            to providing a fun and educational experience, QuizzTurn offers a diverse<br />
            range of quizzes and trivia on various topics, making it an excellent<br />
            destination for both leisure and learning.<br />
            <br />
          </p>
          <p className='text-center'>
            Feel free to design your unique quiz or take a shot at solving<br />
            those crafted by your peers. Join us today by signing up and<br />
            dive into the world of QuizzTurn.<br />
            <Link to='/signup' className='btn-link'>
              Sign up
            </Link>
          </p>
        </div>
      </div>
      <hr style={{ color: 'white' }} />
      <Footer />
    </div>
  );
}
