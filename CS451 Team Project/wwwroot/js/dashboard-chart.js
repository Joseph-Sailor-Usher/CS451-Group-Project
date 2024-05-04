// Function to create the budget tracking pie chart

// Function to create the actual amount spent line chart
function createSpendingLineChart() {

     var ctxL = document.getElementById('myLineChart').getContext('2d');
     window.myLineChart = new Chart(ctxL, {
          type: 'line',
          data: {
               labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
               datasets: [{
                    label: 'Actual Spending',
                    backgroundColor: 'rgba(255, 99, 132, 0.2)',
                    borderColor: 'rgb(255, 99, 132)',
                    data: [200, 300, 250, 400, 450, 500, 550, 600, 650, 700, 750, 800],
                    fill: true,
               }]
          },
          options: {
               scales: {
                    y: {
                         beginAtZero: true,
                         grid: {
                              color: getThemeColor()
                         },
                         ticks: {
                              color: getThemeColor()
                         },
                    },
                    x: {
                         grid: {
                              color: getThemeColor()
                         },
                         ticks: {
                              color: getThemeColor()
                         }
                    }
               },
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

// Helper function to get the theme color for chart labels and grid lines
function getThemeColor() {
     return localStorage.getItem('theme') === 'light' ? "black" : "white";
}

createSpendingLineChart();

function updateChartsForCurrentTheme() {

     if (window.myLineChart && typeof window.myLineChart.destroy === 'function') {
          window.myLineChart.destroy();
     }


     createSpendingLineChart(); // Recreates the line chart
}
