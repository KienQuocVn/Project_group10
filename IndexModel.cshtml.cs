using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnDemandTutor.Pages.Slots
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Slot> Slots { get; set; }

        public async Task OnGetAsync()
        {
            // Lấy tất cả các slot từ cơ sở dữ liệu
            Slots = await _context.Slots.ToListAsync();
        }
    }
}
