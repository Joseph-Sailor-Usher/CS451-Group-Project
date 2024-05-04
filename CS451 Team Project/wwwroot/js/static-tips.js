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