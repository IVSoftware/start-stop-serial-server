
___

**Reading Serial Port Data**

What I have found is that retrieving asynchronous data from a `SerialPort` takes on a different flavor because we're often listening to the `DataReceived` event and responding on an interrupt basis. This code snippet:

- Opens or Closes the Serial Port based on the button toggle.
- Is listening for data to become available by subscribing to the `DataReceived` event (in this case, using an inline Lambda method).
- Marshals the data onto the UI thread in order to display it.

```
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
                                txtbox_log.AppendText($@"[{DateTime.Now:hh\:mm\:ss.ff tt}] ", false, Color.CornflowerBlue);
                                txtbox_log.AppendText( BitConverter.ToString(buffer, 0, success).Replace("-", " "),  true);
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
                txtbox_log.AppendText($"Serial Server Started", true, Color.Green);
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
```

[![responding to SerialPort DataReceived](https://i.sstatic.net/rEmgEV5k.png)](https://i.sstatic.net/rEmgEV5k.png)