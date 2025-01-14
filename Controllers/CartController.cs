using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using ECommerceMVC.Helpers;

namespace ECommerceMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly Hshop2023Context db;


        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        public CartController(Hshop2023Context context)
        {
            db = context;
        }

        // GET: HangHoa
        public async Task<IActionResult> Index()
        {

            return View(Cart);
        }

        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {


            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);

            if(item == null) {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if(hangHoa == null) {
                    TempData["Message"] = $"Khong tim thay san pham voi ma ={id}";
                    return Redirect("/404");
                } else {
                    item =  new CartItem {
                        MaHh = hangHoa.MaHh,
                        TenHh = hangHoa.TenHh,
                        DonGia = hangHoa.DonGia ?? 0,
                        Hinh = hangHoa.Hinh ?? "",
                        SoLuong = quantity
                    };
                    gioHang.Add(item);
                }
            } else {
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AdjustQuantity(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);

            if(quantity == 0) {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
                return RedirectToAction("Index");
            }

            if(item == null) {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if(hangHoa == null) {
                    TempData["Message"] = $"Khong tim thay san pham voi ma ={id}";
                    return Redirect("/404");
                } else {
                    item =  new CartItem {
                        MaHh = hangHoa.MaHh,
                        TenHh = hangHoa.TenHh,
                        DonGia = hangHoa.DonGia ?? 0,
                        Hinh = hangHoa.Hinh ?? "",
                        SoLuong = quantity
                    };
                    gioHang.Add(item);
                }
            } else {
                item.SoLuong = quantity;
            }

            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);

            if(item != null) {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }

            return RedirectToAction("Index");
        }
    }
}
