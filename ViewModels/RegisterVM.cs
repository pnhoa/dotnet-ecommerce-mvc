using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.ViewModels {
    public class RegisterVM {
    
    [Display(Name = "Ten dang nhap")]
    [Required(ErrorMessage = "*")]
    [MaxLength(20, ErrorMessage = "Limit 20 characters")]
    public string MaKh { get; set; }

    [Display(Name = "Mat khau")]
    [Required(ErrorMessage = "*")]
    [DataType(DataType.Password)]
    public string? MatKhau { get; set; }

    [Display(Name = "Ho ten")]
    [Required(ErrorMessage = "*")]
    [MaxLength(50, ErrorMessage = "Limit 50 characters")]
    public string HoTen { get; set; } 

    [Display(Name = "Gioi Tinh")]
    public bool GioiTinh { get; set; }  = true;

    [Display(Name = "Ngay Sinh")]
    [DataType(DataType.Date)]
    public DateTime NgaySinh { get; set; }

    [Display(Name = "Dia Chi")]
    [MaxLength(60, ErrorMessage = "Limit 60 characters")]
    public string? DiaChi { get; set; }

    [Display(Name = "Dien Thoai")]
    [MaxLength(24, ErrorMessage = "Limit 24 characters")]
    [RegularExpression(@"0[9875]\d\d{8}", ErrorMessage ="Format incorrect!!!")]
    public string? DienThoai { get; set; }

    [Display(Name = "Email")]
    [EmailAddress(ErrorMessage ="Format incorrect!!!")]
    public string Email { get; set; } = null!;

    [Display(Name = "Avatar")]
    public string? Hinh { get; set; }
    }

    

}