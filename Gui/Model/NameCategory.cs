namespace Gui.Model
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Xml.Serialization;

    public class NameCategory
    {
        private NameStyle selectedStyle;

        public NameCategory()
        {
            this.Styles = new ObservableCollection<NameStyle>();
        }

        public NameCategory(string name, ObservableCollection<NameStyle> styles)
        {
            this.Name = name;
            this.Styles = styles;
        }

        public string Name { get; set; }
        public ObservableCollection<NameStyle> Styles { get; set; }

        [XmlIgnore]
        public NameStyle SelectedStyle
        {
            get
            {
                if (this.selectedStyle == null)
                {
                    this.selectedStyle = this.Styles.First();
                }

                return this.selectedStyle;
            }

            set
            {
                this.selectedStyle = value;
            }
        }
    }
}
