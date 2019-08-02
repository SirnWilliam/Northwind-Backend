using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Practic.Model
{
    /*
     * I put Table annotation because Order Details table has space in it. I had a problem with quering data from the table that has name space.
     */
    [Table("Order Details")]
    public class OrderDetail
    {
        [Key]
        public int OrderID { get; set; }
        public int ProductID { get; set; }
    }
}
