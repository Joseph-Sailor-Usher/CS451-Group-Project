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

// Dynamic pie chart
// need to link to backend 
function createDynamicPieChart() {
     var dynamicChart = document.getElementById('dynamicRatioPieChart').getContext('2d');

     window.dynamicRatioPieChart = new Chart(dynamicChart, {
          type: 'pie',
          data: {
               labels: ["Needs", "Wants", "Savings"],
               datasets: [{
                    data: [50, 30, 20],
                    backgroundColor: ["#F7464A", "#46BFBD", "#FDB45C"],
                    hoverOffset: 4
               }]
          },
          options: {
               plugins: {
                    legend: {
                         labels: {
                              color: getThemeColor()
                         }
                    }
               }
          }
     });
}

document.addEventListener('DOMContentLoaded', function () {
     createDynamicPieChart();
     createStaticPieChart(); // Recreates the pie chart
});

//Static pie chart for financial tips 
function createStaticPieChart() {
     var staticChart = document.getElementById('staticRatioPieChart').getContext('2d');

     window.staticRatioPieChart = new Chart(staticChart, {
          type: 'pie',
          data: {
               labels: ["Needs", "Wants", "Savings"],
               datasets: [{
                    data: [50, 30, 20],
                    backgroundColor: ["#F7464A", "#46BFBD", "#FDB45C"],
                    hoverOffset: 4
               }]
          },
          options: {
               plugins: {
                    legend: {
                         labels: {
                              color: getThemeColor()
                         }
                    }
               }
          }
     });
}
document.addEventListener('DOMContentLoaded', function () {
     createStaticPieChart(); // Recreates the pie chart
});

function updateChartsForCurrentTheme() {
     if (window.staticRatioPieChart && typeof window.staticRatioPieChart.destroy === 'function') {
          window.staticRatioPieChart.destroy();
     }

     createDynamicPieChart();
}

// Helper function to get the theme color for chart labels and grid lines
function getThemeColor() {
     return localStorage.getItem('theme') === 'light' ? "black" : "white";
}

function updateChartsForCurrentTheme() {
     if (window.staticRatioPieChart && typeof window.staticRatioPieChart.destroy === 'function') {
          window.staticRatioPieChart.destroy();
     }
     if (window.dynamicRatioPieChart && typeof window.dynamicRatioPieChart.destroy === 'function') {
          window.dynamicRatioPieChart.destroy();
     }

     createStaticPieChart(); // Recreates the pie chart
     createDynamicPieChart();
}