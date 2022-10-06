﻿using SuperControls.Style;
using SuperUtils.Common;
using SuperUtils.IO;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace Jvedio
{
    /// <summary>
    /// Window_TagStamp.xaml 的交互逻辑
    /// </summary>
    public partial class Window_SelectPaths : BaseWindow
    {
        public SolidColorBrush _BackgroundBrush = Brushes.Red;

        public SolidColorBrush BackgroundBrush
        {
            get { return _BackgroundBrush; }
            set { _BackgroundBrush = value; }
        }

        public SolidColorBrush _ForegroundBrush = Brushes.White;

        public SolidColorBrush ForegroundBrush
        {
            get { return _ForegroundBrush; }
            set { _ForegroundBrush = value; }
        }

        public string TagName { get; set; }

        public List<string> Folders { get; set; }

        public Window_SelectPaths()
        {
            InitializeComponent();
            Folders = new List<string>();
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void PathListBox_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Link;
            e.Handled = true; // 必须加
        }

        // 检视
        private void PathListBox_Drop(object sender, DragEventArgs e)
        {
            string[] dragdropFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (var item in dragdropFiles)
            {
                if (!FileHelper.IsFile(item))
                {
                    if (!Folders.Contains(item) && !Folders.IsIntersectWith(item))
                        Folders.Add(item);
                    else
                        MessageCard.Error(SuperControls.Style.LangManager.GetValueByKey("FilePathIntersection"));
                }
            }

            OnListChange();
        }

        public void AddPath(object sender, RoutedEventArgs e)
        {
            var path = FileHelper.SelectPath(this);
            if (Directory.Exists(path))
            {
                if (!Folders.Contains(path) && !Folders.IsIntersectWith(path))
                    Folders.Add(path);
                else
                    MessageCard.Error(SuperControls.Style.LangManager.GetValueByKey("FilePathIntersection"));
            }

            OnListChange();
        }

        public void DelPath(object sender, RoutedEventArgs e)
        {
            if (PathListBox.SelectedIndex >= 0)
            {
                for (int i = PathListBox.SelectedItems.Count - 1; i >= 0; i--)
                {
                    Folders.Remove(PathListBox.SelectedItems[i].ToString());
                }
            }

            OnListChange();
        }

        public void OnListChange()
        {
            PathListBox.ItemsSource = null;
            PathListBox.ItemsSource = Folders;
        }

        public void ClearPath(object sender, RoutedEventArgs e)
        {
            Folders.Clear();
            OnListChange();
        }
    }
}
