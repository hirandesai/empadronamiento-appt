using System.Text;

namespace PadronAppt
{
    internal class ClaveApp : BaseApp
    {
        int currentOfficeIndex = 0;
        public Office[] offices = new Office[]
        {
            new Office { OfficeName = "OAC Aravaca", OfficeId = 111 },
            new Office { OfficeName = "OAC Arganzuela", OfficeId = 110 },
            new Office { OfficeName = "OAC Barajas", OfficeId = 121 },
            new Office { OfficeName = "OAC Carabanchel", OfficeId = 127 },
            new Office { OfficeName = "OAC Centro", OfficeId = 133 },
            new Office { OfficeName = "OAC Chamartín", OfficeId = 123 },
            new Office { OfficeName = "OAC Chamberí", OfficeId = 120 },
            new Office { OfficeName = "OAC Ciudad Lineal", OfficeId = 129 },
            new Office { OfficeName = "OAC El Pardo", OfficeId = 109 },
            new Office { OfficeName = "OAC Fuencarral", OfficeId = 128 },
            new Office { OfficeName = "OAC Hortaleza", OfficeId = 112 },
            new Office { OfficeName = "OAC Latina", OfficeId = 132 },
            new Office { OfficeName = "OAC Moncloa", OfficeId = 138 },
            new Office { OfficeName = "OAC Moratalaz", OfficeId = 119 },
            new Office { OfficeName = "OAC Numancia [Puente de Vallecas]", OfficeId = 117 },
            new Office { OfficeName = "OAC Puente Vallecas", OfficeId = 131 },
            new Office { OfficeName = "OAC Retiro", OfficeId = 122 },
            new Office { OfficeName = "OAC Salamanca", OfficeId = 116 },
            new Office { OfficeName = "OAC San Blas/Canillejas", OfficeId = 115 },
            new Office { OfficeName = "OAC Sanchinarro [Hortaleza]", OfficeId = 114 },
            new Office { OfficeName = "OAC Tetuán", OfficeId = 102 },
            new Office { OfficeName = "OAC Usera", OfficeId = 134 },
            new Office { OfficeName = "OAC Valverde", OfficeId = 113 },
            new Office { OfficeName = "OAC Vicálvaro", OfficeId = 126 },
            new Office { OfficeName = "OAC Villa de Vallecas", OfficeId = 125 },
            new Office { OfficeName = "OAC Villaverde", OfficeId = 130 }
        };


        public ClaveApp() : base()
        {
        }

        protected override Office GetOfficeToProcess()
        {
            return offices[currentOfficeIndex];
        }

        protected override HttpRequestMessage GetRequest(int officeId)
        {
            var requestContent = new StringContent($"idCiudadanoCitaAnterior=0" +
                $"&idOficinaEdicion=261" +
                $"&idFamiliaCitaEdicion=303" +
                $"&usaVariablesEdicion=0" +
                $"&valido=true" +
                $"&idCategoria=121" +
                $"&idFamiliaCita=261" +
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
            currentOfficeIndex++;
            if (currentOfficeIndex >= offices.Length)
            {
                await Task.CompletedTask;
            }
            await Start();
        }
    }
}
