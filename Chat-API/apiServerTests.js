const chai = require('chai');
const chaiHttp = require('chai-http');
const { expect } = chai;

// HTTP requests
chai.use(chaiHttp);

const serverUrl = 'http://localhost:3000';

describe('API Tests', () => {
  // Test for successful response from the /api/get-answer endpoint
  it('should receive a valid response for a proper prompt', (done) => {
    chai.request(serverUrl)
      .post('/api/get-answer')
      .send({ prompt: 'What is the interest rate for a savings account?' })
      .end((err, res) => {
        expect(res).to.have.status(200);
        expect(res.body).to.have.property('response');
        expect(res.body.response).to.be.a('string');
        done();
      });
  });

  // Test to check handling of a prompt that is too short
  it('should return an error for a prompt that is too short', (done) => {
    chai.request(serverUrl)
      .post('/api/get-answer')
      .send({ prompt: 'Rate?' })
      .end((err, res) => {
        expect(res).to.have.status(200);
        expect(res.body.response).to.equal('Prompt must be at least 10 characters long');
        done();
      });
  });

  // Test to check handling of inappropriate language
  it('should return an error for prompts containing inappropriate language', (done) => {
    chai.request(serverUrl)
      .post('/api/get-answer')
      .send({ prompt: 'How is the damn interest rate?' })
      .end((err, res) => {
        expect(res).to.have.status(200);
        expect(res.body.response).to.equal('Prompt contains inappropriate language');
        done();
      });
  });
});

