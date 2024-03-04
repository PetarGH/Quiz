import axios from 'axios';

export const checkTokenAndFetchUser = async () => {
  try {
    const response = await axios.get('https://localhost:7267/api/User/GetUser', {
      withCredentials: true,
    });

    return response.data.value;
  } catch (error) {
    console.error('Error fetching user:', error);
    return null;
  }
};