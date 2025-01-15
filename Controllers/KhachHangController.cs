using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using ECommerceMVC.Helpers;
using AutoMapper;

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

        // GET: HangHoa
        public async Task<IActionResult> Index()
        {

            return View();
        }

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

        
    }
}
