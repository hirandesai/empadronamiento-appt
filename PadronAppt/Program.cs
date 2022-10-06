using Microsoft.Extensions.Configuration;
using PadronAppt;

try
{
	var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();
	
	//Change this Id 129 which is for OAC Cuidad Lineal to what ever is appropriate for you.
	await new PadronApp().Start(Convert.ToInt16(config["OfficeId"]),config["OfficeName"]);
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}
Console.ReadLine();
