using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using WiseGuysFrameWork2015;
using WiseGuysFrameWork2015DIV;

namespace RaidTheCageCtrl
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string resource1 = "WiseGuysFrameWork2015.SkinSoft.VisualStyler.dll";
            string resource2 = "RaidTheCageCtrl.hasp_net_windows.dll";
            string resource4 = "WiseGuysFrameWork2015.Softgroup.NetResize.dll";
            EmbeddedAssemblyFrameWork.Load(resource1, "SkinSoft.VisualStyler.dll");
         
            EmbeddedAssembly2.Load(resource2, "hasp_net_windows.dll");
            EmbeddedDllClass.ExtractEmbeddedDlls("SocketTools9.Interop.dll", resourcesfromdll.gethandletoSocketTools());
            EmbeddedAssemblyFrameWork.Load(resource4, "Softgroup.NetResize.dll");
         
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            // Creating a Global culture specific to our application.
            System.Globalization.CultureInfo cultureInfo =
                new System.Globalization.CultureInfo("en-GB");
            // Creating the DateTime Information specific to our application.
            System.Globalization.DateTimeFormatInfo dateTimeInfo =
                new System.Globalization.DateTimeFormatInfo();
            // Defining various date and time formats.
            dateTimeInfo.DateSeparator = "/";
            dateTimeInfo.LongDatePattern = "dd-MMM-yyyy";
            dateTimeInfo.ShortDatePattern = "dd-MMM-yy";
            dateTimeInfo.LongTimePattern = "hh:mm:ss tt";
            dateTimeInfo.ShortTimePattern = "hh:mm tt";
            // Setting application wide date time format.
            cultureInfo.DateTimeFormat = dateTimeInfo;
            // Assigning our custom Culture to the application.
            Application.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                // put this in a try, because stopping the application from running result in an Exception... just let it :)
                Application.Run(new MainForm());
            }
            catch (Exception)
            {
                
                
            }
            
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
             
                 Assembly asm;
                 asm = EmbeddedAssemblyFrameWork.Get(args.Name);
                 if (asm != null)
                     return asm;
                 else
                 {
                     return EmbeddedAssembly2.Get(args.Name);
                 }     
             

    // Not our custom, use the default loading
    return null;

            /*
             
             */
        }
    }
}
