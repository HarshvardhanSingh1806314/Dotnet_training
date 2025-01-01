using Assignment_1.Models;
using Assignment_1.Repository;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Assignment_1.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactRepository _contactRepository = null;

        public ContactsController()
        {
            _contactRepository = new ContactRepository();
        }

        // GET: Contacts
        public async Task<ActionResult> Index()
        {
            var contactList = await _contactRepository.GetAllAsync();
            return View(contactList);
        }

        public async Task<ActionResult> Details(int Id)
        {
            return View(await _contactRepository.GetByIdAsync(Id));
        }

        public ActionResult CreateContact()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateContact(Contact contact)
        {
            Tuple<bool, string> result = await _contactRepository.CreateAsync(contact);
            Console.WriteLine(result.Item2);
            TempData["success"] = result.Item2;
            if (result.Item1)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public ActionResult Update(Contact contact)
        {
            return View(contact);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateContact(Contact contact)
        {
            Tuple<bool, string> result = await _contactRepository.UpdateAsync(contact);
            Console.WriteLine(result.Item2);
            TempData["success"] = result.Item2;
            if (result.Item1)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public ActionResult DeleteContact(Contact contact)
        {
            return View(contact);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteContact(int Id)
        {
            Tuple<bool, string> result = await _contactRepository.DeleteAsync(Id);
            Console.WriteLine(result.Item2);
            TempData["success"] = result.Item2;
            if(result.Item1)
            {
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}