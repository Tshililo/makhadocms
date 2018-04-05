using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Helpers
{
   public static class PalBrokerHtmlHelper
   {

      public static PalBrokerHelper PalBrokerHtml(this HtmlHelper helper)
      {
         return new PalBrokerHelper(helper);
      }
   }
}