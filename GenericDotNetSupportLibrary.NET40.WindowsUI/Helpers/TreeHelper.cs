﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;

namespace GenericDotNetSupportLibrary.Helpers
{
    /// <summary>
    /// Collection of methods to provide support for TreeView objects.
    /// <remarks>Also provides additional functionality to the TreeView through the addition of the IsMouseDirectlyOverItem property.</remarks>
    /// </summary>
    public static class TreeHelper
    {
        /// <summary>
        /// Gets the parent of a TreeViewItem.
        /// <remarks>
        /// Starts a visual upward search to find the next TreeViewItem.
        /// </remarks>
        /// </summary>
        /// <param name="source">The source where the search should take place.  Can be another TreeViewItem or an event which occurred in the TreeView's structure.</param>
        /// <returns>The parent TreeViewItem if one was found.</returns>
        public static TreeViewItem GetParent(DependencyObject source)
        {
            return UIHelper.VisualUpwardSearch<TreeViewItem>(VisualTreeHelper.GetParent(source)) as TreeViewItem;
        }

        //
        // The TreeViewItem that the mouse is currently directly over (or null).
        //
        private static TreeViewItem _currentItem = null;

        //
        // IsMouseDirectlyOverItem:  A DependencyProperty that will be true only on the
        // TreeViewItem that the mouse is directly over.  I.e., this won't be set on that
        // parent item.
        //
        // This is the only public member, and is read-only.
        //

        // The property key (since this is a read-only DP)
        private static readonly DependencyPropertyKey IsMouseDirectlyOverItemKey = DependencyProperty.RegisterAttachedReadOnly("IsMouseDirectlyOverItem",
            typeof(bool), typeof(TreeHelper), 
            new FrameworkPropertyMetadata(null, new CoerceValueCallback(CalculateIsMouseDirectlyOverItem)));

        /// <summary>
        /// The IsMouseDirectlyOverItem property
        /// </summary>
        public static readonly DependencyProperty IsMouseDirectlyOverItemProperty =
            IsMouseDirectlyOverItemKey.DependencyProperty;

        // A strongly-typed getter for the property.
        /// <summary>
        /// A strongly 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetIsMouseDirectlyOverItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMouseDirectlyOverItemProperty);
        }

        /// <summary>
        /// A coercion method for the property
        /// </summary>
        /// <param name="item"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static object CalculateIsMouseDirectlyOverItem(DependencyObject item, object value)
        {
            // This method is called when the IsMouseDirectlyOver property is being calculated
            // for a TreeViewItem. 

            if (item == _currentItem)
                return true;
            else
                return false;
        }

        /// <summary>
        /// A private RoutedEvent used to find the nearest encapsulating
        /// TreeViewItem to the mouse's current position.
        /// </summary>
        private static readonly RoutedEvent UpdateOverItemEvent = EventManager.RegisterRoutedEvent(
            "UpdateOverItem", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TreeHelper));

        //
        // Class constructor
        //

        static TreeHelper()
        {
            // Get all Mouse enter/leave events for TreeViewItem.
            EventManager.RegisterClassHandler(typeof(TreeViewItem),
                                      TreeViewItem.MouseEnterEvent,
                                      new MouseEventHandler(OnMouseTransition), true);
            EventManager.RegisterClassHandler(typeof(TreeViewItem),
                                      TreeViewItem.MouseLeaveEvent,
                                      new MouseEventHandler(OnMouseTransition), true);

            // Listen for the UpdateOverItemEvent on all TreeViewItem's.
            EventManager.RegisterClassHandler(typeof(TreeViewItem),
                                      UpdateOverItemEvent,
                                      new RoutedEventHandler(OnUpdateOverItem));
        }


        /// <summary>
        /// This method is a listener for the UpdateOverItemEvent.  When it is received,
        /// it means that the sender is the closest TreeViewItem to the mouse (closest in the sense of the
        /// tree, not geographically).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void OnUpdateOverItem(object sender, RoutedEventArgs args)
        {
            // Mark this object as the tree view item over which the mouse
            // is currently positioned.
            _currentItem = sender as TreeViewItem;

            // Tell that item to re-calculate the IsMouseDirectlyOverItem property
            _currentItem.InvalidateProperty(IsMouseDirectlyOverItemProperty);

            // Prevent this event from notifying other tree view items higher in the tree.
            args.Handled = true;
        }

        /// <summary>
        /// This method is a listener for both the MouseEnter event and
        /// the MouseLeave event on TreeViewItems.  It updates the _currentItem, and updates
        /// the IsMouseDirectlyOverItem property on the previous TreeViewItem and the new
        /// TreeViewItem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void OnMouseTransition(object sender, MouseEventArgs args)
        {
            lock (IsMouseDirectlyOverItemProperty)
            {
                if (_currentItem != null)
                {
                    // Tell the item that previously had the mouse that it no longer does.
                    DependencyObject oldItem = _currentItem;
                    _currentItem = null;
                    oldItem.InvalidateProperty(IsMouseDirectlyOverItemProperty);
                }

                // Get the element that is currently under the mouse.
                IInputElement currentPosition = Mouse.DirectlyOver;

                // See if the mouse is still over something (any element, not just a tree view item).
                if (currentPosition != null)
                {
                    // Yes, the mouse is over something.
                    // Raise an event from that point.  If a TreeViewItem is anywhere above this point
                    // in the tree, it will receive this event and update _currentItem.

                    RoutedEventArgs newItemArgs = new RoutedEventArgs(UpdateOverItemEvent);
                    currentPosition.RaiseEvent(newItemArgs);

                }
            }
        }
    }

}
