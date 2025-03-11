using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;

namespace MockSerialPort
{
    public enum SerialData{Chars = 1, Eof }
    public class SerialPort 
    {
        Random _rando = new Random(3);
        public int BytesToRead => _rx.Count;
        public void Open() => IsOpen = true;
        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (!Equals(_isOpen, value))
                {
                    _isOpen = value;
                    if(_isOpen) Run();
                    else
                    {
                        _cts?.Cancel();
                    }
                }
            }
        }
        bool _isOpen = default;


        public event EventHandler<SerialDataReceivedEventArgs> DataReceived;

        public int Read(byte[] buffer, int offset, int count)
        {
            int success;
            for(success = 0; success < count; success++, offset++)
            {
                if (_rx.Count == 0) break;
                buffer[offset] = _rx.Dequeue();
            }
            return success;
        }

        private async void Run()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
            }
            if (_run != null)
            {
                await _run;
                _run.Dispose();
            }
            _cts = new CancellationTokenSource();
            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(0.5 + (1 * _rando.NextDouble())), _cts.Token);
                    byte[] bytes = new byte[_rando.Next(1, 9)];
                    _rando.NextBytes(bytes);
                    foreach (var @byte in bytes) _rx.Enqueue(@byte);
                    DataReceived?.Invoke(this, new SerialDataReceivedEventArgs(SerialData.Chars));
                }
            }
            catch (OperationCanceledException)
            {
                Close();
            }
        }
        public void Close() => IsOpen = false;
        private Queue<byte> _rx = new Queue<byte>();
        Task _run = null;
        CancellationTokenSource _cts = null;
    }
    public class SerialDataReceivedEventArgs : EventArgs
    {
        public SerialData EventType { get; }
        internal SerialDataReceivedEventArgs(SerialData eventCode) =>
            EventType = eventCode;
    }
}
