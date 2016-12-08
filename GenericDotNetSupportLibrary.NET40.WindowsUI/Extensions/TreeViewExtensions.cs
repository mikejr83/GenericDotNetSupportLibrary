using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Media;

namespace GenericDotNetSupportLibrary.Extensions
{
    /// <summary>
    /// Extensions to the TreeView class for additional functionality.
    /// </summary>
    public static class TreeViewExtensions
    {
        /// <summary>
        /// Expands all children of a TreeView
        /// </summary>
        /// <param name="treeView">The TreeView whose children will be expanded</param>
        public static void ExpandAll(this TreeView treeView)
        {
            ExpandSubContainers(treeView);
        }

        /// <summary>
        /// Expands all children of a TreeView or TreeViewItem
        /// </summary>
        /// <param name="parentContainer">The TreeView or TreeViewItem containing the children to expand</param>
        private static void ExpandSubContainers(ItemsControl parentContainer)
        {
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (currentContainer != null && currentContainer.Items.Count > 0)
                {
                    //expand the item
                    currentContainer.IsExpanded = true;

                    //if the item's children are not generated, they must be expanded
                    if (currentContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                    {
                        //store the event handler in a variable so we can remove it (in the handler itself)
                        EventHandler eh = null;
                        eh = new EventHandler(delegate
                        {
                            //once the children have been generated, expand those children's children then remove the event handler
                            if (currentContainer.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                            {
                                ExpandSubContainers(currentContainer);
                                currentContainer.ItemContainerGenerator.StatusChanged -= eh;
                            }
                        });

                        currentContainer.ItemContainerGenerator.StatusChanged += eh;
                    }
                    else //otherwise the children have already been generated, so we can now expand those children
                    {
                        ExpandSubContainers(currentContainer);
                    }
                }
            }
        }

        /// <summary>
        /// Searches a TreeView for the provided object and selects it if found
        /// </summary>
        /// <param name="treeView">The TreeView containing the item</param>
        /// <param name="item">The item to search and select</param>
        public static void SelectItem(this TreeView treeView, object item)
        {
            ExpandAndSelectItem(treeView, item);
        }

        /// <summary>
        /// Searches through the tree to find an item and select it.
        /// </summary>
        /// <param name="treeView">The TreeView which will be searched.</param>
        /// <param name="item">The item to find.</param>
        /// <returns>The TreeViewItem containing the item.</returns>
        public static TreeViewItem FindTreeViewItem(this TreeView treeView, object item)
        {
            return ExpandAndSelectItem(treeView, item);
        }

        /// <summary>
        /// Finds the provided object in an ItemsControl's children and selects it
        /// </summary>
        /// <param name="parentContainer">The parent container whose children will be searched for the selected item</param>
        /// <param name="itemToSelect">The item to select</param>
        /// <returns>True if the item is found and selected, false otherwise</returns>
        private static TreeViewItem ExpandAndSelectItem(ItemsControl parentContainer, object itemToSelect)
        {
            //check all items at the current level
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = null;

                if (item is TreeViewItem)
                    currentContainer = item as TreeViewItem;
                else
                    currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

                //if the data item matches the item we want to select, set the corresponding
                //TreeViewItem IsSelected to true
                if ((item == itemToSelect && currentContainer != null) || (item == currentContainer && currentContainer.DataContext == itemToSelect))
                {
                    currentContainer.IsSelected = true;
                    currentContainer.BringIntoView();
                    currentContainer.Focus();

                    //the item was found
                    return currentContainer;
                }
            }

            //if we get to this point, the selected item was not found at the current level, so we must check the children
            foreach (Object item in parentContainer.Items)
            {
                TreeViewItem currentContainer = null;

                if (item is TreeViewItem)
                    currentContainer = item as TreeViewItem;
                else
                    currentContainer = parentContainer.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;

                //if children exist
                if (currentContainer != null && currentContainer.Items.Count > 0)
                {
                    //keep track of if the TreeViewItem was expanded or not
                    bool wasExpanded = currentContainer.IsExpanded;

                    //expand the current TreeViewItem so we can check its child TreeViewItems
                    currentContainer.IsExpanded = true;

                    //if the TreeViewItem child containers have not been generated, we must listen to
                    //the StatusChanged event until they are
                    if (currentContainer.ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated)
                    {
                        //store the event handler in a variable so we can remove it (in the handler itself)
                        EventHandler eh = null;
                        eh = new EventHandler(delegate
                        {
                            if (currentContainer.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                            {
                                TreeViewItem selectedItem = ExpandAndSelectItem(currentContainer, itemToSelect);
                                if (selectedItem == null)
                                {
                                    //The assumption is that code executing in this EventHandler is the result of the parent not
                                    //being expanded since the containers were not generated.
                                    //since the itemToSelect was not found in the children, collapse the parent since it was previously collapsed
                                    currentContainer.IsExpanded = false;
                                }

                                //remove the StatusChanged event handler since we just handled it (we only needed it once)
                                currentContainer.ItemContainerGenerator.StatusChanged -= eh;
                            }
                        });
                        currentContainer.ItemContainerGenerator.StatusChanged += eh;
                    }
                    else //otherwise the containers have been generated, so look for item to select in the children
                    {
                        TreeViewItem selectedItem = ExpandAndSelectItem(currentContainer, itemToSelect);
                        if (selectedItem == null)
                        {
                            //restore the current TreeViewItem's expanded state
                            currentContainer.IsExpanded = wasExpanded;
                        }
                        else //otherwise the node was found and selected, so return true
                        {
                            return selectedItem;
                        }
                    }
                }
            }

            //no item was found
            return null;
        }
    }
}
