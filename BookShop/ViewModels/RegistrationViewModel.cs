using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BookShop.ViewModels
{
	public class RegistrationViewModel
	{
		public IFormFile Image { get; set; }
		public string UserName { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		[EmailAddress]
		public string Email { get; set; }
		
		public byte Age { get; set; }
		[DataType(DataType.Password)]
		public string Password { get; set; }
        [DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get;set; }
		public bool Gender { get; set; }

		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber { get; set; }

		public string Adress { get; set; }

	}
}
