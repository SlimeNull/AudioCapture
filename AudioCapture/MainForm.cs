using System;
using System.IO.Ports;
using System.Windows.Forms;
using AudioCapture.Controls;
using AudioCapture.SerialPorts;
using LibAudioVisualizer;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace AudioCapture
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitSerialPortOptions();

            // ���� UI Ϊδ��ʼ��״̬
            SetStateToNotInitailizedCapture();

            // һЩ����ͨ����������еĳ�ʼ������
            timeZoneWaveVisual.MouseWheel += WaveVisualScroll;
            frequencyZoneWaveVisual.MouseWheel += WaveVisualScroll;
        }

        IWaveIn? capture;                 // �ɼ���
        MMDevice? selectedDevice;               // ѡ��Ĳɼ��豸
        string? selectedSerialPort;             // ѡ��Ĵ���
        int? selectedSerialPortBoudRate;          // ѡ��Ĵ��ڲ�����
        int? selectedSerialPortDataBits;          // ѡ��Ĵ�������λ
        Parity? selectedSerialPortParity;         // ѡ��Ĵ���У��
        StopBits? selectedSerialPortStopBits;     // ѡ��Ĵ���ֹͣλ

        Timer? autoTimer;                       // ��ʱ��
        Visualizer? visualizer;                 // ���ӻ���
        WaveFileWriter? waveFileWriter;         // �����ļ����
        bool enableCaptureAuto = false;         // �Ƿ��Զ��ɼ� (��ʱ����Ⱦ����Ļ)
        bool enableFftAuto = false;             // �Ƿ��Զ�FFT (��Ƶ����Ⱦ����Ļ)

        double[]? sampleDataToShow;             // ����������ʾ�Ĳ�������
        double[]? spectrumDataToShow;           // ����������ʾ��Ƶ������

        private void RefreshDevices(object sender, EventArgs e)
        {
            // ��ʼ���ɼ��豸�б�
            MMDeviceEnumerator mmDeviceEnumerator = new MMDeviceEnumerator();
            MMDeviceCollection mmDevices =
                mmDeviceEnumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);

            // ��ӵ�ѡ����
            devices.Items.Clear();
            foreach (var device in mmDevices)
                devices.Items.Add(device);
        }

        private void RefreshSerialPorts(object sender, EventArgs e)
        {
            serialPorts.Items.Clear();

            foreach (var name in SerialPort.GetPortNames())
                serialPorts.Items.Add(name);
        }

        private void InitSerialPortOptions()
        {
            bauds.Items.AddRange(new object[] { 43000, 56000, 57600, 115200, 128000, 230400, 256000, 460800 });
            bauds.SelectedIndex = 3;

            dataBitsPresets.Items.AddRange(new object[] { 5, 6, 7, 8 });
            dataBitsPresets.SelectedIndex = 3;

            parities.Items.AddRange(new object[] { Parity.None, Parity.Odd, Parity.Even, Parity.Mark, Parity.Space });
            parities.SelectedIndex = 0;

            stopBitsPresets.Items.AddRange(new object[] { StopBits.One, StopBits.OnePointFive, StopBits.Two });
            stopBitsPresets.SelectedIndex = 0;
        }

        private void InitCapture(object? sender, EventArgs e)
        {
            // һ��� if, �û����������У��
            if (!float.TryParse(magnification.Text, out float magnificationValue))
            {
                MessageBox.Show("Invalid magnification value, must be a float number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!float.TryParse(frequencyZoneMagnification.Text, out float frequencyZoneMagnificationValue))
            {
                MessageBox.Show("Invalid frequency zone magnification value, must be a float number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(visualizationRate.Text, out int visualizationRateValue) || visualizationRateValue < 1)
            {
                MessageBox.Show("Invalid capture rate value, must be an integer greator than 1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(captureCount.Text, out int captureCountValue) || captureCountValue < 100 || !IsPowerOfTwo(captureCountValue))
            {
                MessageBox.Show("Invalid capture count value, must be an integer greator than 100 and a power of 2", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(sampleRate.Text, out int sampleRateValue) || sampleRateValue < 1000)
            {
                MessageBox.Show("Invalid sample rate value, must be an integer greator than 1000", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // ����ָ���Ĳ�����, ����һ���µ�32λ���������θ�ʽ
            WaveFormat waveFormat =
                WaveFormat.CreateIeeeFloatWaveFormat(sampleRateValue, 1);

            // ����һ�����ӻ���
            visualizer = new Visualizer(captureCountValue);
            visualizer.DisableWindow = disableWindow.Checked;
            visualizer.DisableEasing = disableEasing.Checked;

            // �����ѡ�˱��浽�ļ�, �򴴽�һ�������ļ����
            if (saveToFile.Checked)
                waveFileWriter = new WaveFileWriter($"{DateTime.Now:yyMMdd-hhmmss}.wav", waveFormat);

            if (!useSerialPort.Checked)
            {
                if (selectedDevice == null)
                {
                    MessageBox.Show("Please select a device to capture", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ��ʼ���ɼ� (���� DataFlow �����Ǵ�����ͨ�Ĳ�׽���ǻػ���׽)
                if (selectedDevice.DataFlow == DataFlow.Capture)
                    capture = new WasapiCapture(selectedDevice);
                else
                    capture = new WasapiLoopbackCapture(selectedDevice);
            }
            else
            {
                if (selectedSerialPort == null)
                {
                    MessageBox.Show("Please select a serial port to capture", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (selectedSerialPortBoudRate == null ||
                    selectedSerialPortDataBits == null ||
                    selectedSerialPortParity == null ||
                    selectedSerialPortStopBits == null)
                {
                    MessageBox.Show("Please configure the serial port options correctly", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    SerialPort serialPort =
                        new SerialPort(
                            selectedSerialPort,
                            selectedSerialPortBoudRate.Value,
                            selectedSerialPortParity.Value,
                            selectedSerialPortDataBits.Value,
                            selectedSerialPortStopBits.Value);

                    capture = new SerialPortWaveIn(serialPort, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Cannot initialize serial port to capture. Exception: {ex.GetType().Name}, Message: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // ��ʼ���ɼ�
            capture.WaveFormat = waveFormat;
            capture.DataAvailable += Capture_DataAvailable;         // ���ɼ����ݿ��õ�ʱ��
            capture.StartRecording();

            // ����ʱ��Ƶ��ͼ�������
            timeZoneWaveVisual.Magnification = magnificationValue;
            frequencyZoneWaveVisual.Magnification = frequencyZoneMagnificationValue;

            // �Ƿ�����ƽ������
            timeZoneWaveVisual.EnableSmoothCurve = smoothImage.Checked;
            frequencyZoneWaveVisual.EnableSmoothCurve = smoothImage.Checked;

            // ���Զ��ɼ��� FFT ȫ������Ϊ false
            enableCaptureAuto = false;
            enableFftAuto = false;

            // ��ʱ��, �����Զ��ɼ�
            autoTimer = new Timer();
            autoTimer.Interval = 1000 / visualizationRateValue;     // ����ָ��Ƶ�ʵõ����
            autoTimer.Tick += AutoTimer_Tick;
            autoTimer.Start();

            SetStateToInitailizedCapture();
        }

        private void Capture_DataAvailable(object? sender, NAudio.Wave.WaveInEventArgs e)
        {
            if (visualizer == null)
                return;

            // ����в����ļ����, ��д��
            if (waveFileWriter != null)
                waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);

            // �õ�ʵ�ʵ����ݴ�С (ÿ4�ֽ�Ϊһ������)
            int length = e.BytesRecorded / 4;
            double[] result = new double[length];

            // ������������ת��Ϊ������
            for (int i = 0; i < length; i++)
                result[i] = BitConverter.ToSingle(e.Buffer, i * 4);

            // ���������͵����ӻ���
            visualizer.PushSampleData(result);
        }

        private void AutoTimer_Tick(object? sender, EventArgs e)
        {
            if (visualizer == null)
                return;

            // ����������ʾ���������ݺ�Ƶ������
            sampleDataToShow = visualizer.SampleData;
            spectrumDataToShow = visualizer.GetSpectrumData();

            // ����Զ��ɼ�, �򽫲�����Ⱦ����Ļ��
            if (enableCaptureAuto)
                CaptureOne(sender, e);

            // ����Զ� FFT, ��Ƶ����Ⱦ����Ļ��
            if (enableFftAuto)
                FftOne(sender, e);
        }

        private void CaptureOne(object? sender, EventArgs e)
        {
            if (visualizer == null)
                return;

            // ���ؼ������ݸ�ֵΪ��������, Ȼ����Ⱦ
            timeZoneWaveVisual.Data = sampleDataToShow;
            timeZoneWaveVisual.Render();
        }

        private void FftOne(object? sender, EventArgs e)
        {
            if (visualizer == null)
                return;

            // ���ؼ������ݸ�ֵΪƵ������, Ȼ����Ⱦ
            frequencyZoneWaveVisual.Data = spectrumDataToShow;
            frequencyZoneWaveVisual.Render();
        }

        private void DisposeCapture(object sender, EventArgs e)
        {
            // ���û�г�ʼ��, ����Ҫ�ر�
            if (capture == null ||
                autoTimer == null)
                return;

            // ����в����ļ����, �رղ�����ֶ�
            if (waveFileWriter != null)
            {
                waveFileWriter.Close();
                waveFileWriter = null;
            }

            // ֹͣ�ɼ�������ֶ�
            capture.StopRecording();
            capture.Dispose();
            capture = null;

            // ֹͣ��ʱ��������ֶ�
            autoTimer.Stop();
            autoTimer.Dispose();
            autoTimer = null;

            // ���ӻ���û�з��й�����, ֻ��Ҫ����ֶμ���
            visualizer = null;

            // �� UI ����Ϊδ��ʼ����״̬
            SetStateToNotInitailizedCapture();
        }

        /// <summary>
        /// �� UI ״̬����Ϊ�Ѿ���ʼ�� (����ʹ�òɼ���صĹ���, ������ѡ��)
        /// </summary>
        private void SetStateToInitailizedCapture()
        {
            optionsGroup.Enabled = false;

            initCapture.Enabled = false;
            captureOne.Enabled = true;
            captureAuto.Enabled = true;
            fftOne.Enabled = true;
            fftAuto.Enabled = true;
            disposeCapture.Enabled = true;
        }

        /// <summary>
        /// �� UI ״̬����Ϊδ��ʼ�� (������ʹ�òɼ���صĹ���, ������ѡ��)
        /// </summary>
        private void SetStateToNotInitailizedCapture()
        {
            optionsGroup.Enabled = true;

            initCapture.Enabled = true;
            captureOne.Enabled = false;
            captureAuto.Enabled = false;
            fftOne.Enabled = false;
            fftAuto.Enabled = false;
            disposeCapture.Enabled = false;
        }

        /// <summary>
        /// �л��Զ��ɼ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchCaptureAuto(object sender, EventArgs e)
        {
            enableCaptureAuto ^= true;
        }

        /// <summary>
        /// �л��Զ� FFT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchFftAuto(object sender, EventArgs e)
        {
            enableFftAuto ^= true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshDevices(sender, e);
            RefreshSerialPorts(sender, e);
        }

        /// <summary>
        /// ����ر�ʱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeCapture(sender, e);
        }

        /// <summary>
        /// �ж��Ƿ��� 2 ����������
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private bool IsPowerOfTwo(int num)
        {
            if (num < 1)
                return false;
            return (num & num - 1) == 0;
        }

        private void WaveVisualScroll(object? sender, MouseEventArgs e)
        {
            if (sender is not WaveVisual waveVisual)
                return;

            waveVisual.Magnification =
                waveVisual.Magnification * MathF.Pow(10, e.Delta / 500f);
        }

        private void BaudsSelectedValueChanged(object sender, EventArgs e)
        {
            if (bauds.SelectedItem is int baud)
                selectedSerialPortBoudRate = baud;
        }

        private void DataBitsPresetsSelectedValueChanged(object sender, EventArgs e)
        {
            if (dataBitsPresets.SelectedItem is int dataBits)
                selectedSerialPortDataBits = dataBits;
        }

        private void ParitiesSelectedValueChanged(object sender, EventArgs e)
        {
            if (parities.SelectedItem is Parity parity)
                selectedSerialPortParity = parity;
        }

        private void StopBitsPresetsSelectedValueChanged(object sender, EventArgs e)
        {
            if (stopBitsPresets.SelectedItem is StopBits stopBits)
                selectedSerialPortStopBits = stopBits;
        }

        private void DevicesSelectedValueChanged(object sender, EventArgs e)
        {
            // ��ѡ����ʱ, �� "selectedDevice" 
            if (devices.SelectedItem is MMDevice device)
                selectedDevice = device;
        }

        private void SerialPortsSelectedValueChanged(object sender, EventArgs e)
        {
            if (serialPorts.SelectedItem is string name)
                selectedSerialPort = name;
        }
    }
}