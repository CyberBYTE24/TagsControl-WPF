using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsControl.Models
{
    /// <summary>
    /// Represents a single tag item with display name and value properties.
    /// This model is used to store information about individual tags in the TagsControl.
    /// </summary>
    /// <remarks>
    /// The TagItem class provides a simple data structure for tags that can be 
    /// displayed in the UI and managed through the TagsControl. It supports both
    /// display text (DisplayName) and an internal value (Value) for identification purposes.
    /// </remarks>
    public class TagItem
    {
        /// <summary>
        /// Gets or sets the text to display in tag
        /// </summary>
        /// <remarks>
        /// This property represents the user-facing text that will be displayed 
        /// on the tag in the UI. It's used for visual identification of the tag.
        /// </remarks>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the string value
        /// </summary>
        /// <remarks>
        /// This property represents an internal identifier for the tag. 
        /// It can be used to uniquely identify tags in data processing and 
        /// is particularly useful when working with databases or APIs.
        /// </remarks>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the TagItem class with specified display name and value.
        /// </summary>
        /// <param name="displayName">The text to display in the tag</param>
        /// <param name="value">The internal string value for identification</param>
        /// <remarks>
        /// This constructor allows creating a new tag with both display name and value 
        /// properties set at initialization time.
        /// </remarks>
        public TagItem(string displayName, string value)
        {
            this.DisplayName = displayName;
            this.Value = value;
        }
    }
}