using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CS451_Team_Project.Pages
{
    public class FileUploadModel(UserManager<IdentityUser> userManager) : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        
        [BindProperty]
        public IFormFile? UploadedFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            var userId = user?.Id;

            var filePath = Path.Combine("UserImages", userId, UploadedFile.FileName);

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }

            using (var stream = System.IO.File.Create(filePath))
            {
                await UploadedFile.CopyToAsync(stream);
            }

            return RedirectToPage("./Index");
        }
    }
}
