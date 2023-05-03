# WikiDiscordNotifier

[![Build status](https://ci.appveyor.com/api/projects/status/5n6fifahr986mj8c/branch/main?svg=true)](https://ci.appveyor.com/project/Manu404/wikidiscordnotifier/branch/main)



A free, open source and multiplatform using Discord webhooks to publish message when a change is made in a MediaWiki project. The tool is triggered by cronjob that check periodically if new changes have been made and then post messages into discord channels about those changes.

Basic usage:

```
politilog --wiki <wiki_url> --webhook <discord_webhook_url> 
```

#### Parameter reference

|      |           | Flag | Required | Description                                                  |
| ---- | --------- | :--: | :------: | ------------------------------------------------------------ |
|      | --wiki    |      |    x     | The url of the wiki                                          |
|      | --webhook |      |    x     | The url of the discord webhook                               |
| -l   | --limit   |      |          | Number of changes requested by queries. Max 500 (MediaWiki api limit) |
| -n   | --no-log  |  x   |          | Don't output to logfile                                      |
| -s   | --silent  |  x   |          | Don't output to console                                      |

### Building the tool

#### Getting the code

Clone the repository and you're, there's on submodules used by this project.

#### Dependencies

Build is done on Windows using gitbash (normally provided with git) or on Linux. You don't need any IDE like Visual Studio or similar. What you need is:

- Git for Windows: https://git-scm.com/download/win
- DotNet 7 SDK: https://dotnet.microsoft.com/en-us/download/dotnet/7.0
- Zip and bzip2 2 need to be added to your gitbash if you're on windows, (it's really easy to install); here's a straight forward tutorial: https://ranxing.wordpress.com/2016/12/13/add-zip-into-git-bash-on-windows/

If under linux:

- DotNet 7 SDK: https://dotnet.microsoft.com/en-us/download/dotnet/7.0
- Zip command

#### Compilation

Once the dependencies are installed, you're ready to compile by yourself the project.

The compilation rely on two compile script:

- *build.sh*: the root script containing project variable and calling a generic build script. You can edit it if you want to change what parameters are provided to the real build script, it's pretty straight forward.

- *multiplateform_build.sh*: the "real" compile script, it can be given the following parameter. If a paramter is not given, an interactive prompt will ask you for informations

  | Parameter |           | Description                                               |
  | --------- | --------- | --------------------------------------------------------- |
  | -t        | --target  | The target plateteforme (cfr suppported plateform)        |
  | -p        | --project | Path to the project file                                  |
  | -n        | --name    | Project name used for the zip file                        |
  | -v        | --version | Version use for the zip file                              |
  | -e        | --embeded | Produce a SelfContained ("portable") file (default false) |
  | -a        | --all     | Build all plateform available                             |

A clean is done before each build.

The build output is placed in *""./output/build/\<plateform\>""*

A zip containing the build output is placed in *"./output/zip/\<plateform\>.zip"*

The zip name use the folllowing convention: 

```
<name>_<version>_<plateform>.zip
```

#### Remarks

- Sadly, WSL has compatibility issues with the "dotnet" command, so it can't being used.

#### Build for unofficially supported system

You can build for 'unofficially supported system' using the -p parameter of the build script and using for a platform available in the list [here](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)

Example, building for macOS 13 Ventura ARM 64 : "./multiplateform_build -p osx.13-arm64"
