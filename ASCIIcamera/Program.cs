//Vojtěch Chmelař
//3.C
//ASCII kamera
//Pro optimální zobrazení výstupu kamery přibližte/oddalte okno konzole.

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ASCIIcamera
{
    internal class Program
    {
        class CameraSettings
        {
            public int Width { get; set; } = 160;
            public int Height { get; set; } = 90;
            public string Ramp { get; set; } = " .:-=+*#%@";
            public int DelayMs { get; set; } = 33;
        }

        static CameraSettings settings = new CameraSettings();
        static VideoCapture capture;

        static int lastW = -1;
        static int lastH = -1;

        static void Main(string[] args)
        {
            Console.SetWindowSize(settings.Width, settings.Height);
            Console.SetBufferSize(settings.Width, settings.Height);
            Console.CursorVisible = false;

            Console.CursorVisible = false;
            Console.Clear();

            capture = new VideoCapture(0);

            ApplyCameraSettings(true);

            using Mat frame = new Mat();
            using Mat gray = new Mat();

            while (true)
            {
                var sw = Stopwatch.StartNew();

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Q)
                    {
                        OpenSettingsMenu();
                        Console.Clear();
                        ApplyCameraSettings(true);
                    }

                    if (key.Key == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        Environment.Exit(0);
                    }
                }

                capture.Read(frame);
                if (!frame.IsEmpty)
                {
                    CvInvoke.CvtColor(frame, gray, ColorConversion.Bgr2Gray);

                    using Mat small = new Mat();
                    CvInvoke.Resize(gray, small, new System.Drawing.Size(settings.Width, settings.Height));

                    RenderFrame(small);
                }

                sw.Stop();

                int sleep = settings.DelayMs - (int)sw.ElapsedMilliseconds;
                if (sleep > 0)
                    Thread.Sleep(sleep);
            }
        }

        static void ApplyCameraSettings(bool force = false)
        {
            if (!force && settings.Width == lastW && settings.Height == lastH)
                return;

            capture.Set(CapProp.FrameWidth, settings.Width);
            capture.Set(CapProp.FrameHeight, settings.Height);

            lastW = settings.Width;
            lastH = settings.Height;
        }

        static void RenderFrame(Mat small)
        {
            using Image<Gray, byte> img = small.ToImage<Gray, byte>();

            var sb = new StringBuilder(settings.Width);

            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < img.Height; y++)
            {
                sb.Clear();

                for (int x = 0; x < img.Width; x++)
                {
                    byte brightness = img.Data[y, x, 0];

                    int index = brightness * (settings.Ramp.Length - 1) / 255;
                    sb.Append(settings.Ramp[index]);
                }

                Console.WriteLine(sb.ToString());
            }
        }

        //nastaveni
        static void OpenSettingsMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== SETTINGS ===");
                Console.WriteLine("1. Rozlišení");
                Console.WriteLine("2. ASCII Set");
                Console.WriteLine("3. FPS");
                Console.WriteLine("4. Uložit a zpět");
                Console.Write("Vyber: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ChangeResolution();
                        break;
                    case "2":
                        ChangeAsciiSet();
                        break;
                    case "3":
                        ChangeFps();
                        break;
                    case "4":
                        return;
                }
            }
        }

        static void ChangeResolution()
        {
            Console.Clear();
            Console.WriteLine("1. 80x45");
            Console.WriteLine("2. 160x90");
            Console.WriteLine("3. 240x135");

            switch (Console.ReadLine())
            {
                case "1":
                    settings.Width = 80;
                    settings.Height = 45;
                    break;
                case "2":
                    settings.Width = 160;
                    settings.Height = 90;
                    break;
                case "3":
                    settings.Width = 240;
                    settings.Height = 135;
                    break;
            }

            ApplyCameraSettings(true);
        }

        static void ChangeAsciiSet()
        {
            Console.Clear();
            Console.WriteLine("1. .#");
            Console.WriteLine("2. .:-=+*#%@");
            Console.WriteLine("3. .'`^,:;Il!i~+_-?][}{1)(|/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B@$");

            switch (Console.ReadLine())
            {
                case "1":
                    settings.Ramp = " .#";
                    break;
                case "2":
                    settings.Ramp = " .:-=+*#%@";
                    break;
                case "3":
                    settings.Ramp = " .'`^,:;Il!i~+_-?][}{1)(|/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B@$";
                    break;
            }
        }

        static void ChangeFps()
        {
            Console.Clear();
            Console.WriteLine("1. 15 FPS");
            Console.WriteLine("2. 30 FPS");
            Console.WriteLine("3. 60 FPS");

            switch (Console.ReadLine())
            {
                case "1":
                    settings.DelayMs = 67;
                    break;
                case "2":
                    settings.DelayMs = 33;
                    break;
                case "3":
                    settings.DelayMs = 16;
                    break;
            }
        }
    }
}