using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VpnCard
{
    public class AutoSet
    {
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        private const int WM_GETTEXT = 13;

        [DllImport("USER32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);


        public bool FindAndSetKeysOK()
        {
            IntPtr clientHandle = GetWindow();
            if (clientHandle == IntPtr.Zero) return false;

            string coordPedidas = GetCoordText(clientHandle);

            string keysToSend = FindKey(StripCoord(coordPedidas, true)) + FindKey(StripCoord(coordPedidas, false));

            SetForegroundWindow(clientHandle);
            SendKeys.SendWait(keysToSend);
            SendKeys.SendWait("{ENTER}");
            return true;
        }

        private IntPtr GetWindow()
        {
            IntPtr clientHandle = FindWindow("#32770", "Cisco AnyConnect | Cencosud AR");

            // Verify that Calculator is a running process.
            if (clientHandle == IntPtr.Zero)
            {
                MessageBox.Show("Cisco AnyConnect is not running.");
                return IntPtr.Zero;
            }
            return clientHandle;
        }

        private string GetCoordText(IntPtr winHandle)
        {
            IntPtr lblHandle = FindWindowEx(winHandle, IntPtr.Zero, "Edit", "");
            IntPtr Handle = Marshal.AllocHGlobal(50);
            // send WM_GWTTEXT message to the notepad window
            int NumText = (int)SendMessage(lblHandle, WM_GETTEXT, (IntPtr)49, Handle);
            // copy the characters from the unmanaged memory to a managed string

            string text = Marshal.PtrToStringUni(Handle);
            if (text.StartsWith("Enter")) return text;

            lblHandle = FindWindowEx(winHandle, lblHandle, "Edit", "");

            NumText = (int)SendMessage(lblHandle, WM_GETTEXT, (IntPtr)49, Handle);
            return Marshal.PtrToStringUni(Handle);
        }

        private string StripCoord(string coordText, bool primero)
        {
            return coordText.Substring(coordText.IndexOf("[", primero ? 0 : coordText.IndexOf("[") + 1) + 1, 2);
        }

        private string FindKey(string coord)
        {
            string col = coord.Substring(0, 1);
            int row = int.Parse(coord.Substring(1, 1))-1;

            StringCollection keys = (StringCollection)Properties.Settings.Default[col];
            return keys[row];
        }
    }
}
