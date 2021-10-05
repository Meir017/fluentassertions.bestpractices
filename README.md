# [FluentAssertions](http://fluentassertions.com/) Analyzers

<img src="./assets/fluent_assertions.svg.png" width="400">

***"With Fluent Assertions, the assertions look beautiful, natural and most importantly, extremely readable"*** -[_Girish_](https://twitter.com/girishracharya)

* Latest stable nuget [![NuGet Badge](https://buildstats.info/nuget/fluentassertions.analyzers)](https://www.nuget.org/packages/fluentassertions.analyzers/)
* Latest nuget [![NuGet Badge](https://buildstats.info/nuget/fluentassertions.analyzers?includePreReleases=true)](https://www.nuget.org/packages/fluentassertions.analyzers/)
* The build status is [![CI](https://github.com/fluentassertions/fluentassertions.analyzers/actions/workflows/ci.yml/badge.svg)](https://github.com/fluentassertions/fluentassertions.analyzers/actions/workflows/ci.yml)

A collection of Analyzers based on the best practices [tips](https://fluentassertions.com/tips/).

## Analysis and Code Fix in Action

![Demo](assets/demo.gif)

## Install

using the latest stable version:

```powershell
Install-Package FluentAssertions.Analyzers
```

using the latest prerelease version:

```powershell
Install-Package FluentAssertions.Analyzers -IncludePrerelease -Source https://ci.appveyor.com/nuget/fluentassertions-bestpractices
```


## Getting Started

### Build

using [cake](https://cakebuild.net/)

#### windows:

```ps1
.\build.ps1
```

#### linux

```sh
. build.sh
```

### Run Tests

```ps1
.\build.ps1 -Target Run-Unit-Tests
```
