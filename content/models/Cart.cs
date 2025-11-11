using GamesShop.content.models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Cart
{
    [Key]
    [Column("ID")]
    public int ID { get; set; }

    [Column("UserID")]
    public int UserID { get; set; }

    [ForeignKey("UserID")]
    public virtual User User { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; }

    public Cart()
    {
        CartItems = new HashSet<CartItem>();
    }
}