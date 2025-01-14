using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;

namespace ECommerceMVC.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly Hshop2023Context _context;

        public HangHoaController(Hshop2023Context context)
        {
            _context = context;
        }

        // GET: HangHoa
        public async Task<IActionResult> Index(int? loai)
        {
            // var hshop2023Context = _context.HangHoas.Include(h => h.MaLoaiNavigation).Include(h => h.MaNccNavigation);
            // return View(await hshop2023Context.ToListAsync());

            var hangHoas = _context.HangHoas.AsQueryable();

            if(loai.HasValue ) {
                hangHoas =  hangHoas.Where(p => p.MaLoai == loai.Value);
            } 

            var result = hangHoas.Select(p => new HangHoaVM {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });

            return View(result);
        }

        public async Task<IActionResult> Search(string? query)
        {

            var hangHoas = _context.HangHoas.AsQueryable();

            if(query != null) {
                hangHoas =  hangHoas.Where(p => p.TenHh.Contains(query));
            }

            var result = hangHoas.Select(p => new HangHoaVM {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });

            return View(result);
        }

        public async Task<IActionResult> Detail(int id)
        {

            var data = _context.HangHoas
            .Include(p => p.MaLoaiNavigation)
            .SingleOrDefault(p => p.MaHh == id);

            if(data == null ) {
                TempData["Message"] = $"Khong tim thay san pham co ma {id}";
                return Redirect("/404");
            } 

            var result =  new HangHoaChiTietVM {
                MaHh = data.MaHh,
                TenHh = data.TenHh,
                DonGia = data.DonGia ?? 0,
                Hinh = data.Hinh ?? "",
                MoTaNgan = data.MoTaDonVi ?? "",
                TenLoai = data.MaLoaiNavigation.TenLoai,
                ChiTiet = data.MoTa ?? "",
                DiemDanhGia = 5,
                SoLuongTon = 10
            };

            return View(result);
        }
    }
}
