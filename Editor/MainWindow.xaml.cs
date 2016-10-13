using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using EnumerableExtensions;
using Girvel.Graph;
using Isometric.CommonStructures;
using Isometric.Core.Modules.SettingsModule;
using Isometric.Core.Modules.WorldModule.Buildings;
using Isometric.Editor.Containers;
using Isometric.Editor.CustomControls;
using Isometric.Editor.Extensions;
using Isometric.GameDataTools.Exceptions;
using Isometric.Parser;
using Microsoft.Win32;

namespace Isometric.Editor
{
    public partial class MainWindow
    {
        protected BuildingPattern SelectedPattern;
        protected int SelectedPatternListIndex = -1;

        private readonly Control[] _buildingSettingsControls;

        private readonly LabeledTextBox[] _constantTextBoxes;

        protected SaveFileDialog SaveFileDialog
            = new SaveFileDialog();

        protected OpenFileDialog OpenFileDialog
            = new OpenFileDialog();

        private const string DefaultFilter 
            = "Isometric buildings|*.isob|All files|*.*";

        private readonly Dictionary<BuildingPattern, GraphNode<BuildingPattern>> _nodes
            = new Dictionary<BuildingPattern, GraphNode<BuildingPattern>>(); 


        
        public MainWindow()
        {
            InitializeComponent();

            foreach (var type in typeof (BuildingType).GetEnumNames())
            {
                BuildingTypeComboBox.Items.Add(type);
            }

            SaveFileDialog.Filter = DefaultFilter;
            OpenFileDialog.Filter = DefaultFilter;

            var properties = GameConstantAttribute.GetProperties();
            _constantTextBoxes = new LabeledTextBox[properties.Length];

            var i = 0;
            foreach (var property in properties.OrderBy(property => $"{property.DeclaringType.Name}.{property.Name}"))
            {
                var newBox = _constantTextBoxes[i] = new LabeledTextBox
                {
                    LabelText = $"{property.DeclaringType.Name}.{property.Name}",
                    VerticalAlignment = VerticalAlignment.Top,
                };

                newBox.TextBox.FontFamily = new FontFamily("Consolas");

                ConstantsStackPanel.Children.Add(newBox);
                newBox.TextBox.TextChanged += ConstantBoxEvents.GenerateTextChangedEventHandler(property.Name);

                i++;
            }

            _buildingSettingsControls = new Control[]
            {
                IdLabel,
                NameLabel,
                PriceLabel,
                ResourcesLabel,
                TypeLabel,
                UpgradesLabel,
                IdTextBox,
                NameTextBox,
                PriceTextBox,
                ResourcesTextBox,
                BuildingTypeComboBox,
                IdIncrementButton,
                IdDecrementButton,
                UpgradesComboBox,
                UpgradesAddButton,
                UpgradesRemoveButton,
                UpgradesListBox,
            };

            Reset();
        }



        public void Reset()
        {
            BuildingsListBox.Items.Clear();

            GameData.Instance = new GameData();

            ResetBuildingSelection();
        }

        public void AddBuilding(BuildingPattern pattern)
        {
            BuildingsListBox.Items.Add(pattern.Name);

            _nodes[pattern] = GameData.Instance.BuildingGraph.FirstOrDefault(node => node.Value == pattern)
                ?? GameData.Instance.BuildingGraph.NewNode(pattern);
        }

        public void RemoveBuilding(BuildingPattern pattern)
        {
            BuildingsListBox.Items.Remove(pattern.Name);

            _nodes.Remove(pattern);
            GameData.Instance.BuildingPatterns.Remove(pattern);
        }

        public void SelectBuilding(string name, int listIndex)
        {
            SelectedPattern = GameData.Instance.BuildingPatterns.First(p => p.Name == name);
            SelectedPatternListIndex = listIndex;

            foreach (var control in _buildingSettingsControls)
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
            IdTextBox.Text = SelectedPattern.Id.ToString();
            BuildingTypeComboBox.SelectedItem = SelectedPattern.Type.ToString();
            ResourcesTextBox.Text = SelectedPattern.Resources.GetValueString(typeof(Resources));
            PriceTextBox.Text = SelectedPattern.Price.GetValueString(typeof(Resources));

            UpgradesComboBox.Items.Clear();
            UpgradesListBox.Items.Clear();

            foreach (
                var obj 
                    in BuildingsListBox.Items
                        .Cast<object>()
                        .Where(item =>
                            item.ToString() != SelectedPattern.Name
                            && _nodes[SelectedPattern]
                                .GetChildren()
                                .All(node => node.Value.Name != item.ToString())))
            {
                UpgradesComboBox.Items.Add(obj);
            }

            foreach (var child in _nodes[SelectedPattern].GetChildren())
            {
                UpgradesListBox.Items.Add(child.Value.Name);
            }
        }

        public void ResetBuildingSelection()
        {
            SelectedPatternListIndex = -1;
            SelectedPattern = null;

            foreach (var control in _buildingSettingsControls)
            {
                control.IsEnabled = false;
            }
        }

