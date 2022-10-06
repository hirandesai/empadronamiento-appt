using System.Text;

namespace PadronAppt
{
    internal class PadronApp
    {
        private const string PadronWebsiteBasUrl = "https://gestionturnos.madrid.es";

        private HttpClient httpClient;
        public PadronApp()
        {
            httpClient = new HttpClient()
            {
                BaseAddress = new Uri(PadronWebsiteBasUrl)
            };
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-GB,en;q=0.9,gu-IN;q=0.8,gu;q=0.7,en-US;q=0.6");
            httpClient.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            httpClient.DefaultRequestHeaders.Add("Origin", PadronWebsiteBasUrl);
            httpClient.DefaultRequestHeaders.Add("Referer", $"{PadronWebsiteBasUrl}/GNSIS_WBCIUDADANO/horarioTramite.do");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36");
        }

        public async Task Start(int officeId, string officeName)
        {
            while (true)
            {
                try
                {
                    var requestContent = new StringContent($"idCiudadanoCitaAnterior=0&idOficinaEdicion=131&idFamiliaCitaEdicion=303&usaVariablesEdicion=&valido=true&idCategoria=162&idFamiliaCita=303&idOficina={officeId}", Encoding.UTF8, "application/x-www-form-urlencoded");
                    var httpRequest = new HttpRequestMessage(HttpMethod.Post, "GNSIS_WBCIUDADANO/horarioTramite.do")
                    {
                        Content = requestContent
                    };

                    var response = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (responseContent.Contains("Seleccione el tipo de atención:"))
                    {
                        WriteToConsole($"Appointment is available at office {officeName}:{officeId}", ConsoleColor.Green);
                    }
                    else
                    {
                        WriteToConsole($"Appointment is not available at office {officeName}:{officeId}", ConsoleColor.Yellow);
                    }
#if RELEASE
                    await Task.Delay(TimeSpan.FromMinutes(9));
#endif

                }
                catch (Exception ex)
                {
                    WriteToConsole(ex.Message, ConsoleColor.Red);
#if RELEASE
                    await Task.Delay(TimeSpan.FromMinutes(4));
#endif

                }
                finally
                {
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
            }

        }

        private void WriteToConsole(string message, ConsoleColor color = default)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"{DateTime.Now}: {message}");
        }
    }    
}
