using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ATEMWeb.Controllers
{
    public class APIController : ApiController
    {
        public string Get(string Function, int Duration, int Input)
        {
            string result = string.Empty;
            try
            {
                AtemHelper.Atem.SetPGM((long)Input);
                result = "Ok";
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
    }

    public static class AtemHelper
    {
        public static SixteenMedia.ATEM.Broker.Atem Atem { get; private set; }

        static AtemHelper()
        {
            Atem = new SixteenMedia.ATEM.Broker.Atem();
            Atem.Connect(ConfigurationManager.AppSettings["atemIP"]);
        }
    }
}