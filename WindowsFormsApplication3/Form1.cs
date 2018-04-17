using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Diagnostics;




namespace WindowsFormsApplication3
{
    
    public partial class Form1 : Form
    {
        float a, b;
        int count, conv;        
        bool znak = true;
        string Path = Application.StartupPath.ToString();
        string UrlS = "http://zapomnika.zzz.com.ua/Lab4.php";
        string DataS = "user=Михов А.А.&pass=k272";
        string[] prov = { "Log.txt" };
        string str;
        string str1;
        string[] ctrl = new string[9];
        const string name = "WindowsFormsApplication3";
        int ckey = 0;
        Process[] localByName = Process.GetProcessesByName(name);
        List<string> filess = new List<string>();
        List<Keys> _pressedKeys;
        
    void KBDHook_KeyUp(Hooks.LLKHEventArgs e)
    {
        _pressedKeys.Remove(e.Keys);
    }

    void KBDHook_KeyDown(Hooks.LLKHEventArgs e)
        {           
            if (!_pressedKeys.Contains(e.Keys))
                _pressedKeys.Add(e.Keys);
            string key = string.Join<Keys>(" + ", _pressedKeys);
            if (key == "LControlKey + C + D0")
            {

                if (ckey > 8)
                    MessageBox.Show("Массив заполнен, если хотите переписать его нажмите");
               else
                {            
                    str = Clipboard.GetText();
                    Regex newReg = new Regex(@"\d");
                    MatchCollection matches = newReg.Matches(str);
                    foreach (Match mat in matches)
                    {
                        str1 += mat.ToString();
                        ctrl[ckey] = str1;
                    }                                       
                    ++ckey;
                }
            }            
            switch (key)
            {
                case "LControlKey + V + D1":                    
                    textBox1.Text = ctrl[0];
                    regsave();
                    break;
                case "LControlKey + V + D2":
                    textBox1.Text = ctrl[1];
                    regsave();
                    break;
                case "LControlKey + V + D3":
                    textBox1.Text = ctrl[2];
                    regsave();
                    break;
                case "LControlKey + V + D4":
                    textBox1.Text = ctrl[3];
                    regsave();
                    break;
                case "LControlKey + V + D5":
                    textBox1.Text = ctrl[4];
                    regsave();
                    break;
                case "LControlKey + V + D6":
                    textBox1.Text = ctrl[5];
                    regsave();
                    break;
                case "LControlKey + V + D7":
                    textBox1.Text = ctrl[6];
                    regsave();
                    break;
                case "LControlKey + V + D8":
                    textBox1.Text = ctrl[7];
                    regsave();
                    break;
                case "LControlKey + V + D9":
                    textBox1.Text = ctrl[8];
                    regsave();
                    break;

                default:
                    break;
            }
        }

    public bool SetAutorunValue(bool autorun)
        {
            string ExePath = System.Windows.Forms.Application.ExecutablePath;                        
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (autorun)
                    reg.SetValue(name, ExePath);
                else
                    reg.DeleteValue(name);

                reg.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

      
        private void getallfile(string startdirectory)
        {
            int count = 0;
            int enter;
            int index = Path.Length + 1;
            int ni = 1;
            string text, texts;
            string slash = "\\";
            string[] searchdirectory = Directory.GetDirectories(startdirectory);        
            if (searchdirectory.Length > 0)
            {
                for (int i = 0; i < searchdirectory.Length; i++)
                {                                 
                    text = searchdirectory[i].Substring(index);
                    enter = 0;
                    do
                    {                        
                        ni = text.IndexOf(slash);
                        if (ni > 0)
                        {
                            text = text.Substring(ni + 1);
                            enter++;
                        }

                    } while (ni > 0);
                    for (int j = 0; j < enter; j++)
                        text = "\t->" + text;
                    filess.Add(text);
                    getallfile(searchdirectory[i] + @"\");
                    
                }
            }
            string[] filesss = Directory.GetFiles(startdirectory);
            for (int i = 0; i < filesss.Length; i++)
            {
                texts = filesss[i].Substring(index);
                enter = 0;
                do
                {
                    ni = texts.IndexOf(slash);
                    if (ni > 0)
                    {
                        texts = texts.Substring(ni + 1);
                        enter++;
                    }
                } while (ni > 0);
                for (int k = 0; k < enter; k++)
                    texts = "\t->" + texts;
                for (int j = 0; j < prov.Length; j++)
                    {
                    if (texts.EndsWith(prov[j]))
                        count = 1;
                    else
                        count = 0;
                    }
                if (count == 1)
                  filess.Add(texts + " +");
                else
                    filess.Add(texts);
            }
         }

        private string GET(string Url, string Data)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Data);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream(); // создаем поток
            os.Write(bytes, 0, bytes.Length); // отправляем в сокет
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null)
            {
                return "Ответ пуст";
            }
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return (sr.ReadToEnd().Trim());
        }



