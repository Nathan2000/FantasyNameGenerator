namespace Gui
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Net.Mime;
    using System.Windows;
    using System.Windows.Input;
    using System.Xml.Serialization;

    using Gui.Model;
    using Gui.Model.Templates;

    /// <summary>
    /// The ViewModel generating names.
    /// </summary>
    public class NamesViewModel
    {
        /// <summary>
        /// The random numbers generator.
        /// </summary>
        private readonly Random random = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="NamesViewModel"/> class.
        /// </summary>
        public NamesViewModel()
        {
            this.Categories = this.LoadCategories();

            this.GeneratedNames = this.DefaultGeneratedNames();
            this.SelectedCategory = this.Categories.First();
            this.Settings = new NameSettings
                                {
                                    Gender = Gender.Male,
                                    SequenceSize = 3,
                                    GenerateNameCount = 20,
                                    LengthModifier = 1.0,
                                    AddToResults = false,
                                    ControlLength = true,
                                    BeginWith = null
                                };

            this.GenerateNamesCommand = new SimpleCommand
                                     {
                                         CanExecuteMethod =
                                             parameter =>
                                             this.SelectedCategory != null
                                             && this.SelectedCategory.SelectedStyle != null,
                                         ExecuteMethod = parameter => this.Generate(),
                                     };
            this.ClearNamesCommand = new SimpleCommand
                                  {
                                      CanExecuteMethod = p => this.GeneratedNames.Any(),
                                      ExecuteMethod = parameter => this.GeneratedNames.Clear()
                                  };
        }

        public ObservableCollection<NameCategory> Categories { get; set; }
        public ObservableCollection<NameData> GeneratedNames { get; set; }
        public NameCategory SelectedCategory { get; set; }
        public NameSettings Settings { get; set; }

        public ICommand GenerateNamesCommand { get; set; }
        public ICommand ClearNamesCommand { get; set; }

        /// <summary>
        /// Generates a list of names.
        /// </summary>
        public void Generate()
        {
            var style = this.SelectedCategory.SelectedStyle;
            var template = style.Template;
            var generators = template.Prepare(style, this.Settings);

            if (!this.Settings.AddToResults)
            {
                this.GeneratedNames.Clear();
            }

            for (int i = 0; i < this.Settings.GenerateNameCount; i++)
            {
                var name = template.GenerateName(generators);
                this.GeneratedNames.Add(name);
            }
        }

        private ObservableCollection<NameData> DefaultGeneratedNames()
        {
            var designMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());
            if (designMode)
            {
                return new ObservableCollection<NameData>
                           {
                               new NameData { Name = "Adam", IsCanonical = true },
                               new NameData { Name = "Balladyna", IsCanonical = false },
                               new NameData { Name = "Celina", IsCanonical = true }
                           };
            }

            return new ObservableCollection<NameData>();
        }

        #region Serialization

        /// <summary>
        /// Deserializes the category list from XML.
        /// </summary>
        private ObservableCollection<NameCategory> LoadCategories()
        {
            var designMode = DesignerProperties.GetIsInDesignMode(new DependencyObject());
            if (designMode)
            {
                return new ObservableCollection<NameCategory>
                            {
                                new NameCategory(
                                    "Elder Scrolls",
                                    new ObservableCollection<NameStyle>
                                        {
                                            new NameStyle("Altmer") { Template = new NameOnly(), Description = "This is a description box" },
                                            new NameStyle("Khajiit") { Template = new NameOnly() },
                                            new NameStyle("Imperial") { Template = new FirstAndLastName() }
                                        }),
                                new NameCategory(
                                    "Real world",
                                    new ObservableCollection<NameStyle>
                                        {
                                            new NameStyle("Polish") { Template = new GenderedLastNames() }
                                        })
                            };
            }

            var serializer = new XmlSerializer(typeof(ObservableCollection<NameCategory>));
            var stream = new FileStream("Data\\Styles.xml", FileMode.Open);
            return (ObservableCollection<NameCategory>)serializer.Deserialize(stream);
        }

        /// <summary>
        /// Serializes the category list into XML.
        /// </summary>
        private void SaveCategories(IEnumerable<NameCategory> categories)
        {
            var serializer = new XmlSerializer(categories.GetType());
            var stream = new FileStream("Data\\Styles.xml", FileMode.Create);
            serializer.Serialize(stream, categories);
        }

        #endregion
    }
}
