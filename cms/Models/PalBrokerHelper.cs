using cms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cms.Helpers
{
   public class PalBrokerHelper
   {
      private GridHelper gridHelper;
      private HtmlHelper Html;
      public PalBrokerHelper(HtmlHelper helper)
      {
         Html = helper;
      }

      public GridHelper GridView
      {
         get
         {
            if(gridHelper == null)
               gridHelper = new GridHelper(Html);

            return gridHelper;
         }
      }
   }
}