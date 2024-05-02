using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace CS451_Team_Project.Models
{
    //[Keyless]
    public class Transaction
    {
        // Define properties

        [Key]
        public int TransactionID { get; set; } // Define a primary key

        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string UniqueTransactionID { get; set; } // Define a primary key

    }
}
