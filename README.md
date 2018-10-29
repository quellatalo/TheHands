# TheHands

This TheHands library provides methods to send Windows Inputs such as Mouse inputs and Keyboard inputs.

_This library was built and tested on Windows 10 (64bit) with .NET Framework 4.5._

### Installation

The package is available on Nuget: [Quellatalo.Nin.TheHands](https://www.nuget.org/packages/Quellatalo.Nin.TheHands/)

## Example code

```cs
/// <summary>
/// A test to run on windows 8 and later.
/// This code will access start menu, launch notepad, and do some typing actions
/// </summary>
void HandsTest()
{
    KeyboardHandler keyboard = new KeyboardHandler();
    keyboard.KeyTyping(Keys.LWin);
    Thread.Sleep(2222); // wait (for start menu to appear)
    keyboard.KeyTyping(Keys.N);
    keyboard.StringInput("otepad");
    Thread.Sleep(2222); // wait, it might take some time for slow PCs to find the program
    keyboard.KeyTyping(Keys.Enter);
    Thread.Sleep(2222); // wait (for start menu to appear)
    keyboard.StringInput("Kawaii is Justice!");
    keyboard.KeyTyping(Keys.Enter);
    Thread.Sleep(2222); // wait (just to see what is happening)

    // Set action delay (in milliseconds) for further keyboard inputs
    // in order to have a better feel of human-like actions
    keyboard.DefaultKeyboardActionDelay = 41;

    // Cut all current text to clipboard
    // ctrl + A, ctrl + X
    keyboard.KeyDown(Keys.LControlKey);
    keyboard.KeyTyping(Keys.A);
    keyboard.KeyTyping(Keys.X);
    keyboard.KeyUp(Keys.LControlKey);

    // Type something (Vv)
    keyboard.KeyDown(Keys.LShiftKey);
    keyboard.KeyTyping(Keys.V);
    keyboard.KeyUp(Keys.LShiftKey);
    keyboard.KeyTyping(Keys.V);
    keyboard.KeyTyping(Keys.Enter);

    // Paste the text stored in clipboard
    keyboard.KeyDown(Keys.LControlKey);
    keyboard.KeyTyping(Keys.V);
    keyboard.KeyUp(Keys.LControlKey);

    // Type something again
    keyboard.KeyDown(Keys.LShiftKey);
    keyboard.KeyTyping(Keys.V);
    keyboard.KeyUp(Keys.LShiftKey);
    keyboard.StringInput("ictory");
    keyboard.CharacterInput('!');
}
```
_The example only covered Keyboard part. For Mouse part, please refer to [TheEyes](https://github.com/quellatalo/TheEyes)._

License
----

MIT


**It's free. El Psy Congroo!**
