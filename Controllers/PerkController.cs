#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Game.Data;
using Game.Models;

namespace Game
{
    public class PerkController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerkController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Perk
        public async Task<IActionResult> Index()
        {
            return View(await _context.Perk.ToListAsync());
        }

        // GET: Perk/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perk = await _context.Perk
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perk == null)
            {
                return NotFound();
            }

            return View(perk);
        }

        // GET: Perk/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Increment")] Perk perk)
        {
            Console.Write(perk.Name);
            if (ModelState.IsValid)
            {
                _context.Add(perk);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(perk);
        }

        // GET: Perk/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perk = await _context.Perk.FindAsync(id);
            if (perk == null)
            {
                return NotFound();
            }
            return View(perk);
        }

        // POST: Perk/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Increment")] Perk perk)
        {
            if (id != perk.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(perk);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PerkExists(perk.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(perk);
        }

        // GET: Perk/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var perk = await _context.Perk
                .FirstOrDefaultAsync(m => m.Id == id);
            if (perk == null)
            {
                return NotFound();
            }

            return View(perk);
        }

        // POST: Perk/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perk = await _context.Perk.FindAsync(id);
            _context.Perk.Remove(perk);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PerkExists(int id)
        {
            return _context.Perk.Any(e => e.Id == id);
        }
    }
}
