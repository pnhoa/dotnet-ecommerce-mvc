using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using ECommerceMVC.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceMVC.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly Hshop2023Context db;

        private readonly IMapper _mapper;



        public KhachHangController(Hshop2023Context context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        
        #region Register

        [HttpGet]
        public async Task<IActionResult> DangKy()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangKy(RegisterVM model, IFormFile Hinh)
        {
            if(ModelState.IsValid) {

                var data = db.KhachHangs
                .SingleOrDefault(p => p.MaKh == model.MaKh);

                if(data != null) {
                    TempData["Message"] = $"Ton tai khach hang voi ma {model.MaKh}";
                    return View();
                }

                var khachHang = _mapper.Map<KhachHang>(model);
                khachHang.RandomKey = Util.GenerateRandomKey();
                khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
                khachHang.HieuLuc = true; // update logic later
                khachHang.VaiTro = 0;

                if(Hinh != null) {
                    khachHang.Hinh = Util.UploadHinh(Hinh, "KhachHang");
                }

                db.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index", "HangHoa");
            }

            return View();
        }

        #endregion

        #region Login
        [HttpGet]
        public async Task<IActionResult> DangNhap(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            return View();
        }
       [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;

            if(ModelState.IsValid) {
                var data = db.KhachHangs
                .SingleOrDefault(p => p.MaKh == model.MaKh);

                if(data == null) {
                    ModelState.AddModelError("Loi", "Sai thong tin dang nhap");
                } else {
                    if(!data.HieuLuc) {
                        ModelState.AddModelError("Loi", "Tai khoan bi khoa. Vui long lien he admin");
                    } else {
                        if(data.MatKhau != model.MatKhau.ToMd5Hash(data.RandomKey)) {
                            ModelState.AddModelError("Loi", "Sai thong tin dang nhap");
                        } else {
                            var claims =  new List<Claim> {
                                new Claim(ClaimTypes.Email, data.Email),
                                new Claim(ClaimTypes.Name, data.HoTen),
                                new Claim("CustomerID", data.MaKh),
                                new Claim(ClaimTypes.Role, "Customer")
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var claimsPricipal = new ClaimsPrincipal(claimsIdentity);

                            await HttpContext.SignInAsync(claimsPricipal);

                            if(Url.IsLocalUrl(ReturnUrl)) {
                                return Redirect(ReturnUrl);
                            } else {
                                return Redirect("/");
                            }
                        }
                    }
                }
            }

            return View();
        }

        #endregion

        [Authorize]
        public IActionResult Profile() {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> DangXuat() {
            await HttpContext.SignOutAsync();

            return Redirect("/");
        }

        
    }
}
