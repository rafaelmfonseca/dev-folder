# Dev Folder

DevFolder is a free and open-source command-line tool which allows you to clone multiples Git repositories at once.
This tool uses a configuration file that defines categories of repositories and their corresponding URLs.

<p align="center">
    <a href="https://github.com/rafaelmfonseca/dev-folder/releases/">
        <img src="https://img.shields.io/github/v/release/rafaelmfonseca/dev-folder"/>
    </a>
    <a href="https://github.com/rafaelmfonseca/dev-folder/blob/master/LICENSE">
        <img src="https://img.shields.io/github/license/rafaelmfonseca/dev-folder"/>
    </a>
</p>

## 🚀 Getting Started

Firstly, create a file named `options.json` in JSON format, that defines categories of repositories and their corresponding URLs. Here is an example:

```json
{
    "categories": [
        {
            "folder": "angular-libraries",
            "repositories": [
                { "url" : "git@github.com:angular/components.git", "folder": "angular-components" },
                { "url" : "git@github.com:nartc/ng-conduit.git" },
                { "url" : "git@github.com:ng-select/ng-select.git" }
            ]
        },
        {
            "folder": "csharp-projects",
            "repositories": [
                { "url" : "git@github.com:dotnet/efcore.git" },
                { "url" : "git@github.com:WireMock-Net/WireMock.Net.git" }
            ]
        }
    ]
}
```

To clone all repositories run the following command where options.json is:

```
DevFolder.exe clone
```

## 📝 License

DevFolder software is provided under [MIT License](LICENSE.md).