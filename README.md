# BMDL
 Command line tool for downloading osu! beatmaps
 
> *Might work on Linux, but not tested yet*

## How to use
 Create an empty folder and put the executable into it
 BMDL create files in the folder it is located in

## ???

### Why?
 I have a really old PC so I wanted to have a simple and fast way of downloading beatmaps
 
 *jk i just got really bored*
 
### Should I use it?
 Probably not, unless you have a very slow PC.
 
 It's still not a great solution and needs some work, but there might be people actually interested in this so...

### Why do I need to login with my osu! account?
 Because downloading beatmaps requires logging in.
 > Though there might be a better solution to authenticating

### Ok, but I don't trust your executables
 That's ok, I get it! If you feel sceptical about the release, you can build the code yourself:
 
 `dotnet publish BMDL -r win-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true -o build/win-x64`

## TO-DOs

- [ ] Track console window size updates and re-render on resize
- [ ] *To be continued...*
