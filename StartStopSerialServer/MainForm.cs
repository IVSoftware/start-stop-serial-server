using MockSerialPort;

namespace StartStopSerialServer
{
    public partial class MainForm : Form
    {
        SerialPort _serialPort = new();
        public MainForm()
        {
            InitializeComponent();

            _serialPort.DataReceived += async (sender, e) =>
            {
                await _criticalSection.WaitAsync();
                if (!IsDisposed) BeginInvoke((MethodInvoker)delegate
                {
                    try
                    {
                        if (sender is SerialPort port)
                        {
                            while (port.BytesToRead > 0)
                            {
                                byte[] buffer = new byte[16];
                                int success = port.Read(buffer, 0, buffer.Length);
                                BeginInvoke(() =>
                                {
                                    txtbox_log.AppendText(
                                        BitConverter.ToString(buffer, 0, success).Replace("-", " "),
                                        true);
                                });
                            }
                        }
                    }
                    finally
                    {
                        _criticalSection.Release();
                    }
                });
            };

            checkBoxToggleServer.CheckedChanged += (sender, e) =>
            {
                if (checkBoxToggleServer.Checked)
                {
                    _serialPort.Open();
                    txtbox_log.AppendText("Serial Server Started", true, Color.Green);
                }
                else
                {
                    _serialPort.Close();
                    txtbox_log.AppendText("Serial Server Canceled", true, Color.Maroon);
                }
            };
        }
        SemaphoreSlim 
            _awaiter = new SemaphoreSlim(1, 1),
            _criticalSection = new SemaphoreSlim(1, 1);
        CancellationTokenSource? _cts = null;
    }
    static class Extensions
    {
        public static void AppendText(this RichTextBox @this, string text, bool newLine, Color? color = null)
        {
            var colorB4 = @this.SelectionColor;
            if(color is Color altColor) @this.SelectionColor = altColor;
            @this.AppendText($"{text}{(newLine ? Environment.NewLine : string.Empty)}");
            @this.SelectionColor = colorB4;
        }
    }
}
