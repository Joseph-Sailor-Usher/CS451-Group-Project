// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var ctxP = document.getElementById('myPieChart').getContext('2d');
var myPieChart = new Chart(ctxP, {
     type: 'pie',
     data: {
          labels: ["Rent/Mortgage", "Utilities", "Groceries", "Misc", "Savings", "Investing"],
          datasets: [{
               data: [800, 150, 300, 150, 200, 100],
               backgroundColor: ["#F7464A", "#46BFBD", "#FDB45C", "#949FB1", "#4D5360", "#FFA07A"],
               borderColor: "white",
          }]
     },
     options: {
          plugins: {
               legend: {
                    labels: {
                         color: localStorage.getItem('theme') === 'light' ? "gray" : "white"
                    }
               }
          }
     }
});
var ctxL = document.getElementById('myLineChart').getContext('2d');
var myLineChart = new Chart(ctxL, {
     type: 'line',
     data: {
          labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
          datasets: [{
               label: 'Investment Account',
               backgroundColor: 'rgb(255, 99, 132)',
               borderColor: 'rgb(255, 99, 132)',
               data: [0, 10, 5, 2, 20, 30, 45, 60, 75, 100, 150, 200]
          }]
     },
     options: {
          scales: {
               y: {
                    grid: {
                         color: localStorage.getItem('theme') === 'light' ? "gray" : "white"
                    },
                    ticks: {
                         color: localStorage.getItem('theme') === 'light' ? "gray" : "white"
                    },
               },
               x: {
                    grid: {
                         color: localStorage.getItem('theme') === 'light' ? "gray" : "white"
                    },
                    ticks: {
                         color: localStorage.getItem('theme') === 'light' ? "gray" : "white"
                    }
               }

          },
          plugins: {
               legend: {
                    labels: {
                         color: localStorage.getItem('theme') === 'light' ? "gray" : "white"
                    }
               }
          }
     }
});