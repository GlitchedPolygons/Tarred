# Tarred

## _"all tarred up..."_

Damn... Just select folders and files inside Windows Explorer and compress them into a `.tar.gz` archive using the right-mouse button context menu.

Works on Linux and Mac too, but there it's used via the CLI (where it's kinda redundant with the original `tar` CLI, except the arguments list is slightly cleaner I guess).

![Screenshot 1](https://static-files.glitchedpolygons.com/tarred-windows-screenshot-01.png)

For multiple filed and folders the entry is inside the "Send to" context menu:

![Screenshot 2](https://static-files.glitchedpolygons.com/tarred-windows-screenshot-02.png)

### How to use

* Download and extract the correct binaries based on your system
* * Or build from source if you want
* **[OPTIONAL]** Make sure that the directory where you extracted _Tarred_ (the one with the `.exe` and `.ico` inside) is in your `$PATH` so that you can call it from everywhere :D
* On Windows:
* Install the multi-file context menu entry (inside "Send to") by double-clicking on `Install.bat`
* **[OPTIONAL]** Install the context menu entry by double-clicking on the `.reg` file included alongside the `Tarred.exe`
* * Not really necessary, as the entry inside the "Send to" context menu is always there after installation :)
* Enjoy <3

Alternatively, you can also copy and paste `Tarred.exe` into the directory of your precious files, and then select what you want to compress and **drag 'n' drop** them _into_ the `Tarred.exe`

![Screenshot 3](https://static-files.glitchedpolygons.com/tarred-windows-screenshot-03.gif)

## Oh shit, how do I decompress these back to normal again?

[`Untarred.exe`](https://github.com/GlitchedPolygons/Untarred)
