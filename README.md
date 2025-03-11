You post is a bit tricky because it asks about stopping a thread (we'll call that "X") but strongly implies that reading `SerialPort` data is the ultimate goal (that we'll call "Y").

___

**Stopping the Thread (X)**

If the code that you poted is the code you wish to keep, you might want to experiment with making the loop asynchronous so that you don't lose the UI thread context while the background work proceeds on an alternate thread.

```
```
