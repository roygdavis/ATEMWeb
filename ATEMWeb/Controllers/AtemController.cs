using ATEM.Services.Interfaces;
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
    public class AtemController : ApiController
    {
        public IAtem Get()
        {
            try
            {
                return AtemHelper.Instance.Atem;
            }
            catch
            {
                return null;
            }
        }
    }
}