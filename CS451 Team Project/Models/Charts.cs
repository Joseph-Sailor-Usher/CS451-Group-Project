using CS451_Team_Project.Models;
using CS451_Team_Project.Pages;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web.UI.DataVisualization.Charting;

namespace CS451_Team_Project.Models
{
    public class Charts
    {
        private readonly ILogger<NewLoginModel> _logger;
        public Charts(ILogger<NewLoginModel> logger)
        {
            _logger = logger;
        }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public async Task GenerateBudgetPieChartAsync([FromServices] AppDbContext db, string email)
        {

            List<string> allowedCategories = new List<string>
            {
                "Utilities",
                "Groceries",
                "Misc",
                "Savings",
                "Investing",
                "Rent",
                "Bills",
                "Transportation",
                "Insurance",
                "Entertainment",
                "Other"
            };
            // Create a new chart
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();

            // Set chart properties
            chart.Width = 700;
            chart.Height = 750;
            chart.BackColor = System.Drawing.Color.Transparent; // Set background color to transparent

            // Create chart areas
            System.Web.UI.DataVisualization.Charting.ChartArea chartArea = new System.Web.UI.DataVisualization.Charting.ChartArea();
            chartArea.BackColor = System.Drawing.Color.Transparent; // Set background color to transparent
            chartArea.BackSecondaryColor = System.Drawing.Color.Transparent; // Set secondary background color to transparent
            chartArea.BackGradientStyle = GradientStyle.None; // Set gradient style to none
            chart.ChartAreas.Add(chartArea);
            Transactions = await db.Transactions
                .Where(t => t.UserEmail == email)
                .ToListAsync();

            // Create color map for categories
            Dictionary<string, System.Drawing.Color> categoryColors = new Dictionary<string, System.Drawing.Color>
            {
                { "Utilities", System.Drawing.Color.Purple },
                { "Groceries", System.Drawing.Color.Pink },
                { "Misc", System.Drawing.Color.LightBlue },
                { "Savings", System.Drawing.Color.LightGreen },
                { "Investing", System.Drawing.Color.Violet },
                { "Rent", System.Drawing.Color.LightPink },
                { "Bills", System.Drawing.Color.LightGray },
                { "Transportation", System.Drawing.Color.DarkRed },
                { "Insurance", System.Drawing.Color.DarkBlue },
                { "Entertainment", System.Drawing.Color.DarkGreen },
                { "Other", System.Drawing.Color.DarkOrange },

            };


            // Aggregate transaction amounts for each category
            Dictionary<string, decimal> categoryTotals = new Dictionary<string, decimal>();
            foreach (var transaction in Transactions)
            {
                if (allowedCategories.Contains(transaction.Category))
                {
                    if (!categoryTotals.ContainsKey(transaction.Category))
                    {
                        categoryTotals[transaction.Category] = 0m;
                    }
                    categoryTotals[transaction.Category] += transaction.Amount;
                }
            }

            // Add legend to the chart
            System.Web.UI.DataVisualization.Charting.Legend legend = new System.Web.UI.DataVisualization.Charting.Legend();
            legend.Docking = System.Web.UI.DataVisualization.Charting.Docking.Top; // Position the legend above the chart
            chart.Legends.Add(legend);
            legend.BackColor = System.Drawing.Color.Transparent;
            legend.ForeColor = System.Drawing.Color.White;

            // Adjust the size of the legend
            legend.Font = new System.Drawing.Font("Aharoni", 18); // Set the font size (adjust as needed)

            // Add series to the chart
            System.Web.UI.DataVisualization.Charting.Series series = new System.Web.UI.DataVisualization.Charting.Series();
            series.ChartType = SeriesChartType.Pie;
            series.BorderWidth = 6; // Set border width
            series.BorderColor = System.Drawing.Color.White; // Set border color
            series.ChartType = SeriesChartType.Pie;
            // Add legend items for each allowed category
            foreach (var categoryTotal in categoryTotals)
            {
                string category = categoryTotal.Key;
                decimal totalAmount = categoryTotal.Value;
                if (categoryColors.ContainsKey(category))
                {
                    // Create a legend item for the category
                    LegendItem legendItem = new LegendItem();
                    legendItem.Name = category; // Category name
                    legendItem.Color = categoryColors[category]; // Category color
                    legendItem.ImageStyle = System.Web.UI.DataVisualization.Charting.LegendImageStyle.Marker; // Display as marker
                    legendItem.MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Square; // Marker style
                    legendItem.MarkerSize = 10; // Marker size
                    legendItem.MarkerBorderColor = System.Drawing.Color.Black; // Marker border color
                    legendItem.MarkerBorderWidth = 1; // Marker border width


                    // Add the category total to the chart series
                    System.Web.UI.DataVisualization.Charting.DataPoint point = new System.Web.UI.DataVisualization.Charting.DataPoint();
                    point.SetValueY(totalAmount);
                    point.Color = categoryColors[category];
                    point.LegendText = $"{category} ({totalAmount})"; // Display category name and total amount in the legend
                    series.Points.Add(point);
                }
            }

            chart.Series.Add(series);

            // Save the chart as an image
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "budgetChart.png");
            chart.SaveImage(imagePath, ChartImageFormat.Png);
        }