        public void IncrementId(BuildingPattern pattern, int step)
        {
            if (pattern.Id + step > GameData.Instance.BuildingPatterns.LastId
                || pattern.Id + step < 0
                || step == 0)
            {
                return;
            }

            var pattern2 = GameData.Instance.BuildingPatterns.FirstOrDefault(p => p.Id == pattern.Id + step);

            pattern.Id += step;

            if (pattern2 != null)
            {
                pattern2.Id -= step;
            }

            SortBuildings();

            if (pattern == SelectedPattern)
            {
                IdTextBox.Text = (int.Parse(IdTextBox.Text) + step).ToString();
            }
        }

        public void AddUpgrade(string name)
        {
            _nodes[SelectedPattern].AddChild(
                _nodes.First(pair => pair.Value.Value.Name == name).Value);

            UpgradesListBox.Items.Add(name);
            UpgradesComboBox.Items.Remove(name);
        }

        public void RemoveUpgrade(string name)
        {
            _nodes[SelectedPattern].RemoveChild(
                _nodes.First(pair => pair.Value.Value.Name == name).Value);
            
            UpgradesListBox.Items.Remove(name);
            UpgradesComboBox.Items.Add(name);
        }

        public void SortBuildings()
        {
            BuildingsListBox.Items.Clear();
            var patterns = new BuildingPattern[GameData.Instance.BuildingPatterns.LastId + 1];

            foreach (var pattern in GameData.Instance.BuildingPatterns)
            {
                patterns[pattern.Id] = pattern;
            }

            foreach (var pattern in patterns.Where(p => p != null))
            {
                BuildingsListBox.Items.Add(pattern.Name);
            }
        }



        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string name;
            var i = BuildingsListBox.Items.Count;

            do
            {
                name = $"building{i++}";
            } while (GameData.Instance.BuildingPatterns.Any(pattern => pattern.Name == name));

            AddBuilding(GameData.Instance.BuildingPatterns.NewPattern(name));
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (BuildingsListBox.SelectedIndex == -1)
            {
                return;
            }

            RemoveBuilding(
                GameData.Instance.BuildingPatterns.First(
                    pattern => pattern.Name == BuildingsListBox.SelectedItem.ToString()));
        }

        private void BuildingsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
            {
                return;
            }

            SelectBuilding(BuildingsListBox.SelectedItem.ToString(), BuildingsListBox.SelectedIndex);
        }

        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedPattern == null || SelectedPatternListIndex == -1)
            {
                return;
            }

            SelectedPattern.Name = NameTextBox.Text;
            BuildingsListBox.Items[SelectedPatternListIndex] = NameTextBox.Text;
        }

        private void BuildingTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedPattern.Type = 
                (BuildingType) Enum.Parse(
                    typeof (BuildingType), 
                    BuildingTypeComboBox.SelectedItem.ToString());
        }

        private void ResourcesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedPattern == null)
            {
                return;
            }

            SelectedPattern.Resources = ResourcesTextBox.GameResources;
        }

        private void PriceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedPattern == null)
            {
                return;
            }

            SelectedPattern.Price = PriceTextBox.GameResources;
        }

        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog.ShowDialog(this);

            try
            {
                using (var stream = OpenFileDialog.OpenFile())
                {
                    Reset();
                    GameData.Instance = new GameData(stream);
                }
            }
            catch (InvalidOperationException)
            {
                return;
            }
            catch (InvalidGameDataException)
            {
                MessageBox.Show("Invalid file");
                return;
            }

            foreach (var pattern in GameData.Instance.BuildingPatterns)
            {
                AddBuilding(pattern);
            }

            foreach (var constantPair in GameData.Instance.Constants)
            {
                _constantTextBoxes.First(box => box.LabelText == constantPair.Key).Text = constantPair.Value.Value.GetValueString(constantPair.Value.Type);
            }

            SortBuildings();
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog.OverwritePrompt = true;
            SaveFileDialog.ShowDialog(this);

            try
            {
                using (var stream = SaveFileDialog.OpenFile())
                {
                    GameData.Instance.SerializeData(stream);
                }
            }
            catch (InvalidOperationException) { }

            SaveFileDialog.Reset();
        }

        private void UpgradesAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (UpgradesComboBox.SelectedIndex != -1)
            {
                AddUpgrade(UpgradesComboBox.SelectedItem.ToString());
            }
        }

        private void UpgradesRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (UpgradesListBox.SelectedIndex == -1)
            {
                return;
            }

            RemoveUpgrade(UpgradesListBox.SelectedItem.ToString());
        }

        private void IdIncrementButton_Click(object sender, RoutedEventArgs e)
        {
            IncrementId(SelectedPattern, 1);
        }

        private void IdDecrementButton_Click(object sender, RoutedEventArgs e)
        {
            IncrementId(SelectedPattern, -1);
        }
    }
}
