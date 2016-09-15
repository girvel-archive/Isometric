using System;
using System.Windows;
using System.Windows.Controls;
using Isometric.CommonStructures;
using Isometric.Core.Modules.WorldModule.Buildings;

namespace Isometric.Editor
{
    public partial class MainWindow
    {
        protected BuildingPattern SelectedPattern;
        protected int SelectedPatternListIndex = -1;

        protected readonly Control[] BuildingSettingsControls;



        public MainWindow()
        {
            InitializeComponent();
            
            foreach (var type in typeof (BuildingType).GetEnumNames())
            {
                BuildingTypeComboBox.Items.Add(type);
            }

            BuildingSettingsControls = new Control[]
            {
                IdLabel,
                NameLabel,
                PriceLabel,
                ResourcesLabel,
                TypeLabel,
                IdTextBox,
                NameTextBox,
                PriceTextBox,
                ResourcesTextBox,
                BuildingTypeComboBox,
            };

            ResetBuildingSelection();
        }



        public void AddBuilding(string name)
        {
            BuildingsListBox.Items.Add(name);
            BuildingPatternCollection.Instance.CurrentPatterns.Add(
                new BuildingPattern(
                    name,
                    new Resources(),
                    new Resources(), 
                    new TimeSpan()));
        }

        public void SelectBuilding(string name, int listIndex)
        {
            SelectedPattern = BuildingPatternCollection.Instance.CurrentPatterns.Find(p => p.Name == name);
            SelectedPatternListIndex = listIndex;

            foreach (var control in BuildingSettingsControls)
            {
                control.IsEnabled = true;
            }

            if (SelectedPattern == null)
            {
                MessageBox.Show(
                    "I can't find this building. I'm sorry. " +
                    "Please, report a bug (email: widauka@ya.ru).");

                return;
            }

            NameTextBox.Text = SelectedPattern.Name;
            IdTextBox.Text = SelectedPattern.ID.ToString();
            BuildingTypeComboBox.SelectedItem = SelectedPattern.Type.ToString();
        }

        public void ResetBuildingSelection()
        {
            SelectedPatternListIndex = -1;
            SelectedPattern = null;

            foreach (var control in BuildingSettingsControls)
            {
                control.IsEnabled = false;
            }
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddBuilding($"building{BuildingsListBox.Items.Count}");
        }

        private void BuildingsListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            SelectBuilding(BuildingsListBox.SelectedItem.ToString(), BuildingsListBox.SelectedIndex);
        }

        private void NameTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedPattern == null || SelectedPatternListIndex == -1)
            {
                return;
            }

            SelectedPattern.Name = NameTextBox.Text;
            BuildingsListBox.Items[SelectedPatternListIndex] = NameTextBox.Text;
        }

        private void BuildingTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedPattern.Type = 
                (BuildingType) Enum.Parse(
                    typeof (BuildingType), 
                    BuildingTypeComboBox.SelectedItem.ToString());
        }
    }
}
