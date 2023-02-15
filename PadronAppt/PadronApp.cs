using Microsoft.Extensions.Configuration;
using System.Text;

namespace PadronAppt
{
    internal class PadronApp : BaseApp
    {
        public PadronApp() : base()
        {
        }

        protected override Office GetOfficeToProcess()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .Build();
            return new Office()
            {
                OfficeId = Convert.ToInt16(config["OfficeId"]),
                OfficeName = config["OfficeName"]
            };
        }

        protected override HttpRequestMessage GetRequest(int officeId)
        {
            var requestContent = new StringContent($"idCiudadanoCitaAnterior=0" +
                $"&idOficinaEdicion=131" +
                $"&idFamiliaCitaEdicion=303" +
                $"&usaVariablesEdicion=" +
                $"&valido=true" +
                $"&idCategoria=162" +
                $"&idFamiliaCita=303" +
                $"&idOficina={officeId}", Encoding.UTF8, "application/x-www-form-urlencoded");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "GNSIS_WBCIUDADANO/horarioTramite.do")
            {
                Content = requestContent
            };
            return httpRequest;
        }

        protected override void ProcessResponse(int officeId, string officeName, string responseContent)
        {
            if (responseContent.Contains("Seleccione el tipo de atención:"))
            {
                WriteToConsole($"Appointment is available at office {officeName}:{officeId}", ConsoleColor.Green);
            }
            else
            {
                WriteToConsole($"Appointment is not available at office {officeName}:{officeId}", ConsoleColor.Yellow);
            }
        }

        protected override async Task DoAfterProcessing()
        {
            await Start();
        }
    }
}
