# TheHands

This TheHands library provides methods to send Windows Inputs such as Mouse inputs and Keyboard inputs. A part of alternative tools to [sikulix](http://sikulix.com/).

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
    Keyboard.KeyTyping(Keys.LWin);
    Thread.Sleep(2222); // wait (for start menu to appear)
    Keyboard.StringInput("notepad");
    Thread.Sleep(2222); // wait, it might take some time for slow PCs to find the program
    Keyboard.KeyTyping(Keys.Enter);
    Thread.Sleep(2222); // wait (for start menu to appear)
    Keyboard.StringInput("Kawaii is Justice!");
    Keyboard.KeyTyping(Keys.Enter);
    Thread.Sleep(2222); // wait (just to see what is happening)

    // Set action delay (in milliseconds) for further keyboard inputs
    // in order to have a better feel of human-like actions
    Keyboard.DefaultKeyboardActionDelay = 41;

    // Cut all current text to clipboard
    // ctrl + A, ctrl + X
    Keyboard.KeyDown(Keys.LControlKey);
    Keyboard.KeyTyping(Keys.A);
    Keyboard.KeyTyping(Keys.X);
    Keyboard.KeyUp(Keys.LControlKey);

    // Type something (Vv)
    Keyboard.KeyDown(Keys.LShiftKey);
    Keyboard.KeyTyping(Keys.V);
    Keyboard.KeyUp(Keys.LShiftKey);
    Keyboard.KeyTyping(Keys.V);
    Keyboard.KeyTyping(Keys.Enter);

    // Paste the text stored in clipboard
    Keyboard.KeyDown(Keys.LControlKey);
    Keyboard.KeyTyping(Keys.V);
    Keyboard.KeyUp(Keys.LControlKey);

    // Type something again
    Keyboard.KeyDown(Keys.LShiftKey);
    Keyboard.KeyTyping(Keys.V);
    Keyboard.KeyUp(Keys.LShiftKey);
    Keyboard.StringInput("ictory");
    Keyboard.CharacterInput('!');
}
```
_The example only covered Keyboard part. For Mouse part, please refer to [TheEyes](https://github.com/quellatalo/TheEyes)._

License
----

MIT


**It's free. El Psy Congroo!**
