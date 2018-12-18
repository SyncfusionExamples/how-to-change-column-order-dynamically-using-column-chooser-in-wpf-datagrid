#region Copyright Syncfusion Inc. 2001 - 2018
// Copyright Syncfusion Inc. 2001 - 2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using ColumnChooserDemo;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace ColumnChooserDemo
{
    public class InitialBehavior : Behavior<MainWindow>
    {
        #region Overrides

        protected override void OnAttached()
        {
            this.AssociatedObject.Loaded += OnAssociatedObjectLoaded;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            this.AssociatedObject.Loaded -= OnAssociatedObjectLoaded;
            base.OnDetaching();
        }

        #endregion

        ViewModel viewModel;

        void OnAssociatedObjectLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel = (this.AssociatedObject.DataContext) as ViewModel;
            viewModel.ColumnChooserCommand = new DelegateCommand<object>(ColumnChooserCommandHandler);
            ShowColumnChooser();
        }

        public void ColumnChooserCommandHandler(object param)
        {
            ShowColumnChooser();
        }

        void ShowColumnChooser()
        {
            var columns = this.AssociatedObject.dataGrid.Columns;
            ObservableCollection<ColumnChooserItems> hiddenColumns;
            ObservableCollection<ColumnChooserItems> visibleColumns;
            GetColumnsDetails(columns, out hiddenColumns, out visibleColumns);
            CustomColumnChooserViewModel chooserViewModel = new CustomColumnChooserViewModel(hiddenColumns, visibleColumns);
            CustomColumnChooser ColumnChooserView = new CustomColumnChooser();
            ColumnChooserView.DataContext = chooserViewModel;
            ColumnChooserView.Owner = Application.Current.MainWindow;
            if ((bool)ColumnChooserView.ShowDialog())
                ClickOKButton(chooserViewModel.HiddenColumnCollection, chooserViewModel.VisibleColumnCollection, this.AssociatedObject.dataGrid);
        }

        public void ClickOKButton(ObservableCollection<ColumnChooserItems> hiddenColumnCollection, ObservableCollection<ColumnChooserItems> visibleColumnCollection, SfDataGrid dataGrid)
        {
            dataGrid.Columns.Suspend();
            Columns columns = new Columns();
            foreach(var item in visibleColumnCollection)
            {
                var column = dataGrid.Columns.FirstOrDefault(col => col.MappingName == item.Name);
                if (column != null)
                    columns.Add(column);
                if (column.IsHidden)
                    column.IsHidden = false;
            }
            foreach (var item in hiddenColumnCollection)
            {
                var column = dataGrid.Columns.FirstOrDefault(col => col.MappingName == item.Name);
                if (column != null)
                    columns.Add(column);
                if (!column.IsHidden)
                    column.IsHidden = true;
            }
            dataGrid.Columns.Clear();
            dataGrid.Columns = columns;
            dataGrid.Columns.Resume();
            dataGrid.RefreshColumns();
            viewModel.ShowColumnChooser = false;
        }

        public static void GetColumnsDetails(Columns totalVisibleColumns, out ObservableCollection<ColumnChooserItems> hiddenColumns, out ObservableCollection<ColumnChooserItems> visibleColumns)
        {
            hiddenColumns = new ObservableCollection<ColumnChooserItems>();
            visibleColumns = new ObservableCollection<ColumnChooserItems>();
            foreach (var actualColumn in totalVisibleColumns)
            {
                ColumnChooserItems item = new ColumnChooserItems();
                if (actualColumn.IsHidden)
                {
                    item.Name = actualColumn.MappingName;
                    hiddenColumns.Add(item);
                }
                else
                {
                    item.Name = actualColumn.MappingName;
                    visibleColumns.Add(item);
                }
            }
        }
    }
}
