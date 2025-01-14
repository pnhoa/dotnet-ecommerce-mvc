using System.Data.Common;
using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.ViewComponents
{
    public class MenuCategoryViewComponent : ViewComponent {

        private readonly Hshop2023Context db;
        public MenuCategoryViewComponent(Hshop2023Context context) => db = context;

        public IViewComponentResult Invoke() {
            var data = db.Loais.Select(lo => new MenuCategoryVM{ MaLoai = lo.MaLoai,
            TenLoai = lo.TenLoai, SoLuong = lo.HangHoas.Count}).OrderBy(p => p.MaLoai);

            return View(data);

        }
    }
}