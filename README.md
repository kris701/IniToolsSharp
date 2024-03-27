
<p align="center">
    <img src="https://github.com/kris701/IniToolsSharp/assets/22596587/058e2a34-1864-46e9-868c-cc94d6ae11d7" width="200" height="200" />
</p>

[![Build and Publish](https://github.com/kris701/IniToolsSharp/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/kris701/IniToolsSharp/actions/workflows/dotnet-desktop.yml)
![Nuget](https://img.shields.io/nuget/v/IniToolsSharp)
![Nuget](https://img.shields.io/nuget/dt/IniToolsSharp)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/kris701/IniToolsSharp/main)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/kris701/IniToolsSharp)
![Static Badge](https://img.shields.io/badge/Platform-Windows-blue)
![Static Badge](https://img.shields.io/badge/Platform-Linux-blue)
![Static Badge](https://img.shields.io/badge/Framework-dotnet--8.0-green)

INI Tools Sharp is a little project to manipulate and output INI files.
You can find it on the [NuGet Package Manager](https://www.nuget.org/packages/IniToolsSharp/) or the [GitHub Package Manager](https://github.com/kris701/IniToolsSharp/pkgs/nuget/IniToolsSharp).

# How to Use
The package is inspired by that of [System.Text.Json](https://www.nuget.org/packages/System.Text.Json/9.0.0-preview.2.24128.5), where you can access two primary static methods, `INISerialiser.Deserialise` and `INISerialiser.Serialise` to convert generic classes into INI format and back.
A class represents a section by giving it a `IniSectionAttribute` and a name.

## Example
Test class to work with:
```csharp
public class Section1
{
    public bool Value1 { get; set; } = false;
    public int Value2 { get; set; } = -1;
}
public class SomeSettings
{
    [IniSection("SectionName")]
    public Section1 Section { get; set; } = new Section1();
}
```
You can then serialise it into a INI file format:
```csharp
var text = IniSerialiser.Serialise(new SomeSettings());
```
This will output text as follows:
```ini
[SectionName]
Value1=False
Value2=-1
```
The same text can be deserialised back into the `SomeSettings` object.

## Example
You can also use simple list types as follows:
```csharp
public class Section1
{
    public List<int> Values { get; set; } = new List<int>()
    {
        5,
        123,
        -1
    }
}
public class SomeSettings
{
    [IniSection("SectionName")]
    public Section1 Section { get; set; } = new Section1();
}
```
You can then serialise it into a INI file format:
```csharp
var text = IniSerialiser.Serialise(new SomeSettings());
```
This will output text as follows:
```ini
[SectionName]
Values=[5,123,-1]
```
The same text can be deserialised back into the `SomeSettings` object.