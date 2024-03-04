import React, { useState, useEffect } from 'react';
import axios from 'axios';
import Navigation from '../Navigation';
import Footer from '../Footer';
import 'bootstrap/dist/css/bootstrap.min.css';

const Users = ({user}) => {
  const [users, setUsers] = useState([]);
  const [editingUser, setEditingUser] = useState(null);
  const [filter, setFilter] = useState('');

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      const response = await axios.get('https://localhost:7267/api/User');
      setUsers(response.data);
    } catch (error) {
      console.error('Error fetching users:', error);
    }
  };

  const handleEditUser = (user) => {

    const age = user.age !== null ? parseInt(user.age, 10) : null;
    setEditingUser({ ...user, age });
  };

  const handleUpdateUser = async () => {
    try {
      // Basic validation
      if (!editingUser.name || !editingUser.address || !editingUser.age) {
        alert('All fields are required.');
        return;
      }

      // Additional validation for age
      const ageNumber = parseInt(editingUser.age, 10);
      if (isNaN(ageNumber) || ageNumber <= 0) {
        alert('Age must be a valid positive number.');
        return;
      }
      
      // Additional validation to prevent sending JSON strings
      if (/[{}[\]:,",]/.test(editingUser.age)) {
        alert('Invalid characters in the age field.');
        return;
      }

      await axios.put(`https://localhost:7267/api/User/Update/${editingUser.id}`, editingUser);
      fetchUsers();
      setEditingUser(null);
    } catch (error) {
      console.error('Error updating user:', error);
    }
  };

  const handleDeleteUser = async (userId) => {
    try {
      await axios.delete(`https://localhost:7267/api/User/Delete/${userId}`);
      fetchUsers();
    } catch (error) {
      console.error('Error deleting user:', error);
    }
  };

  const handleToggleFreeze = async (userId) => {
    try {
      // Find the user by ID
      const updatedUsers = users.map((user) =>
        user.id === userId ? { ...user, isFrozen: !user.isFrozen } : user
      );
      setUsers(updatedUsers);
      await axios.put(`https://localhost:7267/api/User/Freeze/${userId}`);
    } catch (error) {
      console.error('Error toggling user freeze status:', error);
    }
  };

  const handleCancelEdit = () => {
    setEditingUser(null);
  };

  const handleFilterChange = (e) => {
    setFilter(e.target.value);
  };

  const filteredUsers = users.filter((user) =>
    user.name.toLowerCase().includes(filter.toLowerCase())
  );

  return (
    <div>
      <Navigation user={(user)}/> <br />

      <div className="container">
        <div className="mb-3">
          <input
            type="text"
            placeholder="Filter users by name"
            className="form-control"
            value={filter}
            onChange={handleFilterChange}
            data-testid="filter-input"
          />
        </div>

        <div>
          <h3 style={{color: 'white'}}>Users List</h3>
          <ul className="list-group">
            {filteredUsers.map((user) => (
              <li key={user.id} className="list-group-item" data-testid={`user-item-${user.id}`}>
                {editingUser && editingUser.id === user.id ? (
                  <>
                    {/* Update user fields */}
                    <input
                      type="text"
                      className="form-control mb-2"
                      name="name"
                      value={editingUser.name}
                      onChange={(e) => setEditingUser({ ...editingUser, name: e.target.value })}
                      data-testid="edit-user-name-input"
                    />
                    <input
                      type="text"
                      className="form-control mb-2"
                      name="address"
                      value={editingUser.address}
                      onChange={(e) => setEditingUser({ ...editingUser, address: e.target.value })}
                    />
                    <input
                      type="number"
                      className="form-control mb-2"
                      name="age"
                      value={editingUser.age || ''}
                      onChange={(e) =>
                        setEditingUser({ ...editingUser, age: parseInt(e.target.value, 10) })
                      }
                    />
                    <button className="btn btn-success mr-2" onClick={handleUpdateUser}>
                      Update
                    </button>
                    <button style={{ marginLeft: '10px' }} className="btn btn-secondary" onClick={handleCancelEdit}>
                      Cancel
                    </button>
                  </>
                ) : (
                  <>
                    {/* Display user details */}
                    {user.name} - {user.email}
                    <button data-testid={`edit-user-button-${user.id}`} style={{marginLeft: '8px'}} className="btn btn-primary" onClick={() => handleEditUser(user)}>
                      Edit
                    </button>
                    <button data-testid={`delete-user-button-${user.id}`} style={{marginLeft: '10px'}} className="btn btn-danger ml-2" onClick={() => handleDeleteUser(user.id)}>
                      Delete
                    </button>
                    <button style={{marginLeft: '10px'}} className={`btn ${user.isFrozen ? 'btn-warning' : 'btn-danger'}`} onClick={() => handleToggleFreeze(user.id)}> 
                    {user.isFrozen ? 'Unfreeze' : 'Freeze'}
                  </button>
                  </>
                )}
              </li>
            ))}
          </ul>
          <br />
        </div>
      </div>
      <hr />
      <Footer />
    </div>
  );
};

export default Users;