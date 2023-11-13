// See https://aka.ms/new-console-template for more information
using System.IO.Ports;
using NAudio.CoreAudioApi;
using NAudio.Wave;

Console.WriteLine("Hello, World!");

Console.WriteLine("Serial Ports:");
foreach (var name in SerialPort.GetPortNames())
    Console.WriteLine($"  {name}");

Console.WriteLine("Input one name:");
string? selected = Console.ReadLine();

if (string.IsNullOrWhiteSpace(selected))
{
    Console.WriteLine("No selection.");
    return;
}

SerialPort serialPort = new SerialPort(selected, 115200, Parity.None, 8, StopBits.One);

serialPort.Open();

WasapiCapture capture = new WasapiLoopbackCapture();
capture.WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(8192, 1);
capture.DataAvailable += (s, e) =>
{
    serialPort.Write(e.Buffer, 0, e.BytesRecorded);
};

capture.StartRecording();

while (true)
{
    Console.ReadKey();
}
