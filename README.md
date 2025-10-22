# Fantasy Name Generator
This is an application that can generate names for fantasy cultures, based on a predefined list of names. It was primarily meant for writing The Elder Scrolls fan-fiction, but can be easily extended for custom cultures.

The project includes a WPF application and a Blazor Single-Page app. The Blazor app is a Progressive Web Application, so you can install it on your own computer.

* Try out a live demo: [Fantasy Name Generator on GitHub Pages](https://nathan2000.github.io/FantasyNameGenerator/)
* Get the latest release: [![Latest Release](https://img.shields.io/github/v/release/Nathan2000/FantasyNameGenerator?label=Download)](https://github.com/Nathan2000/FantasyNameGenerator/releases/latest)

## Custom cultures
You can extend the name generator by adding your own categories and cultures. This allows you to define custom naming conventions, templates, and name lists.

### Folder structure
Each culture belongs to a category and is defined by a folder structure like this:
```
/Data
├── <CategoryName>
│   └── <CultureName>
│       ├── metadata.json
│       ├── male.txt
│       ├── female.txt
│       └── surname.txt
```

### Metadata
The `metadata.json` file defines how this culture's names are constructed. The main part is the `template`, which is made out of name components. Each component can have a male or female variant and is dynamically replaced by the generated value. Below is a sample file:
```json
{
  "description": "The culture description.",
  "template": "{name} {surname}",
  "components": {
    "name": {
      "type": "Markov",
      "male": "male.txt",
      "female": "female.txt"
    },
    "surname": {
      "type": "Markov",
      "male": "surname.txt",
      "female": "surname.txt"
    }
  }
}
```

### Gendered literals
In the template, you can use gendered literals, which are used like this: `{g|male variant|female variant}`. They can be used to enforce gender-specific naming schemes.

## Contributing
Pull requests are welcome! If you’d like to contribute, please fork the repository and submit a PR. For major changes, open an issue first to discuss your ideas.

## License
This project is licensed under the MIT License. See the [LICENSE](https://github.com/Nathan2000/FantasyNameGenerator/blob/master/LICENSE) file for details
