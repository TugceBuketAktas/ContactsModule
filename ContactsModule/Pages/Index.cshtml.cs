using ContactsModule.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsModule.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ContactsModule.Models.BuketSQLContext _context;
        private IWebHostEnvironment _environment;

        public IndexModel(ContactsModule.Models.BuketSQLContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }


        public IList<Contacts> Contacts { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }
        

        public async Task OnGetAsync()
        {


            IQueryable<string> genreQuery = from m in _context.Contacts
                                            orderby m.Firstname
                                            select m.Firstname;

            var contacts = from m in _context.Contacts
                           select m;


            if (!string.IsNullOrEmpty(SearchString))
            {

                contacts = contacts.Where(s => s.Firstname.Contains(SearchString));
                ViewData["MyString"] = "(" + SearchString + ")";
            }
            Contacts = await contacts
                .AsNoTracking()
                .ToListAsync();

        }
        // Silme operasyonunu icra eden metodumuz
        public async Task<IActionResult> OnPostDeleteAsync(int Id)
        {
            // Silme operasyonu için Identity alanından önce
            var contact = await _context.Contacts.FindAsync(Id);
            if (contact != null) // bulduysan
            {
                _context.Contacts.Remove(contact);

                await _context.SaveChangesAsync();
            }
            return RedirectToPage(); // Scotty bizi o anki sayfaya döndür
        }


        StringBuilder sb = new StringBuilder();
        public async Task<IActionResult> OnPostExportAsync()
        {

            List<object> export = (from expcontact in _context.Contacts.ToList()
                                   select new[] { expcontact.ContactId.ToString(),
                                          expcontact.Firstname,
                                          expcontact.Lastname,
                                          expcontact.PhoneNumber,
                                          expcontact.Email,
                                          expcontact.Address,
                                          expcontact.Job,
                                          

                                          }).ToList<object>();

            //export.Insert(0, new string[7] { "Id", "Firstname", "Lastname", "Address", "Email", "Job", "Phone" });

            for (int i = 0; i < export.Count; i++)
            {
                string[] customer = (string[])export[i];
                for (int j = 0; j < customer.Length; j++)
                {
                    //Append data with separator.
                    sb.Append(customer[j] + ',');
                }

                //Append new line character.
                sb.Append("\r\n");

            }
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "Contacts.csv");
        }
        public async Task<IActionResult> OnPostImportAsync()
        {

            var file = Path.Combine(_environment.WebRootPath, "uploads", Upload.FileName);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await Upload.CopyToAsync(fileStream);
            }

            string path = file;


            DataTable dtable = new DataTable();
            dtable.Columns.AddRange(new DataColumn[7] {
                new DataColumn("contact_id", typeof(int)),
                new DataColumn("firstname", typeof(string)),
                new DataColumn("lastname", typeof(string)),
                new DataColumn("phoneNumber", typeof(string)),
                new DataColumn("email", typeof(string)),
                new DataColumn("address", typeof(string)),               
                new DataColumn("job", typeof(string)),
               

            });
            string data = System.IO.File.ReadAllText(path);
            foreach (string row in data.Split("\r\n"))
            {
                
                if (!string.IsNullOrEmpty(row))
                {
                    dtable.Rows.Add();
                    int i = 0;
                    foreach (string cell in row.Split(','))
                    {
                        if(i <= 6)
                        {
                            dtable.Rows[dtable.Rows.Count - 1][i] = cell;
                            i++;

                        }
                        
                    }

                }
              
            }

            //db connection stringi mainconn içerisine yazılır.
            string mainconn = "";
             
            using (SqlConnection sqlconn = new SqlConnection(mainconn))
            {
                using(SqlBulkCopy sqlbkcpy = new SqlBulkCopy(sqlconn))
                {
                    sqlbkcpy.DestinationTableName = "dbo.Contacts";
                    sqlconn.Open();
                    sqlbkcpy.WriteToServer(dtable);
                    sqlconn.Close();
                }
            }

            return RedirectToPage();

        }
       
    }

}
