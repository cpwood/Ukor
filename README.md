# Ukor
A fake Roku Streaming Stick device that can be used as gateway to enable home automation.

## What is it? Why was it written?

I have a Logitech Harmony remote at home and it's connected to a few different devices: Fire TV Cubes, Chromecasts, Rokus, amplifiers, light bulbs, etc. However, I ran into a use case where as part of an activity, I wanted to do something slightly unusual (change the behaviour of my DNS server). 

Unfortunately, Harmony doesn't allow you to do such things; you can work with a very specific range of devices, but not beyond those.

The Harmony system can, however, speak to Roku devices and uses HTTP requests to do so.

I decided to see whether I could build a *fake* Roku Streaming Stick+ - i.e. something that imitates the device's simple XML-based HTTP API. It would receive instructions from the Harmony Hub, but rather doing what was requested (e.g. launch a channel or fast-forward), it would do something completely different that could be defined through configuration - either via a command line task or a C# plugin.

Ukor is so-named because it's Roku spelled backwards. I'm clearly not very imaginative!

## What can it do?

Ukor speaks "SSDP" - a Universal Plug And Play protocol - so it can be discovered on your home network by home automation systems such as Logitech's Harmony Hub. 

Once discovered, Ukor has been designed to launch applications in two ways:

* using a sequence of Roku button presses; or
* by launching a channel.

For example, you could get a Harmony activity to send `Fwd, Fwd` (i.e. the fast-forward button twice) and use this sequence to launch an executable. 

Alternatively, all your configured Ukor applications can be launched as if they were a channel. So much like you'd ordinarily tell a *real* Roku device to launch the Netflix channel, you could instead tell your fake Roku to launch your "Turn On Intruder Alarm" channel.

The reason both methods are offered is that the Harmony Hub only allows you to launch channels in a "Watch TV" activity, which isn't always ideally suited for every Harmony activity. Just being able to do stuff on a simple button press avoids those limitations. However, the channel-based approach may be easier on other automation platforms.

## What can't it do?

Hopefully it's obvious by now: it's *not* a real Roku. So you can't install apps, watch videos or anything like that! 

It's simply a means-to-an-end of opening up more automation possibilities within the home.

When setting up Ukor within your home automation system, you'll probably be asked to confirm that your Roku device is now showing stuff on your television. Obviously, just lie and say "yes" in this scenario. We both know it's never going to actually display anything!

## What are the pre-requisites to run Ukor?

To run Ukor, you'll need the [.NET Core 3.1 runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1).

To develop using Ukor, you'll need Visual Studio or Visual Studio Code.

## Where do I get the software?

Either clone this repository and compile the code yourself, or [download the latest compiled release](https://github.com/cpwood/Ukor/releases).

## How do I configure it?

### Software Configuration

All configuration happens via `appsettings.json` . 

Within the file, you'll see an array called `Applications`. This is an individual application:

```json
    {
      "Id": 1000,
      "Name": "Set DNS",
      "Action": "CSharp",
      "CSharpDetails": {
        "AssemblyPath": "C:\\MyPlugin\\Chris.Ukor.dll",
        "ClassName": "SetDns"
      },
      "LaunchKeySequence": [
        "Left"
      ]
    }
```

Applications have an integer-based `Id` which are used as channel IDs in the fake Roku. Start numbering these at `1000`. You can specify a `Name` that will be used as a channel name as well as an action that is either `CSharp` or `CommandLine`. 

For `CSharp` actions, provide a `CSharpDetails` property as shown above. This contains the file path to a .NET Standard or .NET Core DLL containing your plugin and the name of the class to instantiate. All fields are required. Plugin classes must implement the `ICSharpAction` interface found within the Ukor project and must have a parameterless constructor.

For `CommandLine` actions, specify a `CommandLineDetails` property:

```json
    {
      "Id": 1002,
      "Name": "Launch Notepad",
      "Action": "CommandLine",
      "CommandLineDetails": {
        "Executable": "notepad.exe",
        "Arguments": null,
        "WorkingFolder": null,
        "WaitForExit": false 
      },
      "LaunchKeySequence": [
        "Fwd"
      ]
    }
```

`Arguments` and `WorkingFolder` are both optional; `Executable` and `WaitForExit` are required fields.

An application can be launched optionally with a `LaunchKeySequence`. This is an array of button presses made up of the following values:

```
            Home
            Rev
            Fwd
            Play
            Select
            Left
            Right
            Down
            Up
            Back
            InstantReplay
            Info
            Backspace
            Search
            Enter
```

The length of the sequence is specified by `KeySequenceLength` at the root of `appsettings.json`. All `LaunchKeySequence` arrays *must* be the same length as `KeySequenceLength`. 

In the above configuration examples, the `KeySequenceLength` is 1 since I'll personally never need anywhere near the 15 options afforded to me by the range of buttons above. If you're a bit more ambitious than me, a two-button sequence will give you [225 potential options](two-button-permutations.txt) and the linked file will allow you to work through the permutations systematically. A Harmony Hub typically executes both button presses within the same second, so there is only a minimal overhead of a longer sequence.

### Firewall configuration

You'll need to ensure that the following ports are opened on the machine hosting Ukor:

* UDP port 1900 - this is used to receive SSDP search requests - e.g. when a Harmony Hub looks for devices to add on the local network;
* TCP port 8060 - this is used for Ukor's HTTP API, just like a real Roku.

## How do I run it?

Just run:

```
dotnet Ukor.dll
```

You might want to run Ukor when the hosting machine starts up using something like [NSSM](https://nssm.cc). 

## Example ICSharpAction class

This is the typical structure of a C#-based plugin class:

```csharp
using System.Threading.Tasks;
using Ukor.Configuration;

namespace Chris.Ukor
{
    public class SetDns : ICSharpAction
    {
        public async Task DoActionAsync(Application application)
        {
            // Do whatever async action you like!
            // "application" is the application object from your appsettings.json
            // that's been triggered by a button-press sequence or channel launch.
        }
    }
}
```



## Why a custom version of RSSDP?

Ukor uses a customised version of Yortw's [RSSDP](https://github.com/Yortw/RSSDP) library so that the fake Roku device can be discovered on the local network. 

As Ukor emulates a *real* Roku device, I needed more control over the USN and Notification Type values emitted by RSSDP to ensure they were faithful to the values a real Roku device would emit. I also needed to respond to search requests beyond those supported out-of-the-box by RSSDP.

The RSSDP-derived code in this repo is therefore a very much cut-down and tweaked version of the official implementation, but I'm incredibly grateful to Yortw for the effort that's been put into that official implementation.
