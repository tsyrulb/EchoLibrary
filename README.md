# EchoServer
***
## Instructions:
* It's the same API that was done in EX2, but now it's capable to work with MariaDB and Firebase.
* Firstly, after intalling MariaDB Server and HeidySQL, you must open session in HeidySQL. Pay attention, that your name and password that you use in HeidySQL you must also write in API in file _appsettings.json_ in EchoAPI project in line _MariaDbContext_ that on Connection Strings. 
* After that you run in package manager: _Update-Database -Context MariaDbContext_ that will create tables of user/contact/messages in MariaDb that you can check in HeidySQL.
* Now, when you finish create database, you need to connect Firebase:
* in file _appsettings.json_ in EchoAPI project in 'FcmNotification' we wrote SenderId and ServerKey of project that already connected with android project in Firebase Console. You might change it, if you want to connect android client with your Firebase Console account.
* Now you ready to use EchoAPI.
