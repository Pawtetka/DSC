namespace Lab03_Business;

public class Bank
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Country { get; set; } = "";
    public Type Type { get; set; }
    public string Depositor { get; set; } = "";
    public string AccountId { get; set; } = "";
    public int AmountOnDeposit { get; set; } = 0;
    public float Profitability { get; set; } = 0;
    public DateTime TimeConstraints { get; set; } = DateTime.MinValue;
}