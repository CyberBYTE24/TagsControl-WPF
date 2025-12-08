# TagsControl

A WPF User Control for managing collections of tags with visual feedback and auto-complete functionality.

## Overview

The TagsControl is a reusable WPF user control that allows users to manage collections of tags through an intuitive interface. It supports adding tags by typing and selecting from suggestions, removing individual tags, and provides color-coded visualization for better organization.

## Features

- **Tag Management**: Add, remove, and edit tags with visual feedback
- **Auto-complete Suggestions**: Type to see available tags in real-time
- **Color-coded Tags**: Each tag is assigned a unique color based on its display name
- **Customizable Appearance**: Configure colors, borders, and other visual properties
- **Two-way Data Binding**: Seamless integration with MVVM patterns
- **Responsive Design**: Adapts to different container sizes

## Getting Started

### Prerequisites

- .NET Framework 4.7.2 or later
- Visual Studio 2019 or later (recommended)

### Installation

1. Add the TagsControl project to your solution
2. Reference the TagsControl project in your WPF application
3. Include the control in your XAML:

```xml
<Window x:Class="YourApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:tags="clr-namespace:TagsControl;assembly=TagsControl">
    
    <Grid>
        <tags:TagsControl ItemsSource="{Binding AvailableTags}" 
                          SelectedTags="{Binding SelectedTags}" />
    </Grid>
</Window>
```

## Usage

### Properties

| Property | Type | Description |
|----------|------|-------------|
| `ItemsSource` | `IEnumerable<TagItem>` | Collection of available tags for selection |
| `SelectedTags` | `IList<TagItem>` | Collection of currently selected tags |
| `TagsBackgroundBrushes` | `ObservableCollection<Brush>` | Custom color palette for tags |
| `BorderCornerRadius` | `CornerRadius` | Corner radius for the outer border |
| `InnerBorderBrush` | `Brush` | Brush for inner border |
| `InnerBorderThickness` | `Thickness` | Thickness of inner border |
| `InnerBackground` | `Brush` | Background brush for inner elements |

### Data Model

The control uses the `TagItem` model which has two properties:

```csharp
public class TagItem
{
    public string DisplayName { get; set; }  // Text to display in tag
    public string Value { get; set; }        // Internal identifier
}
```

## Example Usage

In your ViewModel:

```csharp
public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<TagItem> AvailableTags { get; set; }
    public ObservableCollection<TagItem> SelectedTags { get; set; }

    public MainViewModel()
    {
        AvailableTags = new ObservableCollection<TagItem>
        {
            new TagItem("C#", "c_sharp"),
            new TagItem("MVVM", "mvvm"),
            new TagItem("XAML", "xaml"),
            new TagItem("DotNet", "dotnet")
        };

        SelectedTags = new ObservableCollection<TagItem>();
    }
}
```

In your XAML:

```xml
<tags:TagsControl ItemsSource="{Binding AvailableTags}" 
                  SelectedTags="{Binding SelectedTags}" />
```

## Customization

### Custom Colors

To customize tag colors, set the `TagsBackgroundBrushes` property:

```xml
<tags:TagsControl ItemsSource="{Binding AvailableTags}" 
                  SelectedTags="{Binding SelectedTags}">
    <tags:TagsControl.TagsBackgroundBrushes>
        <SolidColorBrush Color="Red"/>
        <SolidColorBrush Color="Blue"/>
        <SolidColorBrush Color="Green"/>
    </tags:TagsControl.TagsBackgroundBrushes>
</tags:TagsControl>
```

### Styling

You can override the default styles by defining new styles in your application resources:

```xml
<Style TargetType="Border" x:Key="TagChipStyle">
    <!-- Custom style for tag chips -->
</Style>

<Style TargetType="Button" x:Key="DeleteButtonStyle">
    <!-- Custom style for delete buttons -->
</Style>
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- Inspired by tag management UI patterns in modern web applications
- Uses deterministic color assignment for consistent tag visualization