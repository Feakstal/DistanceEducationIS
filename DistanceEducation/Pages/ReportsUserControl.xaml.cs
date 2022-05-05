﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DistanceEducation.DataBase;

namespace DistanceEducation.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportsUserControl.xaml
    /// </summary>
    public partial class ReportsUserControl : UserControl
    {
        Entities Entities = new Entities();

        public ReportsUserControl()
        {
            InitializeComponent();
            lvReports.ItemsSource = Entities.Report.ToList();
        }
    }
}