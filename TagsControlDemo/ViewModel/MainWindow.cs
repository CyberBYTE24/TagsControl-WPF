using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagsControl.Models;

namespace TagsControlDemo.ViewModel
{
    /// <summary>
    /// View model for the main window that manages available and selected tags.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the collection of available tags.
        /// </summary>
        public ObservableCollection<TagItem> AvailableTags { get; set; }
        
        private ObservableCollection<TagItem> _selectedTags;
        /// <summary>
        /// Gets or sets the collection of selected tags.
        /// </summary>
        public ObservableCollection<TagItem> SelectedTags
        {
            get => _selectedTags;
            set
            {
                if (_selectedTags != value)
                {
                    // Unsubscribe from the old collection
                    if (_selectedTags != null)
                    {
                        _selectedTags.CollectionChanged -= SelectedTagsCollectionChanged;
                    }
                    
                    _selectedTags = value;
                    
                    // Subscribe to the new collection
                    if (_selectedTags != null)
                    {
                        _selectedTags.CollectionChanged += SelectedTagsCollectionChanged;
                    }
                    
                    OnPropertyChanged(nameof(SelectedTags));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            // Initialize available tags
            AvailableTags = new ObservableCollection<TagItem>
        {
            new TagItem("C#", "c_sharp"),
            new TagItem("MVVM", "mvvm"),
            new TagItem("F#", "f_sharp"),
            new TagItem("DotNet", "dotnet")
        };

            SelectedTags = new ObservableCollection<TagItem>(AvailableTags);
        }

        /// <summary>
        /// Handles changes to the selected tags collection.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments containing information about the change.</param>
        private void SelectedTagsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Handle only adding items
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                return;

            // Check that there are new items
            if (e.NewItems == null || e.NewItems.Count == 0)
                return;

            TagItem newTag = e.NewItems[0] as TagItem;
            if (newTag == null)
                return;

            // If such tag already exists in the available list, do nothing
            if (AvailableTags.Any(x => x.Value == newTag.Value))
                return;

            // Add new tag to the available list
            AvailableTags.Add(newTag);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
