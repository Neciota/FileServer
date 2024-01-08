# FileServer
FileServer is a small web-based project to allow users to host files remotely on a server without any knowledge of FTP clients. Users can share content of folders with other users.

## Additionally required settings files
The files `disksettings.json` and `jwtsettings.json` are required in `FileServer.Server` in addition to `FileServer.Server/appsettings.json` but do not exist in the git repository.

Example `disksettings.json`:
```
{
	"Disk": {
		"RootFolder": "C:\\Root\\Shared\\TestShareSpace"
	}
}
```
This determines where the files are stored on server disk.

Example `jwtsettings.json`:
```
{
	"Jwt": {
		"Issuer": "https://github.com/Neciota",
		"Audience": "https://github.com/Neciota",
		"Key": "example private key for generating JWTs"
	}
}
```
This is required for the creation of the JWTs.
