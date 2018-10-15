# Fission
> Damn Easy File Scanner using Virustotal

**Fission** is the implementation of [virustotal](https://developers.virustotal.com/reference) api. 

It first of all check if file if already scanned or not. If file is scanned then it extract its scan results otherwise it will send file to the server for scanning and adds to misc file. Then you can scan only misc files after it is being scanned on server

![image](https://user-images.githubusercontent.com/28386721/46957245-c86bd180-d0b4-11e8-8278-76a0d6e8b815.png)


# Features
1. Easy to use
2. Scans one file on 67 antiviruses
3. Total scans is 5000/per day
4. Free Scan Utility
5. Fast scanning
6. Cross Platform
7. Scans files upto 128mB

# Requirement
### Windows
+ .net Framework 4.7
+ Visual Studio / Mono Develop (for development and contribution)
### Linux/macOS
+ mono
+ monodevelop (for development and contribution)

# Setup and Install
_after fulfilling [Requirements](#requirement)_

### Windows
+ Add the path of fission to enviroment variable
+ Open command prompt
+ Get your api key.  [see here](#Getting-Api-Key)
+ Goto directory you want to scan
    + execute **fission.exe**
### Linux
+ copy `build/` to `/opt/`
    + for **x86** architecture
        ```
        $ sudo cp -rf build/x86 /opt/fission
        ```
    + for **x64** architecture
        ```
        $ sudo cp -rf build/x64 /opt/fission
        ```
+ add alias to `.bashrc`
    ```
    $ echo -e "\nalias fission='mono /opt/fission/fission.exe'" >> ~/.bashrc
    ```
+ update .bashrc
    ```
    $ source ~/.bashrc
    ```
+ Get your api key.  [see here](#Getting-Api-Key)
+ Goto directory you want to scan
    + execute **fission**

# Getting Api Key
1. Sign Up/Login to [virustotal](https://virustotal.com)
2. After verifying account (only when created new account)
3. Navigate to [api page](https://www.virustotal.com/#/settings/apikey)
4. Copy and paste your api key in the application
    + open application
    + select 2 - Settings
    + select 2 - Set api key
    + paste api key here

# License
**Fission** is licensed under [GNU GPL v3](https://github.com/tbhaxor/fission/blob/master/LICENSE)

# Contribution
Feel free for contributing to _fission_

### Scope of contribution
1. Enhancing documentation
2. Increasing file scan limit beyond 5000/per day
3. Increasing speed
4. New features
    + file shredder
    + service to perform auto scan

### Rules
1. Pull Request from master branch won't be merged
2. One should commit file(s) with proper commit message