using CS451_Team_Project.Models;
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

namespace CS451_Team_Project.Pages
{
    public class BudgetTrackerModel : PageModel 
    {
        [FromQuery]
        [FromRoute]
        public string email { get; set; }
        [FromQuery]
        [FromRoute]
        public string token { get; set; }

        public List<Transaction> Transactions { get; set; } = new List<Transaction>(); // Initialize Transactions list

        private readonly AppDbContext _dbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<NewLoginModel> _logger;

        [BindProperty]
        public InputModel TransactionInput { get; set; } // Model for transaction input form
        public class InputModel
        {
            private DateTime _date;
            [Required]
            public DateTime Date
            {
                get => _date;
                set => _date = value.Kind == DateTimeKind.Unspecified ? DateTime.SpecifyKind(value, DateTimeKind.Utc) : value.ToUniversalTime();
            }
            [Required]
            public string Description { get; set; }
            [Required]
            public string Category { get; set; }
            [Required]
            public decimal Amount { get; set; }

        }

        public BudgetTrackerModel(AppDbContext dbContext, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<NewLoginModel> logger)
        {
            _dbContext = dbContext; // Assign the dbContext
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            TransactionInput = new InputModel();
        }
        // Other methods...

        public async Task<IActionResult> OnPost([FromServices] AppDbContext db, [FromQuery] string token, [FromQuery] string email, string action, string transactionId)
        {
            string decryptedEmail = EncryptionHelper.Decrypt(email);
            var user = db.Users.FirstOrDefault(u => u.Email == decryptedEmail);

            if (action == "add")
            {
                _logger.LogWarning($"add");
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning($"something wrong");
                    return Page(); // Return the page if model state is not valid
                }

                // Create a new Transaction object
                var transaction = CreateTransaction();

                db.Transactions.Add(transaction);
                await db.SaveChangesAsync();

                return RedirectToPage("BudgetTracker", new { token = token, email = email });
            }
            else if (action == "remove")
            {
                _logger.LogWarning($"remove");
                // Remove the transaction
                var transactionToRemove = db.Transactions.FirstOrDefault(t => t.UniqueTransactionID == transactionId);

                if (transactionToRemove != null)
                {
                    db.Transactions.Remove(transactionToRemove);
                    await db.SaveChangesAsync();
                }
                return RedirectToPage("BudgetTracker", new { token = token, email = email });
            }
            _logger.LogWarning($"not called");
            return RedirectToPage("BudgetTracker", new { token = token, email = email });
        }


        private Transaction CreateTransaction()
        {
            try
            {
                string decryptedEmail = EncryptionHelper.Decrypt(email);
                // Get the current user
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == decryptedEmail);
                return new Transaction
                {
                    Date = TransactionInput.Date,
                    Description = TransactionInput.Description,
                    Category = TransactionInput.Category,
                    Amount = TransactionInput.Amount,
                    UserId = user.Id, // Set the UserId to the current user's Id
                    UserEmail = user.Email
                };

            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }


        public async Task<IActionResult> OnGetAsync([FromServices] AppDbContext db, [FromQuery] string token, [FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Error");
            }

            string decryptedEmail = EncryptionHelper.Decrypt(email);

            if (!ValidateTokenForDashboard(decryptedEmail, token))
            {
                return RedirectToPage("/Error");
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == decryptedEmail);
            if (user == null)
            {
                return RedirectToPage("/Error");
            }

            // Asynchronously generate the chart
            Charts chart = new Charts(_logger);
            await chart.GenerateBudgetPieChartAsync(_dbContext, decryptedEmail);


            Charts chart2 = new Charts(_logger);
            await chart2.GenerateSecondBudgetPieChartAsync(_dbContext, decryptedEmail);

            Transactions = await _dbContext.Transactions
                .Where(t => t.UserId == user.Id)
                .ToListAsync();

            return Page();
        }


        private bool ValidateTokenForDashboard(string email, string token)
        {
            // Validate the token
            // let's assume the token is valid if it's not null or empty
            return !string.IsNullOrEmpty(token);
        }

    }

}

