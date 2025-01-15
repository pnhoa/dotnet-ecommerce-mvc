using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.ViewModels {
    public class LoginVM {
    
    [Display(Name = "Ten dang nhap")]
    [Required(ErrorMessage = "Vui long nhap thong tin ten dang nhap")]
    [MaxLength(20, ErrorMessage = "Limit 20 characters")]
    public string MaKh { get; set; }

    [Display(Name = "Mat khau")]
    [Required(ErrorMessage = "Vui long nhap mat khau")]
    [DataType(DataType.Password)]
    public string? MatKhau { get; set; }

    }

    

}