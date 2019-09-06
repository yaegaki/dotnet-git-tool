# dotnet-git-tool
Install .NET Core Tool from git repository

## Requirements

* [.NET Core 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)

or

* [.NET Core 3.0](https://dotnet.microsoft.com/download/dotnet-core/3.0)


## Install

```sh
$ dotnet tool install dotnet-git-tool -g
```



## Usage

```sh
$ dotnet git-tool install <repository>
# or
$ dotnet git-tool install <repository>/<cmd>

# Install non packable project
$ dotnet git-tool install -f <repository>

# Ignore global.json
$ dotnet git-tool install --any-sdk <repository>

# Update already installed tool
$ dotnet git-tool update <repository>
```



## Example

* Install botsay-z tool from [yaegaki/botsay](https://github.com/yaegaki/botsay).

```sh
$ dotnet git-tool install github.com/yaegaki/botsay
You can invoke the tool using the following command: botsay-z
Tool 'botsay-z' (version '1.0.0') was successfully installed.


$ botsay-z hello

        hello
    __________________
                      \
                       \
                          ....
                          ....'
                           ....
                        ..........
                    .............'..'..
                 ................'..'.....
               .......'..........'..'..'....
              ........'..........'..'..'.....
             .'....'..'..........'..'.......'.
             .'..................'...   ......
             .  ......'.........         .....
             .    _            __        ......
            ..    #            ##        ......
           ....       .                 .......
           ......  .......          ............
            ................  ......................
            ........................'................
           ......................'..'......    .......
        .........................'..'.....       .......
     ........    ..'.............'..'....      ..........
   ..'..'...      ...............'.......      ..........
  ...'......     ...... ..........  ......         .......
 ...........   .......              ........        ......
.......        '...'.'.              '.'.'.'         ....
.......       .....'..               ..'.....
   ..       ..........               ..'........
          ............               ..............
         .............               '..............
        ...........'..              .'.'............
       ...............              .'.'.............
      .............'..               ..'..'...........
      ...............                 .'..............
       .........                        ..............
        .....
```

* Install botsay-xyz tool from [yaegaki/botsay](https://github.com/yaegaki/botsay).

```sh
$ dotnet git-tool install github.com/yaegaki/botsay/botsay-xyz
You can invoke the tool using the following command: botsay-xyz
Tool 'botsay-xyz' (version '1.0.0') was successfully installed.


$ botsay-xyz hello
# ...
```
