using System;
using System.Windows.Forms;
using AudioCapture.Controls;
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

            // 设置 UI 为未初始化状态
            SetStateToNotInitailizedCapture();

            // 一些不能通过设计器进行的初始化代码
            timeZoneWaveVisual.MouseWheel += WaveVisualScroll;
            frequencyZoneWaveVisual.MouseWheel += WaveVisualScroll;
        }

        MMDevice? selectedDevice;               // 选择的采集设备
        WasapiCapture? capture;                 // 采集器

        Timer? autoTimer;                       // 计时器
        Visualizer? visualizer;                 // 可视化器
        WaveFileWriter? waveFileWriter;         // 波形文件输出
        bool enableCaptureAuto = false;         // 是否自动采集 (将时域渲染到屏幕)
        bool enableFftAuto = false;             // 是否自动FFT (将频域渲染到屏幕)

        double[]? sampleDataToShow;             // 保存用于显示的采样数据
        double[]? spectrumDataToShow;           // 保存用于显示的频谱数据

        private void RefreshDevices(object sender, EventArgs e)
        {
            // 初始化采集设备列表
            MMDeviceEnumerator mmDeviceEnumerator = new MMDeviceEnumerator();
            MMDeviceCollection mmDevices =
                mmDeviceEnumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);

            // 添加到选框中
            devices.Items.Clear();
            foreach (var device in mmDevices)
                devices.Items.Add(device);
        }

        private void DevicesSelectedIndexChanged(object sender, EventArgs e)
        {
            // 当选择变更时, 给 "selectedDevice" 
            if (devices.SelectedItem is MMDevice device)
                selectedDevice = device;
        }

        private void InitCapture(object? sender, EventArgs e)
        {
            // 一大堆 if, 用户输入的数据校验

            if (selectedDevice == null)
            {
                MessageBox.Show("Please select a device to capture", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            // 根据指定的采样率, 创建一个新的32位浮点数波形格式
            WaveFormat waveFormat =
                WaveFormat.CreateIeeeFloatWaveFormat(sampleRateValue, 1);

            // 创建一个可视化器
            visualizer = new Visualizer(captureCountValue);
            visualizer.DisableWindow = disableWindow.Checked;
            visualizer.DisableEasing = disableEasing.Checked;

            // 如果勾选了保存到文件, 则创建一个波形文件输出
            if (saveToFile.Checked)
                waveFileWriter = new WaveFileWriter($"{DateTime.Now:yyMMdd-hhmmss}.wav", waveFormat);

            // 初始化采集 (根据 DataFlow 决定是创建普通的捕捉还是回环捕捉)
            if (selectedDevice.DataFlow == DataFlow.Capture)
                capture = new WasapiCapture(selectedDevice);
            else
                capture = new WasapiLoopbackCapture(selectedDevice);

            // 初始化采集
            capture.WaveFormat = waveFormat;
            capture.DataAvailable += Capture_DataAvailable;         // 当采集数据可用的时候
            capture.StartRecording();

            // 设置时域频域图像的缩放
            timeZoneWaveVisual.Magnification = magnificationValue;
            frequencyZoneWaveVisual.Magnification = frequencyZoneMagnificationValue;

            // 是否启用平滑曲线
            timeZoneWaveVisual.EnableSmoothCurve = smoothImage.Checked;
            frequencyZoneWaveVisual.EnableSmoothCurve = smoothImage.Checked;

            // 将自动采集和 FFT 全都重置为 false
            enableCaptureAuto = false;
            enableFftAuto = false;

            // 计时器, 用于自动采集
            autoTimer = new Timer();
            autoTimer.Interval = 1000 / visualizationRateValue;     // 根据指定频率得到间隔
            autoTimer.Tick += AutoTimer_Tick;
            autoTimer.Start();

            SetStateToInitailizedCapture();
        }

        private void Capture_DataAvailable(object? sender, NAudio.Wave.WaveInEventArgs e)
        {
            if (visualizer == null)
                return;

            // 如果有波形文件输出, 则写入
            if (waveFileWriter != null)
                waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);

            // 拿到实际的数据大小 (每4字节为一个采样)
            int length = e.BytesRecorded / 4;
            double[] result = new double[length];

            // 将二进制数据转换为浮点数
            for (int i = 0; i < length; i++)
                result[i] = BitConverter.ToSingle(e.Buffer, i * 4);

            // 将数据推送到可视化器
            visualizer.PushSampleData(result);
        }

        private void AutoTimer_Tick(object? sender, EventArgs e)
        {
            if (visualizer == null)
                return;

            // 保存用于显示的样本数据和频谱数据
            sampleDataToShow = visualizer.SampleData;
            spectrumDataToShow = visualizer.GetSpectrumData();

            // 如果自动采集, 则将采样渲染到屏幕上
            if (enableCaptureAuto)
                CaptureOne(sender, e);

            // 如果自动 FFT, 则将频谱渲染到屏幕上
            if (enableFftAuto)
                FftOne(sender, e);
        }

        private void CaptureOne(object? sender, EventArgs e)
        {
            if (visualizer == null)
                return;

            // 将控件的数据赋值为样本数据, 然后渲染
            timeZoneWaveVisual.Data = sampleDataToShow;
            timeZoneWaveVisual.Render();
        }

        private void FftOne(object? sender, EventArgs e)
        {
            if (visualizer == null)
                return;

            // 将控件的数据赋值为频谱数据, 然后渲染
            frequencyZoneWaveVisual.Data = spectrumDataToShow;
            frequencyZoneWaveVisual.Render();
        }

        private void DisposeCapture(object sender, EventArgs e)
        {
            // 如果没有初始化, 则不需要关闭
            if (capture == null ||
                autoTimer == null)
                return;

            // 如果有波形文件输出, 关闭并清空字段
            if (waveFileWriter != null)
            {
                waveFileWriter.Close();
                waveFileWriter = null;
            }

            // 停止采集并清空字段
            capture.StopRecording();
            capture.Dispose();
            capture = null;

            // 停止计时器并清空字段
            autoTimer.Stop();
            autoTimer.Dispose();
            autoTimer = null;

            // 可视化器没有非托管数据, 只需要清空字段即可
            visualizer = null;

            // 将 UI 设置为未初始化的状态
            SetStateToNotInitailizedCapture();
        }

        /// <summary>
        /// 将 UI 状态设置为已经初始化 (可以使用采集相关的功能, 并禁用选项)
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
        /// 将 UI 状态设置为未初始化 (不可以使用采集相关的功能, 并启用选项)
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
        /// 切换自动采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchCaptureAuto(object sender, EventArgs e)
        {
            enableCaptureAuto ^= true;
        }

        /// <summary>
        /// 切换自动 FFT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchFftAuto(object sender, EventArgs e)
        {
            enableFftAuto ^= true;
        }

        /// <summary>
        /// 窗体关闭时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeCapture(sender, e);
        }

        /// <summary>
        /// 判断是否是 2 的整数次幂
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
    }
}