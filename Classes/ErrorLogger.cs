using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.Classes
{
    public class ErrorLogger
    {
        private string Destination;
        public ErrorLogger(string _Destination)
        {
            Destination = _Destination;
            try
            {
                if (!Directory.Exists(_Destination))
                    Directory.CreateDirectory(_Destination);
            }
            catch (Exception ex)
            {
            }
        }
        public void WriteClientLog(string ex)
        {
            try
            {
                string file = Destination + "/" + DateTime.Now.Date.ToString("dd_MM_yyyy_") + "ClientErrorLog.txt";
                bool hasInnerException = false;
                // Dim fs = IO.File.Open(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
                var fs = System.IO.File.Open(file, FileMode.Append, FileAccess.Write, FileShare.Write);


                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(string.Format("======================================={0}=======================================", DateTime.Now.ToString()));
                    writer.WriteLine(ex);
                }
            }
            catch (Exception eex)
            {
            }
        }
        public void WriteLog(Exception ex)
        {
            if (ex.Message == "Thread was being aborted.")
                return;
            try
            {
                string file = Destination + "/" + DateTime.Now.Date.ToString("dd_MM_yyyy_") + "ErrorLog.txt";
                bool hasInnerException = false;
                FileStream fs;
                // If IO.File.Exists(file) Then
                // fs = IO.File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                // Else
                // fs = IO.File.Open(file, FileMode.Create, FileAccess.Read, FileShare.ReadWrite)
                // End If
                fs = System.IO.File.Open(file, FileMode.Append, FileAccess.Write, FileShare.Write);
                // fs = IO.File.Open(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)


                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(string.Format("======================================={0}=======================================", DateTime.Now.ToString()));
                    writer.WriteLine(ex.Message);
                    writer.WriteLine(string.Format("---------------Stack Trace---------------"));
                    writer.WriteLine(ex.StackTrace);


                    if (ex.InnerException != null)
                    {
                        hasInnerException = true;
                        writer.WriteLine(string.Format("**********************Inner Exception**********************"));
                    }


                    if (ex.InnerException is System.Data.SqlClient.SqlException)
                    {
                        int i;
                        var SQLExcep = (System.Data.SqlClient.SqlException)ex.InnerException;
                        var SqlError = string.Empty;
                        for (i = 0; i <= SQLExcep.Errors.Count - 1; i++)
                            SqlError += "Index #" + i.ToString() + Environment.NewLine
                                + "Message: " + SQLExcep.Errors[i].Message + Environment.NewLine
                                + "Error Number: " + SQLExcep.Errors[i].Number + Environment.NewLine
                                + "LineNumber: " + SQLExcep.Errors[i].LineNumber + Environment.NewLine
                                + "Source: " + SQLExcep.Errors[i].Source + Environment.NewLine
                                + "Procedure: " + SQLExcep.Errors[i].Procedure + Environment.NewLine;
                        writer.WriteLine(string.Format("---------------SQL Exception Details---------------"));
                        writer.WriteLine(SqlError);
                    }
                }


                if (hasInnerException)
                    WriteLog(ex.InnerException);
            }
            catch (Exception eex)
            {
            }
        }
        public void WriteActivity(string Activity, string ActivitiyLogDestination)
        {
            try
            {
                if (!Directory.Exists(ActivitiyLogDestination))
                    Directory.CreateDirectory(ActivitiyLogDestination);

                string file = ActivitiyLogDestination + "/" + DateTime.Now.Date.ToString("dd_MM_yyyy_") + "ActivityLog.txt";
                var fs = System.IO.File.Open(file, FileMode.Append, FileAccess.Write, FileShare.Write);
                // Dim fs = IO.File.Open(file, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(string.Format(DateTime.Now.ToString() + ":  "));
                    writer.Write(Activity);
                    writer.WriteLine(Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
