using ATEMWeb.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ATEMWeb.Controllers
{
    //[System.Web.Http.RoutePrefix("api")]
    public class AtemController : ApiController
    {
        public string Get(string Function, int Duration, int Input)
        {
            string result = string.Empty;
            try
            {
                AtemHelper.Instance.Atem.MixEffectsBlocks.First().ProgramInput = Input;
                result = "Ok";
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
    }
}