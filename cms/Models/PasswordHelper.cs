using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace cms.Models
{
    public class ApplicationModel
    {
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
                //return "Error Importing CSV File: Error: " + e.InnerException.Message;
            }
            return null;

        }
        DataSet GetData(string fileName)
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