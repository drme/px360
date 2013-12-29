# px360

![logo](http://farm6.staticflickr.com/5492/11633219966_efb45bde85.jpg)

# Problem 

Playing some types of games is not so confortable using game controller compared with playing them on PC using keyboard and mouse. This is, of course, mostly related of FPS games. It’s definitely possible to play those games with controller, and lots’ of gamers does that. But for some using keyboard and mouse is better.

# Solution

Then Microsoft has presented smart glass software for windows phone and windows 8, it was quite exciting to hear that it would be possible to control Xbox from pc or phone. The reality is a little bit disappointing – yes, it’s possible to control but only dashboard, and some games that have built-in support for smart glass. Then the game is launched, the gamer has to grab controller.
So what will do, we will hack the Xbox controller, plug it to the Arduino, make some software for PC and windows phone, to give some support for keyboard and mouse in fps games. There is also some commercial and free similar solutions [1][2].

# Design

The main idea is that we have software on a PC that listens for keyboard and mouse events, and according to them sends commands to the Arduino, and when the Arduino presses the corresponding buttons, turns relevant joysticks on the Xbox controller.
The solution diagram is presented in the picture:

![design](http://farm8.staticflickr.com/7443/11630583533_84b14203e2.jpg)

PC runs XNA application that allows configuring key bindings, listens for keyboard and mouse input, translates keyboard and mouse commands into Xbox controller commands and sends commands to the Arduino controller. For example, pressing E on keyboard is translated to the ‘A’ button press Xbox controller, slightly moving a mouse to the left is converted to a slight left joysticks’ shift to the left.
Arduino – receives commands for doing actual Xbox controller pressing and adjusting joysticks.
Button press is quite simple: then the button is pressed is on Xbox controller the connection between two contacts is made and controller interprets it as a button press. So what Arduino has to do is just to make that connection. That can be easily achieved by using transistor. Arduino can easily instruct transistor to make or break a connection. 

Joysticks: thumb sticks and trigger buttons, uses potentiometers.  A potentiometer is a resistor which resistance can be adjusted. So basically, then the stick is pushed left its resistance is lowest, then it’s pushed to the right, its resistance is largest. When the thumb stick is idle – resistance is in the middle. The thumb stick is composed of two potentiometers: one controls movement on X axis, the other one controls movement on Y axis. All in all, there are 6 potentiometers on a controller – 2 for the right stick, 2 for the left stick, 1 for the left trigger, and 1 for the right trigger. So in order to control thumb sticks and trigger buttons from Arduino, all is needed, is to replace potentiometers with the digital ones (that could be controller from Arduino). AD5260 chip is a perfect match, as it has 6 digitally controllable potentiometers, which have the same resistance as potentiometers on the Xbox controller. Arduino can, just send command to one of digital potentiometers, and specify required resistance. The resistance values are in the range of [0-255]. So turning right (or down, or pressing trigger) equals to 255 value (127 for trigger), and turning left (or up, or releasing trigger) equals to 0. The centre positions value is 127 for thumb sticks.

# Modifying and building hardware

For this solution to work, we have to modify Xbox controller (piggyback on its joysticks and buttons) and to build the Arduino shield, which does all the controlling.
The first thing what we have to do is to solder wires to the Xbox controller. Each button just makes contact between two pads on the controller board. But one of those pads is ground (and it’s the same for all buttons), so all we have to do is to solder one wire for each button. That wire will be connected to the Arduino shield.

![soldered button](http://farm4.staticflickr.com/3780/11631089454_794f488119.jpg)

The joysticks are made of potentiometers, and each potentiometer contains 3 connections: power, ground and wiper. What we are interested here is the wiper, as the wiper value controls the joystick or trigger position. So for hacking joysticks, we have to de-solder them and solder wires to their wipers and power connections (as ground again is the same we will not be soldering wires for it on the controller as well, the connection on the Arduino shield will be used instead).  The same soldering is needed for each trigger button.

![thumb sticks connection](http://farm3.staticflickr.com/2866/11630390575_62d619bd0e.jpg)

Here is the schematics of all required wires to be soldered and their labels, it’s important to keep track which wire is which, as all of them will have to be plugged in into the Arduino shield.

<< controller schematics >>

# Arduino shield
The next step is to construct the Arduino shield, that would contain transistors for pushing buttons and digital potentiometer for controlling (or acing instead of) thumb sticks and trigger buttons. For each Xbox controllers button one Arduino pin will be needed, that pin will be connected to the transistors base through the resistor (for limiting a current and not frying the transistors). AS there are 15-teen buttons on the controller we would need 15 transistors and 15 resistors. So for saving soldering job and using less components and less shield space, the integrated circuit that contains 5 transistors in one package is used. It’s basically the same 5 transistors just tightly packed into one package.

For controlling the thumb sticks the AD5206 digital potentiometer is used, it contains 6 digital potentiometers in one package – that’s the exact amount of potentiometers present on the Xbox controller. The AD5206 uses SPI interface for communication and allows setting wiper value for each of its 6 digital potentiometers.

![button to transistor and pot to ad5206](http://farm4.staticflickr.com/3677/11631814256_db5163107e.jpg)

The shields schematic is as follows: the transistors controls buttons, the potentiometer controls thumb sticks and triggers, one set of headers are for connecting controller to the Arduino shield and another set of headers is for plugging into the Arduino.

![schematics](http://farm6.staticflickr.com/5538/11631098003_d76102ef48.jpg)
![pcb](http://farm6.staticflickr.com/5483/11631634736_634a671281.jpg)

The shield designed using frintzing software and its PSB was manufactured at fritzing fab.

![shield](http://farm4.staticflickr.com/3697/11631340083_e6356f17b0.jpg)

But before building shield, its recommended to test the solution on the breadboard, to see if everything works.

![breadboard prototype](http://farm6.staticflickr.com/5477/11630936653_a796d2d09b.jpg)

# Arduino sketch

For all assembled hardware to work, some software is needed. Arduino needs simple sketch that sets high or low values to the buttons then it’s needed. The Arduino sketch is pretty simple:
* It opens serial connection and waits for commands from PC
* Then commands is received , it interprets them
* And changes buttons and potentiometer states on the shields, thus interacting with the controller and the Xbox as well.

The Arduino sketch is available in the github here.

PC software
Finally, the software on PC is needed that would read keyboard and mouse actions and pass them to the Arduino, which in turn would control the Xbox controller. The PC software is the XNA application that renders simple UI for setting up controls and displays key bindings. This application opens serial port connection to the Arduino and passes the desired button press states and desired thumb sticks positions to the Arduino.
<< status window>>

![settings](http://farm4.staticflickr.com/3776/11631302445_51de952c83.jpg)

# Enclosure

And finally we need some nice box to store modified controller and Arduino shield with the Arduino itself. One of the options is to put back everything into the controller shell.

![controller + shield](http://farm8.staticflickr.com/7430/11631766294_d5f1db2d13.jpg)

Or put into some nice enclosure. WE decided to reuse a case for broken mac-mini computer, as its pretty nice case with enough space for storing hacked controller and the Arduino with its shield. Thus this case has one problem – it’s made of aluminium, which blocks wireless signals pretty well. So as we also decided to keep this case nearby the Xbox itself, there is no problem with that. To fit all nicely into mac mini case we have designed a ports PCB for nicely plugging the USB cable into mac mini USB ports, which are connected to the Arduino itself. Here is the PCB design of ports extension board.

![mac-mini ports](http://farm4.staticflickr.com/3806/11632141856_57c4f4fece.jpg)

As mac mini case is quite spacious we have added raspberry pi inside as well. It is not related to this project at it just sits in the same case and does some other unrelated stuff.

![mini + shield](http://farm6.staticflickr.com/5513/11632124116_e9bee88691.jpg)
![with controller](http://farm8.staticflickr.com/7375/11632245296_6a5e8f1a5c.jpg)

Final assembly
So we have:
* Hacked Xbox controller,
* Arduino shield,
* Arduino,
* Arduino sketch,
* PC software,
* And some pc running software.

![finall solution](http://farm4.staticflickr.com/3826/11632695473_8baaae612e.jpg)

Not it’s time to play games.

# References

[1] http://xim3.com/
[2] http://blog.gimx.fr/
