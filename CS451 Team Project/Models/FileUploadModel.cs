// import
using CS451_Team_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

public class FileUploadModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly PhotoAppContext _context;

    public FileUploadModel(UserManager<ApplicationUser> userManager, PhotoAppContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [BindProperty]
    public IFormFile UploadedFile { get; set; }

    public async Task<IActionResult> OnPostAsync([FromServices] AppDbContext db)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        string email = "webapptest2024@gmail.com";
        var user = db.Users.FirstOrDefault(u => u.Email == email);
        var userId = user?.Id;

        if (userId == null || UploadedFile == null)
        {
            // Handle the error appropriately
            return Page();
        }

        var filePath = Path.Combine("UserImages", userId, UploadedFile.FileName);

        if (!Directory.Exists(Path.GetDirectoryName(filePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        try
        {
            using (var stream = System.IO.File.Create(filePath))
            {
                await UploadedFile.CopyToAsync(stream);
            }
        }
        catch
        {
            // Handle the error appropriately
            return Page();
        }

        var photo = new Photo
        {
            UserId = userId,
            PhotoName = UploadedFile.FileName,
            PhotoPath = filePath
        };

        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}
