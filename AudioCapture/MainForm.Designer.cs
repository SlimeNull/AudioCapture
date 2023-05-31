using System.Drawing;
using System.Windows.Forms;

namespace AudioCapture
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            splitWaveContainer = new SplitContainer();
            timeZonePanel = new Panel();
            timeZoneWaveVisual = new Controls.WaveVisual();
            frequencyZonePanel = new Panel();
            frequencyZoneWaveVisual = new Controls.WaveVisual();
            imagePanel = new Panel();
            initCapture = new Button();
            captureOne = new Button();
            captureAuto = new Button();
            fftOne = new Button();
            fftAuto = new Button();
            disposeCapture = new Button();
            optionsGroup = new GroupBox();
            useSerialPort = new CheckBox();
            lbRefreshSerialPorts = new LinkLabel();
            lbStopBits = new Label();
            stopBitsPresets = new ComboBox();
            lbParity = new Label();
            parities = new ComboBox();
            lbDataBits = new Label();
            dataBitsPresets = new ComboBox();
            lbBaud = new Label();
            bauds = new ComboBox();
            lbSerialPorts = new Label();
            serialPorts = new ComboBox();
            lbRefreshDevices = new LinkLabel();
            lbSampleRate = new Label();
            sampleRate = new TextBox();
            lbCaptureCount = new Label();
            captureCount = new TextBox();
            lbVisualizationRate = new Label();
            visualizationRate = new TextBox();
            lbFrequencyZoneMagnification = new Label();
            frequencyZoneMagnification = new TextBox();
            lbMagnification = new Label();
            magnification = new TextBox();
            disableEasing = new CheckBox();
            disableWindow = new CheckBox();
            saveToFile = new CheckBox();
            smoothImage = new CheckBox();
            lbDevice = new Label();
            devices = new ComboBox();
            consoleGroup = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)splitWaveContainer).BeginInit();
            splitWaveContainer.Panel1.SuspendLayout();
            splitWaveContainer.Panel2.SuspendLayout();
            splitWaveContainer.SuspendLayout();
            timeZonePanel.SuspendLayout();
            frequencyZonePanel.SuspendLayout();
            imagePanel.SuspendLayout();
            optionsGroup.SuspendLayout();
            consoleGroup.SuspendLayout();
            SuspendLayout();
            // 
            // splitWaveContainer
            // 
            resources.ApplyResources(splitWaveContainer, "splitWaveContainer");
            splitWaveContainer.Name = "splitWaveContainer";
            // 
            // splitWaveContainer.Panel1
            // 
            resources.ApplyResources(splitWaveContainer.Panel1, "splitWaveContainer.Panel1");
            splitWaveContainer.Panel1.Controls.Add(timeZonePanel);
            // 
            // splitWaveContainer.Panel2
            // 
            resources.ApplyResources(splitWaveContainer.Panel2, "splitWaveContainer.Panel2");
            splitWaveContainer.Panel2.Controls.Add(frequencyZonePanel);
            // 
            // timeZonePanel
            // 
            resources.ApplyResources(timeZonePanel, "timeZonePanel");
            timeZonePanel.BorderStyle = BorderStyle.FixedSingle;
            timeZonePanel.Controls.Add(timeZoneWaveVisual);
            timeZonePanel.Name = "timeZonePanel";
            // 
            // timeZoneWaveVisual
            // 
            resources.ApplyResources(timeZoneWaveVisual, "timeZoneWaveVisual");
            timeZoneWaveVisual.CenterWave = true;
            timeZoneWaveVisual.Data = null;
            timeZoneWaveVisual.EnableSmoothCurve = false;
            timeZoneWaveVisual.Magnification = 1F;
            timeZoneWaveVisual.Name = "timeZoneWaveVisual";
            // 
            // frequencyZonePanel
            // 
            resources.ApplyResources(frequencyZonePanel, "frequencyZonePanel");
            frequencyZonePanel.BorderStyle = BorderStyle.FixedSingle;
            frequencyZonePanel.Controls.Add(frequencyZoneWaveVisual);
            frequencyZonePanel.Name = "frequencyZonePanel";
            // 
            // frequencyZoneWaveVisual
            // 
            resources.ApplyResources(frequencyZoneWaveVisual, "frequencyZoneWaveVisual");
            frequencyZoneWaveVisual.CenterWave = false;
            frequencyZoneWaveVisual.Data = null;
            frequencyZoneWaveVisual.EnableSmoothCurve = false;
            frequencyZoneWaveVisual.Magnification = 1F;
            frequencyZoneWaveVisual.Name = "frequencyZoneWaveVisual";
            // 
            // imagePanel
            // 
            resources.ApplyResources(imagePanel, "imagePanel");
            imagePanel.BorderStyle = BorderStyle.FixedSingle;
            imagePanel.Controls.Add(splitWaveContainer);
            imagePanel.Name = "imagePanel";
            // 
            // initCapture
            // 
            resources.ApplyResources(initCapture, "initCapture");
            initCapture.Name = "initCapture";
            initCapture.UseVisualStyleBackColor = true;
            initCapture.Click += InitCapture;
            // 
            // captureOne
            // 
            resources.ApplyResources(captureOne, "captureOne");
            captureOne.Name = "captureOne";
            captureOne.UseVisualStyleBackColor = true;
            captureOne.Click += CaptureOne;
            // 
            // captureAuto
            // 
            resources.ApplyResources(captureAuto, "captureAuto");
            captureAuto.Name = "captureAuto";
            captureAuto.UseVisualStyleBackColor = true;
            captureAuto.Click += SwitchCaptureAuto;
            // 
            // fftOne
            // 
            resources.ApplyResources(fftOne, "fftOne");
            fftOne.Name = "fftOne";
            fftOne.UseVisualStyleBackColor = true;
            fftOne.Click += FftOne;
            // 
            // fftAuto
            // 
            resources.ApplyResources(fftAuto, "fftAuto");
            fftAuto.Name = "fftAuto";
            fftAuto.UseVisualStyleBackColor = true;
            fftAuto.Click += SwitchFftAuto;
            // 
            // disposeCapture
            // 
            resources.ApplyResources(disposeCapture, "disposeCapture");
            disposeCapture.Name = "disposeCapture";
            disposeCapture.UseVisualStyleBackColor = true;
            disposeCapture.Click += DisposeCapture;
            // 
            // optionsGroup
            // 
            resources.ApplyResources(optionsGroup, "optionsGroup");
            optionsGroup.Controls.Add(useSerialPort);
            optionsGroup.Controls.Add(lbRefreshSerialPorts);
            optionsGroup.Controls.Add(lbStopBits);
            optionsGroup.Controls.Add(stopBitsPresets);
            optionsGroup.Controls.Add(lbParity);
            optionsGroup.Controls.Add(parities);
            optionsGroup.Controls.Add(lbDataBits);
            optionsGroup.Controls.Add(dataBitsPresets);
            optionsGroup.Controls.Add(lbBaud);
            optionsGroup.Controls.Add(bauds);
            optionsGroup.Controls.Add(lbSerialPorts);
            optionsGroup.Controls.Add(serialPorts);
            optionsGroup.Controls.Add(lbRefreshDevices);
            optionsGroup.Controls.Add(lbSampleRate);
            optionsGroup.Controls.Add(sampleRate);
            optionsGroup.Controls.Add(lbCaptureCount);
            optionsGroup.Controls.Add(captureCount);
            optionsGroup.Controls.Add(lbVisualizationRate);
            optionsGroup.Controls.Add(visualizationRate);
            optionsGroup.Controls.Add(lbFrequencyZoneMagnification);
            optionsGroup.Controls.Add(frequencyZoneMagnification);
            optionsGroup.Controls.Add(lbMagnification);
            optionsGroup.Controls.Add(magnification);
            optionsGroup.Controls.Add(disableEasing);
            optionsGroup.Controls.Add(disableWindow);
            optionsGroup.Controls.Add(saveToFile);
            optionsGroup.Controls.Add(smoothImage);
            optionsGroup.Controls.Add(lbDevice);
            optionsGroup.Controls.Add(devices);
            optionsGroup.Name = "optionsGroup";
            optionsGroup.TabStop = false;
            // 
            // useSerialPort
            // 
            resources.ApplyResources(useSerialPort, "useSerialPort");
            useSerialPort.Name = "useSerialPort";
            useSerialPort.UseVisualStyleBackColor = true;
            // 
            // lbRefreshSerialPorts
            // 
            resources.ApplyResources(lbRefreshSerialPorts, "lbRefreshSerialPorts");
            lbRefreshSerialPorts.LinkColor = Color.FromArgb(16, 63, 145);
            lbRefreshSerialPorts.Name = "lbRefreshSerialPorts";
            lbRefreshSerialPorts.TabStop = true;
            lbRefreshSerialPorts.LinkClicked += RefreshSerialPorts;
            // 
            // lbStopBits
            // 
            resources.ApplyResources(lbStopBits, "lbStopBits");
            lbStopBits.Name = "lbStopBits";
            // 
            // stopBitsPresets
            // 
            resources.ApplyResources(stopBitsPresets, "stopBitsPresets");
            stopBitsPresets.DropDownStyle = ComboBoxStyle.DropDownList;
            stopBitsPresets.FormattingEnabled = true;
            stopBitsPresets.Name = "stopBitsPresets";
            stopBitsPresets.SelectedValueChanged += StopBitsPresetsSelectedValueChanged;
            // 
            // lbParity
            // 
            resources.ApplyResources(lbParity, "lbParity");
            lbParity.Name = "lbParity";
            // 
            // parities
            // 
            resources.ApplyResources(parities, "parities");
            parities.DropDownStyle = ComboBoxStyle.DropDownList;
            parities.FormattingEnabled = true;
            parities.Name = "parities";
            parities.SelectedValueChanged += ParitiesSelectedValueChanged;
            // 
            // lbDataBits
            // 
            resources.ApplyResources(lbDataBits, "lbDataBits");
            lbDataBits.Name = "lbDataBits";
            // 
            // dataBitsPresets
            // 
            resources.ApplyResources(dataBitsPresets, "dataBitsPresets");
            dataBitsPresets.DropDownStyle = ComboBoxStyle.DropDownList;
            dataBitsPresets.FormattingEnabled = true;
            dataBitsPresets.Name = "dataBitsPresets";
            dataBitsPresets.SelectedValueChanged += DataBitsPresetsSelectedValueChanged;
            // 
            // lbBaud
            // 
            resources.ApplyResources(lbBaud, "lbBaud");
            lbBaud.Name = "lbBaud";
            // 
            // bauds
            // 
            resources.ApplyResources(bauds, "bauds");
            bauds.DropDownStyle = ComboBoxStyle.DropDownList;
            bauds.FormattingEnabled = true;
            bauds.Name = "bauds";
            bauds.SelectedValueChanged += BaudsSelectedValueChanged;
            // 
            // lbSerialPorts
            // 
            resources.ApplyResources(lbSerialPorts, "lbSerialPorts");
            lbSerialPorts.Name = "lbSerialPorts";
            // 
            // serialPorts
            // 
            resources.ApplyResources(serialPorts, "serialPorts");
            serialPorts.DropDownStyle = ComboBoxStyle.DropDownList;
            serialPorts.FormattingEnabled = true;
            serialPorts.Name = "serialPorts";
            serialPorts.SelectedValueChanged += SerialPortsSelectedValueChanged;
            // 
            // lbRefreshDevices
            // 
            resources.ApplyResources(lbRefreshDevices, "lbRefreshDevices");
            lbRefreshDevices.LinkColor = Color.FromArgb(16, 63, 145);
            lbRefreshDevices.Name = "lbRefreshDevices";
            lbRefreshDevices.TabStop = true;
            lbRefreshDevices.LinkClicked += RefreshDevices;
            // 
            // lbSampleRate
            // 
            resources.ApplyResources(lbSampleRate, "lbSampleRate");
            lbSampleRate.Name = "lbSampleRate";
            // 
            // sampleRate
            // 
            resources.ApplyResources(sampleRate, "sampleRate");
            sampleRate.Name = "sampleRate";
            // 
            // lbCaptureCount
            // 
            resources.ApplyResources(lbCaptureCount, "lbCaptureCount");
            lbCaptureCount.Name = "lbCaptureCount";
            // 
            // captureCount
            // 
            resources.ApplyResources(captureCount, "captureCount");
            captureCount.Name = "captureCount";
            // 
            // lbVisualizationRate
            // 
            resources.ApplyResources(lbVisualizationRate, "lbVisualizationRate");
            lbVisualizationRate.Name = "lbVisualizationRate";
            // 
            // visualizationRate
            // 
            resources.ApplyResources(visualizationRate, "visualizationRate");
            visualizationRate.Name = "visualizationRate";
            // 
            // lbFrequencyZoneMagnification
            // 
            resources.ApplyResources(lbFrequencyZoneMagnification, "lbFrequencyZoneMagnification");
            lbFrequencyZoneMagnification.Name = "lbFrequencyZoneMagnification";
            // 
            // frequencyZoneMagnification
            // 
            resources.ApplyResources(frequencyZoneMagnification, "frequencyZoneMagnification");
            frequencyZoneMagnification.Name = "frequencyZoneMagnification";
            // 
            // lbMagnification
            // 
            resources.ApplyResources(lbMagnification, "lbMagnification");
            lbMagnification.Name = "lbMagnification";
            // 
            // magnification
            // 
            resources.ApplyResources(magnification, "magnification");
            magnification.Name = "magnification";
            // 
            // disableEasing
            // 
            resources.ApplyResources(disableEasing, "disableEasing");
            disableEasing.Name = "disableEasing";
            disableEasing.UseVisualStyleBackColor = true;
            // 
            // disableWindow
            // 
            resources.ApplyResources(disableWindow, "disableWindow");
            disableWindow.Name = "disableWindow";
            disableWindow.UseVisualStyleBackColor = true;
            // 
            // saveToFile
            // 
            resources.ApplyResources(saveToFile, "saveToFile");
            saveToFile.Name = "saveToFile";
            saveToFile.UseVisualStyleBackColor = true;
            // 
            // smoothImage
            // 
            resources.ApplyResources(smoothImage, "smoothImage");
            smoothImage.Name = "smoothImage";
            smoothImage.UseVisualStyleBackColor = true;
            // 
            // lbDevice
            // 
            resources.ApplyResources(lbDevice, "lbDevice");
            lbDevice.Name = "lbDevice";
            // 
            // devices
            // 
            resources.ApplyResources(devices, "devices");
            devices.DropDownStyle = ComboBoxStyle.DropDownList;
            devices.FormattingEnabled = true;
            devices.Name = "devices";
            devices.SelectedValueChanged += DevicesSelectedValueChanged;
            // 
            // consoleGroup
            // 
            resources.ApplyResources(consoleGroup, "consoleGroup");
            consoleGroup.Controls.Add(initCapture);
            consoleGroup.Controls.Add(captureOne);
            consoleGroup.Controls.Add(captureAuto);
            consoleGroup.Controls.Add(fftOne);
            consoleGroup.Controls.Add(disposeCapture);
            consoleGroup.Controls.Add(fftAuto);
            consoleGroup.Name = "consoleGroup";
            consoleGroup.TabStop = false;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(consoleGroup);
            Controls.Add(optionsGroup);
            Controls.Add(imagePanel);
            Name = "MainForm";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            splitWaveContainer.Panel1.ResumeLayout(false);
            splitWaveContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitWaveContainer).EndInit();
            splitWaveContainer.ResumeLayout(false);
            timeZonePanel.ResumeLayout(false);
            frequencyZonePanel.ResumeLayout(false);
            imagePanel.ResumeLayout(false);
            optionsGroup.ResumeLayout(false);
            optionsGroup.PerformLayout();
            consoleGroup.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel imagePanel;
        private Button initCapture;
        private Button captureOne;
        private Button captureAuto;
        private Button fftOne;
        private Button fftAuto;
        private Button disposeCapture;
        private GroupBox optionsGroup;
        private GroupBox consoleGroup;
        private Label lbDevice;
        private ComboBox devices;
        private CheckBox smoothImage;
        private Label lbVisualizationRate;
        private TextBox visualizationRate;
        private Label lbMagnification;
        private TextBox magnification;
        private SplitContainer splitWaveContainer;
        private Panel timeZonePanel;
        private Panel frequencyZonePanel;
        private Label lbCaptureCount;
        private TextBox captureCount;
        private Label lbSampleRate;
        private TextBox sampleRate;
        private Label lbFrequencyZoneMagnification;
        private TextBox frequencyZoneMagnification;
        private CheckBox saveToFile;
        private LinkLabel lbRefreshDevices;
        private CheckBox disableEasing;
        private CheckBox disableWindow;
        private LinkLabel lbRefreshSerialPorts;
        private Label lbSerialPorts;
        private ComboBox serialPorts;
        private CheckBox useSerialPort;
        private Label lbBaud;
        private ComboBox bauds;
        private Label lbDataBits;
        private ComboBox dataBitsPresets;
        private Label lbParity;
        private ComboBox parities;
        private Label lbStopBits;
        private ComboBox stopBitsPresets;
        private Controls.WaveVisual timeZoneWaveVisual;
        private Controls.WaveVisual frequencyZoneWaveVisual;
    }
}