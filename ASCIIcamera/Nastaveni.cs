//using System;
//using System.Diagnostics;
//using System.IO.Pipes;
//using System.Text;

//class SettingsProgram
//{
//    static void Main()
//    {
//        Console.WriteLine("Starting camera output window...");
//        Process.Start("ASCIIcamera.exe");

//        using var pipe = new NamedPipeServerStream("ascii_pipe", PipeDirection.Out);
//        Console.WriteLine("Waiting for camera program to connect...");
//        pipe.WaitForConnection();
//        Console.WriteLine("Connected!");

//        while (true)
//        {
//            Console.Write("Enter new ramp: ");
//            string ramp = Console.ReadLine();

//            byte[] data = Encoding.UTF8.GetBytes(ramp + "\n");
//            pipe.Write(data, 0, data.Length);
//            pipe.Flush();
//        }
//    }
//}

