using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Specialized;
using GenericDotNetSupportLibrary.Attributes;

namespace GenericDotNetSupportLibrary.Helpers
{
    /// <summary>
    /// Helper methods for objects which support INotifyPropertyChanged. <seealso cref="INotifyPropertyChanged"/>
    /// </summary>
    public static class NotifyPropertyChangedHelper
    {
        /// <summary>
        /// Takes a component which supports INotifyPropertyChanged and applies a handler to it and all of its descendnets.
        /// </summary>
        /// <param name="component">The component which is to be watched for changes.</param>
        /// <param name="changedHandler">The handler to be invoked when a change occurs.</param>
        public static void SetupPropertyChanged(INotifyPropertyChanged component, EventHandler changedHandler)
        {
            SetupPropertyChanged(new List<object>(), component, changedHandler);
        }

        static void SetupPropertyChanged(IList<object> closed, INotifyPropertyChanged component, EventHandler changedHandler)
        {
            if (closed.Contains(component)) return; // event was already registered

            closed.Add(component); //adds the property that is to be processed

            //sets the property changed event if the property isn't a collection
            if (!(component is INotifyCollectionChanged))
                component.PropertyChanged += (sender, e) =>
                    {
                        PropertyInfo info = sender.GetType().GetProperties().Where(p => p.Name == e.PropertyName).FirstOrDefault();
                        if (info.GetCustomAttributes(typeof(SkipPropertyChangedAttribute), false).Length == 0)
                            changedHandler(sender, EventArgs.Empty);
                    };

            /*
             * If the component is an enumerable there are two steps. First check to see if it supports the INotifyCollectionChanged event.
             * If it supports it add and handler on to this object to support notification.  Next iterate through the collection of objects
             * to add hook up their PropertyChangedEvent.
             * 
             * If the component isn't a collection then iterate through its properties and attach the changed handler to the properties.
             */
            if (component is IEnumerable<object>)
            {
                if (component is INotifyCollectionChanged)
                {
                    //((INotifyCollectionChanged)component).CollectionChanged += collectionHandler;
                    ((INotifyCollectionChanged)component).CollectionChanged += (sender, e) =>
                        {
                            changedHandler(sender, EventArgs.Empty);
                        };
                }

                foreach (object obj in component as IEnumerable<object>)
                {

                    if (obj is INotifyPropertyChanged)
                        SetupPropertyChanged(closed, (INotifyPropertyChanged)obj, changedHandler);
                }
            }
            else
            {
                foreach (PropertyInfo info in component.GetType().GetProperties())
                {
                    var propertyValue = info.GetValue(component, new object[] { });
                    var inpc = propertyValue as INotifyPropertyChanged;
                    if (inpc == null) continue;
                    SetupPropertyChanged(closed, inpc, changedHandler);
                }
            }
        }
    }
}
