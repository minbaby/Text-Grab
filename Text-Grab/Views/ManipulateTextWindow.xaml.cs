﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Text_Grab.Utilities;
using Text_Grab.Views;

namespace Text_Grab
{
    /// <summary>
    /// Interaction logic for ManipulateTextWindow.xaml
    /// </summary>
    public partial class ManipulateTextWindow : Window
    {
        public string CopiedText { get; set; } = "";

        public bool WrapText { get; set; } = false;

        public ManipulateTextWindow()
        {
            InitializeComponent();
        }

        public ManipulateTextWindow(string rawPassedString)
        {
            int lastCommaPosition = rawPassedString.AllIndexesOf(",").LastOrDefault();            
            CopiedText = rawPassedString.Substring(0,lastCommaPosition);
            InitializeComponent();
            PassedTextControl.Text = CopiedText;
            string langString = rawPassedString.Substring(lastCommaPosition + 1, (rawPassedString.Length - (lastCommaPosition + 1)));
            XmlLanguage lang = XmlLanguage.GetLanguage(langString);
            CultureInfo culture = lang.GetEquivalentCulture();
            if (culture.TextInfo.IsRightToLeft)
            {
                PassedTextControl.TextAlignment = TextAlignment.Right;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RoutedCommand newFullscreenGrab = new RoutedCommand();
            _ = newFullscreenGrab.InputGestures.Add(new KeyGesture(Key.F, ModifierKeys.Control));
            _ = CommandBindings.Add(new CommandBinding(newFullscreenGrab, keyedCtrlF));

            RoutedCommand newGrabFrame = new RoutedCommand();
            _ = newGrabFrame.InputGestures.Add(new KeyGesture(Key.G, ModifierKeys.Control));
            _ = CommandBindings.Add(new CommandBinding(newGrabFrame, keyedCtrlG));
        }

        private void keyedCtrlF(object sender, ExecutedRoutedEventArgs e)
        {
            WindowUtilities.NormalLaunch(true);
        }

        private void keyedCtrlG(object sender, ExecutedRoutedEventArgs e)
        {
            CheckForGrabFrameOrLaunch();
        }

        private void CopyCloseBTN_Click(object sender, RoutedEventArgs e)
        {
            string clipboardText = PassedTextControl.Text;
            Clipboard.SetText(clipboardText);
            this.Close();
        }

        private void SaveBTN_Click(object sender, RoutedEventArgs e)
        {
            string fileText = PassedTextControl.Text;

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt|All(*.*)|*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                RestoreDirectory = true,
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, fileText);
            }
        }

        private void SingleLineBTN_Click(object sender, RoutedEventArgs e)
        {
            string textToEdit = PassedTextControl.Text;
            PassedTextControl.Text = "";
            textToEdit = textToEdit.Replace('\n', ' ');
            textToEdit = textToEdit.Replace('\r', ' ');
            textToEdit = textToEdit.Replace(Environment.NewLine, " ");
            Regex regex = new Regex("[ ]{2,}");
            textToEdit = regex.Replace(textToEdit, " ");
            textToEdit = textToEdit.Trim();
            PassedTextControl.Text = textToEdit;
        }

        private void WrapTextCHBOX_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)WrapTextMenuItem.IsChecked)
                PassedTextControl.TextWrapping = TextWrapping.Wrap;
            else
                PassedTextControl.TextWrapping = TextWrapping.NoWrap;
        }

        private void TrimEachLineMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string workingString = PassedTextControl.Text;
            List<string> stringSplit = workingString.Split('\n').ToList();

            string finalString = "";
            foreach (string line in stringSplit)
            {
                if(string.IsNullOrWhiteSpace(line) == false)
                    finalString += line.Trim() + "\n";
            }

            PassedTextControl.Text = finalString;
        }

        public void AddThisText(string textToAdd)
        {
            PassedTextControl.Text += textToAdd;
        }

        private void TryToNumberMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string workingString = PassedTextControl.Text;

            workingString = workingString.TryFixToNumbers();

            PassedTextControl.Text = workingString;
        }
        private void TryToAlphaMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string workingString = PassedTextControl.Text;

            workingString = workingString.TryFixToLetters();

            PassedTextControl.Text = workingString;
        }

        private void ClearSeachBTN_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox searchBox = sender as TextBox;
            searchBox.Text = "";
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PassedTextControl.SelectedText = SearchTextBox.Text;
        }

        private void SplitLineBeforeSelectionMI_Click(object sender, RoutedEventArgs e)
        {
            string selectedText = PassedTextControl.SelectedText;

            if(string.IsNullOrEmpty(selectedText))
            {
                MessageBox.Show("No text selected", "Did not split lines");
                return;
            }

            string textToManipulate = PassedTextControl.Text;

            textToManipulate = textToManipulate.Replace(selectedText, "\n" + selectedText);

            PassedTextControl.Text = textToManipulate;
        }

        private void RejoinLinesAtSelectionMI_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddTextBTN_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CheckForGrabFrameOrLaunch()
        {
            WindowCollection allWindows = System.Windows.Application.Current.Windows;

            foreach (Window window in allWindows)
            {
                if (window is GrabFrame grabFrame)
                {
                    grabFrame.Activate();
                    return;
                }
            }

            GrabFrame gf = new GrabFrame();
            gf.Show();
        }

        private void OpenGrabFrame_Click(object sender, RoutedEventArgs e)
        {
            CheckForGrabFrameOrLaunch();
        }

        private void NewFullscreen_Click(object sender, RoutedEventArgs e)
        {
            WindowUtilities.NormalLaunch(true);
        }
    }
}
