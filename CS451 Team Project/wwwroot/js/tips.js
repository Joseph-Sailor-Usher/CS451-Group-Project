document.addEventListener("DOMContentLoaded", function () {
     const questions = [
          "How can I improve my credit score?",
          "How can I effectively pay off my debt?",
          "How should I plan for large expenditures?",
          "What should my emergency fund look like?",
          "How can I optimize my budget for long-term financial success?",
          "What steps should I take to start investing if I haven't already?",
          "How can I protect myself from identity theft and financial fraud?",
          "Should I consider refinancing my loans or mortgages?",
          "What are the potential risks and rewards of investing in cryptocurrencies or other alternative assets?",
          "What strategies can I use to negotiate better terms on my loans, credit cards, or other financial products?"
     ];

     questions.forEach((question, index) => {
          fetch('http://localhost:3000/api/get-answer', {
               method: 'POST',
               headers: {
                    'Content-Type': 'application/json'
               },
               body: JSON.stringify({ prompt: question })
          })
               .then(response => response.json())
               .then(data => {
                    const isActive = index === 0 ? ' active' : '';
                    const itemHTML = `<div class="carousel-inner"><div class="carousel-item"><div class="financial_title">${question}</div><div class="financial_description">${data.response}</div></div>`;
                    document.getElementById('financialTipsContainer').innerHTML += itemHTML;
               })
               .catch(error => {
                    console.error('Error:', error);
               });
     });
});


//<div class="carousel-inner">
//     <div class="carousel-item active">
//          <div class="financial_title">${question}</div>
//          <div class="financial_description">${data.tip}</div>
//     </div>
//</div>