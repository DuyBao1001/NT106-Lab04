using System;
using System.Windows.Forms;
using System.Net; 

namespace Bai7
{
    internal static class Program
    {
        
        [STAThread]
        static void Main()
        {
            
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}