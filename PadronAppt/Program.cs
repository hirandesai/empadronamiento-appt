using Microsoft.Extensions.Configuration;
using PadronAppt;

try
{
	//await new PadronApp().Start();
	await new ClaveApp().Start();
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}
Console.ReadLine();
