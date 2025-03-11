
**Stopping the Thread (X)**

If the code that you posted is the code you wish to keep (i.e. having a polling loop) then you might want to experiment with making the loop asynchronous so that you don't lose the UI thread context while the background work proceeds on an alternate thread. In this snippet:

- The Task.Delay makes use of the `CancellationToken` to exit the delay immediately.
- We make a point of catching the `OperationCancelled` exception.
- At the top of each loop iteration, we have the token `throw` if it's been cancelled.

```
public partial class MainForm : Form
{
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
                        var timestamp = $@"[{DateTime.Now:hh\:mm\:ss\ tt}] ";
                        txtbox_log.AppendText($"{timestamp} TEST! I'm running", true, Color.Blue);
                        await Task.Delay(TimeSpan.FromSeconds(1.5), _cts.Token); // Taken from posted code.
                        await Task.Run(() => 
                        {
                            // "do some more serial stuff here"
                            _cts.Token.ThrowIfCancellationRequested();
                        }, _cts.Token);
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
            else
            {
                if (_cts is not null && !_cts.IsCancellationRequested) _cts.Cancel();
            }
        };
    }
    SemaphoreSlim 
        _awaiter = new SemaphoreSlim(1, 1),
        _criticalSection = new SemaphoreSlim(1, 1);
    CancellationTokenSource? _cts = null;
}
```

[![stopping the thread](https://i.sstatic.net/wQAJvQY8.png)](https://i.sstatic.net/wQAJvQY8.png)

##### Where `AppendText` is an Extension for RichTextBox.

```
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
```
