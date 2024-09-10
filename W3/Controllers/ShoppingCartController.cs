using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using W3.Data;
using W3.Models;

namespace W3.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly W3Context _context;

        public ShoppingCartController(W3Context context)
        {
            _context = context;
        }

        // GET: ShoppingCart/Index
        public async Task<IActionResult> Index()
        {
            var userId = User.Identity.Name;

            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Book) 
                .FirstOrDefaultAsync(c => c.UserId == userId);

            return View(cart);
        }

        // POST: ShoppingCart/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int bookId)
        {
            var userId = User.Identity.Name;

            var book = await _context.Books.FindAsync(bookId);
            if (book == null)
            {
                return NotFound();
            }

            // Retrieve or create the cart
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = userId,
                    Items = new List<CartItem>()
                };
                _context.ShoppingCarts.Add(cart);
            }

            // Check if the item is already in the cart
            var cartItem = cart.Items.FirstOrDefault(i => i.BookId == bookId);
            if (cartItem != null)
            {
                cartItem.Quantity++; // Increase quantity if already exists
            }
            else
            {
                // Add new item to cart
                cart.Items.Add(new CartItem
                {
                    BookId = bookId,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: ShoppingCart/RemoveFromCart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int bookId)
        {
            var userId = User.Identity.Name;

            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound();
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.BookId == bookId);
            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: ShoppingCart/UpdateCart
        [HttpPost]
        public async Task<IActionResult> UpdateCart(int bookId, int quantity)
        {
            var userId = User.Identity.Name;

            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound();
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.BookId == bookId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ShoppingCart/Checkout
        public IActionResult Checkout()
        {
            return View();
        }

        // POST: ShoppingCart/ClearCart
        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.Identity.Name;

            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.Items); // Remove all items
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
