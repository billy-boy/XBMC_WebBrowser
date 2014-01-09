XBMC Webbrowser
original by AddonScriptorDE
modified by billy-boy

This program is a webbrowser optimized for the use in an htpc with XBMC as media center software. In this readme you will understand the functions and how you could modify them.

Run this program:
This program only needs one thing for running: A parameter pointing to the '%appdata%\Roaming\XBMC\userdata\xmbc_webbrowser\' folder.
E.g.: XMBC_WebBrowser.exe "C:\Users\foo\AppData\Roaming\XBMC\userdata\xbmc_webbrowser"

You can add some more parameters as it was defined in earlier versions but they are deprecated and should not longer be used (use the config files instead).

KeyMappings
-----------
Each control reason inside this webbrowser could be triggered by one or more (!) keyboard keys. The default keymapping is printed below. 
If you want to modify these keymappings you have to create a file called 'keymap' inside your '%appdata%\Roaming\XBMC\userdata\xmbc_webbrowser\' Folder.

If you want to assign a new keymapping you have to add an entry (one entry is one line) which assigns the letter by it's name. To assign more than one key to a control reason you only have to add more than one entry.

Up = NumPad8
Down = NumPad2
Left = NumPad4
Right = NumPad6
UpLeft = NumPad7
UpRight = NumPad9
DownLeft = NumPad1
DownRight = NumPad3
Click = NumPad5
DoubleClick = 
ZoomIn = Add
ZoomOut = Subtract
Magnifier = Menu, Alt 
Navigate = 
Close = NumPad0
Keyboard = 
Favourites = 
ShortCuts = 
TAB = 
ESC = 
ToggleMouse = Multiply
ContextMenu = Divide
F5 = 
Delete = Decimal

So e.g. if you want the "UP"-Reason to be thrown by the W-Key and the Arrow-Up-Key you have to add the following lines:
Up=A
Up=Up

If you don't know the correct name of the key you could try it out with a little .NET program. We use the Windows-API function 'protected override bool ProcessCmdKey(ref Message msg, Keys keyData)' and use the keyData.toString() result.

 Menu
-----------
Via the menu you could use all the functions of this webbrowser. So it is important to assign a keyboard key for it...

Favourites
-----------
Favourites are like you know them from InternetExplorer or Firefox as boomarks. You can add as many bookmarks as you want. Inside the Favourite-Menu you could scroll thorugh them.
Inside the Favourites-Menu you could add the site you are actually browsing on to your favourites but you could also delete one favorite. This is done by selecting the favorit and then pressing the key you assigned for the 'Delete-Reason' inside your keymap.

Favourites are stored in  '%appdata%\Roaming\XBMC\userdata\xmbc_webbrowser\sites' where each file with the extension '.link' represents one single bookmark.
The files contain some informations. For each information there is one line. The importants are 'title=' and 'url='. The name of the file has to be like the title is.

Shortcuts
-----------
Shortcuts are different to Favourites. They represent a sort of special bookmarks for one "parent site". So if you are browsing on "google.de" you can add as many shortcuts as you want but if you browse to "heise.de" they do not appear. They only appear on "google.de".
Inside the Shortcut-Menu you could add and delete sites as it is described for Favourites also.

Shortcuts are stored a little bit different than favourites. They are stored in '%appdata%\Roaming\XBMC\userdata\xmbc_webbrowser\shortcuts'. Each file with the extension '.links' stands for all the shortcuts of one "parent site".
The name of the file is representing the parents site. So if your browsing "http://www.google.de/" the filename would be "www.google.de.links". The protocol (http or https) and all behind the TLD (after the .de/) is not important.
Inside the file each line is representing one shortcut in form of "title=url".

Zoom
-----------
You can zoom the website you are viewing as much as you want by the keyboar keys specified in the keymap. It happens like zooming in InternetExplorer directly.

Note: If you're activating the magnifier the zoom function does not work. The zoom in/out is then applied on the magnifier not on the website.

The zoom is controlled via these Config-File settings:
activexZoomStep=50		<- The zoom factor which is added/substracted each zoom in/out

Magnifier
-----------
This function is a little bit buggy for now. If you activate it you will see the area under your cursor in a zommed window. If you are moving your mouse the window will disappear and you can go on.

The magnifier is controlled via these Config-File settings:
magnifierWidth=1280		<- Width of the magnifier window
magnifierHeigth=720		<- Height of the magnifier window
magnifierZoom=2			<- Initial zoom factor

Keyboard
-----------
If you have selected one element on the website you are viewing (e.g. logon name fields) you can start the keyboard and enter the data you would entered inside this field. You can use the Up-/Down-/Right-/Left- and Click-Keys (like on a remote) or you could use the keyboard.

The layout of the keyboard you can modify as you want (because it should be language specific). It is saved in "%appdata%\Roaming\XBMC\userdata\xmbc_webbrowser\keyboardLayout".
Each row is represented as a letter (A for row 1) and each button in a row is represented by its number. If you want to modify the upper case variant you have to look for the "_u" entry. Thats all.

The keyboard is controlled via these Config-File settings:
urlKeyboardEnabed=true		<- currently not used (!)

Browsing/Navigating
--------------------
You can either use the "Navigate to" entry in the menu or you simply could go to the adress bar with your mouse and enter the url with your keyboard.

Cursor
--------------------
The custom and big cursor is activated by default (because on a big screen the windows cursor is not really helpful). If you want to modify the cursor (if you want it bigger or colored) simply place your cursor.png inside
"%appdata%\Roaming\XBMC\userdata\xmbc_webbrowser".

The custom cursor is controlled via these Config-File settings:
customCursorSize=64		<- A value representing the size of the cursor (if the image is smaller/bigger it will be resized)
useCustomCursor=true	<- If set to false the windows standard cursor will appear

Scrolling
--------------------
This is done by itself if your cursor (not your controlled by your mouse!!! only controlled via the Up-/Down-/Left-/Right- Keymap-Keys) went to the edges of the browser.

The scrolling is controlled via these Config-File settings:
mouseEnabled=true		<- if the mouse is disabled then the Up-/Down-/Right-/Left- Keymap-Keys only acts as scrolling-keys
showScrollBar=true		<- showing scrollbars or not
scrollSpeed=20			<- The speed of scrolling at the edges

Config-File
--------------------
Inside the config file which is located in "%appdata%\Roaming\XBMC\userdata\xmbc_webbrowser\config" you an simply modify some default behaviour. These options are avail: 

mainTitle=
mainURL=http://www.google.de
userAgent=
showPopups=false
showScrollBar=true
useCustomCursor=true
mouseEnabled=true
urlKeyboardEnabed=false
supressScriptWarnings=true
customCursorSize=64
scrollSpeed=20
magnifierWidth=1280
magnifierHeigth=720
magnifierZoom=2
