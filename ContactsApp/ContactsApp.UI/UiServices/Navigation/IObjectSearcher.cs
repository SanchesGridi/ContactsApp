using System.Windows;

using ContactsApp.UI.Models;

namespace ContactsApp.UI.UiServices.Navigation
{
    public interface IObjectSearcher
    {
        TWpfObject GetLogicalObject<TWpfObject>(DependencyObject dependencyObject, string objectName) where TWpfObject : DependencyObject;
        TWpfObject GetLogicalObject<TWpfObject>(string objectName) where TWpfObject : DependencyObject;
        TWpfObject GetVisualObject<TWpfObject>(DependencyObject dependencyObject, int objectIndex) where TWpfObject : DependencyObject;
    }
}