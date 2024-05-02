using CS451_Team_Project.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Net;

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

        public async Task<IActionResult> OnPost([FromServices] AppDbContext db, [FromQuery] string token, [FromQuery] string email)
        {

            string decryptedEmail = EncryptionHelper.Decrypt(email);
            // Get the current user
            var user = db.Users.FirstOrDefault(u => u.Email == decryptedEmail);
            _logger.LogWarning($"Invalid login attempt for user: {TransactionInput.Date}");
            _logger.LogWarning($"Invalid login attempt for user: {TransactionInput.Description}");
            _logger.LogWarning($"Invalid login attempt for user: {TransactionInput.Category}");
            _logger.LogWarning($"Invalid login attempt for user: {TransactionInput.Amount}");
            _logger.LogWarning($"Invalid login attempt for user: {user.Id}");
            _logger.LogWarning($"Invalid login attempt for user: {user.Email}");
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

        public async Task<IActionResult> OnPostRemoveTransactionAsync([FromServices] AppDbContext db, string transactionId)
        {
            try
            {
                // Find the transaction in the database
                var transaction = await db.Transactions.FindAsync(transactionId);

                // If the transaction exists, remove it from the database
                if (transaction != null)
                {
                    db.Transactions.Remove(transaction);
                    await db.SaveChangesAsync();
                }
                else
                {
                    // Transaction not found in the database
                    return NotFound();
                }

                // Return a success response
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Error occurred while removing the transaction.");

                // Return an error response
                return new JsonResult(new { success = false, error = ex.Message });
            }
        }


        public async Task<IActionResult> OnGet([FromServices] AppDbContext db, [FromQuery] string token, [FromQuery] string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                // Handle the case where email or token is missing in the URL query parameters
                // You may want to redirect to an error page or show a message
                return RedirectToPage("/Error");
            }

            string decryptedEmail = EncryptionHelper.Decrypt(email);

            // Validate the token
            if (ValidateTokenForDashboard(decryptedEmail, token))
            {
                // Token is valid, proceed with your logic

                // For demonstration, let's assume we're fetching the user from the database
                var user = db.Users.FirstOrDefault(u => u.Email == decryptedEmail);

                if (user != null)
                {
                    // Proceed with your logic using the user and token
                    // For example, update the user's last login time, or perform any other action
                    _logger.LogWarning(decryptedEmail);
                    _logger.LogWarning($"WORKS!!!!!!!!!!!!!!!!!!!");
                    Transactions = await db.Transactions
                        .Where(t => t.UserId == user.Id)
                        .ToListAsync();
                    return Page();
                }
                else
                {
                    // Handle the case where the user is not found in the database
                    return RedirectToPage("/Error");
                }

            }
            else
            {
                // Token is invalid, handle accordingly
                // You may want to redirect to an error page or show a message
                return RedirectToPage("/Error");
            }
        }

        private bool ValidateTokenForDashboard(string email, string token)
        {
            // Validate the token
            // You can implement your own method to validate the token

            // For demonstration, let's assume the token is valid if it's not null or empty
            return !string.IsNullOrEmpty(token);
        }

    }

}

