using System.ComponentModel.DataAnnotations;
using System.Web.WebPages;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Ecobit_Blockchain_Frontend.Models
{
	[FunctionOutput]
	public class User
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Bedrijfsnaam niet ingevuld")]
		[Display(Name = "Bedrijfsnaam")]
		[Parameter("string", "companyName")]
		public string Companyname { get; set; }

		[Required(ErrorMessage = "Wachtwoord niet ingevuld")]
		[Display(Name = "Wachtwoord")]
		[Parameter("string", "password", 2)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Emailadres niet ingevuld")]
		[EmailAddress]
		[Display(Name = "Emailadres")]
		[Parameter("string", "email", 3)]
		public string Email { get; set; }

		[Required(ErrorMessage = "Contact naam is niet ingevuld")]
		[Display(Name = "Contactpersoon")]
		[Parameter("string", "contact", 4)]
		public string Contact { get; set; }

		public bool IsNotValid()
		{
			return Email.IsEmpty() || Password.IsEmpty() || Companyname.IsEmpty() || Contact.IsEmpty();
		}
	}
}