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
**Android** - https://play.google.com/store/apps/details?id=com.karuzohikari.lyokoapp<br>
**iOS** - https://apps.apple.com/app/replikapp/id1495977213

2. Place the plugin under the plugins folder. In IFSCL, you can click the "Open Plugins Folder" icon in Kolossus Launcher, or go manually to this directory:  
```\Program Files\CodeLyokoGames\Kolossus Launcher\app\IFSCL\IFSCL_Data\StreamingAssets\LyokoAPI_UserFile~```

3. Start the computer game (in this case, IFSCL), and go to the main menu. The plugin will then be initialize, create a folder in the plugins folder by itself (it'll be called ReplikAPP, in the plugin's location) and disable. Once the folder is there, you can close the game.\
*(Note: If you're sure that the plugin is in the right location, but no folder is created, run IFSCL as Administrator)*.

4. In your phone, open the app and you'll automatically receive a token. This token will only change if you uninstall the app; updating or restarting the app won't change it. ***Keep in mind that with this token you'll be able to send notifications to yourself, but other people would be able to do it too. Do not share it around.***

5. Click the "Copy token to clipboard" button and send the content of your clipboard to your computer. You can use whatever method you want, though pasting in the body of an email (by holding and clicking "Paste") and emailing it to yourself is fairly easy.

6. In your computer, go to the plugins folder, enter the ReplikAPP folder and open the UserToken.txt file. Paste in there the token you just got from your phone, nothing else (Make sure you don't hit "Enter" or the spacebar by mistake). Save it and close it.

7. Open the computer game again. *TADA*, you're done. You should be able to get notifications in your phone now.
