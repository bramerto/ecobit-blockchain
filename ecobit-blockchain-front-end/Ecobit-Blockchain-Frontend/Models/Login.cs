using System.ComponentModel.DataAnnotations;

namespace Ecobit_Blockchain_Frontend.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Het e-mailadres is verplicht")]
        [Display(Name = "Gebruikersnaam")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Het wachtwoord is verplicht")]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }
    }
}