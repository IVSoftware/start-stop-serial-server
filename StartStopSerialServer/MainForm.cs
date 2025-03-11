using MockSerialPort;

namespace StartStopSerialServer
{
    public partial class MainForm : Form
    {
        SerialPort _serialPort = new();
        public MainForm()
        {
            InitializeComponent();
            checkBoxToggleServer.CheckedChanged += async (sender, e) =>
            {
                if (checkBoxToggleServer.Checked)
                {
                    _cts?.Cancel();
                    // Wait for previous run (if any) to cancel
                    await _awaiter.WaitAsync();
                    _cts = new CancellationTokenSource();
                    try
                    {
                        txtbox_log.AppendText("Serial Server Started", true, Color.Green);
                        while (true)
                        {
                            _cts.Token.ThrowIfCancellationRequested();
                            txtbox_log.AppendText($@"[{DateTime.Now:hh\:mm\:ss\ tt}] TEST! I'm running", true, Color.Blue);
                            await Task.Delay(TimeSpan.FromSeconds(2.5), _cts.Token);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        txtbox_log.AppendText("Serial Server Canceled", true, Color.Maroon);
                        checkBoxToggleServer.Checked = false;
                        _awaiter.Wait(0);
                        _awaiter.Release();
                    }
                }
                else if(_cts is not null && !_cts.IsCancellationRequested) _cts.Cancel();
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
