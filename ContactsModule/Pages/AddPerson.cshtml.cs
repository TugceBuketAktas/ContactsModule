using ContactsModule.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ContactsModule.Pages
{
    public class AddPersonModel : PageModel
    {
        private readonly ContactsModule.Models.BuketSQLContext _context;
        //private string modelStateKey;

        public AddPersonModel(ContactsModule.Models.BuketSQLContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Contacts Contacts { get; set; }


        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Contacts.Add(Contacts);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
