import axios from 'axios';

const API_URL = 'https://localhost:7267/api/Category'; // Update with your API URL

const CategoryService = {
  getAllCategories: () => axios.get(`${API_URL}`),
  getCategoryById: (id) => axios.get(`${API_URL}/${id}`),
  createCategory: (categoryData) => axios.post(`${API_URL}/CreateCategory`, categoryData),
  updateCategory: (id, categoryData) => axios.put(`${API_URL}/Update/${id}`, categoryData),
  deleteCategory: (id) => axios.delete(`${API_URL}/DeleteCategory/${id}`),
};

export default CategoryService;