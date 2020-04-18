# FastIOW

FastIOW is a .NET Framework Library for fast and easy, arduino-like access to Code Mercenaries [IOWarrior](https://www.codemercs.com/io) devices.

This library simplifies the way of handling communication between computer and IOWarrior by abstracting and simplifing various components. It gives newcomers the ability to achive first results quickly.

Experienced developers can use this library to get things fast and and with minimal effort done, so they can focus on their task.

## Supported boards and components

  | Board | GPIO  | I2C | SPI | ADC | PWM | Timer | 
  | :--- | :---: | :---: | :---: | :---: | :---: | :---: |
  | IOWarrior40 | Yes (untested) | Yes (untested) | N/A | N/A | N/A | N/A |
  | IOWarrior24 | Yes (tested) | Yes (tested) | Yes (tested) | N/A | N/A | Yes (tested)|
  | IOWarrior56 | Yes (tested) | Yes (tested) | Yes (tested) | Yes (tested) | Yes (tested) | N/A |
  | IOWarrior28 | Yes (tested) | Yes (tested) | N/A | Yes (tested) | NA | N/A |
  | IOWarrior28L | Yes (untested) | Yes (untested) | N/A | N/A | NA | N/A |
  
You can find some software examples [here](https://github.com/Tederean/FastIOW/tree/master/Examples).

## Installation (64-bit operating system)

Download [FastIOW.dll](https://github.com/Tederean/FastIOW/releases/download/V1.0/FastIOW.dll) and [FastIOW.xml](https://github.com/Tederean/FastIOW/releases/download/V1.0/FastIOW.xml). This library requires iowkit.dll in version 1.5, that is part of the official SDK, you can download it [here](https://www.codemercs.com/downloads/iowarrior/IO-Warrior_SDK_win.zip) - extract the following file "/sdk/iowkit api/x64 dll/iowkit.dll".

Place FastIOW.dll, FastIOW.xml and iowkit.dll in your standard library folder of your operating system. For Windows x64 this path is "C:\Windows\SysWOW64".

## Limitations

This library is not thread safe, access it not simultainously with multiple threads.
