# QzHalley
## Overview
QzHalley is a .NET-based application designed to facilitate the creation, management, and completion of quizzes in an educational or training environment. This project allows administrators to manage questions, monitor user performance, and provides users with an intuitive interface to take quizzes. Developed with a focus on simplicity and control, QzHalley is a versatile tool for learning and assessment.
Requirements

    Space: At least 192 MB of disk space.
    RAM: Minimum 256 MB.
    Software: .NET SDK (version 9.0 or later) must be installed on your system.

## Cloning

Clone the repository to your local machine:
```
git clone https://github.com/AkoForU/QzHalley.git
```


## Build and Run

Build the application and run using the .NET CLI:
```
cd QzHalley
dotnet publish
cd bin/Release/net9.0-windows/
./Main.exe
```
from the project directory after building to start the application directly.

## Usage

    Administrator: Run the QzHalley executable from the bin folder to start the app and for it to use the server, because it's using relative path for locating the server
    Client: Ensure the server is running, then use the client application to authenticate and start a quiz. Results will be displayed upon completion.

## Notes

    The server must be running on the local network (e.g., IP 192.168.1.1:5000) for the client to connect.
    For detailed setup, refer to the console output for server IP and port information.
    And don't change the location of the exe file, leave it being in the bin and releases as its using relative path

## Contributing
Feel free to fork this repository, submit issues, or create pull requests to improve QzHalley. Contributions are welcome!

