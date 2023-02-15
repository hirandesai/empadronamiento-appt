using System.Text;

namespace PadronAppt
{
    internal abstract class BaseApp
    {
        private const string PadronWebsiteBasUrl = "https://gestionturnos.madrid.es";

        protected HttpClient httpClient;
        public BaseApp()
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

        protected abstract Office GetOfficeToProcess();

        protected abstract HttpRequestMessage GetRequest(int officeId);

        protected abstract void ProcessResponse(int officeId, string officeName, string responseContent);

        protected abstract Task DoAfterProcessing();

        public async Task Start()
        {
            var office = GetOfficeToProcess();
            try
            {   
                var httpRequest = GetRequest(office.OfficeId);
                var response = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                ProcessResponse(office.OfficeId, office.OfficeName, responseContent);
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
            await DoAfterProcessing();
        }

        protected void WriteToConsole(string message, ConsoleColor color = default)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"{DateTime.Now}: {message}");
        }
    }
}
