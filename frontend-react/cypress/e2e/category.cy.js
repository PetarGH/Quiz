/*Cypress.Commands.add('loginAndVisitCategoryPage', () => {
    cy.intercept('GET', 'https://localhost:7267/api/User/GetUser', {
      fixture: 'mockUser.json',
    }).as('getUser');
  
    cy.visit('http://localhost:3000/Home');
  
    cy.get('[data-testid=category-link]').click();
  
    cy.wait('@getUser');
  });

  describe('Category Page', () => {
    beforeEach(() => {
      cy.loginAndVisitCategoryPage();
    });

  it('should display a list of categories', () => {
    cy.get('[data-testid^=category-item-]').should('have.length.greaterThan', 0);
  });

  it('should create, update, and delete a category', () => {
    const newCategoryName = 'Test Category';
    const updatedCategoryName = 'Updated Test Category';

    // Create a new category
    cy.get('[data-testid=new-category-name]').type(newCategoryName);
    cy.get('[data-testid=create-category-button]').click();
    cy.get('.list-group-item').should('contain.text', newCategoryName);

    // Update the newly added category
    cy.get('[data-testid^=edit-category-button-]').last().click();
    cy.get('[data-testid=edit-category-name-input]').clear().type(updatedCategoryName);
    cy.get('[data-testid=update-category-button]').click();
    cy.get('.list-group-item').should('contain.text', updatedCategoryName);

    // Delete the updated category
    cy.get('[data-testid^=delete-category-button-]').last().click();
  });
});*/