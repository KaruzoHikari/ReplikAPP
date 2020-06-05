ReplikAPP Project
======
ReplikAPP is a project designed for LAPI - Phone interaction.\
You can use it, for example, with IFSCL. It uses LyokoAPI (LAPI), which can be found [in here](https://github.com/LyokoAPI/LyokoAPI "LAPI's GitHub").\
The project contains 2 elements:

### - ReplikAPP (App)
This app is designed to get your user token and as a receiver to get the notifications from the computer.
Inside the app, right now, you can do certain things like checking the history of the towers, but in the future versions you'll be able to interact with a lot more stuff.

### - ReplikAPP (Plugin)
This plugin is the one that listens to the events and send the info to the app.


How to use:
======
1. Download the application on your smartphone. You can find it by searching for "ReplikAPP" or through these links:<br>
**[Android](https://play.google.com/store/apps/details?id=com.karuzohikari.lyokoapp)**<br>
**[iOS](https://apps.apple.com/app/replikapp/id1495977213)**

2. Download the plugin from the [Releases](https://github.com/KaruzoHikari/ReplikAPP/releases/latest) page up here in this GitHub (it's the "ReplikAPP_Plugin.dll" file), and place it under the plugins folder. In IFSCL, you can click the "Open Plugins Folder" icon in the Kolossus Launcher, or, in Windows, go to:
```%USERPROFILE%\Documents\CodeLyokoGames\IFSCLxxx\LyokoAPI_UserFile```
(where ```%USERPROFILE%``` can be replaced by the path to your user folder (```[Main drive]:\Users\[your user name]```), although it's not necessary, and ```IFSCLxxx``` must be changed by the version number of IFSCL you will install the plugin to, for example ```IFSCL404```)

3. Start the computer game (in this case, IFSCL), and go to the main menu. The plugin will then be initialized and display a window asking for your PIN.\
*(Note: If you're sure that the plugin is in the right location, but the window doesn't appear, run IFSCL as Administrator)*.

4. In your phone, open the app and touch the "Obtain PIN" button. This PIN will only change if you uninstall the app; updating or restarting the app won't change it. ***Keep in mind that with this PIN you'll be able to send notifications to yourself, but other people would be able to do it too. Do not share it around.***

5. Copy the PIN provided by your phone to the window on your computer. You can copy it manually or send it to your computer with the method you want. The pin is composed of a few numbers only, so the first option is probably the best.

6. Hit enter on your computer and *TADA*, you're done. You should be able to get notifications in your phone now.
