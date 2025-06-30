# ShsRss

A command line utility to download the [IE Modding News](https://www.gibberlings3.net/forums/forum/71-infinity-engine-modding-news.xml/) RSS feed from [Gibberlings 3](https://www.gibberlings3.net/) and move it to a local directory. The intent is the local directory is a website with a lower SSL requirement than the original host. Two files are produced:

- index.xml -> a file identical to the orignal feed file
- index-shs.xml -> a file with the title attribute trimmed to 55 characters

## Usage
Ensure appsettings.json contains appropriate values:
- `FeedUrl` - The full url to the IE Modding News RSS feed, e.g. https://www.gibberlings3.net/forums/forum/71-infinity-engine-modding-news.xml/
- `DestinationFolder` - The local folder to move the output files to


## Compiling

To clone and run this application, you'll need [Git](https://git-scm.com) and [.NET](https://dotnet.microsoft.com/) installed on your computer. From your command line:

```
# Clone this repository
$ git clone https://github.com/btigi/shsrss

# Go into the repository
$ cd tlktosql

# Build  the app
$ dotnet build
```

## License

shsrss is licensed under [CC0 1.0 Universal](https://creativecommons.org/publicdomain/zero/1.0/)
