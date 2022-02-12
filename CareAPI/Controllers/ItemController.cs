using CareAPI.Helpers;
using CareAPI.Models;
using CareAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CareAPI.Controllers
{
    [Route("api/item")]
    [ApiController]
    [MethodLogger]
    public class ItemController : Controller
    {
        private readonly ServiceDbContext _context;

        public ItemController(ServiceDbContext context)
        {
            _context = context;
        }

        // GET: api/Item
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemModel>>> GetItems()
        {
            return await _context.Items.ToListAsync();
        }

        // GET: api/Item/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemModel>> GetItemModel(int id)
        {
            var itemModel = await _context.Items.FindAsync(id);

            if (itemModel == null)
            {
                return NotFound();
            }

            return itemModel;
        }

        // PUT: api/Item/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemModel(int id, ItemModel itemModel)
        {
            if (id != itemModel.ImageId)
            {
                return BadRequest();
            }

            _context.Entry(itemModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Item
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ItemModel>> PostItemModel(ItemModel itemModel)
        {
            _context.Items.Add(itemModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemModel), new { id = itemModel.ImageId }, itemModel);
        }
        //"GetItemModel"
        //nameof(GetTodoItem)

        // DELETE: api/Item/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ItemModel>> DeleteItemModel(int id)
        {
            var itemModel = await _context.Items.FindAsync(id);
            if (itemModel == null)
            {
                return NotFound();
            }

            _context.Items.Remove(itemModel);
            await _context.SaveChangesAsync();

            return itemModel;
        }

        private bool ItemModelExists(int id)
        {
            return _context.Items.Any(e => e.ImageId == id);
        }
    }
}
