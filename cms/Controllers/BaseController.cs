using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cms;
using System.IO;

namespace cms.Controllers
{
    // [Authorize]
    public class BaseController : Controller
    {
        internal void CopyProperties(object source, object target, string[] skip = null)
        {
            var customerType = target.GetType();
            foreach (var prop in source.GetType().GetProperties())
            {
                if (skip != null && skip.Contains(prop.Name))
                    continue;
                // prop.Attributes

                if (customerType.GetProperty(prop.Name) == null)
                    continue;

                var propSetter = customerType.GetProperty(prop.Name).GetSetMethod();
                var propGetter = prop.GetGetMethod();
                var valueToSet = propGetter.Invoke(source, null);

                try
                {
                    propSetter.Invoke(target, new[] { valueToSet });
                }
                catch (Exception e)
                {
                    var exm = e.Message;
                    var st = e.StackTrace;
                }
            }
        }

        public cmsEntities db = new cmsEntities();


        public DataSet ReadCsvIntoDataSet(string fileFullPath)
        {
            System.Data.DataSet dSet = new System.Data.DataSet("CSV File");
            try
            {
                dSet = GetData(fileFullPath);
                return dSet;

            }
            catch (Exception e)
            {
                //  e.InnerException = "Error Importing CSV File: Error: ";
            }
            return null;

        }

        private static DataSet GetData(string fileName)
        {
            string strLine;

            string[] strArray;
            char[] charArray = new char[] { ',' };
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add("TheData");
            FileStream aFile = new FileStream(fileName, FileMode.Open);
            StreamReader sr = new StreamReader(aFile);

            strLine = sr.ReadLine();

            strArray = strLine.Split(charArray);

            for (int x = 0; x <= strArray.GetUpperBound(0); x++)
            {
                dt.Columns.Add(strArray[x].Trim());
            }

            strLine = sr.ReadLine();
            while (strLine != null)
            {
                strArray = strLine.Split(charArray);
                System.Data.DataRow dr = dt.NewRow();
                for (int i = 0; i <= strArray.GetUpperBound(0); i++)
                {
                    dr[i] = strArray[i].Trim();
                }
                dt.Rows.Add(dr);
                strLine = sr.ReadLine();
            }
            sr.Close();
            return ds;
        }


    }


}