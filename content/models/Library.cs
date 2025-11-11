using GamesShop.content.models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Library
{
    [Key]
    [Column("ID")]
    public int ID { get; set; }

    [Column("UserID")]
    public int UserID { get; set; }

    [ForeignKey("UserID")]
    public virtual User User { get; set; }

    public virtual ICollection<LibraryItem> LibraryItems { get; set; }

    public Library()
    {
        LibraryItems = new HashSet<LibraryItem>();
    }
}