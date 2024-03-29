﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using SimpleClassCreator.Code_Factory;

namespace SimpleClassCreatorUI
{
    /// <summary>
    /// Interaction logic for DTOMakerControl.xaml
    /// </summary>
    public partial class DTOMakerControl : UserControl
    {
        private string AssemblyName { get { return txtAssemblyFullFilePath.Text; } }
        private string ClassName { get { return txtFullyQualifiedClassName.Text; } }

        public DTOMakerControl()
        {
            InitializeComponent();
        }

        private void btnAssemblyOpenDialog_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

            if (!ofd.ShowDialog().Value)
                return;

            txtAssemblyFullFilePath.Text = ofd.FileName;

            lblAssemblyChosen.Content = System.IO.Path.GetFileName(ofd.FileName);
        }

        private void lblClassName_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Help;
        }

        private void lblClassName_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void lblClassName_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string strHelp = 
@"A Fully Qualified Class Name is providing the class name with its namespace separated by periods. 
Example: If you have a class named Product, but it exists in My.Project.Business,
then enter: My.Project.Business.Product, in the text box below. 
Please keep in mind casing matters.";

            MessageBox.Show(strHelp, "What is a Fully Qualified Class Name?");
        }

        private void btnLoadClass_Click(object sender, RoutedEventArgs e)
        {
            AssemblyInfo asm = Proxy.GetClassProperties(AssemblyName, ClassName);

            LoadTreeView(asm);
        }

        private void LoadTreeView(AssemblyInfo assembly)
        { 
            TreeViewItem asm = new TreeViewItem();
            TreeViewItem cls = null;
            
            asm.Header = assembly.Name;

            foreach(ClassInfo ci in assembly.Classes)
            {
                cls = new TreeViewItem();
                cls.Header = ci.FullName;

                foreach(PropertyInfo pi in ci.Properties)
                    cls.Items.Add(MakeOption(pi));

                asm.Items.Add(cls);
            }

            tvAssebliesAndClasses.Items.Add(asm);
        }

        private StackPanel MakeOption(PropertyInfo info)
        {
            CheckBox cbx = new CheckBox();
            cbx.Name = "cbx_" + info.Name;
            cbx.IsChecked = true;

            Label lbl = new Label();
            lbl.Name = "lbl_" + info.Name;
            lbl.Content = info.ToString();

            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Children.Add(cbx);
            sp.Children.Add(lbl);

            //TreeViewItem tvi = new TreeViewItem();

            //tvi.Items.Add(sp);
            //tvi.Header = info.ToString();

            return sp;
        }
    }
}
