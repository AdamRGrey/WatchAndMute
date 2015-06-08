using CoreAudio;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace WatchAndMute
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        static void Main(string[] args)
        {
            const string Target = "SkullGirls";

            while (true)
            {
                MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
                MMDevice device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
                for (int i = 0; i < device.AudioSessionManager2.Sessions.Count; i++)
                {
                    AudioSessionControl2 session = device.AudioSessionManager2.Sessions[i];
                    Process p;
                    try
                    {
                        p = Process.GetProcessById((int)session.GetProcessID);
                    }
                    catch (System.ArgumentException)
                    {
                        continue;
                    }
                    if (p.ProcessName == Target)
                    {
                        Console.WriteLine("found {0}", Target);
                        while (true)
                        {
                            if (GetForegroundWindow() == p.MainWindowHandle)
                            {
                                session.SimpleAudioVolume.Mute = false;
                            }
                            else
                            {
                                session.SimpleAudioVolume.Mute = true;
                            }
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
        }
    }

}
