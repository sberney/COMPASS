# Pre-requisites

## Preliminaries

This program is designed to run on Windows and, currently, only Windows.
It can be developed using Visual Studio 2022 Community Edition (or any paid edition).
Once you install VS and attempt to open the `.sln` file, VS will prompt you to install the
proper 'toolkit' if you haven't got it already.

## Secondaries

### Magick.NET

This application relies on the library Magick.NET, which in turn relies on a locally installed version of Ghostscript.
Magick.NET [provides directions](https://github.com/dlemstra/Magick.NET/blob/main/docs/Readme.md#ghostscript) on how to accomplish this.
You can change the install directory specified in `MainViewModel.cs`.


## Goal

Ultimately you should be able to clone the source, open the solution, and press F5 to run the application.