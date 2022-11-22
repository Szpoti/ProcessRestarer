// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Restart service started...");

string arguments = "";
if(args.DefaultIfEmpty() is null)
{
    throw new ArgumentNullException(nameof(args));
} else if (args.Length > 1)
{
    arguments = string.Join(" ", args.Skip(1));
}

var startInfo = new ProcessStartInfo
{
    FileName = args[0],
    Arguments = arguments,
    UseShellExecute = true,
    CreateNoWindow = false,
    WindowStyle = ProcessWindowStyle.Normal,
};
Console.WriteLine($"Starting {startInfo.FileName} with arguments: {arguments}");

var process = Process.Start(startInfo);
Thread.Sleep(5000);
if (process is null)
{
    Console.WriteLine("Process is null.");
    return;
}
string processName = startInfo.FileName.Split("\\").Last();
processName = Path.GetFileNameWithoutExtension(processName);
Process? runningProcess = null;

byte counter = 0;
while (true)
{
    runningProcess = Process.GetProcessesByName(processName).FirstOrDefault();
    if (runningProcess is null)
    {
            Console.WriteLine($"Restarting {processName}...");
            Process.Start(startInfo);
            Thread.Sleep(5000);
            counter = 0;
            continue;
    }
    else if (!runningProcess.Responding)
    {
        counter++;
        if (counter == 5)
        {
            Console.WriteLine($" {processName} has not responded. Killing...");
            runningProcess.Close();
            runningProcess.Kill(true);
            Thread.Sleep(3000);
            Process.Start(startInfo);
            counter = 0;
            continue;

        }
        Console.WriteLine($" {processName} has not responded. Retrying...");
        Thread.Sleep(2000);
    }
    else
    {
        Console.WriteLine("Is responding...");
        Thread.Sleep(TimeSpan.FromMinutes(5));
    }
}