        public async Task GenerateSecondBudgetPieChartAsync([FromServices] AppDbContext db, string email)
        {
            List<string> allowedCategoriesForSecondChart = new List<string>
            {
                "Mortgage",
                "Auto Loan",
                "Student Loan",
                "Credit Card",
                "Loan",
                "Personal Loans",
                "Medical Debt",
                "Business Loans",
                "Tax Debt",
                "Debt"
            };

            // Create a new chart
            System.Web.UI.DataVisualization.Charting.Chart chart = new System.Web.UI.DataVisualization.Charting.Chart();

            // Set chart properties
            chart.Width = 700;
            chart.Height = 750;
            chart.BackColor = System.Drawing.Color.Transparent; // Set background color to transparent
            // Create chart areas
            System.Web.UI.DataVisualization.Charting.ChartArea chartArea = new System.Web.UI.DataVisualization.Charting.ChartArea();
            chartArea.BackColor = System.Drawing.Color.Transparent; // Set background color to transparent
            chartArea.BackSecondaryColor = System.Drawing.Color.Transparent; // Set secondary background color to transparent
            chartArea.BackGradientStyle = GradientStyle.None; // Set gradient style to none
            chart.ChartAreas.Add(chartArea);

            // Fetch transactions from the database
            Transactions = await db.Transactions
                .Where(t => t.UserEmail == email)
                .ToListAsync();

            // Create color map for categories
            Dictionary<string, System.Drawing.Color> categoryColors = new Dictionary<string, System.Drawing.Color>
            {
                { "Mortgage", System.Drawing.Color.Red },
                { "Auto Loan", System.Drawing.Color.Blue },
                { "Student Loan", System.Drawing.Color.Green },
                { "Credit Card", System.Drawing.Color.Yellow },
                { "Loan", System.Drawing.Color.Orange },
                { "Personal Loans", System.Drawing.Color.DarkMagenta },
                { "Medical Debt", System.Drawing.Color.DarkCyan },
                { "Business Loans", System.Drawing.Color.DarkGoldenrod },
                { "Tax Debt", System.Drawing.Color.DarkKhaki },
                { "Debt", System.Drawing.Color.Purple },
            };




            // Aggregate transaction amounts for each category
            Dictionary<string, decimal> categoryTotals = new Dictionary<string, decimal>();
            foreach (var transaction in Transactions)
            {
                if (allowedCategoriesForSecondChart.Contains(transaction.Category))
                {
                    if (!categoryTotals.ContainsKey(transaction.Category))
                    {
                        categoryTotals[transaction.Category] = 0m;
                    }
                    categoryTotals[transaction.Category] += transaction.Amount;
                }
            }

            // Add legend to the chart
            System.Web.UI.DataVisualization.Charting.Legend legend = new System.Web.UI.DataVisualization.Charting.Legend();
            legend.Docking = System.Web.UI.DataVisualization.Charting.Docking.Top; // Position the legend above the chart
            chart.Legends.Add(legend);
            legend.BackColor = System.Drawing.Color.Transparent;
            legend.ForeColor = System.Drawing.Color.White;

            // Adjust the size of the legend
            legend.Font = new System.Drawing.Font("Aharoni", 18); // Set the font size (adjust as needed)

            // Add series to the chart
            System.Web.UI.DataVisualization.Charting.Series series = new System.Web.UI.DataVisualization.Charting.Series();
            series.ChartType = SeriesChartType.Pie;
            series.BorderWidth = 6; // Set border width
            series.BorderColor = System.Drawing.Color.White; // Set border color
            series.ChartType = SeriesChartType.Pie;
            // Add legend items for each allowed category
            foreach (var categoryTotal in categoryTotals)
            {
                string category = categoryTotal.Key;
                decimal totalAmount = categoryTotal.Value;
                if (categoryColors.ContainsKey(category))
                {
                    // Create a legend item for the category
                    LegendItem legendItem = new LegendItem();
                    legendItem.Name = category; // Category name
                    legendItem.Color = categoryColors[category]; // Category color
                    legendItem.ImageStyle = System.Web.UI.DataVisualization.Charting.LegendImageStyle.Marker; // Display as marker
                    legendItem.MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Square; // Marker style
                    legendItem.MarkerSize = 1000000; // Marker size
                    legendItem.MarkerBorderColor = System.Drawing.Color.Black; // Marker border color
                    legendItem.MarkerBorderWidth = 1; // Marker border width

                    // Add the category total to the chart series
                    System.Web.UI.DataVisualization.Charting.DataPoint point = new System.Web.UI.DataVisualization.Charting.DataPoint();
                    point.SetValueY(totalAmount);
                    point.Color = categoryColors[category];
                    point.LegendText = $"{category} ({totalAmount})"; // Display category name and total amount in the legend
                    series.Points.Add(point);
                }
            }


            chart.Series.Add(series);

            // Save the chart as an image
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "debtChart.png");
            chart.SaveImage(imagePath, ChartImageFormat.Png);
        }

    }
}
