using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;
using System.Reflection.PortableExecutable;
using System.Transactions;
using CS451_Team_Project.Controllers;
using CS451_Team_Project.Models;


namespace CS451_Team_Project.Pages
{
    public class FileUploadModel : PageModel
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<EmailController> _logger;

        public FileUploadModel(ILogger<EmailController> logger, UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file.");
                return Page();
            }

            string text = "";

            // Handle PDF files
            if (System.IO.Path.GetExtension(file.FileName).ToLower() == ".pdf")
            {
                // error is also coming from here 
                text = await ExtractTextFromPDFAsync(file);
            }
            // Handle DOCX files
            else if (System.IO.Path.GetExtension(file.FileName).ToLower() == ".docx")
            {
                text = await ExtractTextFromDOCXAsync(file);
            }
            else
            {
                ModelState.AddModelError("File", "Unsupported file format.");
                return Page();
            }

            // Now you have the text extracted from the file
            // You can further process or save it as needed

            // Redirect to another page after successful upload
            return RedirectToPage("/FileUpload");
        }

        private async Task<string> ExtractTextFromPDFAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                //PDF header signature not found
                using (var pdfReader = new PdfReader(memoryStream))
                {
                    var text = new StringBuilder();
                    for (int i = 1; i <= pdfReader.NumberOfPages; i++)
                    {
                        text.Append(iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(pdfReader, i));
                    }
                    return text.ToString();
                }
            }
        }

        private async Task<string> ExtractTextFromDOCXAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                using (var doc = WordprocessingDocument.Open(memoryStream, false))
                {
                    var text = new StringBuilder();
                    var body = doc.MainDocumentPart.Document.Body;
                    foreach (var paragraph in body.Elements<Paragraph>())
                    {
                        text.AppendLine(paragraph.InnerText);
                    }
                    return text.ToString();
                }
            }
        }

        private async Task CategorizeTransactions(string text)
        {
            // Define keyword-category mappings
        Dictionary<string, string> keywordCategoryMap = new Dictionary<string, string>
        {
            {"rent", "bills"},
            {"electricity", "bills"},
            {"groceries", "food"},
            {"restaurant", "food"},
            // Add more keywords and categories
        };

            // Split text into lines
            string[] lines = text.Split('\n');

            foreach (string line in lines)
            {
                // Iterate through the keyword-category mappings
                foreach (KeyValuePair<string, string> entry in keywordCategoryMap)
                {
                    // Check if the keyword is present in the line
                    if (line.Contains(entry.Key, StringComparison.OrdinalIgnoreCase)) // Case-insensitive comparison
                    {
                        // Assign the transaction to the corresponding category
                        string category = entry.Value;

                        // Store or process the transaction accordingly
                        //await StoreTransactionInDatabase(category, line); // Assuming line contains transaction details
                        break; // Exit loop after finding the first match
                    }
                }
            }
        }

        //private async Task StoreTransactionInDatabase(string category, string transactionDetails)
        //{
        //    // You need to implement the logic to store the transaction in the database
        //    // Here's a simplified example assuming you have a DbContext named YourDbContext

        //    using (var dbContext = new YourDbContext())
        //    {
        //        // Create a new transaction object
        //        Transaction transaction = new Transaction
        //        {
        //            Category = category,
        //            TransactionDetails = transactionDetails
        //        };

        //        // Add the transaction to the database and save changes
        //        dbContext.Transactions.Add(transaction);
        //        await dbContext.SaveChangesAsync();
        //    }
        //}

    }
}