        private void regsave()
        {
            RegistryKey currentUserKey = Registry.CurrentUser;
            RegistryKey calc = currentUserKey.OpenSubKey("calc", true);
            calc.SetValue("textBox1", textBox1.Text);
            calc.SetValue("label1", label1.Text);
            calc.Close();
            
        }
        private void calculate()
        {
            switch (count)
            {
                case 1:
                    b = a + float.Parse(textBox1.Text);
                    textBox1.Text = b.ToString();
                    regsave();
                    break;
                case 2:
                    b = a - float.Parse(textBox1.Text);
                    textBox1.Text = b.ToString();
                    regsave();
                    break;
                case 3:
                    b = a * float.Parse(textBox1.Text);
                    textBox1.Text = b.ToString();
                    regsave();
                    break;
                case 4:
                    if (float.Parse(textBox1.Text) == 0)
                    {
                        MessageBox.Show("Делить на ноль нельзя");
                        break;
                    }
                    else
                    {
                        b = a / float.Parse(textBox1.Text);
                        textBox1.Text = b.ToString();
                        regsave();
                        break;
                    }
                case 5:              
                    b = Convert.ToSingle(Math.Sin(a));
                    textBox1.Text = b.ToString();
                    regsave();
                    break;
                case 6:
                    b = Convert.ToSingle(Math.Cos(a));
                    textBox1.Text = b.ToString();
                    regsave();
                    break;
                case 7:
                    b = Convert.ToSingle(Math.Sqrt(a));
                    textBox1.Text = b.ToString();
                    regsave();
                    break;
                case 8:
                    b = Convert.ToSingle(Math.Pow(a,2));
                    textBox1.Text = b.ToString();
                    regsave();
                    break;
                case 9:
                    b = Convert.ToSingle(Math.Pow(a,int.Parse(textBox1.Text)));
                    textBox1.Text = b.ToString();
                    regsave();
                    break;

                default:
                    break;
            }

        }

        private void convert()
        {
            switch (conv)
            {
                case 1:
                    b = (a - 32) * 5 / 9;
                    textBox1.Text = b.ToString() + "°C";
                    regsave();
                    break;
                case 2:
                    b = (a * 9 / 5) + 32;
                    textBox1.Text = b.ToString() + "°F";
                    regsave();
                    break;
                case 3:
                    b = a * 180/ Convert.ToSingle(Math.PI);
                    textBox1.Text = b.ToString() + "°";
                    regsave();
                    break;
                case 4:
                    b = a * Convert.ToSingle(Math.PI) / 180 ;
                    textBox1.Text = b.ToString() + "Rad";
                    regsave();
                    break;
                case 5:
                    b = a / Convert.ToSingle(2.54);
                    textBox1.Text = b.ToString() + "См";
                    regsave();
                    break;
                case 6:
                    b = a * Convert.ToSingle(2.54);
                    textBox1.Text = b.ToString() + "Inch";
                    regsave();
                    break;
                

                default:
                    break;
            }

        }
        private void click(int c)
        {
            a = float.Parse(textBox1.Text);
            textBox1.Clear();
            count = c;
            znak = true;
        }

