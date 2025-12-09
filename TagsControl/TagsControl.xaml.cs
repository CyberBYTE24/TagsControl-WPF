using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using TagsControl.Models;

namespace TagsControl
{
    /// <summary>
    /// A user control for managing tags in the application.
    /// Provides functionality to add, remove, and edit tags with visual feedback.
    /// </summary>
    /// <remarks>
    /// This control allows users to manage collections of tags through a user-friendly interface.
    /// It supports adding tags by typing and selecting from suggestions, removing individual tags,
    /// and provides color-coded visualization for better organization.
    /// </remarks>
    public partial class TagsControl : UserControl
    {
        /// <summary>
        /// DependencyProperty for Source of Tags to selection
        /// </summary>
        /// <remarks>
        /// This property represents the collection of available tags that can be selected.
        /// It is used to populate the auto-complete suggestions as the user types.
        /// </remarks>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(TagsControl),
                new PropertyMetadata(null, OnItemsSourceChanged));

        /// <summary>
        /// DependencyProperty of Source of selected Tags
        /// </summary>
        /// <remarks>
        /// This property represents the collection of currently selected tags.
        /// It supports two-way data binding and updates the UI when changed.
        /// </remarks>
        public static readonly DependencyProperty SelectedTagsProperty =
            DependencyProperty.Register("SelectedTags", typeof(IList), typeof(TagsControl),
                new PropertyMetadata(null, OnSelectedTagsChanged));

        /// <summary>
        /// DependencyProperty for Background of inner Border
        /// </summary>
        /// <remarks>
        /// This property controls the background brushes used for tag visualization.
        /// It allows customization of tag colors through a collection of Brush objects.
        /// </remarks>
        public static readonly DependencyProperty TagsBackgroundBrushesProperty =
            DependencyProperty.Register("TagsBackgroundBrushes", typeof(ObservableCollection<Brush>), typeof(TagsControl),
                new PropertyMetadata(null, OnTagsBackgroundBrushesChanged));

