using Google.Authenticator;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using CS451_Team_Project.Models;


namespace CS451_Team_Project.Pages
{
    public class VerificationModel : PageModel
    {
        public string QrCodeUrl { get; set; }
        public string ManualEntryCode {  get; set; }

        [FromForm]
        public string EmailAddress { get; set; }

        public void OnGet()
        {
            string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10);

            // TODO: Make this key cryptographically sound
            // string key = GenerateRandomString(10);

            // TODO: Save to database 
            // User { ID, EmailAddress, Key }
            //db.Users.Add(new User
            //{
            //    Id = Guid.NewGuid(),
            //    EmailAddress = EmailAddress,
            //    Key = key
            //});
            //db.SaveChanges();

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            SetupCode setupInfo = tfa.GenerateSetupCode("Test Two Factor", "EmailAddress", key, false, 3);

            QrCodeUrl = setupInfo.QrCodeSetupImageUrl;
            ManualEntryCode = setupInfo.ManualEntryKey;
        }
    }

        //public void OnPost([FromServices] AppDbContext db)
        //{
        //    string key = Guid.NewGuid().ToString().Replace("-", "").Substring(0,10);

        //    // TODO: Make this key cryptographically sound
        //    // string key = GenerateRandomString(10);

        //    // TODO: Save to database 
        //    // User { ID, EmailAddress, Key }
        //    //db.Users.Add(new User
        //    //{
        //    //    Id = Guid.NewGuid(),
        //    //    EmailAddress = EmailAddress,
        //    //    Key = key
        //    //});
        //    //db.SaveChanges();

        //    TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
        //    SetupCode setupInfo = tfa.GenerateSetupCode("Test Two Factor", "EmailAddress", key, false, 3);

        //    QrCodeUrl = setupInfo.QrCodeSetupImageUrl;
        //    ManualEntryCode = setupInfo.ManualEntryKey;
        //}

        //public static string GenerateRandomString(int length, string allowableChars = null)
        //{
        //    if (string.IsNullOrEmpty(allowableChars)) {
        //        allowableChars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    }

        //    var rnd = new byte[length];
        //    using (var rng = new RNGCryptoServiceProvider())
        //            rng.GetBytes(rnd);

        //    var allowable = allowableChars.ToCharArray();
        //    var l = allowable.Length;
        //    var chars = new char[length];
        //    for (var i = 0; i < length; i++) {
        //        chars[i] = allowable[rnd[i] % l];
        //    }

        //    return new string(chars);
        //}
        //}
}
