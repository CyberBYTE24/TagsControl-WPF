# TagsControl Project

## Project Overview

This is a WPF User Control project that implements a reusable tags management component. The control allows users to:
- Add tags by typing and selecting from suggestions
- Remove existing tags using delete buttons
- Visualize tags with color-coded backgrounds
- Manage both available tags and selected tags through data binding

The control supports customization of appearance including:
- Background colors for tags
- Border styling
- Theme support (light/dark)

## Project Structure

```
TagsControl/
├── TagsControl.sln                 # Visual Studio solution file
├── TagsControl/                    # Main control project
│   ├── TagsControl.csproj          # Project file
│   ├── TagsControl.xaml            # XAML UI definition
│   ├── TagsControl.xaml.cs         # Code-behind logic
│   └── Models/
│       └── TagItem.cs              # Model for tag data
├── TagsControlDemo/                # Demo application showing usage
│   ├── TagsControlDemo.csproj      # Demo project file
│   ├── MainWindow.xaml             # Demo UI
│   ├── MainWindow.xaml.cs          # Demo code-behind
│   └── ViewModel/
│       └── MainWindow.cs           # Demo view model
└── .gitignore                      # Git ignore rules
```

## Key Features

1. **Tag Management**:
   - Add tags by typing and selecting from suggestions
   - Remove tags using delete buttons on each tag
   - Visual feedback with color-coded tags

2. **Suggestions System**:
   - Auto-complete functionality as you type
   - Keyboard navigation (up/down arrows, tab, enter)
   - Color-coded suggestion items matching tag colors

3. **Customization**:
   - Customizable background brushes for tags
   - Border styling options
   - Theme support with light/dark modes

4. **Data Binding**:
   - Two-way binding for selected tags
   - ItemsSource for available tags
   - Collection change notifications

## Technical Details

### TagItem Model
The `TagItem` class represents a single tag with:
- `DisplayName`: Text to display in the UI
- `Value`: Internal string value for identification

### TagsControl Implementation
The control is implemented as a WPF UserControl with:
- Dependency properties for data binding
- Dynamic UI generation for tags and suggestions
- Color management using deterministic hashing
- Keyboard navigation support
- Visual styling through XAML resources

### Usage Example (from demo)
```xml
<tagscontrol:TagsControl
    ItemsSource="{Binding AvailableTags}"
    SelectedTags="{Binding SelectedTags, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"/>
```

## Building and Running

1. Open `TagsControl.sln` in Visual Studio
2. Build the solution (both projects)
3. Run the `TagsControlDemo` project to see the control in action

The control is designed as a reusable component that can be added to any WPF application by referencing the `TagsControl` project.