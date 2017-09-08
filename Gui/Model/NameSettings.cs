namespace Gui.Model
{
    using System.ComponentModel;

    public class NameSettings : INotifyPropertyChanged
    {
        public int GenerateNameCount
        {
            get
            {
                return this.generateNameCount;
            }

            set
            {
                this.generateNameCount = value;
                this.NotifyPropertyChanged("GenerateNameCount");
            }
        }

        public int SequenceSize
        {
            get
            {
                return this.sequenceSize;
            }

            set
            {
                this.sequenceSize = value;
                this.NotifyPropertyChanged("SequenceSize");
            }
        }

        public Gender Gender
        {
            get
            {
                return this.gender;
            }

            set
            {
                this.gender = value;
                this.NotifyPropertyChanged("Gender");
            }
        }

        public double LengthModifier
        {
            get
            {
                return this.lengthModifier;
            }

            set
            {
                this.lengthModifier = value;
                this.NotifyPropertyChanged("LengthModifier");
            }
        }

        public bool AddToResults
        {
            get
            {
                return this.addToResults;
            }

            set
            {
                this.addToResults = value;
                this.NotifyPropertyChanged("AddToResults");
            }
        }

        public bool ControlLength
        {
            get
            {
                return this.controlLength;
            }

            set
            {
                this.controlLength = value;
                this.NotifyPropertyChanged("ControlLength");
            }
        }

        public string BeginWith
        {
            get
            {
                return this.beginWith;
            }

            set
            {
                this.beginWith = value;
                this.NotifyPropertyChanged("BeginWith");
            }
        }

        public bool SortNames
        {
            get
            {
                return this.sortNames;
            }

            set
            {
                this.sortNames = value;
                this.NotifyPropertyChanged("SortNames");
            }
        }


        private int generateNameCount;

        private int sequenceSize;

        private Gender gender;

        private double lengthModifier;

        private bool addToResults;

        private bool controlLength;

        private string beginWith;

        private bool sortNames;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
