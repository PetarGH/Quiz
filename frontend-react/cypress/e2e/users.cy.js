/*let registrationDone = false;

//Create a mock user which is later updated and deleted
Cypress.Commands.add('registerMockUser', () => {
  if (!registrationDone) {
    cy.fixture('registerMockUser.json').then((mockUserData) => {
      cy.request({
        method: 'POST',
        url: 'https://localhost:7267/api/User/Register',
        body: mockUserData,
        headers: {
          'Content-Type': 'application/json',
        },
      });

      registrationDone = true;
    });
  }
});

Cypress.Commands.add('loginAndVisitUsersPage', () => {
    cy.intercept('GET', 'https://localhost:7267/api/User/GetUser', {
      fixture: 'mockUser.json', 
    }).as('getUser');
  
    cy.visit('http://localhost:3000/Home');
  
    cy.get('[data-testid=users-link]').click();
  
    cy.wait('@getUser');
  });

  describe('Users Page', () => {
    beforeEach(() => {
      cy.registerMockUser();
      cy.loginAndVisitUsersPage();
    });
  
    it('should navigate to the Users page', () => {
      cy.url().should('include', '/Users');
    });
  
    it('should display a list of users', () => {
      cy.get('[data-testid^=user-item-]').should('have.length.greaterThan', 0);
    });
  
    it('should update and delete a user', () => {
      cy.get('[data-testid^=edit-user-button]').last().click();
  
      cy.get('[data-testid=edit-user-name-input]').clear().type('UpdatedName');
      cy.get('.form-control[name="address"]').clear().type('UpdatedAddress');
      cy.get('.form-control[name="age"]').clear().type('30');
  
      cy.get('.btn-success').click();
  
      cy.get('.list-group-item').should('contain.text', 'UpdatedName');

      //Deleting
      cy.get('[data-testid^=delete-user-button]').last().click();

      cy.get('.list-group-item').should('not.contain.text', 'UpdatedName');
    });
  
  });*/