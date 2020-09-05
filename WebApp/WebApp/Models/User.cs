using System;
using System.ComponentModel.DataAnnotations;


namespace WebApp.Models
{
    public class User
    {
        [Required(ErrorMessage = "ユーザIDは必ず入力してください。")]
        public String UserId { get; set; }

        public String Password { get; set; }

        public bool isValid { get; set; }
    }
}
