// See https://aka.ms/new-console-template for more information

using Lab03_Console;
using Lab03_Business;

Console.WriteLine("XML validation result for Bank.xml:");
XmlService.ValidateXml("Bank.xml");

Console.WriteLine("Banks:");
var banks = XmlService.GetBanksFromXml();
foreach (var bank in banks)
{
    Console.WriteLine("Id = " + bank.Id + 
                      " Country = " + bank.Country +
                      " Bank Name = " + bank.Name + 
                      " Depositor = " + bank.Depositor +
                      " AccountId = " + bank.AccountId);
}

Console.WriteLine("\nXML validation result for WrongBank.xml:");
XmlService.ValidateXml("WrongBank.xml");

Console.WriteLine("\nConvert banks from XML to JSON:");
XmlService.XmlToJson("Banks.xml");