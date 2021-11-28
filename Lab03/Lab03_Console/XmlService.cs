using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Lab03_Business;
using Type = Lab03_Business.Type;

namespace Lab03_Console;

public static class XmlService
{
    public static Bank[] GetBanksFromXml()
    {
        var banks = new List<Bank>();
        var xmlDocument = new XmlDocument();
        xmlDocument.Load("Banks.xml");
        var xRoot = xmlDocument.DocumentElement;
        var childNodes = xRoot!.SelectNodes("*")!;
        
        foreach (XmlNode node in childNodes)
        {
            var bank = new Bank
            {
                Id = node.SelectSingleNode("@Id")?.Value ?? "",
                Name = node.SelectSingleNode("Name")?.InnerText ?? "",
                Country = node.SelectSingleNode("Country")?.InnerText ?? "",
                Type = Enum.Parse<Type>(node.SelectSingleNode("Type")?.InnerText ?? ""),
                Depositor = node.SelectSingleNode("Depositor")?.InnerText ?? "",
                AccountId = node.SelectSingleNode("AccountId")?.InnerText ?? "",
                AmountOnDeposit = Convert.ToInt32(node.SelectSingleNode("AmountOnDeposit")?.InnerText),
                Profitability = (float)Convert.ToDouble(node.SelectSingleNode("Profitability")?.InnerText),
                TimeConstraints = Convert.ToDateTime(node.SelectSingleNode("TimeConstraints")?.InnerText),
            };

            banks.Add(bank);
        }
        
        banks.Sort(new BankComparer());

        return banks.ToArray();
    }

    public static void ValidateXml(string xmlFile)
    {
        var schema = new XmlSchemaSet();
        schema.Add("", "Bank.xsd");

        var xmlDocument = new XmlDocument();
        xmlDocument.Load(xmlFile);
        xmlDocument.Schemas.Add(schema);
        xmlDocument.Validate(ValidationEventHandler!);
    }

    private static void ValidationEventHandler(object sender, ValidationEventArgs e)
    {
        if (e.Severity == XmlSeverityType.Error)
            Console.WriteLine(e.Message);
    }
    
    public static void XmlToJson(string xmlFile)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.Load(xmlFile);
        var xRoot = xmlDocument.DocumentElement;

        var childNodes = xRoot!.SelectNodes("*")!;
        foreach (XmlNode n in childNodes)
            Console.WriteLine(JsonConvert.SerializeXmlNode(n, Newtonsoft.Json.Formatting.Indented, false));
    }
}