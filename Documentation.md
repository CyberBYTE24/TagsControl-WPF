 | ![ReadMe](README.md) | Documentation |

<a name='assembly'></a>
# TagsControl

## Contents

- [Resources](#T-TagsControl-Properties-Resources 'TagsControl.Properties.Resources')
  - [Culture](#P-TagsControl-Properties-Resources-Culture 'TagsControl.Properties.Resources.Culture')
  - [ResourceManager](#P-TagsControl-Properties-Resources-ResourceManager 'TagsControl.Properties.Resources.ResourceManager')
- [TagItem](#T-TagsControl-Models-TagItem 'TagsControl.Models.TagItem')
  - [#ctor(displayName,value)](#M-TagsControl-Models-TagItem-#ctor-System-String,System-String- 'TagsControl.Models.TagItem.#ctor(System.String,System.String)')
  - [DisplayName](#P-TagsControl-Models-TagItem-DisplayName 'TagsControl.Models.TagItem.DisplayName')
  - [Value](#P-TagsControl-Models-TagItem-Value 'TagsControl.Models.TagItem.Value')
- [TagsControl](#T-TagsControl-TagsControl 'TagsControl.TagsControl')
  - [#ctor()](#M-TagsControl-TagsControl-#ctor 'TagsControl.TagsControl.#ctor')
  - [BorderCornerRadiusProperty](#F-TagsControl-TagsControl-BorderCornerRadiusProperty 'TagsControl.TagsControl.BorderCornerRadiusProperty')
  - [InnerBackgroundProperty](#F-TagsControl-TagsControl-InnerBackgroundProperty 'TagsControl.TagsControl.InnerBackgroundProperty')
  - [InnerBorderBrushProperty](#F-TagsControl-TagsControl-InnerBorderBrushProperty 'TagsControl.TagsControl.InnerBorderBrushProperty')
  - [InnerBorderThicknessProperty](#F-TagsControl-TagsControl-InnerBorderThicknessProperty 'TagsControl.TagsControl.InnerBorderThicknessProperty')
  - [ItemsSourceProperty](#F-TagsControl-TagsControl-ItemsSourceProperty 'TagsControl.TagsControl.ItemsSourceProperty')
  - [SelectedTagsProperty](#F-TagsControl-TagsControl-SelectedTagsProperty 'TagsControl.TagsControl.SelectedTagsProperty')
  - [TagsBackgroundBrushesProperty](#F-TagsControl-TagsControl-TagsBackgroundBrushesProperty 'TagsControl.TagsControl.TagsBackgroundBrushesProperty')
  - [BorderCornerRadius](#P-TagsControl-TagsControl-BorderCornerRadius 'TagsControl.TagsControl.BorderCornerRadius')
  - [InnerBackground](#P-TagsControl-TagsControl-InnerBackground 'TagsControl.TagsControl.InnerBackground')
  - [InnerBorderBrush](#P-TagsControl-TagsControl-InnerBorderBrush 'TagsControl.TagsControl.InnerBorderBrush')
  - [InnerBorderThickness](#P-TagsControl-TagsControl-InnerBorderThickness 'TagsControl.TagsControl.InnerBorderThickness')
  - [ItemsSource](#P-TagsControl-TagsControl-ItemsSource 'TagsControl.TagsControl.ItemsSource')
  - [SelectedTags](#P-TagsControl-TagsControl-SelectedTags 'TagsControl.TagsControl.SelectedTags')
  - [TagsBackgroundBrushes](#P-TagsControl-TagsControl-TagsBackgroundBrushes 'TagsControl.TagsControl.TagsBackgroundBrushes')
  - [AddSuggestionItem(item)](#M-TagsControl-TagsControl-AddSuggestionItem-TagsControl-Models-TagItem- 'TagsControl.TagsControl.AddSuggestionItem(TagsControl.Models.TagItem)')
  - [AddTag(tag)](#M-TagsControl-TagsControl-AddTag-TagsControl-Models-TagItem- 'TagsControl.TagsControl.AddTag(TagsControl.Models.TagItem)')
  - [ClearTagSelection()](#M-TagsControl-TagsControl-ClearTagSelection 'TagsControl.TagsControl.ClearTagSelection')
  - [ClickArea_MouseDown(sender,e)](#M-TagsControl-TagsControl-ClickArea_MouseDown-System-Object,System-Windows-Input-MouseButtonEventArgs- 'TagsControl.TagsControl.ClickArea_MouseDown(System.Object,System.Windows.Input.MouseButtonEventArgs)')
  - [CreateNewTag(text)](#M-TagsControl-TagsControl-CreateNewTag-System-String- 'TagsControl.TagsControl.CreateNewTag(System.String)')
  - [CreateTagChip(tag)](#M-TagsControl-TagsControl-CreateTagChip-TagsControl-Models-TagItem- 'TagsControl.TagsControl.CreateTagChip(TagsControl.Models.TagItem)')
  - [DeleteButton_Click(sender,e)](#M-TagsControl-TagsControl-DeleteButton_Click-System-Object,System-Windows-RoutedEventArgs- 'TagsControl.TagsControl.DeleteButton_Click(System.Object,System.Windows.RoutedEventArgs)')
  - [FocusInputTextBox()](#M-TagsControl-TagsControl-FocusInputTextBox 'TagsControl.TagsControl.FocusInputTextBox')
  - [GetContrastColor(backgroundBrush)](#M-TagsControl-TagsControl-GetContrastColor-System-Windows-Media-Brush- 'TagsControl.TagsControl.GetContrastColor(System.Windows.Media.Brush)')
  - [GetDeterministicBrush(displayName)](#M-TagsControl-TagsControl-GetDeterministicBrush-System-String- 'TagsControl.TagsControl.GetDeterministicBrush(System.String)')
  - [GetPropertyValue(obj,propertyName)](#M-TagsControl-TagsControl-GetPropertyValue-System-Object,System-String- 'TagsControl.TagsControl.GetPropertyValue(System.Object,System.String)')
  - [GetStableHash(input)](#M-TagsControl-TagsControl-GetStableHash-System-String- 'TagsControl.TagsControl.GetStableHash(System.String)')
  - [HandleLeftArrow(e)](#M-TagsControl-TagsControl-HandleLeftArrow-System-Windows-Input-KeyEventArgs- 'TagsControl.TagsControl.HandleLeftArrow(System.Windows.Input.KeyEventArgs)')
  - [HandleRightArrow(e)](#M-TagsControl-TagsControl-HandleRightArrow-System-Windows-Input-KeyEventArgs- 'TagsControl.TagsControl.HandleRightArrow(System.Windows.Input.KeyEventArgs)')
  - [HighlightSelectedTag()](#M-TagsControl-TagsControl-HighlightSelectedTag 'TagsControl.TagsControl.HighlightSelectedTag')
  - [HighlightSuggestion(border)](#M-TagsControl-TagsControl-HighlightSuggestion-System-Windows-Controls-Border- 'TagsControl.TagsControl.HighlightSuggestion(System.Windows.Controls.Border)')
  - [InitializeComponent()](#M-TagsControl-TagsControl-InitializeComponent 'TagsControl.TagsControl.InitializeComponent')
  - [InputTextBox_KeyDown(sender,e)](#M-TagsControl-TagsControl-InputTextBox_KeyDown-System-Object,System-Windows-Input-KeyEventArgs- 'TagsControl.TagsControl.InputTextBox_KeyDown(System.Object,System.Windows.Input.KeyEventArgs)')
  - [InputTextBox_PreviewKeyDown(sender,e)](#M-TagsControl-TagsControl-InputTextBox_PreviewKeyDown-System-Object,System-Windows-Input-KeyEventArgs- 'TagsControl.TagsControl.InputTextBox_PreviewKeyDown(System.Object,System.Windows.Input.KeyEventArgs)')
  - [InputTextBox_TextChanged(sender,e)](#M-TagsControl-TagsControl-InputTextBox_TextChanged-System-Object,System-Windows-Controls-TextChangedEventArgs- 'TagsControl.TagsControl.InputTextBox_TextChanged(System.Object,System.Windows.Controls.TextChangedEventArgs)')
  - [IsInsideTagOrDeleteButton(source)](#M-TagsControl-TagsControl-IsInsideTagOrDeleteButton-System-Windows-DependencyObject- 'TagsControl.TagsControl.IsInsideTagOrDeleteButton(System.Windows.DependencyObject)')
  - [MoveSelection(direction)](#M-TagsControl-TagsControl-MoveSelection-System-Int32- 'TagsControl.TagsControl.MoveSelection(System.Int32)')
  - [OnItemsSourceChanged(d,e)](#M-TagsControl-TagsControl-OnItemsSourceChanged-System-Windows-DependencyObject,System-Windows-DependencyPropertyChangedEventArgs- 'TagsControl.TagsControl.OnItemsSourceChanged(System.Windows.DependencyObject,System.Windows.DependencyPropertyChangedEventArgs)')
  - [OnSelectedTagsChanged(d,e)](#M-TagsControl-TagsControl-OnSelectedTagsChanged-System-Windows-DependencyObject,System-Windows-DependencyPropertyChangedEventArgs- 'TagsControl.TagsControl.OnSelectedTagsChanged(System.Windows.DependencyObject,System.Windows.DependencyPropertyChangedEventArgs)')
  - [OnTagsBackgroundBrushesChanged(d,e)](#M-TagsControl-TagsControl-OnTagsBackgroundBrushesChanged-System-Windows-DependencyObject,System-Windows-DependencyPropertyChangedEventArgs- 'TagsControl.TagsControl.OnTagsBackgroundBrushesChanged(System.Windows.DependencyObject,System.Windows.DependencyPropertyChangedEventArgs)')
  - [SelectSuggestion(item)](#M-TagsControl-TagsControl-SelectSuggestion-TagsControl-Models-TagItem- 'TagsControl.TagsControl.SelectSuggestion(TagsControl.Models.TagItem)')
  - [SelectedTags_CollectionChanged(sender,e)](#M-TagsControl-TagsControl-SelectedTags_CollectionChanged-System-Object,System-Collections-Specialized-NotifyCollectionChangedEventArgs- 'TagsControl.TagsControl.SelectedTags_CollectionChanged(System.Object,System.Collections.Specialized.NotifyCollectionChangedEventArgs)')
  - [TagsControl_PreviewKeyDown(sender,e)](#M-TagsControl-TagsControl-TagsControl_PreviewKeyDown-System-Object,System-Windows-Input-KeyEventArgs- 'TagsControl.TagsControl.TagsControl_PreviewKeyDown(System.Object,System.Windows.Input.KeyEventArgs)')
  - [UnhighlightSuggestion(border)](#M-TagsControl-TagsControl-UnhighlightSuggestion-System-Windows-Controls-Border- 'TagsControl.TagsControl.UnhighlightSuggestion(System.Windows.Controls.Border)')
  - [UpdateSuggestions()](#M-TagsControl-TagsControl-UpdateSuggestions 'TagsControl.TagsControl.UpdateSuggestions')
  - [UpdateTagsUI()](#M-TagsControl-TagsControl-UpdateTagsUI 'TagsControl.TagsControl.UpdateTagsUI')

<a name='T-TagsControl-Properties-Resources'></a>
## Resources `type`

##### Namespace

TagsControl.Properties

##### Summary

Класс ресурсов со строгим типом для поиска локализованных строк и пр.

<a name='P-TagsControl-Properties-Resources-Culture'></a>
### Culture `property`

##### Summary

Переопределяет свойство CurrentUICulture текущего потока для всех
  подстановки ресурсов с помощью этого класса ресурсов со строгим типом.

<a name='P-TagsControl-Properties-Resources-ResourceManager'></a>
### ResourceManager `property`

##### Summary

Возврат кэшированного экземпляра ResourceManager, используемого этим классом.

<a name='T-TagsControl-Models-TagItem'></a>
## TagItem `type`

##### Namespace

TagsControl.Models

##### Summary

Represents a single tag item with display name and value properties.
This model is used to store information about individual tags in the TagsControl.

##### Remarks

The TagItem class provides a simple data structure for tags that can be 
displayed in the UI and managed through the TagsControl. It supports both
display text (DisplayName) and an internal value (Value) for identification purposes.

<a name='M-TagsControl-Models-TagItem-#ctor-System-String,System-String-'></a>
### #ctor(displayName,value) `constructor`

##### Summary

Initializes a new instance of the TagItem class with specified display name and value.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| displayName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The text to display in the tag |
| value | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The internal string value for identification |

##### Remarks

This constructor allows creating a new tag with both display name and value 
properties set at initialization time.

<a name='P-TagsControl-Models-TagItem-DisplayName'></a>
### DisplayName `property`

##### Summary

Gets or sets the text to display in tag

##### Remarks

This property represents the user-facing text that will be displayed 
on the tag in the UI. It's used for visual identification of the tag.

<a name='P-TagsControl-Models-TagItem-Value'></a>
### Value `property`

##### Summary

Gets or sets the string value

##### Remarks

This property represents an internal identifier for the tag. 
It can be used to uniquely identify tags in data processing and 
is particularly useful when working with databases or APIs.

<a name='T-TagsControl-TagsControl'></a>
## TagsControl `type`

##### Namespace

TagsControl

##### Summary

A user control for managing tags in the application.
Provides functionality to add, remove, and edit tags with visual feedback.

##### Remarks

This control allows users to manage collections of tags through a user-friendly interface.
It supports adding tags by typing and selecting from suggestions, removing individual tags,
and provides color-coded visualization for better organization.

<a name='M-TagsControl-TagsControl-#ctor'></a>
### #ctor() `constructor`

##### Summary

Initializes a new instance of the TagsControl class

##### Parameters

This constructor has no parameters.

##### Remarks

This constructor initializes the control's UI elements and sets up event handlers.
It creates the input TextBox programmatically and configures its properties.

<a name='F-TagsControl-TagsControl-BorderCornerRadiusProperty'></a>
### BorderCornerRadiusProperty `constants`

##### Summary

DependencyProperty for CornerRadius of outer Border

##### Remarks

This property controls the corner radius of the outer border of the control.
It allows for rounded corners on the entire control container.

<a name='F-TagsControl-TagsControl-InnerBackgroundProperty'></a>
### InnerBackgroundProperty `constants`

##### Summary

DependencyProperty for Background of inner Border

##### Remarks

This property controls the background color of the inner elements in the control.
It defines the background color of the tag container and suggestions panel.

<a name='F-TagsControl-TagsControl-InnerBorderBrushProperty'></a>
### InnerBorderBrushProperty `constants`

##### Summary

DependencyProperty for BorderBrush inner Border

##### Remarks

This property controls the border brush of the inner elements in the control.
It defines the color of the border around the tag container and suggestions.

<a name='F-TagsControl-TagsControl-InnerBorderThicknessProperty'></a>
### InnerBorderThicknessProperty `constants`

##### Summary

DependencyProperty for BorderThickness of inner Border

##### Remarks

This property controls the thickness of the border around inner elements.
It defines how thick the border appears around the tag container and suggestions.

<a name='F-TagsControl-TagsControl-ItemsSourceProperty'></a>
### ItemsSourceProperty `constants`

##### Summary

DependencyProperty for Source of Tags to selection

##### Remarks

This property represents the collection of available tags that can be selected.
It is used to populate the auto-complete suggestions as the user types.

<a name='F-TagsControl-TagsControl-SelectedTagsProperty'></a>
### SelectedTagsProperty `constants`

##### Summary

DependencyProperty of Source of selected Tags

##### Remarks

This property represents the collection of currently selected tags.
It supports two-way data binding and updates the UI when changed.

<a name='F-TagsControl-TagsControl-TagsBackgroundBrushesProperty'></a>
### TagsBackgroundBrushesProperty `constants`

##### Summary

DependencyProperty for Background of inner Border

##### Remarks

This property controls the background brushes used for tag visualization.
It allows customization of tag colors through a collection of Brush objects.

<a name='P-TagsControl-TagsControl-BorderCornerRadius'></a>
### BorderCornerRadius `property`

##### Summary

Gets or sets the corner radius for the outer border

##### Remarks

This property controls the roundness of the control's outer border.
A value of 0 means sharp corners, while higher values create more rounded corners.

<a name='P-TagsControl-TagsControl-InnerBackground'></a>
### InnerBackground `property`

##### Summary

Gets or sets the background for inner elements

##### Remarks

This property controls the background color of the tag container and suggestions panel.

<a name='P-TagsControl-TagsControl-InnerBorderBrush'></a>
### InnerBorderBrush `property`

##### Summary

Gets or sets the brush for the inner border

##### Remarks

This property controls the color of the border around the tag container and suggestions.

<a name='P-TagsControl-TagsControl-InnerBorderThickness'></a>
### InnerBorderThickness `property`

##### Summary

Gets or sets the thickness of the inner border

##### Remarks

This property controls how thick the border appears around the tag container and suggestions.

<a name='P-TagsControl-TagsControl-ItemsSource'></a>
### ItemsSource `property`

##### Summary

Gets or sets the collection of available tags for selection

##### Remarks

This property represents the source data that populates the auto-complete suggestions.
It should be an IEnumerable of TagItem objects.

<a name='P-TagsControl-TagsControl-SelectedTags'></a>
### SelectedTags `property`

##### Summary

Gets or sets the collection of currently selected tags

##### Remarks

This property represents the collection of tags that are currently selected.
It supports two-way data binding and updates the UI when changed.

<a name='P-TagsControl-TagsControl-TagsBackgroundBrushes'></a>
### TagsBackgroundBrushes `property`

##### Summary

Gets or sets the collection of background brushes for tags

##### Remarks

This property allows customization of tag colors. When set, it overrides the default color palette.
The control will cycle through these brushes when assigning colors to new tags.

<a name='M-TagsControl-TagsControl-AddSuggestionItem-TagsControl-Models-TagItem-'></a>
### AddSuggestionItem(item) `method`

##### Summary

Adds a suggestion item to the suggestions panel

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| item | [TagsControl.Models.TagItem](#T-TagsControl-Models-TagItem 'TagsControl.Models.TagItem') | The tag item to add as a suggestion |

<a name='M-TagsControl-TagsControl-AddTag-TagsControl-Models-TagItem-'></a>
### AddTag(tag) `method`

##### Summary

Adds a tag to the selected tags collection

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| tag | [TagsControl.Models.TagItem](#T-TagsControl-Models-TagItem 'TagsControl.Models.TagItem') | The tag to add |

<a name='M-TagsControl-TagsControl-ClearTagSelection'></a>
### ClearTagSelection() `method`

##### Summary

Clears selection from all tags

##### Parameters

This method has no parameters.

<a name='M-TagsControl-TagsControl-ClickArea_MouseDown-System-Object,System-Windows-Input-MouseButtonEventArgs-'></a>
### ClickArea_MouseDown(sender,e) `method`

##### Summary

Handles mouse down events on the click area to focus the input textbox

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| sender | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | The event sender |
| e | [System.Windows.Input.MouseButtonEventArgs](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.Input.MouseButtonEventArgs 'System.Windows.Input.MouseButtonEventArgs') | Mouse button event arguments |

##### Remarks

This method ensures that clicking anywhere in the control focuses the input field,
except when clicking directly on a tag or delete button.

<a name='M-TagsControl-TagsControl-CreateNewTag-System-String-'></a>
### CreateNewTag(text) `method`

##### Summary

Creates a new tag from text input

##### Returns

A new TagItem object

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| text | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The text to create a tag from |

<a name='M-TagsControl-TagsControl-CreateTagChip-TagsControl-Models-TagItem-'></a>
### CreateTagChip(tag) `method`

##### Summary

Creates a visual representation of a tag chip

##### Returns

A Border control representing the tag chip

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| tag | [TagsControl.Models.TagItem](#T-TagsControl-Models-TagItem 'TagsControl.Models.TagItem') | The tag item to create a chip for |

<a name='M-TagsControl-TagsControl-DeleteButton_Click-System-Object,System-Windows-RoutedEventArgs-'></a>
### DeleteButton_Click(sender,e) `method`

##### Summary

Handles click events on delete buttons for tags

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| sender | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | The event sender |
| e | [System.Windows.RoutedEventArgs](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.RoutedEventArgs 'System.Windows.RoutedEventArgs') | Routed event arguments |

<a name='M-TagsControl-TagsControl-FocusInputTextBox'></a>
### FocusInputTextBox() `method`

##### Summary

Sets focus to the input text box and positions the cursor at the end of the text

##### Parameters

This method has no parameters.

<a name='M-TagsControl-TagsControl-GetContrastColor-System-Windows-Media-Brush-'></a>
### GetContrastColor(backgroundBrush) `method`

##### Summary

Calculates the appropriate text color for contrast against a background brush

##### Returns

A Color that provides good contrast with the background

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| backgroundBrush | [System.Windows.Media.Brush](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.Media.Brush 'System.Windows.Media.Brush') | The background brush to calculate contrast for |

<a name='M-TagsControl-TagsControl-GetDeterministicBrush-System-String-'></a>
### GetDeterministicBrush(displayName) `method`

##### Summary

Gets a deterministic brush color for a tag based on its display name

##### Returns

A Brush object representing the color for this tag

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| displayName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The display name of the tag |

<a name='M-TagsControl-TagsControl-GetPropertyValue-System-Object,System-String-'></a>
### GetPropertyValue(obj,propertyName) `method`

##### Summary

Gets a property value from an object by name

##### Returns

The value of the property or the object itself if not found

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| obj | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | The object to get the property from |
| propertyName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The name of the property to retrieve |

<a name='M-TagsControl-TagsControl-GetStableHash-System-String-'></a>
### GetStableHash(input) `method`

##### Summary

Generates a stable hash for a string input

##### Returns

An integer hash value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| input | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The input string to hash |

<a name='M-TagsControl-TagsControl-HandleLeftArrow-System-Windows-Input-KeyEventArgs-'></a>
### HandleLeftArrow(e) `method`

##### Summary

Handles left arrow key navigation

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| e | [System.Windows.Input.KeyEventArgs](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.Input.KeyEventArgs 'System.Windows.Input.KeyEventArgs') | Key event arguments |

<a name='M-TagsControl-TagsControl-HandleRightArrow-System-Windows-Input-KeyEventArgs-'></a>
### HandleRightArrow(e) `method`

##### Summary

Handles right arrow key navigation

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| e | [System.Windows.Input.KeyEventArgs](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.Input.KeyEventArgs 'System.Windows.Input.KeyEventArgs') | Key event arguments |

<a name='M-TagsControl-TagsControl-HighlightSelectedTag'></a>
### HighlightSelectedTag() `method`

##### Summary

Highlights the currently selected tag

##### Parameters

This method has no parameters.

<a name='M-TagsControl-TagsControl-HighlightSuggestion-System-Windows-Controls-Border-'></a>
### HighlightSuggestion(border) `method`

##### Summary

Highlights a suggestion item when the mouse enters

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| border | [System.Windows.Controls.Border](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.Controls.Border 'System.Windows.Controls.Border') | The border element to highlight |

<a name='M-TagsControl-TagsControl-InitializeComponent'></a>
### InitializeComponent() `method`

##### Summary

InitializeComponent

##### Parameters

This method has no parameters.

<a name='M-TagsControl-TagsControl-InputTextBox_KeyDown-System-Object,System-Windows-Input-KeyEventArgs-'></a>
### InputTextBox_KeyDown(sender,e) `method`

##### Summary

Handles key down events for the input textbox

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| sender | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | The event sender |
| e | [System.Windows.Input.KeyEventArgs](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.Input.KeyEventArgs 'System.Windows.Input.KeyEventArgs') | Key event arguments |

<a name='M-TagsControl-TagsControl-InputTextBox_PreviewKeyDown-System-Object,System-Windows-Input-KeyEventArgs-'></a>
### InputTextBox_PreviewKeyDown(sender,e) `method`

##### Summary

Handles preview key down events for the input textbox

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| sender | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | The event sender |
| e | [System.Windows.Input.KeyEventArgs](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.Input.KeyEventArgs 'System.Windows.Input.KeyEventArgs') | Key event arguments |

<a name='M-TagsControl-TagsControl-InputTextBox_TextChanged-System-Object,System-Windows-Controls-TextChangedEventArgs-'></a>
### InputTextBox_TextChanged(sender,e) `method`

##### Summary

Handles text change events in the input textbox

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| sender | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | The event sender |
| e | [System.Windows.Controls.TextChangedEventArgs](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.Controls.TextChangedEventArgs 'System.Windows.Controls.TextChangedEventArgs') | Text changed event arguments |

<a name='M-TagsControl-TagsControl-IsInsideTagOrDeleteButton-System-Windows-DependencyObject-'></a>
### IsInsideTagOrDeleteButton(source) `method`

##### Summary

Determines if the mouse event occurred inside a tag or delete button

##### Returns

True if the event occurred inside a tag or delete button, false otherwise

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| source | [System.Windows.DependencyObject](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.DependencyObject 'System.Windows.DependencyObject') | The dependency object that triggered the event |

<a name='M-TagsControl-TagsControl-MoveSelection-System-Int32-'></a>
### MoveSelection(direction) `method`

##### Summary

Moves the selection in the suggestions list

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| direction | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Direction to move (-1 for up, 1 for down) |

<a name='M-TagsControl-TagsControl-OnItemsSourceChanged-System-Windows-DependencyObject,System-Windows-DependencyPropertyChangedEventArgs-'></a>
### OnItemsSourceChanged(d,e) `method`

##### Summary

Handles changes to the ItemsSource property

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| d | [System.Windows.DependencyObject](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.DependencyObject 'System.Windows.DependencyObject') | The dependency object |
| e | [System.Windows.DependencyPropertyChangedEventArgs](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.DependencyPropertyChangedEventArgs 'System.Windows.DependencyPropertyChangedEventArgs') | Dependency property changed event arguments |

<a name='M-TagsControl-TagsControl-OnSelectedTagsChanged-System-Windows-DependencyObject,System-Windows-DependencyPropertyChangedEventArgs-'></a>
### OnSelectedTagsChanged(d,e) `method`

##### Summary

Handles changes to the SelectedTags property

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| d | [System.Windows.DependencyObject](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.DependencyObject 'System.Windows.DependencyObject') | The dependency object |
| e | [System.Windows.DependencyPropertyChangedEventArgs](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.DependencyPropertyChangedEventArgs 'System.Windows.DependencyPropertyChangedEventArgs') | Dependency property changed event arguments |

<a name='M-TagsControl-TagsControl-OnTagsBackgroundBrushesChanged-System-Windows-DependencyObject,System-Windows-DependencyPropertyChangedEventArgs-'></a>
### OnTagsBackgroundBrushesChanged(d,e) `method`

##### Summary

Handles changes to the TagsBackgroundBrushes property

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| d | [System.Windows.DependencyObject](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.DependencyObject 'System.Windows.DependencyObject') | The dependency object |
| e | [System.Windows.DependencyPropertyChangedEventArgs](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.DependencyPropertyChangedEventArgs 'System.Windows.DependencyPropertyChangedEventArgs') | Dependency property changed event arguments |

<a name='M-TagsControl-TagsControl-SelectSuggestion-TagsControl-Models-TagItem-'></a>
### SelectSuggestion(item) `method`

##### Summary

Selects a suggestion item and adds it as a tag

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| item | [TagsControl.Models.TagItem](#T-TagsControl-Models-TagItem 'TagsControl.Models.TagItem') | The tag item to select |

<a name='M-TagsControl-TagsControl-SelectedTags_CollectionChanged-System-Object,System-Collections-Specialized-NotifyCollectionChangedEventArgs-'></a>
### SelectedTags_CollectionChanged(sender,e) `method`

##### Summary

Handles collection change events for selected tags

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| sender | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | The event sender |
| e | [System.Collections.Specialized.NotifyCollectionChangedEventArgs](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Specialized.NotifyCollectionChangedEventArgs 'System.Collections.Specialized.NotifyCollectionChangedEventArgs') | Collection changed event arguments |

<a name='M-TagsControl-TagsControl-TagsControl_PreviewKeyDown-System-Object,System-Windows-Input-KeyEventArgs-'></a>
### TagsControl_PreviewKeyDown(sender,e) `method`

##### Summary

Handles preview key down events for the control

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| sender | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | The event sender |
| e | [System.Windows.Input.KeyEventArgs](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.Input.KeyEventArgs 'System.Windows.Input.KeyEventArgs') | Key event arguments |

<a name='M-TagsControl-TagsControl-UnhighlightSuggestion-System-Windows-Controls-Border-'></a>
### UnhighlightSuggestion(border) `method`

##### Summary

Removes highlighting from a suggestion item when the mouse leaves

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| border | [System.Windows.Controls.Border](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.Controls.Border 'System.Windows.Controls.Border') | The border element to unhighlight |

<a name='M-TagsControl-TagsControl-UpdateSuggestions'></a>
### UpdateSuggestions() `method`

##### Summary

Updates the suggestions list based on current input text

##### Parameters

This method has no parameters.

<a name='M-TagsControl-TagsControl-UpdateTagsUI'></a>
### UpdateTagsUI() `method`

##### Summary

Updates the user interface to reflect current tag selections

##### Parameters

This method has no parameters.

##### Remarks

This method rebuilds the UI by clearing existing tags and recreating them based on the SelectedTags collection.
It also ensures that the input textbox is always positioned at the end of the tag container.