        private void click_conv(int c)
        {
            a = float.Parse(textBox1.Text);
            textBox1.Clear();
            conv = c;           
        }
        public Form1()
        {
            InitializeComponent();
            _pressedKeys = new List<Keys>();

            Hooks.KBDHook.KeyDown += new Hooks.KBDHook.HookKeyPress(KBDHook_KeyDown);
            Hooks.KBDHook.KeyUp += new Hooks.KBDHook.HookKeyPress(KBDHook_KeyUp);
            Hooks.KBDHook.LocalHook = false;
            Hooks.KBDHook.InstallHook();
            this.FormClosed += (s, e) => {
                Hooks.KBDHook.UnInstallHook();
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 0;
            regsave();
        }

        private void button_1_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 1;
            regsave();
        }

        private void button_2_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 2;
            regsave();
        }

        private void button_3_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 3;
            regsave();
        }

        private void button_4_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 4;
            regsave();
        }

        private void button_5_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 5;
            regsave();
        }

        private void button_6_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 6;
            regsave();
        }

        private void button_7_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 7;
            regsave();
        }

        private void button_8_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 8;
            regsave();
        }

        private void button_9_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 9;
            regsave();
        }

        private void button_plus_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                click(1);
                label1.Text = a.ToString() + "+";
                regsave();
            }

        }

        private void button_minus_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                click(2);
                label1.Text = a.ToString() + "-";
                regsave();
            }
        }

        private void button_multiplication_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                click(3);
                label1.Text = a.ToString() + "*";
                regsave();
            }
        }

        private void button_division_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                click(4);
                label1.Text = a.ToString() + "/";
                regsave();
            }
        }

        private void button_exactly_Click(object sender, EventArgs e)
        {
            calculate();
            label1.Text = "";
            regsave();
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            label1.Text = "";
            regsave();
                    }

        private void button_back_Click(object sender, EventArgs e)
        {
            int lenght = textBox1.Text.Length - 1;
            string text = textBox1.Text;
            textBox1.Clear();
            for (int i = 0; i < lenght; i++)
            {
                textBox1.Text = textBox1.Text + text[i];
            }
            regsave();
        }

        private void button_insert_Click(object sender, EventArgs e)
        {
            if (znak == true)
            {
                textBox1.Text = "-" + textBox1.Text;
                znak = false;
            }
            else if (znak == false)
            {
                textBox1.Text = textBox1.Text.Replace("-", "");
                znak = true;
            }
            regsave();
        }

        private void button_sin_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                click(5);
                label1.Text = "sin" + a.ToString();               
            }
            regsave();
        }

        private void button_cos_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                click(6);
                label1.Text = "cos" + a.ToString();                
            }
            regsave();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                click(7);
                label1.Text = "√" + a.ToString();               
            }
            regsave();
        }

        private void button_sqr_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                click(8);
                label1.Text = a.ToString() + "^2";                
            }
            regsave();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (localByName.Length > 1)
            {
                DialogResult run = MessageBox.Show("Программа " + name + " уже запущена. Хотите запустить ещё одну?", "Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);                
                if (run == DialogResult.No)
                {
                    Close();
                }
            }
            SetAutorunValue(false);
            RegistryKey currentUserKey = Registry.CurrentUser;
            RegistryKey calc = currentUserKey.CreateSubKey("calc");
            string load = (string)calc.GetValue("textBox1");
            textBox1.Text = load;
            load = (string)calc.GetValue("label1");
            label1.Text = load;
            calc.Close();
            getallfile(Path);
            System.IO.File.WriteAllLines(Path + "\\Log.txt", filess);                           
            }

        private void button_pow_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                click(9);
                label1.Text = a.ToString() + "^";                
            }
            regsave();
        }

        private void button_F_Click(object sender, EventArgs e)
        {
            click_conv(2);
            label1.Text = a.ToString() + "°C =";
            regsave();
        }

        private void button_G_Click(object sender, EventArgs e)
        {
            click_conv(3);
            label1.Text = a.ToString() + "Rad =";
            regsave();
        }

        private void button_rad_Click(object sender, EventArgs e)
        {
            click_conv(4);
            label1.Text = a.ToString() + "° =";
            regsave();
        }

        private void button_M_Click(object sender, EventArgs e)
        {
            click_conv(5);
            label1.Text = a.ToString() + "Inch =";
            regsave();
        }

        private void button_inch_Click(object sender, EventArgs e)
        {
            click_conv(6);
            label1.Text = a.ToString() + "См =";
            regsave();
        }

        private void button_convert_Click(object sender, EventArgs e)
        {
            convert();
            label1.Text = "";
            regsave();
        }

        private void button_conn_Click(object sender, EventArgs e)
        {            
            string Message = GET(UrlS, DataS);
            int ns = DataS.IndexOf("=") + 1;
            string user = DataS.Substring(ns);
            ns = DataS.IndexOf("pass") + 1;
            user = user.Remove(ns);
            ns = ns + user.Length + 1;           
            Message = Message.Remove(ns);
            System.IO.File.WriteAllText(Path + "\\Net.txt", Message);
        }

        private void Renew_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Array.Clear(ctrl,0,9);
            ckey = 0;
        }

        private void button_С_Click(object sender, EventArgs e)
        {
            click_conv(1);
            label1.Text = a.ToString() + "°F =";
            regsave();

        }

        private void button_point_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + ",";
            regsave();
        }
    }
    class Hooks
    {
        public static class KBDHook
        {
            #region Declarations
            public delegate void HookKeyPress(LLKHEventArgs e);
            public static event HookKeyPress KeyUp;
            public static event HookKeyPress KeyDown;

            [StructLayout(LayoutKind.Sequential)]
            struct KBDLLHOOKSTRUCT
            {
                public uint vkCode;
                public uint scanCode;
                public KBDLLHOOKSTRUCTFlags flags;
                public uint time;
                public IntPtr dwExtraInfo;
            }

            [Flags]
            enum KBDLLHOOKSTRUCTFlags : int
            {
                LLKHF_EXTENDED = 0x01,
                LLKHF_INJECTED = 0x10,
                LLKHF_ALTDOWN = 0x20,
                LLKHF_UP = 0x80,
            }

            static IntPtr hHook = IntPtr.Zero;
            static IntPtr hModule = IntPtr.Zero;
            static bool hookInstall = false;
            static bool localHook = true;
            static API.HookProc hookDel;
            #endregion

            /// <summary>
            /// Hook install method.
            /// </summary>
            public static void InstallHook()
            {
                if (IsHookInstalled) return;

                hModule = Marshal.GetHINSTANCE(AppDomain.CurrentDomain.GetAssemblies()[0].GetModules()[0]);
                hookDel = new API.HookProc(HookProcFunction);

                if (localHook)
                    hHook = API.SetWindowsHookEx(API.HookType.WH_KEYBOARD,
                        hookDel, IntPtr.Zero, AppDomain.GetCurrentThreadId());
                else
                    hHook = API.SetWindowsHookEx(API.HookType.WH_KEYBOARD_LL,
                        hookDel, hModule, 0);

                if (hHook != IntPtr.Zero)
                    hookInstall = true;
                else
                    throw new Win32Exception("Can't install low level keyboard hook!");
            }
            /// <summary>
            /// If hook installed return true, either false.
            /// </summary>
            public static bool IsHookInstalled
            {
                get { return hookInstall && hHook != IntPtr.Zero; }
            }
            /// <summary>
            /// Module handle in which hook was installed.
            /// </summary>
            public static IntPtr ModuleHandle
            {
                get { return hModule; }
            }
            /// <summary>
            /// If true local hook will installed, either global.
            /// </summary>
            public static bool LocalHook
            {
                get { return localHook; }
                set
                {
                    if (value != localHook)
                    {
                        if (IsHookInstalled)
                            throw new Win32Exception("Can't change type of hook than it install!");
                        localHook = value;
                    }
                }
            }
            /// <summary>
            /// Uninstall hook method.
            /// </summary>
            public static void UnInstallHook()
            {
                if (IsHookInstalled)
                {
                    if (!API.UnhookWindowsHookEx(hHook))
                        throw new Win32Exception("Can't uninstall low level keyboard hook!");
                    hHook = IntPtr.Zero;
                    hModule = IntPtr.Zero;
                    hookInstall = false;
                }
            }
            /// <summary>
            /// Hook process messages.
            /// </summary>
            /// <param name="nCode"></param>
            /// <param name="wParam"></param>
            /// <param name="lParam"></param>
            /// <returns></returns>
            static IntPtr HookProcFunction(int nCode, IntPtr wParam, [In] IntPtr lParam)
            {
                if (nCode == 0)
                {
                    LLKHEventArgs args = null;
                    if (localHook)
                    {
                        bool pressed = false;
                        if (lParam.ToInt32() >> 31 == 0)
                            pressed = true;

                        Keys keys = (Keys)wParam.ToInt32();
                        args = new LLKHEventArgs(keys, pressed, 0U, 0U);

                        if (pressed)
                        {
                            if (KeyDown != null)
                                KeyDown(args);
                        }
                        else
                        {
                            if (KeyUp != null)
                                KeyUp(args);
                        }
                    }
                    else
                    {
                        KBDLLHOOKSTRUCT kbd = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

                        bool pressed = false;
                        if (wParam.ToInt32() == 0x100 || wParam.ToInt32() == 0x104)
                            pressed = true;

                        Keys keys = (Keys)kbd.vkCode;
                        args = new LLKHEventArgs(keys, pressed, kbd.time, kbd.scanCode);

                        if (pressed)
                        {
                            if (KeyDown != null)
                                KeyDown(args);
                        }
                        else
                        {
                            if (KeyUp != null)
                                KeyUp(args);
                        }
                    }

                    if (args != null && args.Hooked)
                        return (IntPtr)1;
                }
                return API.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }

        public class LLKHEventArgs
        {
            Keys keys;
            bool pressed;
            uint time;
            uint scCode;

            public LLKHEventArgs(Keys keys, bool pressed, uint time, uint scanCode)
            {
                this.keys = keys;
                this.pressed = pressed;
                this.time = time;
                this.scCode = scanCode;
            }

            /// <summary>
            /// Key.
            /// </summary>
            public Keys Keys
            { get { return keys; } }
            /// <summary>
            /// Is key pressed or no.
            /// </summary>
            public bool IsPressed
            { get { return pressed; } }
            /// <summary>
            /// The time stamp for this message, equivalent to what GetMessageTime would return for this message.
            /// </summary>
            public uint Time
            { get { return time; } }
            /// <summary>
            /// A hardware scan code for the key.
            /// </summary>
            public uint ScanCode
            { get { return scCode; } }
            /// <summary>
            /// Is user hook key.
            /// </summary>
            public bool Hooked { get; set; }
        }

        static class API
        {
            public delegate IntPtr HookProc(int nCode, IntPtr wParam, [In] IntPtr lParam);

            [DllImport("user32.dll")]
            public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, [In] IntPtr lParam);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr SetWindowsHookEx(HookType hookType, HookProc lpfn, IntPtr hMod, int dwThreadId);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool UnhookWindowsHookEx(IntPtr hhk);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr GetModuleHandle(string lpModuleName);

            public enum HookType : int
            {
                WH_JOURNALRECORD = 0,
                WH_JOURNALPLAYBACK = 1,
                WH_KEYBOARD = 2,
                WH_GETMESSAGE = 3,
                WH_CALLWNDPROC = 4,
                WH_CBT = 5,
                WH_SYSMSGFILTER = 6,
                WH_MOUSE = 7,
                WH_HARDWARE = 8,
                WH_DEBUG = 9,
                WH_SHELL = 10,
                WH_FOREGROUNDIDLE = 11,
                WH_CALLWNDPROCRET = 12,
                WH_KEYBOARD_LL = 13,
                WH_MOUSE_LL = 14
            }
        }
    }
}
