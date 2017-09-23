# PPMControl
Controlling RC-transmitter with PPM (pulse-position modulation) trainer input using just sound card.

This library allow you to control RC-models via PC through standart transmitter. It is using trainer PPM input port to send commands from PC
to transmitter. PPM signal is generating by sound card, so you don't need any microcontroller or any other device to generate PPM.


![](https://github.com/Garrus007/PPMControl/blob/master/readme_resources/Illustration.jpg?raw=true)

## PPM principles and settings

In PPM (Pulse Position Modulation) the analogue sample values determine the position of a narrow pulse relative to the clocking time. It is
very close to PWM (pulse width modulation) where analogue signal is encoding with duty ratio. At the diagram below PPM signal with It's 
properties is shown.

![](https://github.com/Garrus007/PPMControl/blob/master/readme_resources/PPM.png?raw=true)

```
It's very important to use same properties as at your RC-transmitter. Otherwise It won't able to decode signal.
```

## Usage
This repository contains `AudioPPM` library and `ControlGUI` test project. 

1. <a href="#connection">Connect</a> linear output of your sound card to the PPM-in and GND of your transmitter. 
2. Set up trainer mode on your transmitter and enable It.
3. Run `ControlGUI` project
4. Select output device and press `Start`
5. Try to move trackars (sticks) and watch the result. Some transmitters (such as FlySky fs-i6) allow you to watch sticks positions on
a display. If your transmitter doesn't allow this, just add servos to receiver.
6. Write your own programs using `AudioPPM` library
7. ???
8. PROFIT!!!

## Example
Library is so easy, so I don't provide API documentation, little example will cover all of the functionality.

```cs
// Get list of avaliable output sound devices and simple get first device.
var outputDevice = PpmGenerator.GetDevices().First();

// Create PpmGenerator to generate PPM signal for FlySky fs-i6 transmitter (6 channels)
// NOTE: StandartProfiles contains settings for common PPM profiles, but now there is only one - FlySky
var generator = new PpmGenerator(6, StandartProfiles.FlySky, outputDevice);

// Set some initial values. Every channel value is in range [0; 255]
// Aileron, Elevator, Throttle, Rudder but it's just convention
byte[] controlgValues = new byte[]{128, 128, 0, 128, 0, 0});
generator.SetValues(controlgValues);

// Start PPM. Generation will work in other thread
generator.Start();

// Change values - throttle up
controlValues[2] = 128;
generator.SetValues(controlgValues);

// Stop PPM
generator.Stop();
```

<a name="connection"></a>
## Connecting to transmitter
That may be a little tricky. You should know pinout of trainer port of your transmitter. FlySky fs-i6 pinout is shown below.
![](https://static.rcgroups.net/forums/attachments/6/6/8/4/2/0/t9906576-63-thumb-a9299311-218-thumb-FS-I6-TrainerPort.jpg?d=1490657312)

- GND - to the sound card linear output GND
- PPM-in - to the sound card linear output channel (no matter, left or right).

### Amplifier
In my case amplitude of the signal is about 1-2v. Transmitter required about 5v. So, I designed simply amplifier. It amplifies signal
and also make it perferct rectangular form without any noise.

Schematics:

![](https://github.com/Garrus007/PPMControl/blob/master/readme_resources/amplifier_schematics.jpg?raw=true)

PCB overview:

![](https://github.com/Garrus007/PPMControl/blob/master/readme_resources/amplifier_pcb_view.jpg?raw=true)

You can [download PCB ready-to-print image](https://github.com/Garrus007/PPMControl/blob/master/readme_resources/amplifier_pcb_print.jpg?raw=true)


