using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace AudioCapture.SerialPorts
{
    internal class SerialPortWaveIn : IWaveIn
    {
        public SerialPort SerialPort { get; }
        public bool AutoDisposeSerialPort { get; }
        public WaveFormat? WaveFormat { get; set; }

        public event EventHandler<WaveInEventArgs>? DataAvailable;
        public event EventHandler<StoppedEventArgs>? RecordingStopped;

        private CancellationTokenSource? cancellation;

        public void StartRecording()
        {
            cancellation = new CancellationTokenSource();

            Task.Run(() =>
            {
                try
                {
                    if (!SerialPort.IsOpen)
                        SerialPort.Open();

                    ArrayPool<byte> arrayPool = ArrayPool<byte>.Create();

                    while (!cancellation.IsCancellationRequested)
                    {
                        byte[] buffer = arrayPool.Rent(SerialPort.BytesToRead);
                        int bytesRead = SerialPort.Read(buffer, 0, buffer.Length);
                        DataAvailable?.Invoke(this, new WaveInEventArgs(buffer, bytesRead));
                        arrayPool.Return(buffer);
                    }
                }
                catch (Exception ex)
                {
                    RecordingStopped?.Invoke(this, new StoppedEventArgs(ex));
                }
            });
        }

        public void StopRecording()
        {
            if (cancellation != null)
            {
                cancellation.Cancel();
                RecordingStopped?.Invoke(this, new StoppedEventArgs());
            }
        }

        public void Dispose()
        {
            if (AutoDisposeSerialPort)
                SerialPort.Dispose();
        }

        public SerialPortWaveIn(SerialPort serialPort, bool autoDisposeSerialPort)
        {
            SerialPort = serialPort;
            AutoDisposeSerialPort = autoDisposeSerialPort;
        }
    }
}
