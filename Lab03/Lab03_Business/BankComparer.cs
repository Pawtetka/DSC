namespace Lab03_Business;

public class BankComparer : IComparer<Bank>
{
    public int Compare(Bank? x, Bank? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        var countryComparison = string.Compare(x.Country, y.Country, StringComparison.Ordinal);
        if (countryComparison != 0) return countryComparison;
        var nameComparison = string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        if (nameComparison != 0) return nameComparison;
        var depositorComparison = string.Compare(x.Depositor, y.Depositor, StringComparison.Ordinal);
        if (depositorComparison != 0) return depositorComparison;
        return string.Compare(x.AccountId, y.AccountId, StringComparison.Ordinal);
    }
}