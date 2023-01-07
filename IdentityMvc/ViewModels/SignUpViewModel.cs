using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityMvc.ViewModels;

public class SignUpViewModel
{
    [DisplayName("Kullanıcı Adı : ")]
    [Required(ErrorMessage = "Kullanıcı adı boş geçilemez.")]
    public string UserName { get; set; }

    [DisplayName("Email : ")]
    [Required(ErrorMessage = "Email geçilemez.")]
    public string Email { get; set; }

    [DisplayName("Telefon Numarası : ")]
    [Required(ErrorMessage = "Telefon numarası boş geçilemez.")]
    public string PhoneNumber { get; set; }


    [DataType(DataType.Password)]
    [DisplayName("Parola : ")]
    [Required(ErrorMessage = "Parola boş geçilemez.")]
    public string Password { get; set; }


    [DataType(DataType.Password)]
    [DisplayName("Parola Tekrar : ")]
    [Required(ErrorMessage = "Parola boş bırakılamaz.")]
    [Compare(nameof(Password), ErrorMessage = "Girilen şifreler eşleşmiyor.")]
    public string PasswordConfirm { get; set; }
}