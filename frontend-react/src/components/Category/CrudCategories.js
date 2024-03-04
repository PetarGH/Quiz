import React, { useState, useEffect } from 'react';
import Navigation from '../Navigation';
import Footer from '../Footer';
import CategoryService from './CategoryService';

const CrudCategories = ({user}) => {
  const [categories, setCategories] = useState([]);
  const [editingCategory, setEditingCategory] = useState(null);
  const [filter, setFilter] = useState('');
  const [newCategoryName, setNewCategoryName] = useState('');
  const [selectedParentCategoryId, setSelectedParentCategoryId] = useState(null);

  useEffect(() => {
    loadCategories();
  }, []);

  const loadCategories = async () => {
    try {
      const response = await CategoryService.getAllCategories();
      setCategories(response.data);
    } catch (error) {
      console.error('Error loading categories:', error);
    }
  };

  const handleFilterChange = (e) => {
    setFilter(e.target.value);
  };

  const handleEditCategory = (category) => {
    setEditingCategory({ ...category });
  };

  const handleCancelEdit = () => {
    setEditingCategory(null);
  };

  const handleUpdateCategory = async () => {
    try {
      await CategoryService.updateCategory(editingCategory.id, {
        name: editingCategory.name,
      });
      setEditingCategory(null);
      loadCategories();
    } catch (error) {
      console.error('Error updating category:', error);
    }
  };

  const handleDeleteCategory = async (id) => {
    try {
      await CategoryService.deleteCategory(id);
      loadCategories();
    } catch (error) {
      console.error('Error deleting category:', error);
    }
  };

  const handleCreateCategory = async () => {
    try {
      await CategoryService.createCategory({
        name: newCategoryName,
        ParentId: selectedParentCategoryId,
      });
      setNewCategoryName('');
      setSelectedParentCategoryId(null);
      loadCategories();
    } catch (error) {
      console.error('Error creating category:', error);
    }
  };

  const filteredCategories = categories.filter((category) =>
    category.name.toLowerCase().includes(filter.toLowerCase())
  );

  return (
        <div>
      <Navigation user={(user)}/> <br />
      <div className="container">
        <div className="mb-3">
          <input
            type="text"
            placeholder="Filter categories by name"
            className="form-control"
            value={filter}
            onChange={handleFilterChange}
            data-testid="filter-input"
          />
        </div>

        <div>
          <h3 style={{color: 'white'}}>Categories List</h3>
          <ul className="list-group">
            {filteredCategories.map((category) => (
              <li key={category.id} className="list-group-item" data-testid={`category-item-${category.id}`}>
                {editingCategory && editingCategory.id === category.id ? (
                  <>
                    {/* Update category fields */}
                    <input type="text" data-testid="edit-category-name-input" style={{ marginLeft: '10px' }} className="form-control mb-2" name="name" value={editingCategory.name} onChange={(e) => setEditingCategory({ ...editingCategory, name: e.target.value })}/>
                    <button data-testid="update-category-button" style={{ marginLeft: '10px' }} className="btn btn-success mr-2" onClick={handleUpdateCategory}>
                      Update
                    </button>
                    <button data-testid="cancel-edit-button" style={{ marginLeft: '10px' }} className="btn btn-secondary" onClick={handleCancelEdit}>
                      Cancel
                    </button>
                  </>
                ) : (
                  <>
                    {/* Display category details */}
                    {category.name}
                    <button data-testid={`edit-category-button-${category.id}`} style={{ marginLeft: '10px' }} className="btn btn-primary ml-2" onClick={() => handleEditCategory(category)}>
                      Edit
                    </button>
                    <button data-testid={`delete-category-button-${category.id}`} style={{ marginLeft: '10px' }} className="btn btn-danger ml-2" onClick={() => handleDeleteCategory(category.id)}>
                      Delete
                    </button>
                  </>
                )}
              </li>
            ))}
          </ul>
          <br />
        </div>

        <div style={{color: 'white'}}>
          <h3>Create Category</h3>
          <div>
            <label>Name:</label>
            <input
              type="text"
              value={newCategoryName}
              onChange={(e) => setNewCategoryName(e.target.value)}
              className="form-control mb-2"
              data-testid="new-category-name"
            />
          </div>
          <div>
            <label>Parent Category:</label>
            <select
              value={selectedParentCategoryId || ''}
              onChange={(e) => setSelectedParentCategoryId(e.target.value || null)}
              className="form-control mb-2"
              data-testid="parent-category-select"
            >
              <option value="">Select Parent Category</option>
              {categories.map((category) => (
                <option key={category.id} value={category.id}>
                  {category.name}
                </option>
              ))}
            </select>
          </div>
          <button data-testid="create-category-button" className="btn btn-success" onClick={handleCreateCategory}>
            Create Category
          </button>
        </div>
      </div>
      <hr />
      <Footer />
    </div>
  );
};

export default CrudCategories;