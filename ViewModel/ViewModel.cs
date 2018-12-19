#region Copyright Syncfusion Inc. 2001 - 2018
// Copyright Syncfusion Inc. 2001 - 2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Syncfusion.Windows.Shared;
using ColumnChooserDemo.Model;
using Syncfusion.UI.Xaml.Utility;
using System.Windows.Controls;

namespace ColumnChooserDemo
{
    public class ViewModel : NotificationObject
    {
         Northwind northWind;

         /// <summary>
         /// Initializes a new instance of the <see cref="ViewModel"/> class.
         /// </summary>
        public ViewModel()
        {
            OrdersDetail = this.GetOrdersDetail();
        }


        private bool showColumnChooser = true;

        public bool ShowColumnChooser
        {
            get 
            {
                return showColumnChooser; 
            }
            set 
            {
                showColumnChooser = value;
                RaisePropertyChanged("ShowColumnChooser");
            }
        }

        #region Delegate Command

        public DelegateCommand<object> _ColumnChooserCommand;

        /// <summary>
        /// Gets the column chooser command.
        /// </summary>
        /// <value>The column chooser command.</value>
        public DelegateCommand<object> ColumnChooserCommand
        {
            get { return _ColumnChooserCommand; }
            set
            {
                _ColumnChooserCommand = value;
                RaisePropertyChanged("ColumnChooserCommand");
            }
        }
        #endregion

        private ObservableCollection<Orders> _ordersDetail;
        /// <summary>
        /// Gets or sets the orders details.
        /// </summary>
        /// <value>The orders details.</value>
        public ObservableCollection<Orders> OrdersDetail
        {
            get
            {
                return _ordersDetail;
            }
            set
            {
                _ordersDetail = value;
            }
        }

        /// <summary>
        /// Gets the orders details.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Orders> GetOrdersDetail()
        {
            ObservableCollection<Orders> ordersDetail = new ObservableCollection<Orders>();
            string connectionString = string.Format(@"Data Source= " + @"..\..\Northwind.sdf");
            northWind = new Northwind(connectionString);
            var customer = northWind.Orders.Skip(0).Take(100).ToList();
            foreach (var o in customer)
            {
                ordersDetail.Add(o);
            }
            return ordersDetail;
        }
    }

    public class CustomColumnChooserViewModel : NotificationObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomColumnChooserViewModel"/> class.
        /// </summary>
        /// <param name="totalColumns">The total columns.</param>
        public CustomColumnChooserViewModel(ObservableCollection<ColumnChooserItems> hiddenColumns, ObservableCollection<ColumnChooserItems> visibleColumns)
        {
            HiddenColumnCollection = hiddenColumns;
            VisibleColumnCollection = visibleColumns;
        }

        public ObservableCollection<ColumnChooserItems> HiddenColumnCollection
        {
            get;
            set;
        }

        public ObservableCollection<ColumnChooserItems> VisibleColumnCollection
        {
            get;
            set;
        }

        private BaseCommand _MoveRightCommand;
        public BaseCommand MoveRightCommand
        {
            get
            {
                if (_MoveRightCommand == null)
                    _MoveRightCommand = new BaseCommand(MoveRight);
               return _MoveRightCommand;
            }
        }

        void MoveRight(object obj)
        {
            var listBoxes = (object[])obj;
            var hiddenListBox = listBoxes[0] as ListBox;
            var visibleListBox = listBoxes[1] as ListBox;
            if (hiddenListBox.SelectedItem != null)
            {
                var item = hiddenListBox.SelectedItem as ColumnChooserItems;
                var itemsSource = hiddenListBox.ItemsSource as ObservableCollection<ColumnChooserItems>;
                itemsSource.Remove(item);
                (visibleListBox.ItemsSource as ObservableCollection<ColumnChooserItems>).Add(item);
            }
        }

        private BaseCommand _MoveLeftCommand;
        public BaseCommand MoveLeftCommand
        {
            get
            {
                if (_MoveLeftCommand == null)
                    _MoveLeftCommand = new BaseCommand(MoveLeft);
                return _MoveLeftCommand;
            }
        }

        void MoveLeft(object obj)
        {
            var listBoxes = (object[])obj;
            var hiddenListBox = listBoxes[0] as ListBox;
            var visibleListBox = listBoxes[1] as ListBox;
            if (visibleListBox.SelectedItem != null)
            {
                var item = visibleListBox.SelectedItem as ColumnChooserItems;
                var itemsSource = visibleListBox.ItemsSource as ObservableCollection<ColumnChooserItems>;
                itemsSource.Remove(item);
                (hiddenListBox.ItemsSource as ObservableCollection<ColumnChooserItems>).Add(item);
            }
        }

        private BaseCommand _MoveUpCommand;
        public BaseCommand MoveUpCommand
        {
            get
            {
                if (_MoveUpCommand == null)
                    _MoveUpCommand = new BaseCommand(MoveUp);
                return _MoveUpCommand;
            }
        }

        void MoveUp(object obj)
        {
            var listBox = obj as ListBox;
            var itemsSource = listBox.ItemsSource as ObservableCollection<ColumnChooserItems>;
            var selectedIndex = listBox.SelectedIndex;
            if(listBox.SelectedItem != null)
                itemsSource.Move(selectedIndex, selectedIndex - 1);
        }

        private BaseCommand _MoveDownCommand;
        public BaseCommand MoveDownCommand
        {
            get
            {
                if (_MoveDownCommand == null)
                    _MoveDownCommand = new BaseCommand(MoveDown);
                return _MoveDownCommand;
            }
        }

        void MoveDown(object obj)
        {
            var listBox = obj as ListBox;
            var itemsSource = listBox.ItemsSource as ObservableCollection<ColumnChooserItems>;
            var selectedIndex = listBox.SelectedIndex;
            if(listBox.SelectedItem != null)
                itemsSource.Move(selectedIndex, selectedIndex + 1);
        }

        private BaseCommand _OkCommand;
        public BaseCommand OkCommand
        {
            get
            {
                if (_OkCommand == null)
                    _OkCommand = new BaseCommand(OkayClick);
                return _OkCommand;
            }
        }

        void OkayClick(object obj)
        {
            var columnChooserWindow = obj as CustomColumnChooser;
            columnChooserWindow.DialogResult = true;
        }
    }
}
