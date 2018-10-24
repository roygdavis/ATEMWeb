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
        public SixteenMedia.ATEM.Wrapper.Atem Get()
        {
            try
            {
                return AtemHelper.Instance.Atem;
            }
            catch 
            {
                return SixteenMedia.ATEM.Wrapper.Atem.Null;
            }
        }


    }
}