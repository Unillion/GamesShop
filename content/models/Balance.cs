using GamesShop.content.models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Balance
{
    [Key]
    [Column("ID")]
    public int ID { get; set; }

    [Column("BillID")]
    public int BillID { get; set; }

    [ForeignKey("BillID")]
    public virtual Bill Bill { get; set; }

    [Column("CurrencyCode")]
    public string CurrencyCode { get; set; } = "USD";

    [Column("Amount", TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }
}