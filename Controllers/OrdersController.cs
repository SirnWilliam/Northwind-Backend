using System.Linq;
using System.Text;
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

        // Method to return the Order ID information both encrypt and clear. 
        [EnableCors("AllowOrigin")]
        [HttpGet("{orderId}")]
        public async Task<IActionResult> Get(int orderId)
        {
            var order = await _context.Orders.Where(o => o.OrderID == orderId).SingleOrDefaultAsync();
            return Ok(order);
        }

        [EnableCors("AllowOrigin")]
        [HttpGet("search/{orderId}")]
        public async Task<string> searchOrderId(int orderId)
        {
            string money;
            OrderIDInfo orderIDInfo = new OrderIDInfo();
            var order = await _context.Orders.Where(o => o.OrderID == orderId).SingleOrDefaultAsync();
            if (order != null)
            {
                money = order.Freight.ToString("N");
                money = money.Replace(".", string.Empty).PadLeft((7), '0');
                orderIDInfo.ObfuscatedInfo = order.CustomerID + "." + order.OrderID.ToString() + "." + money + "." + Encrypt(CheckID(order.OrderID));
                orderIDInfo.ClearInfo = order.CustomerID + "." + order.OrderID.ToString() + "." + money + "." + CheckID(order.OrderID);
                return JsonConvert.SerializeObject(value: new { orderIDInfo });
            }
            else
            {
                return JsonConvert.SerializeObject(new { orderIDInfo });
            }
        }

        /*
         * Method to check the ID with the order id (11000)
         * 
         */
        public string CheckID(int id)
        {
            string creditCard;
            string expirationDate;
            if (id < 11000 && id % 2 != 0)
            {
                creditCard = "4012000098765439";
                expirationDate = "1221";
            }
            else if (id < 11000 && id % 2 == 0)
            {
                creditCard = "5146312200000035";
                expirationDate = "1222";
            }
            else if (id > 11000 && id % 2 != 0)
            {
                creditCard = "371449635392376";
                expirationDate = "1019";
            }
            else
            {
                creditCard = "3055155515160018";
                expirationDate = "1120";
            }
            return creditCard + "." + expirationDate;
        }

        // Method to encrypt the credit card number and the exp date.
        public string Encrypt(string cc)
        {
            StringBuilder stringBuilder = new StringBuilder(cc);
            for (int i = 0; i < cc.Length - 9; i++)
            {
                stringBuilder[i] = 'D';
            }
            return stringBuilder.ToString();
        }
    }
}