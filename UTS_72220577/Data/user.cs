using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UTS_72220577.Data
{
    internal class user
    {
        public string email { get; set; }
        public string userName { get; set; }
        private string _password;

        public string password
        {
            get => _password;
            set
            {
                if (!IsValidPassword(value))
                {
                    throw new ArgumentException("Password must be at least 8 characters long and include at least one uppercase letter, one number, and one special character.");
                }
                _password = value;
            }
        }

        public string fullName { get; set; }

        private static bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            // Check length (at least 8 characters)
            if (password.Length < 8)
                return false;

            // Check for at least one uppercase letter
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return false;

            // Check for at least one digit
            if (!Regex.IsMatch(password, @"\d"))
                return false;

            // Check for at least one special character
            if (!Regex.IsMatch(password, @"[\W_]"))
                return false;

            return true;
        }
    }
}
