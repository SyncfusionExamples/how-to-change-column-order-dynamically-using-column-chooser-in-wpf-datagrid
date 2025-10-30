# How to Change Column Order Dynamically using Column Chooser in WPF DataGrid?

This example illustrates how to change column order dynamically using column chooser in [WPF DataGrid](https://www.syncfusion.com/wpf-ui-controls/datagrid) (SfDataGrid).

The [ColumnChooser](https://help.syncfusion.com/wpf/datagrid/interactive-features#%22columnchooser%22) allows you to add or remove columns dynamically from the current view of grid by drag-and-drop operations in DataGrid. You can change the column order dynamically through column chooser by using the `BaseCommand` class as shown in the below code snippet,

#### C#
```c#
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
```


#### XAML
```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="40" />
        <RowDefinition Height="*" />
        <RowDefinition Height="50" />
    </Grid.RowDefinitions>
 
    <Grid.ColumnDefinitions>
        <ColumnDefinition/>
        <ColumnDefinition Width="Auto"/>
        <ColumnDefinition/>
        <ColumnDefinition Width="Auto"/>
    </Grid.ColumnDefinitions>
 
    <Label Content="Hidden Columns" Height="24" Margin="3" FontWeight="Bold" Grid.Row="0" Grid.Column="0" Grid.ColumnSpan="2"/>
    <Label Content="Visible Columns" Height="24" Margin="3" FontWeight="Bold" Grid.Row="0" Grid.Column="2" Grid.ColumnSpan="2"/>
    <ListBox x:Name="listBox1"
             Grid.Row="1"
             Grid.Column="0"
             Margin="0,5"
             HorizontalAlignment="Stretch"
             BorderThickness="0"
             ItemsSource="{Binding HiddenColumnCollection}">
        <ListBox.ItemTemplate>
            <StaticResource ResourceKey="MyDataTemplate" />
        </ListBox.ItemTemplate>
    </ListBox>
 
    <StackPanel Grid.Column="1" Grid.Row="1" Orientation="Vertical">
        <Button Content="Move Right" 
                Command="{Binding MoveRightCommand}" 
                Height="30" 
                Margin="5">
            <Button.CommandParameter>
                <MultiBinding Converter="{StaticResource multiConverter}">
                    <Binding ElementName="listBox1"/>
                    <Binding ElementName="listBox2"/>
                </MultiBinding>
            </Button.CommandParameter>
        </Button>
        <Button Content="Move Left" 
                Command="{Binding MoveLeftCommand}"  
                Height="30" 
                Margin="5">
            <Button.CommandParameter>
                <MultiBinding Converter="{StaticResource multiConverter}">
                    <Binding ElementName="listBox1"/>
                    <Binding ElementName="listBox2"/>
                </MultiBinding>
            </Button.CommandParameter>
        </Button>
    </StackPanel>
 
    <ListBox x:Name="listBox2" Grid.Row="1"
             Grid.Column="2"
             Margin="0,5"
             HorizontalAlignment="Stretch"
             BorderThickness="0"
             ItemsSource="{Binding VisibleColumnCollection}">
        <ListBox.ItemTemplate>
            <StaticResource ResourceKey="MyDataTemplate" />
        </ListBox.ItemTemplate>
    </ListBox>
 
    <StackPanel Grid.Column="3" Grid.Row="1" Orientation="Vertical">
        <Button Content="Move Up" 
                Command="{Binding MoveUpCommand}"
                CommandParameter="{Binding ElementName=listBox2}"
                Height="30" Margin="5"/>
        <Button Content="Move Down" 
                Command="{Binding MoveDownCommand}"
                CommandParameter="{Binding ElementName=listBox2}"
                Height="30" Margin="5"/>
    </StackPanel>
 
    <StackPanel Grid.Row="2" Margin="20,0,0,0" Grid.Column="2" Grid.ColumnSpan="2"
                VerticalAlignment="Stretch"
                Background="Transparent"
                Orientation="Horizontal">
        <Button Margin="5"
                Command="{Binding OkCommand}"
                CommandParameter="{Binding ElementName=ColumnChooserWindow}"
                Content="OK" />
        <Button Margin="5"
                Content="Cancel"
                IsCancel="True"/>
    </StackPanel>
 
</Grid>
```