        /// <summary>
        /// DependencyProperty for CornerRadius of outer Border
        /// </summary>
        /// <remarks>
        /// This property controls the corner radius of the outer border of the control.
        /// It allows for rounded corners on the entire control container.
        /// </remarks>
        public static readonly DependencyProperty BorderCornerRadiusProperty =
            DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(TagsControl),
                new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// DependencyProperty for BorderBrush inner Border
        /// </summary>
        /// <remarks>
        /// This property controls the border brush of the inner elements in the control.
        /// It defines the color of the border around the tag container and suggestions.
        /// </remarks>
        public static readonly DependencyProperty InnerBorderBrushProperty =
            DependencyProperty.Register("InnerBorderBrush", typeof(Brush), typeof(TagsControl),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(0, 150, 200))));

        /// <summary>
        /// DependencyProperty for BorderThickness of inner Border
        /// </summary>
        /// <remarks>
        /// This property controls the thickness of the border around inner elements.
        /// It defines how thick the border appears around the tag container and suggestions.
        /// </remarks>
        public static readonly DependencyProperty InnerBorderThicknessProperty =
            DependencyProperty.Register("InnerBorderThickness", typeof(Thickness), typeof(TagsControl),
                new PropertyMetadata(new Thickness(0.5)));

        /// <summary>
        /// DependencyProperty for Background of inner Border
        /// </summary>
        /// <remarks>
        /// This property controls the background color of the inner elements in the control.
        /// It defines the background color of the tag container and suggestions panel.
        /// </remarks>
        public static readonly DependencyProperty InnerBackgroundProperty =
            DependencyProperty.Register("InnerBackground", typeof(Brush), typeof(TagsControl),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the collection of background brushes for tags
        /// </summary>
        /// <remarks>
        /// This property allows customization of tag colors. When set, it overrides the default color palette.
        /// The control will cycle through these brushes when assigning colors to new tags.
        /// </remarks>
        public ObservableCollection<Brush> TagsBackgroundBrushes
        {
            get => (ObservableCollection<Brush>)GetValue(TagsBackgroundBrushesProperty);
            set => SetValue(TagsBackgroundBrushesProperty, value);
        }

        /// <summary>
        /// Gets or sets the corner radius for the outer border
        /// </summary>
        /// <remarks>
        /// This property controls the roundness of the control's outer border.
        /// A value of 0 means sharp corners, while higher values create more rounded corners.
        /// </remarks>
        public CornerRadius BorderCornerRadius
        {
            get => (CornerRadius)GetValue(BorderCornerRadiusProperty);
            set => SetValue(BorderCornerRadiusProperty, value);
        }

        /// <summary>
        /// Gets or sets the thickness of the inner border
        /// </summary>
        /// <remarks>
        /// This property controls how thick the border appears around the tag container and suggestions.
        /// </remarks>
        public Thickness InnerBorderThickness
        {
            get => (Thickness)GetValue(InnerBorderThicknessProperty);
            set => SetValue(InnerBorderThicknessProperty, value);
        }

        /// <summary>
        /// Gets or sets the brush for the inner border
        /// </summary>
        /// <remarks>
        /// This property controls the color of the border around the tag container and suggestions.
        /// </remarks>
        public Brush InnerBorderBrush
        {
            get => (Brush)GetValue(InnerBorderBrushProperty);
            set => SetValue(InnerBorderBrushProperty, value);
        }

        /// <summary>
        /// Gets or sets the background for inner elements
        /// </summary>
        /// <remarks>
        /// This property controls the background color of the tag container and suggestions panel.
        /// </remarks>
        public Brush InnerBackground
        {
            get => (Brush)GetValue(InnerBackgroundProperty);
            set => SetValue(InnerBackgroundProperty, value);
        }

        private int _selectedIndex = -1;
        private List<TagItem> _suggestions = new List<TagItem>();
        private TextBox InputTextBox;
        private int _selectedTagIndex = -1; // index of selected tag
        private INotifyCollectionChanged _currentSelectedTagsCollection; // for tracking changes to the selected tags collection
        private INotifyCollectionChanged _currentBackgroundBrushesCollection; // for tracking changes to the color collection

        // Base color palette for deterministic colors
        private static readonly Brush[] _brushes = new Brush[]
        {
            new SolidColorBrush(Color.FromRgb(255, 87, 34)),    // Red
            new SolidColorBrush(Color.FromRgb(156, 39, 176)),   // Purple
            new SolidColorBrush(Color.FromRgb(63, 81, 181)),    // Indigo
            new SolidColorBrush(Color.FromRgb(33, 150, 243)),   // Blue
            new SolidColorBrush(Color.FromRgb(0, 188, 212)),    // Cyan
            new SolidColorBrush(Color.FromRgb(0, 150, 136)),    // Teal
            new SolidColorBrush(Color.FromRgb(76, 175, 80)),    // Green
            new SolidColorBrush(Color.FromRgb(139, 195, 74)),   // Light Green
            new SolidColorBrush(Color.FromRgb(205, 220, 57)),   // Lime
            new SolidColorBrush(Color.FromRgb(255, 193, 7)),    // Amber
            new SolidColorBrush(Color.FromRgb(255, 152, 0)),    // Orange
            new SolidColorBrush(Color.FromRgb(96, 125, 139)),   // Blue Grey
            new SolidColorBrush(Color.FromRgb(121, 85, 72)),    // Brown
            new SolidColorBrush(Color.FromRgb(158, 158, 158)),  // Grey
            new SolidColorBrush(Color.FromRgb(96, 125, 139)),   // Blue Grey
            new SolidColorBrush(Color.FromRgb(103, 58, 183))    // Deep Purple
        };

        // cache for colors by DisplayName for performance
        private Dictionary<string, int> _colorIndexCache = new Dictionary<string, int>();

        static TagsControl()
        {
            //FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata(
            //    new ObservableCollection<Brush>(),
            //    FrameworkPropertyMetadataOptions.AffectsMeasure);
            //
            //TagsBackgroundBrushesProperty.OverrideMetadata(typeof(TagsControl), metadata);
        }

        /// <summary>
        /// Initializes a new instance of the TagsControl class
        /// </summary>
        /// <remarks>
        /// This constructor initializes the control's UI elements and sets up event handlers.
        /// It creates the input TextBox programmatically and configures its properties.
        /// </remarks>
        public TagsControl()
        {
            InitializeComponent();

            // Create TextBox programmatically BEFORE setting SelectedTags,
            // to avoid calling UpdateTagsUI before TextBox initialization
            InputTextBox = new TextBox
            {
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = new Thickness(2),
                MinWidth = 100,
                BorderThickness = new Thickness(0),
                Background = Brushes.Transparent
            };
            InputTextBox.SetBinding(Control.ForegroundProperty, new Binding("Foreground") { Source = this });
            InputTextBox.SetBinding(TextBox.CaretBrushProperty, new Binding("Foreground") { Source = this });
            InputTextBox.KeyDown += InputTextBox_KeyDown;
            InputTextBox.PreviewKeyDown += InputTextBox_PreviewKeyDown;
            InputTextBox.TextChanged += InputTextBox_TextChanged;

            // Configure PlacementTarget for Popup
            SuggestionsPopup.PlacementTarget = InputTextBox;
            SuggestionsPopupBorder.SetBinding(Control.BackgroundProperty, new Binding("Background") { Source = this});
            // Configure PlacementTarget for Popup
            this.PreviewKeyDown += TagsControl_PreviewKeyDown;

            // Set SelectedTags after creating TextBox
            SelectedTags = new List<TagItem>();

            if(TagsBackgroundBrushes == null)
            {
                TagsBackgroundBrushes = new ObservableCollection<Brush>();
            }
        }

        /// <summary>
        /// Handles mouse down events on the click area to focus the input textbox
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">Mouse button event arguments</param>
        /// <remarks>
        /// This method ensures that clicking anywhere in the control focuses the input field,
        /// except when clicking directly on a tag or delete button.
        /// </remarks>
        private void ClickArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var source = e.OriginalSource as DependencyObject;
            if (IsInsideTagOrDeleteButton(source))
            {
                // Let the event propagate so that the delete button handler can execute
                return;
            }

            FocusInputTextBox();
            e.Handled = true;
        }

        private void TagsScrollViewer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var source = e.OriginalSource as DependencyObject;
            if (IsInsideTagOrDeleteButton(source))
            {
                return;
            }

            // If the click was on an empty space, move focus to the input field
            FocusInputTextBox();
            e.Handled = true;
        }

        /// <summary>
        /// Determines if the mouse event occurred inside a tag or delete button
        /// </summary>
        /// <param name="source">The dependency object that triggered the event</param>
        /// <returns>True if the event occurred inside a tag or delete button, false otherwise</returns>
        private bool IsInsideTagOrDeleteButton(DependencyObject source)
        {
            if (source == null) return false;

            var tagChipStyle = FindResource("TagChipStyle") as Style;

            DependencyObject current = source;
            while (current != null)
            {
                if (current is Button)
                {
                    return true;
                }

                if (current is Border border && border.Style == tagChipStyle)
                {
                    return true;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return false;
        }

        private void ClickArea_MouseEnter(object sender, MouseEventArgs e)
        {
            // Set IBeam cursor on hover only if not over a tag
            var source = e.OriginalSource as DependencyObject;
            if (!IsInsideTagOrDeleteButton(source))
            {
                this.Cursor = Cursors.IBeam;
            }
        }

        private void ClickArea_MouseLeave(object sender, MouseEventArgs e)
        {
            // Return standard arrow cursor
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// Sets focus to the input text box and positions the cursor at the end of the text
        /// </summary>
        private void FocusInputTextBox()
        {
            if (InputTextBox != null)
            {
                InputTextBox.Focus();
                Keyboard.Focus(InputTextBox);
                // Set cursor to end of text
                if (InputTextBox.Text != null)
                {
                    InputTextBox.CaretIndex = InputTextBox.Text.Length;
                }
            }
        }

        /// <summary>
        /// Gets or sets the collection of available tags for selection
        /// </summary>
        /// <remarks>
        /// This property represents the source data that populates the auto-complete suggestions.
        /// It should be an IEnumerable of TagItem objects.
        /// </remarks>
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        /// <summary>
        /// Gets or sets the collection of currently selected tags
        /// </summary>
        /// <remarks>
        /// This property represents the collection of tags that are currently selected.
        /// It supports two-way data binding and updates the UI when changed.
        /// </remarks>
        public IList SelectedTags
        {
            get => (IList)GetValue(SelectedTagsProperty);
            set => SetValue(SelectedTagsProperty, value);
        }

        /// <summary>
        /// Handles changes to the ItemsSource property
        /// </summary>
        /// <param name="d">The dependency object</param>
        /// <param name="e">Dependency property changed event arguments</param>
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TagsControl;
            control?.UpdateSuggestions();
        }

        /// <summary>
        /// Handles changes to the SelectedTags property
        /// </summary>
        /// <param name="d">The dependency object</param>
        /// <param name="e">Dependency property changed event arguments</param>
        private static void OnSelectedTagsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TagsControl;
            if (control != null)
            {
                // Unsubscribe from the old collection
                if (control._currentSelectedTagsCollection != null)
                {
                    control._currentSelectedTagsCollection.CollectionChanged -= control.SelectedTags_CollectionChanged;
                }

                // Subscribe to the new collection if it supports notifications
                control._currentSelectedTagsCollection = e.NewValue as INotifyCollectionChanged;
                if (control._currentSelectedTagsCollection != null)
                {
                    control._currentSelectedTagsCollection.CollectionChanged += control.SelectedTags_CollectionChanged;
                }

                control.UpdateTagsUI();
            }
        }

        /// <summary>
        /// Handles changes to the TagsBackgroundBrushes property
        /// </summary>
        /// <param name="d">The dependency object</param>
        /// <param name="e">Dependency property changed event arguments</param>
        private static void OnTagsBackgroundBrushesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TagsControl;
            if (control != null)
            {
                if (control._currentBackgroundBrushesCollection != null)
                {
                    control._currentBackgroundBrushesCollection.CollectionChanged -= control.SelectedTags_CollectionChanged;
                }

                control._currentBackgroundBrushesCollection = e.NewValue as INotifyCollectionChanged;
                if (control._currentBackgroundBrushesCollection != null)
                {
                    control._currentBackgroundBrushesCollection.CollectionChanged += control.SelectedTags_CollectionChanged;
                }

                control.UpdateTagsUI();
            }
        }

        /// <summary>
        /// Handles collection change events for selected tags
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">Collection changed event arguments</param>
        private void SelectedTags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Update UI when any changes occur in the collection (addition, removal, movement)
            UpdateTagsUI();
        }

        /// <summary>
        /// Updates the user interface to reflect current tag selections
        /// </summary>
        /// <remarks>
        /// This method rebuilds the UI by clearing existing tags and recreating them based on the SelectedTags collection.
        /// It also ensures that the input textbox is always positioned at the end of the tag container.
        /// </remarks>
        private void UpdateTagsUI()
        {
            // Check that InputTextBox is created
            if (InputTextBox == null) return;

            TagsContainer.Children.Clear();

            if (SelectedTags != null)
            {
                foreach (TagItem tag in SelectedTags)
                {
                    var tagChip = CreateTagChip(tag);
                    TagsContainer.Children.Add(tagChip);
                }
            }

            // Always add TextBox at the end after all tags
            TagsContainer.Children.Add(InputTextBox);

            // Reset tag selection when updating UI
            _selectedTagIndex = -1;
            ClearTagSelection();

            // Ensure focus remains in TextBox after UI update
            if (InputTextBox.IsFocused || Keyboard.FocusedElement == InputTextBox)
            {
                // Small delay for proper focus setting
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    InputTextBox.Focus();
                    Keyboard.Focus(InputTextBox);
                }), System.Windows.Threading.DispatcherPriority.Input);
            }
        }

        /// <summary>
        /// Creates a visual representation of a tag chip
        /// </summary>
        /// <param name="tag">The tag item to create a chip for</param>
        /// <returns>A Border control representing the tag chip</returns>
        private Border CreateTagChip(TagItem tag)
        {

            // Get deterministic color for this display name
            Brush backgroundBrush = GetDeterministicBrush(tag.DisplayName);
            Color foregroundColor = GetContrastColor(backgroundBrush);

            var border = new Border
            {
                Style = (Style)FindResource("TagChipStyle"),
                Background = backgroundBrush,
                Cursor = Cursors.Arrow // Normal cursor for tags
            };

            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };

            var textBlock = new TextBlock
            {
                Text = tag.DisplayName,
                Foreground = new SolidColorBrush(foregroundColor),
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 4, 0),
                FontSize = 12,
                FontWeight = FontWeights.Medium
            };

            var deleteButton = new Button
            {
                Style = (Style)FindResource("DeleteButtonStyle"),
                Tag = tag,
                Cursor = Cursors.Hand
            };
            deleteButton.Click += DeleteButton_Click;
            deleteButton.MouseEnter += (s, e) => { deleteButton.Cursor = Cursors.Hand; };
            deleteButton.MouseLeave += (s, e) => { deleteButton.Cursor = Cursors.Hand; };

            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(deleteButton);

            border.Child = stackPanel;

            // Handlers for cursor on tag
            border.MouseEnter += (s, e) =>
            {
                border.Cursor = Cursors.Arrow;
                // Prevent parent element cursor change
                e.Handled = true;
            };
            border.MouseLeave += (s, e) =>
            {
                border.Cursor = Cursors.Arrow;
                e.Handled = true;
            };

            return border;
        }

        /// <summary>
        /// Gets a deterministic brush color for a tag based on its display name
        /// </summary>
        /// <param name="displayName">The display name of the tag</param>
        /// <returns>A Brush object representing the color for this tag</returns>
        private Brush GetDeterministicBrush(string displayName)
        {
            bool userTagsBackgroundInitialized = TagsBackgroundBrushes != null && TagsBackgroundBrushes.Count > 0;


            if (string.IsNullOrEmpty(displayName))
                return userTagsBackgroundInitialized ? TagsBackgroundBrushes[0] : _brushes[0];

            // Generate and limit index
            int index = Math.Abs(GetStableHash(displayName)) % (userTagsBackgroundInitialized ? TagsBackgroundBrushes.Count : _brushes.Length);

            // Check value in cache
            if (_colorIndexCache.TryGetValue(displayName, out var cachedBrush))
                return userTagsBackgroundInitialized ? TagsBackgroundBrushes[index] : _brushes[index];


            // Save Color index in cache
            _colorIndexCache[displayName] = index;

            return userTagsBackgroundInitialized ? TagsBackgroundBrushes[index] : _brushes[index];
        }

        /// <summary>
        /// Calculates the appropriate text color for contrast against a background brush
        /// </summary>
        /// <param name="backgroundBrush">The background brush to calculate contrast for</param>
        /// <returns>A Color that provides good contrast with the background</returns>
        private Color GetContrastColor(Brush backgroundBrush)
        {
            Color color;
            switch(backgroundBrush)
            {
                case SolidColorBrush solidColorBrush:
                    color = solidColorBrush.Color;
                    break;
                case LinearGradientBrush linearGradientBrush:
                    color = linearGradientBrush.GradientStops[0].Color;
                    break;
                default:
                    return Colors.White;
            }
            double brightness = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

            return brightness < 0.6 ? Colors.White : Colors.Black;
        }

        /// <summary>
        /// Generates a stable hash for a string input
        /// </summary>
        /// <param name="input">The input string to hash</param>
        /// <returns>An integer hash value</returns>
        private int GetStableHash(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0;

            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input.ToLower().Trim());
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Take first 4 bytes for int
                return BitConverter.ToInt32(hashBytes, 0);
            }
        }

        /// <summary>
        /// Handles click events on delete buttons for tags
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">Routed event arguments</param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tag = button?.Tag;

            if (tag != null && SelectedTags.Contains(tag))
            {
                SelectedTags.Remove(tag);
                UpdateTagsUI();
            }
        }

        /// <summary>
        /// Handles text change events in the input textbox
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">Text changed event arguments</param>
        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Reset tag selection when typing text
            if (_selectedTagIndex >= 0)
            {
                _selectedTagIndex = -1;
                ClearTagSelection();
            }
            UpdateSuggestions();
        }

        /// <summary>
        /// Updates the suggestions list based on current input text
        /// </summary>
        private void UpdateSuggestions()
        {
            if (ItemsSource == null || string.IsNullOrWhiteSpace(InputTextBox.Text))
            {
                SuggestionsPopup.IsOpen = false;
                return;
            }

            SuggestionsPanel.Children.Clear();
            _suggestions.Clear();

            string searchText = InputTextBox.Text.ToLower().Trim();

            // Filter data sources
            foreach (TagItem item in ItemsSource)
            {
                if (item.DisplayName.ToLower().Contains(searchText))
                {
                    _suggestions.Add(item);
                    AddSuggestionItem(item);
                }
            }

            SuggestionsPopup.IsOpen = _suggestions.Count > 0;
            _selectedIndex = -1;
        }

        /// <summary>
        /// Adds a suggestion item to the suggestions panel
        /// </summary>
        /// <param name="item">The tag item to add as a suggestion</param>
        private void AddSuggestionItem(TagItem item)
        {
            var border = new Border
            {
                Style = (Style)FindResource("SuggestionItemStyle")
            };

            // Get deterministic color for suggestion
            Brush backgroundBrush = GetDeterministicBrush(item.DisplayName);
            Color foregroundColor = GetContrastColor(backgroundBrush);

            // Create visual representation of suggestion
            var grid = new Grid();

            // Color bar on the left
            var colorBar = new Border
            {
                Width = 4,
                Background = backgroundBrush,
                CornerRadius = new CornerRadius(2, 0, 0, 2),
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(0, 2, 4, 2)
            };

            // Main text
            var textBlock = new TextBlock
            {
                Text = item.DisplayName,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 12
            };

            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(colorBar);
            stackPanel.Children.Add(textBlock);

            grid.Children.Add(stackPanel);

            border.Child = grid;
            border.Tag = item;

            border.MouseEnter += (s, e) => HighlightSuggestion(border);
            border.MouseLeave += (s, e) => UnhighlightSuggestion(border);
            border.MouseLeftButtonDown += (s, e) => SelectSuggestion(item);

            SuggestionsPanel.Children.Add(border);
        }

        /// <summary>
        /// Highlights a suggestion item when the mouse enters
        /// </summary>
        /// <param name="border">The border element to highlight</param>
        private void HighlightSuggestion(Border border)
        {
            border.Background = new SolidColorBrush(Color.FromArgb(25, 0, 120, 215));
        }

        /// <summary>
        /// Removes highlighting from a suggestion item when the mouse leaves
        /// </summary>
        /// <param name="border">The border element to unhighlight</param>
        private void UnhighlightSuggestion(Border border)
        {
            border.Background = Brushes.Transparent;
        }

        /// <summary>
        /// Selects a suggestion item and adds it as a tag
        /// </summary>
        /// <param name="item">The tag item to select</param>
        private void SelectSuggestion(TagItem item)
        {
            AddTag(item);
            InputTextBox.Text = "";
            SuggestionsPopup.IsOpen = false;
        }

        /// <summary>
        /// Handles preview key down events for the control
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">Key event arguments</param>
        private void TagsControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Prevent focus transition by Tab from control
            if (e.Key == Key.Tab && Keyboard.FocusedElement == InputTextBox)
            {
                // Tab is handled in InputTextBox_PreviewKeyDown
                return;
            }
        }

        /// <summary>
        /// Handles preview key down events for the input textbox
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">Key event arguments</param>
        private void InputTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab:
                    // If there is a selected suggestion, add it
                    if (_selectedIndex >= 0 && _selectedIndex < _suggestions.Count && SuggestionsPopup.IsOpen)
                    {
                        AddTag(_suggestions[_selectedIndex]);
                        InputTextBox.Text = "";
                        SuggestionsPopup.IsOpen = false;
                        e.Handled = true;
                    }
                    else
                    {
                        // Prevent focus transition to next element
                        e.Handled = true;
                    }
                    break;
                case Key.Down:
                    if (SuggestionsPopup.IsOpen && _suggestions.Count > 0)
                    {
                        MoveSelection(1);
                        e.Handled = true;
                    }
                    break;
                case Key.Up:
                    if (SuggestionsPopup.IsOpen && _suggestions.Count > 0)
                    {
                        MoveSelection(-1);
                        e.Handled = true;
                    }
                    break;
                case Key.Left:
                    HandleLeftArrow(e);
                    break;
                case Key.Right:
                    HandleRightArrow(e);
                    break;
            }
        }

        /// <summary>
        /// Handles key down events for the input textbox
        /// </summary>
        /// <param name="sender">The event sender</param>
        /// <param name="e">Key event arguments</param>
        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (SuggestionsPopup.IsOpen && _selectedIndex >= 0 && _selectedIndex < _suggestions.Count)
                    {
                        // If there is a selected suggestion, add it
                        AddTag(_suggestions[_selectedIndex]);
                        InputTextBox.Text = "";
                        SuggestionsPopup.IsOpen = false;
                        e.Handled = true;
                    }
                    else if (!string.IsNullOrWhiteSpace(InputTextBox.Text))
                    {
                        // Create new tag
                        TagItem newTag = CreateNewTag(InputTextBox.Text);
                        AddTag(newTag);
                        InputTextBox.Text = "";
                        e.Handled = true;
                    }
                    break;
                case Key.Escape:
                    SuggestionsPopup.IsOpen = false;
                    _selectedTagIndex = -1;
                    ClearTagSelection();
                    e.Handled = true;
                    break;
                case Key.Back:
                    // If field is empty and there are tags, remove the last tag
                    if (string.IsNullOrWhiteSpace(InputTextBox.Text) && SelectedTags.Count > 0)
                    {
                        var lastTag = SelectedTags[SelectedTags.Count - 1];
                        SelectedTags.Remove(lastTag);
                        UpdateTagsUI();
                        InputTextBox.Focus();
                        e.Handled = true;
                    }
                    else if (_selectedTagIndex >= 0 && _selectedTagIndex < SelectedTags.Count)
                    {
                        // If tag is selected, remove it
                        SelectedTags.RemoveAt(_selectedTagIndex);
                        _selectedTagIndex = -1;
                        UpdateTagsUI();
                        InputTextBox.Focus();
                        e.Handled = true;
                    }
                    break;
                case Key.Delete:
                    // If tag is selected, remove it
                    if (_selectedTagIndex >= 0 && _selectedTagIndex < SelectedTags.Count)
                    {
                        SelectedTags.RemoveAt(_selectedTagIndex);
                        if (_selectedTagIndex >= SelectedTags.Count)
                            _selectedTagIndex = SelectedTags.Count - 1;
                        UpdateTagsUI();
                        InputTextBox.Focus();
                        e.Handled = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// Handles left arrow key navigation
        /// </summary>
        /// <param name="e">Key event arguments</param>
        private void HandleLeftArrow(KeyEventArgs e)
        {
            // If cursor is at the beginning of text and field is not empty, allow default behavior
            if (!string.IsNullOrWhiteSpace(InputTextBox.Text) && InputTextBox.SelectionStart > 0)
            {
                e.Handled = false;
                return;
            }

            // If field is empty or cursor is at the beginning, navigate to tags
            if (string.IsNullOrWhiteSpace(InputTextBox.Text) && SelectedTags.Count > 0)
            {
                if (_selectedTagIndex < 0)
                {
                    // Select last tag
                    _selectedTagIndex = SelectedTags.Count - 1;
                }
                else if (_selectedTagIndex > 0)
                {
                    // Move to previous tag
                    _selectedTagIndex--;
                }
                else
                {
                    // Already on first tag, allow default behavior
                    _selectedTagIndex = -1;
                    ClearTagSelection();
                    e.Handled = false;
                    return;
                }
                HighlightSelectedTag();
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        /// <summary>
        /// Handles right arrow key navigation
        /// </summary>
        /// <param name="e">Key event arguments</param>
        private void HandleRightArrow(KeyEventArgs e)
        {
            // If there is a selected tag, remove selection and go to input field
            if (_selectedTagIndex >= 0)
            {
                _selectedTagIndex = -1;
                ClearTagSelection();
                InputTextBox.Focus();
                e.Handled = true;
            }
            else
            {
                // Default behavior for right arrow key
                e.Handled = false;
            }
        }

        /// <summary>
        /// Highlights the currently selected tag
        /// </summary>
        private void HighlightSelectedTag()
        {
            ClearTagSelection();
            if (_selectedTagIndex >= 0 && _selectedTagIndex < TagsContainer.Children.Count - 1)
            {
                var tagBorder = TagsContainer.Children[_selectedTagIndex] as Border;
                if (tagBorder != null)
                {
                    // Add visual selection
                    tagBorder.BorderBrush = new SolidColorBrush(Colors.Blue);
                    tagBorder.BorderThickness = new Thickness(2);
                    tagBorder.BringIntoView();
                }
            }
        }

        /// <summary>
        /// Clears selection from all tags
        /// </summary>
        private void ClearTagSelection()
        {
            foreach (var child in TagsContainer.Children)
            {
                if (child is Border border && !(child is TextBox))
                {
                    border.BorderBrush = Brushes.Transparent;
                    border.BorderThickness = new Thickness(0);
                }
            }
        }

        /// <summary>
        /// Moves the selection in the suggestions list
        /// </summary>
        /// <param name="direction">Direction to move (-1 for up, 1 for down)</param>
        private void MoveSelection(int direction)
        {
            if (_suggestions.Count == 0) return;

            _selectedIndex += direction;
            if (_selectedIndex < 0) _selectedIndex = _suggestions.Count - 1;
            if (_selectedIndex >= _suggestions.Count) _selectedIndex = 0;

            // Update selection
            for (int i = 0; i < SuggestionsPanel.Children.Count; i++)
            {
                var border = SuggestionsPanel.Children[i] as Border;
                if (border != null)
                {
                    if (i == _selectedIndex)
                    {
                        HighlightSuggestion(border);
                        border.BringIntoView();
                    }
                    else
                    {
                        UnhighlightSuggestion(border);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a tag to the selected tags collection
        /// </summary>
        /// <param name="tag">The tag to add</param>
        private void AddTag(TagItem tag)
        {
            if (!SelectedTags.Contains(tag))
            {
                SelectedTags.Add(tag);
                UpdateTagsUI();
            }
        }

        /// <summary>
        /// Creates a new tag from text input
        /// </summary>
        /// <param name="text">The text to create a tag from</param>
        /// <returns>A new TagItem object</returns>
        private TagItem CreateNewTag(string text)
        {
            // Create new tag with proper structure
            // This can be configured for your data type
            string cleanText = text.Trim();
            return new TagItem(cleanText, cleanText.ToLower().Replace(" ", "_").Replace("#", "_sharp"));
        }

        /// <summary>
        /// Gets a property value from an object by name
        /// </summary>
        /// <param name="obj">The object to get the property from</param>
        /// <param name="propertyName">The name of the property to retrieve</param>
        /// <returns>The value of the property or the object itself if not found</returns>
        private object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName)) return obj;

            var type = obj.GetType();
            var property = type.GetProperty(propertyName);
            return property?.GetValue(obj);
        }
    }
}