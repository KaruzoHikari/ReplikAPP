LyokoAPP Project
======
LyokoAPP is a project designed for LAPI - Phone interaction.\
You can use it, for example, with IFSCL. It uses LyokoAPI (LAPI), which can be found [in here](https://github.com/LyokoAPI/LyokoAPI "LAPI's GitHub").\
The project contains 2 elements:

### - LyokoAPP (App)
This app is designed to get your user token and as a receiver to get the notifications from the computer.
In the future it could be used to add more interactive parts.

### - LyokoAPP (Plugin)
This plugin is the one that listens to the events and send the info to the app.


How to use:
======
1. Download the APK and install it in your phone (don't worry, it's safe. You can check the code in here. It's not in the Play Store mainly because of the registration fee).

2. Place the plugin under the plugins folder. In IFSCL, it's in:  
```\Program Files\CodeLyokoGames\Kolossus Launcher\app\IFSCL\IFSCL_Data\StreamingAssets\LyokoAPI_UserFile~```

3. Start the computer game (in this case, IFSCL), and go to the main menu. The plugin will then be initialize, create a folder in the plugins folder (it'll be called LyokoAPP, in the plugin's location) and disable itself. Once the folder is there, you can close the game.\
*(Note: If you're sure that the plugin is in the right location, but no folder is created, run IFSCL as Administrator)*.

4. In your phone, open the app. It'll generate your device token. This token will only change if you uninstall the app, updating or restarting the app won't change it. ***Keep in mind that with this token you'll be able to send notifications to yourself, but other people would be able to do it too. Do not share it around.***

5. Click the "Copy to clipboard" button and send to your computer your token. You can use whatever method you want, though emailing it to yourself is fairly easy.

6. In your computer, go to the plugins folder, enter the LyokoAPP folder and open the UserToken.txt file. Paste in there the token you just got from your phone, nothing else. Save it and close it.

7. Open the computer game again. *TADA*, you're done. You should be able to get notifications in your phone now.
