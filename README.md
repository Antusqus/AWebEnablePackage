# WebEnablePackage

WebEnablePackage is a C# Blazor project that allows users registration to a job agency.

## Installation

This project currently only supports Windows users, although Docker should also operate on Linux Systems.

Clone the repo.
Install Docker.



Use the package manager [pip](https://pip.pypa.io/en/stable/) to install foobar.

```bash
pip install foobar
```

## Usage

```python
import foobar

# returns 'words'
foobar.pluralize('word')

# returns 'geese'
foobar.pluralize('goose')

# returns 'phenomenon'
foobar.singularize('phenomena')
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.

## TodoList
* Create a Docker Container
    * [ ] Add blazor program to start up with docker. 
    * [x] ~~Add tangible data to container (Preferrably code-first)~~ 

* [ ] Cookies?

* Connecting from Blazor Project to Docker Env
    * [x] Add connection string to appsettings.json
    * [x] Get connection running from project to docker container

* Insert and retrieve data from Database
    * [x] Razorpage
    * [x] Fix Button Event Handler in Registration.razor ?
    * [ ] Input Sanitization
    * [ ] SQL Security
    * [ ] Get/Post data (and not have it crash)

## Issues
* Site errors when database down
* Exception thrown: 'System.InvalidOperationException' in System.Private.CoreLib.dll: 'Connection must be valid and open.' on Registration.razor
    * Possibly due to MultipleActiveResultSets not being set + terminal having active connection with docker db?
* Docker .env file MIGHT require 'docker-compose up' command over 'docker stack deploy' 

## License

Certainly not licensed.