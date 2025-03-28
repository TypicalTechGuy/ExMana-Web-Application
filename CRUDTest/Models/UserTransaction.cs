﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using CRUDTest.Models;

namespace CRUDTest.Models
{
    public class UserTransaction
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public DateTime date { get; set; }
        public decimal amount { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
        public int category_id { get; set; }

        [ForeignKey("category_id")]
        public TransactionCategory TransactionCategory { get; set; }

        [ForeignKey("user_id")]
        public User User { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }
    }
}