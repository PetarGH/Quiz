describe('Login Page', () => {
  beforeEach(() => {
    cy.visit('/login');
  });

  it('should display the login form', () => {
    // Log some debugging information
    cy.log('Checking if the login form is visible');
    
    // Use cy.get with a longer timeout for debugging purposes
    cy.get('[data-testid=login-form]', { timeout: 10000 }).should('be.visible');
  });

  it('should show an error for invalid login', () => {
    cy.get('[data-testid=email]').type('invalid@example.com');
    cy.get('[data-testid=password]').type('invalidpassword');
    cy.get('[data-testid=login-button]').click();
  
    cy.get('[data-testid=error-message]').should('be.visible').and('contain', 'Invalid email or password');
  });

  it('should navigate to the home page after successful login', () => {
    cy.get('[data-testid=email]').type('petar_dakov03@abv.bg');
    cy.get('[data-testid=password]').type('123');
    cy.get('[data-testid=login-button]').click();

    cy.url().should('include', '/Home'); 
  });
});