/*Cypress.Commands.add('loginAndVisitQuizPage', () => {
  cy.intercept('GET', 'https://localhost:7267/api/User/GetUser', {
    fixture: 'mockUser.json',
  }).as('getUser');

  cy.visit('http://localhost:3000/Home');

  cy.get('[data-testid=createQuiz-link]').click();

  cy.wait('@getUser');
});

describe('Create Quiz', () => {
  beforeEach(() => {
    cy.loginAndVisitQuizPage();
  });

  const updatedQuizName = 'Updated Quiz Name';
  const quizName = 'Test Quiz';
  const quizDescription = 'This is a test quiz.';
  const categoryName = 'Test';
  const questions = [
    {
      questionText: 'What is the capital of France?',
      answers: ['Paris', 'Berlin', 'Madrid', 'Rome'],
      correctAnswerIndex: 0,
    },
    {
      questionText: 'What is the largest mammal?',
      answers: ['Elephant', 'Blue Whale', 'Giraffe', 'Hippopotamus'],
      correctAnswerIndex: 1,
    },
  ];

  it('should create a quiz with questions and answers', () => {
    // Fill in Quiz Details
    cy.get('[data-testid=quiz-name-input]').type(quizName);
    cy.get('[data-testid=quiz-description-input]').type(quizDescription);

    // Select Category
    cy.get('[data-testid=category-select]').select(categoryName);

    // Add Questions and Answers
    questions.forEach((question, questionIndex) => {
      // Add a question only after the first iteration
      if (questionIndex > 0) {
        cy.get('[data-testid=add-question-button]').click();
      }

      // Fill in Question Text
      cy.get(`[data-testid=question-text-input-${questionIndex}]`).type(
        question.questionText
      );

      // Fill in Answers
      question.answers.forEach((answer, answerIndex) => {
        cy.get(`[data-testid=answer-text-input-${questionIndex}-${answerIndex}]`).type(
          answer
        );

        // Check the correct answer
        if (answerIndex === question.correctAnswerIndex) {
          cy.get(`[data-testid=correct-answer-checkbox-${questionIndex}-${answerIndex}]`).check();
        }

        // Add more answers if needed
        if (answerIndex < question.answers.length - 1) {
          cy.get(`[data-testid=add-answer-button-${questionIndex}]`).click();
        }
      });
    });

    // Submit Quiz
    cy.get('[data-testid=create-quiz-button]').click();

    // Assert Quiz Creation
    cy.url().should('include', '/MyQuizzes');

    cy.get('[data-testid^=edit-quiz-button]').last().click();

    // Update Quiz Title
    cy.get('[data-testid=quiz-title-input]').clear().type(updatedQuizName);

    // Click on Update
    cy.get('[data-testid=update-quiz-button]').click();

    // Assert Quiz Update
    cy.url().should('include', '/MyQuizzes');

    // Click on Delete
    cy.get('[data-testid^=delete-quiz-button]').last().click();
  });
});*/