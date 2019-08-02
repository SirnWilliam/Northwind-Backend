using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Practic.Model;

namespace Practic.Controllers
{
    /*
     * route attributes
     */
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public OrdersController(DatabaseContext databaseContext)
        {
            _context = databaseContext;
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("search/{orderId}")]
        public async Task<string> searchOrderId(int orderId)
        {
            HelperMethods hm = new HelperMethods();
            string money;
            OrderIDInfo orderIDInfo = new OrderIDInfo();
            var order = await _context.Orders.Where(o => o.OrderID == orderId).SingleOrDefaultAsync();
            order.CategoryCode = GetCategory(orderId);
            /*
             * Active 2 lines below if you want to save CategoryCode in the database
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            */
            // 
            if (order != null && order.CategoryCode != null)
            {
                money = order.Freight.ToString("N");
                money = money.Replace(".", string.Empty).PadLeft((7), '0');
                orderIDInfo.ObfuscatedInfo = order.CustomerID + "." + order.CategoryCode + order.OrderID.ToString() + "." + money + "." + hm.Encrypt(hm.CheckID(order.OrderID));
                orderIDInfo.ClearInfo = order.CustomerID + "." + GetCategory(orderId) + order.OrderID.ToString() + "." + money + "." + hm.CheckID(order.OrderID);
                return JsonConvert.SerializeObject(value: new { orderIDInfo });
            }
            else
            {
                return JsonConvert.SerializeObject(new { orderIDInfo });
            }
        }     
        /*
         * only operate on orders that include Beverages, Produce, or both
         */
        public string GetCategory(int orderId)
        {
            // I put product ID in the list because each order may have more than on product ID
            List<int> order = _context.OrderDetails.Where(o => o.OrderID == orderId).Select(o => o.ProductID).ToList();
            string categoryName = "";
            bool beverage = false, produce = false;
            foreach (int o in order)
            {
                categoryName = _context.Categories.Where(c => c.CategoryID == _context.Products.Where(p => p.ProductID == o)
                               .Select(p => p.CategoryID).Single()).Select(c => c.CategoryName).Single();
                if (categoryName.Equals("Beverages"))
                {
                    beverage = true;
                }
                else if (categoryName.Equals("Produce"))
                {
                    produce = true;
                }
            }
            if (beverage == true && produce == true)
            {
                return "C";
            }
            else if (beverage == true)
            {
                return "B";
            }
            else if (produce == true)
            {
                return "P";
            }
            else
            {
                return null;
            }
        }
    }
}