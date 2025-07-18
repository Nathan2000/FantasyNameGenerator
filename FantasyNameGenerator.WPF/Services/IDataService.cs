﻿using FantasyNameGenerator.WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyNameGenerator.WPF.Services
{
    internal interface IDataService
    {
        ObservableCollection<NameCategory> GetCategories();
    }
}
