using System;
using System.Windows;
using System.Windows.Media;

using ContactsApp.Infrastructure.Extensions;

namespace ContactsApp.UI.UiServices.Navigation
{
    public class ObjectSearcher : IObjectSearcher
    {
        public TWpfObject GetLogicalObject<TWpfObject>(DependencyObject dependencyObject, string objectName) where TWpfObject : DependencyObject
        {
            try
            {
                var logicalObject = LogicalTreeHelper.FindLogicalNode(dependencyObject, objectName).AsRef<TWpfObject>();

                return logicalObject;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TWpfObject GetLogicalObject<TWpfObject>(string objectName) where TWpfObject : DependencyObject
        {
            try
            {
                var window = Application.Current.MainWindow;
                var logicalObject = LogicalTreeHelper.FindLogicalNode(window, objectName).AsRef<TWpfObject>();

                return logicalObject;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public TWpfObject GetVisualObject<TWpfObject>(DependencyObject dependencyObject, int objectIndex) where TWpfObject : DependencyObject
        {
            try
            {
                var visualObject = VisualTreeHelper.GetChild(dependencyObject, objectIndex).AsRef<TWpfObject>();

                return visualObject;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}