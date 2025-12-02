using Firmeza.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Firmeza.Web.Models.Entities;
using Firmeza.Web.ViewModels.Users;
namespace Firmeza.Web.Controllers;

[Authorize(Policy = "AdminOnly")]
public class UsersController : Controller
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    // GET: Users
    public async Task<IActionResult> Index(int pageNumber = 1)
    {
        const int pageSize = 10;
        var users = await _service.GetPagedAsync(pageNumber, pageSize);
        return View(users);
    }

    // GET: Users/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null)
            return NotFound();

        var user = await _service.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        return View(user);
    }

    // GET: Users/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new User
        {
            UserName = model.UserName,
            Email = model.Email,
            FullName = model.FullName,
            DocumentNumber = model.DocumentNumber,
            Phone = model.Phone,
            RegisterDate = model.RegisterDate,
            EmailConfirmed = true
        };

        try
        {
            await _service.CreateWithPasswordAsync(user, model.Password, "Client");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
            return NotFound();

        var user = await _service.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        var vm = new EditUserViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            DocumentNumber = user.DocumentNumber,
            Email = user.Email?.Trim()!,
            Phone = user.Phone,
            UserName = user.UserName?.Trim()!,
            RegisterDate = user.RegisterDate
        };

        return View(vm);
    }

    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, EditUserViewModel model)
    {
        if (id != model.Id)
            return NotFound();

        if (!ModelState.IsValid)
            return View(model);

        var user = await _service.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        // Map ViewModel → User Entity
        user.FullName = model.FullName;
        user.DocumentNumber = model.DocumentNumber;
        user.Email = model.Email;
        user.Phone = model.Phone;
        user.UserName = model.UserName;

        try
        {
            await _service.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _service.ExistsAsync(user.Id))
                return NotFound();

            throw;
        }
    }

    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
            return NotFound();

        var user = await _service.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        return View(user);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
