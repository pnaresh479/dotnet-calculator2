# Dotnet calculator Setup instructions 

## pre-requisites setup instructions
* Pre-requisites
Download dotnet SDK 8.0.x from https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Verify the dotnet installed version using dotnet --version

* Install WiX Toolset 6.0.2

run below command 
dotnet tool install --global wix --version 6.0.2

verify wix version using
wix --version

* install extentions for vs code IDE
C# Dev Kit
.NET Install Tool

## Dotnet programme setup

* mkdir DotNetCalculator
* cd DotNetCalculator
* dotnet new console --framework net8.0 -n CalculatorApp
* cd CalculatorApp

* Build instructions
    dotnet build
    dotnet run

* publish instrcutions
    dotnet publish -c Release -r win-x64 --self-contained true

## Create WiX Installer (MSI)  

* Go to project root
    cd ..
    mkdir Installer
    cd Installer

* create file <CalculatorApp.wxs> with below content
'''
<?xml version="1.0" encoding="UTF-8"?>
<Wix xmlns="http://wixtoolset.org/schemas/v4/wxs">
  <Package Name="CalculatorApp"
           Manufacturer="Naresh Pabbineedi"
           Version="1.0.0"
           UpgradeCode="PUT-GUID-HERE"
           InstallerVersion="500"
           Scope="perMachine">

    <MajorUpgrade DowngradeErrorMessage="A newer version is already installed." />

    <Feature Id="MainFeature" Title="CalculatorApp" Level="1">
      <ComponentGroupRef Id="CalculatorAppFiles"/>
    </Feature>
  </Package>

  <Fragment>
    <Directory Id="TARGETDIR" Name="SourceDir">
      <Directory Id="ProgramFilesFolder">
        <Directory Id="INSTALLFOLDER" Name="CalculatorApp"/>
      </Directory>
    </Directory>

    <ComponentGroup Id="CalculatorAppFiles">
      <Component Id="CalculatorExe" Guid="PUT-ANOTHER-GUID-HERE">
        <File Source="../CalculatorApp/bin/Release/net8.0/win-x64/publish/CalculatorApp.exe" KeyPath="yes" />
      </Component>
    </ComponentGroup>
  </Fragment>
</Wix>
'''

* commanad to generate new GUID is
    powershell "[guid]::NewGuid()"

## Build MSI using WiX CLI

* cd installer
* wix build CalculatorApp.wxs -o CalculatorApp.msi
