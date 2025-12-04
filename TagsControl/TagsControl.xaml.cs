using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TagsControl.Models;

namespace TagsControl
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class TagsControl : UserControl
    {
        // DependencyProperty для источника данных
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(TagsControl),
                new PropertyMetadata(null, OnItemsSourceChanged));

        // DependencyProperty для выбранных тегов
        public static readonly DependencyProperty SelectedTagsProperty =
            DependencyProperty.Register("SelectedTags", typeof(IList), typeof(TagsControl),
                new PropertyMetadata(null, OnSelectedTagsChanged));

        // DependencyProperty для CornerRadius внешнего Border
        public static readonly DependencyProperty BorderCornerRadiusProperty =
            DependencyProperty.Register("BorderCornerRadius", typeof(CornerRadius), typeof(TagsControl),
                new PropertyMetadata(new CornerRadius(4)));

        public CornerRadius BorderCornerRadius
        {
            get => (CornerRadius)GetValue(BorderCornerRadiusProperty);
            set => SetValue(BorderCornerRadiusProperty, value);
        }

        // DependencyProperty для BorderBrush внутреннего Border
        public static readonly DependencyProperty InnerBorderBrushProperty =
            DependencyProperty.Register("InnerBorderBrush", typeof(Brush), typeof(TagsControl),
                new PropertyMetadata(null));

        public Brush InnerBorderBrush
        {
            get => (Brush)GetValue(InnerBorderBrushProperty);
            set => SetValue(InnerBorderBrushProperty, value);
        }

        // DependencyProperty для BorderThickness внутреннего Border
        public static readonly DependencyProperty InnerBorderThicknessProperty =
            DependencyProperty.Register("InnerBorderThickness", typeof(Thickness), typeof(TagsControl),
                new PropertyMetadata(new Thickness(0)));

        public Thickness InnerBorderThickness
        {
            get => (Thickness)GetValue(InnerBorderThicknessProperty);
            set => SetValue(InnerBorderThicknessProperty, value);
        }

        // DependencyProperty для Background внутреннего Border
        public static readonly DependencyProperty InnerBackgroundProperty =
            DependencyProperty.Register("InnerBackground", typeof(Brush), typeof(TagsControl),
                new PropertyMetadata(null));

        public Brush InnerBackground
        {
            get => (Brush)GetValue(InnerBackgroundProperty);
            set => SetValue(InnerBackgroundProperty, value);
        }

        private int _selectedIndex = -1;
        private List<TagItem> _suggestions = new List<TagItem>();
        private TextBox InputTextBox;
        private int _selectedTagIndex = -1; // Индекс выделенного тега
        private INotifyCollectionChanged _currentSelectedTagsCollection; // Для отслеживания изменений коллекции

        // Фиксированная палитра цветов для детерминированных цветов
        private static readonly Color[] _colorPalette = new[]
        {
            Color.FromRgb(255, 87, 34),    // Red
            Color.FromRgb(156, 39, 176),   // Purple
            Color.FromRgb(63, 81, 181),    // Indigo
            Color.FromRgb(33, 150, 243),   // Blue
            Color.FromRgb(0, 188, 212),    // Cyan
            Color.FromRgb(0, 150, 136),    // Teal
            Color.FromRgb(76, 175, 80),    // Green
            Color.FromRgb(139, 195, 74),   // Light Green
            Color.FromRgb(205, 220, 57),   // Lime
            Color.FromRgb(255, 193, 7),    // Amber
            Color.FromRgb(255, 152, 0),    // Orange
            Color.FromRgb(96, 125, 139),   // Blue Grey
            Color.FromRgb(121, 85, 72),    // Brown
            Color.FromRgb(158, 158, 158),  // Grey
            Color.FromRgb(96, 125, 139),   // Blue Grey
            Color.FromRgb(103, 58, 183)    // Deep Purple
        };

        // Кэш для цветов по DisplayName для производительности
        private static readonly Dictionary<string, SolidColorBrush> _colorCache = new Dictionary<string, SolidColorBrush>();

        public TagsControl()
        {
            InitializeComponent();
            
            // Создаем TextBox программно ДО установки SelectedTags,
            // чтобы избежать вызова UpdateTagsUI до инициализации TextBox
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
            
            // Настраиваем PlacementTarget для Popup
            SuggestionsPopup.PlacementTarget = InputTextBox;
            SuggestionsPopupBorder.SetBinding(Control.BackgroundProperty, new Binding("Background") { Source = this}); 
            // Предотвращаем переход фокуса по Tab из контрола
            this.PreviewKeyDown += TagsControl_PreviewKeyDown;
            
            // Устанавливаем SelectedTags после создания TextBox
            SelectedTags = new List<TagItem>();
            
            // Добавляем TextBox в WrapPanel, чтобы он был inline с тегами
            //TagsContainer.Children.Add(InputTextBox);
        }

        /// <summary>
        /// Клик по любой пустой области контрола переводит фокус на поле ввода.
        /// </summary>
        private void ClickArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var source = e.OriginalSource as DependencyObject;
            if (IsInsideTagOrDeleteButton(source))
            {
                // Даём событию пройти дальше, чтобы обработчик кнопки удаления выполнился
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

            // Если клик пришёлся по пустому месту, переносим фокус в поле ввода
            FocusInputTextBox();
            e.Handled = true;
        }

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
            // Устанавливаем курсор IBeam при наведении только если не на теге
            var source = e.OriginalSource as DependencyObject;
            if (!IsInsideTagOrDeleteButton(source))
            {
                this.Cursor = Cursors.IBeam;
            }
        }

        private void ClickArea_MouseLeave(object sender, MouseEventArgs e)
        {
            // Возвращаем стандартный курсор
            this.Cursor = Cursors.Arrow;
        }

        private void FocusInputTextBox()
        {
            if (InputTextBox != null)
            {
                InputTextBox.Focus();
                Keyboard.Focus(InputTextBox);
                // Устанавливаем курсор в конец текста
                if (InputTextBox.Text != null)
                {
                    InputTextBox.CaretIndex = InputTextBox.Text.Length;
                }
            }
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public IList SelectedTags
        {
            get => (IList)GetValue(SelectedTagsProperty);
            set => SetValue(SelectedTagsProperty, value);
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TagsControl;
            control?.UpdateSuggestions();
        }

        private static void OnSelectedTagsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TagsControl;
            if (control != null)
            {
                // Отписываемся от старой коллекции
                if (control._currentSelectedTagsCollection != null)
                {
                    control._currentSelectedTagsCollection.CollectionChanged -= control.SelectedTags_CollectionChanged;
                }

                // Подписываемся на новую коллекцию, если она поддерживает уведомления
                control._currentSelectedTagsCollection = e.NewValue as INotifyCollectionChanged;
                if (control._currentSelectedTagsCollection != null)
                {
                    control._currentSelectedTagsCollection.CollectionChanged += control.SelectedTags_CollectionChanged;
                }

                control.UpdateTagsUI();
            }
        }

        private void SelectedTags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Обновляем UI при любых изменениях в коллекции (добавление, удаление, перемещение)
            UpdateTagsUI();
        }

        private void UpdateTagsUI()
        {
            // Проверяем, что InputTextBox создан
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

            // Всегда добавляем TextBox в конец после всех тегов
            TagsContainer.Children.Add(InputTextBox);
            
            // Сбрасываем выделение тега при обновлении UI
            _selectedTagIndex = -1;
            ClearTagSelection();
            
            // Убеждаемся, что фокус остается в TextBox после обновления UI
            if (InputTextBox.IsFocused || Keyboard.FocusedElement == InputTextBox)
            {
                // Небольшая задержка для корректной установки фокуса
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    InputTextBox.Focus();
                    Keyboard.Focus(InputTextBox);
                }), System.Windows.Threading.DispatcherPriority.Input);
            }
        }

        private Border CreateTagChip(TagItem tag)
        {

            // Получаем детерминированный цвет для этого displayName
            var backgroundColor = GetDeterministicColorBrush(tag.DisplayName);
            var foregroundColor = GetContrastColor(backgroundColor.Color);

            var border = new Border
            {
                Style = (Style)FindResource("TagChipStyle"),
                Background = backgroundColor,
                Cursor = Cursors.Arrow // Обычный курсор для тегов
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
                Cursor = Cursors.Hand // Курсор "рука" для кнопки удаления
            };
            deleteButton.Click += DeleteButton_Click;
            deleteButton.MouseEnter += (s, e) => { deleteButton.Cursor = Cursors.Hand; };
            deleteButton.MouseLeave += (s, e) => { deleteButton.Cursor = Cursors.Hand; };

            stackPanel.Children.Add(textBlock);
            stackPanel.Children.Add(deleteButton);

            border.Child = stackPanel;
            
            // Обработчики для курсора на теге
            border.MouseEnter += (s, e) => 
            { 
                border.Cursor = Cursors.Arrow;
                // Предотвращаем изменение курсора родительского элемента
                e.Handled = true;
            };
            border.MouseLeave += (s, e) => 
            { 
                border.Cursor = Cursors.Arrow;
                e.Handled = true;
            };
            
            return border;
        }

        private SolidColorBrush GetDeterministicColorBrush(string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
                return new SolidColorBrush(_colorPalette[0]);

            // Проверяем кэш сначала
            if (_colorCache.TryGetValue(displayName, out var cachedBrush))
                return cachedBrush;

            // Генерируем хеш от строки
            int hash = GetStableHash(displayName);

            // Выбираем цвет из палитры на основе хеша
            int colorIndex = Math.Abs(hash) % _colorPalette.Length;
            var color = _colorPalette[colorIndex];

            // Создаем кисть и кэшируем ее
            var brush = new SolidColorBrush(color);
            _colorCache[displayName] = brush;

            return brush;
        }

        private Color GetContrastColor(Color backgroundColor)
        {
            // Рассчитываем яркость фона
            double brightness = (0.299 * backgroundColor.R + 0.587 * backgroundColor.G + 0.114 * backgroundColor.B) / 255;

            // Если фон темный - используем белый текст, если светлый - темный
            return brightness < 0.7 ? Colors.White : Colors.Black;
        }

        // Стабильная хеш-функция для строки
        private int GetStableHash(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0;

            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input.ToLower().Trim());
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // Берем первые 4 байта для int
                return BitConverter.ToInt32(hashBytes, 0);
            }
        }

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

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Сбрасываем выделение тега при вводе текста
            if (_selectedTagIndex >= 0)
            {
                _selectedTagIndex = -1;
                ClearTagSelection();
            }
            UpdateSuggestions();
        }

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

            // Фильтруем источники данных
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

        private void AddSuggestionItem(TagItem item)
        {
            var border = new Border
            {
                Style = (Style)FindResource("SuggestionItemStyle")
            };

            // Получаем детерминированный цвет для предложения
            var backgroundColor = GetDeterministicColorBrush(item.DisplayName);
            var foregroundColor = GetContrastColor(backgroundColor.Color);

            // Создаем визуальное представление предложения
            var grid = new Grid();

            // Цветная полоса слева
            var colorBar = new Border
            {
                Width = 4,
                Background = backgroundColor,
                CornerRadius = new CornerRadius(2, 0, 0, 2),
                VerticalAlignment = VerticalAlignment.Stretch,
                Margin = new Thickness(0, 2, 4, 2)
            };

            // Основной текст
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

        private void HighlightSuggestion(Border border)
        {
            border.Background = new SolidColorBrush(Color.FromArgb(25, 0, 120, 215));
        }

        private void UnhighlightSuggestion(Border border)
        {
            border.Background = Brushes.Transparent;
        }

        private void SelectSuggestion(TagItem item)
        {
            AddTag(item);
            InputTextBox.Text = "";
            SuggestionsPopup.IsOpen = false;
        }

        private void TagsControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Предотвращаем переход фокуса по Tab из контрола
            if (e.Key == Key.Tab && Keyboard.FocusedElement == InputTextBox)
            {
                // Tab обрабатывается в InputTextBox_PreviewKeyDown
                return;
            }
        }

        private void InputTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Tab:
                    // Если есть выбранное предложение, добавляем его
                    if (_selectedIndex >= 0 && _selectedIndex < _suggestions.Count && SuggestionsPopup.IsOpen)
                    {
                        AddTag(_suggestions[_selectedIndex]);
                        InputTextBox.Text = "";
                        SuggestionsPopup.IsOpen = false;
                        e.Handled = true;
                    }
                    else
                    {
                        // Предотвращаем переход фокуса на следующий элемент
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

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    if (SuggestionsPopup.IsOpen && _selectedIndex >= 0 && _selectedIndex < _suggestions.Count)
                    {
                        // Если есть выбранное предложение, добавляем его
                        AddTag(_suggestions[_selectedIndex]);
                        InputTextBox.Text = "";
                        SuggestionsPopup.IsOpen = false;
                        e.Handled = true;
                    }
                    else if (!string.IsNullOrWhiteSpace(InputTextBox.Text))
                    {
                        // Создаем новый тег
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
                    // Если поле пустое и есть теги, удаляем последний тег
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
                        // Если выделен тег, удаляем его
                        SelectedTags.RemoveAt(_selectedTagIndex);
                        _selectedTagIndex = -1;
                        UpdateTagsUI();
                        InputTextBox.Focus();
                        e.Handled = true;
                    }
                    break;
                case Key.Delete:
                    // Если выделен тег, удаляем его
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

        private void HandleLeftArrow(KeyEventArgs e)
        {
            // Если курсор в начале текста и поле не пустое, разрешаем стандартное поведение
            if (!string.IsNullOrWhiteSpace(InputTextBox.Text) && InputTextBox.SelectionStart > 0)
            {
                e.Handled = false;
                return;
            }

            // Если поле пустое или курсор в начале, переходим к навигации по тегам
            if (string.IsNullOrWhiteSpace(InputTextBox.Text) && SelectedTags.Count > 0)
            {
                if (_selectedTagIndex < 0)
                {
                    // Выделяем последний тег
                    _selectedTagIndex = SelectedTags.Count - 1;
                }
                else if (_selectedTagIndex > 0)
                {
                    // Переходим к предыдущему тегу
                    _selectedTagIndex--;
                }
                else
                {
                    // Уже на первом теге, разрешаем стандартное поведение
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

        private void HandleRightArrow(KeyEventArgs e)
        {
            // Если есть выделенный тег, снимаем выделение и переходим в поле ввода
            if (_selectedTagIndex >= 0)
            {
                _selectedTagIndex = -1;
                ClearTagSelection();
                InputTextBox.Focus();
                e.Handled = true;
            }
            else
            {
                // Стандартное поведение для стрелки вправо
                e.Handled = false;
            }
        }

        private void HighlightSelectedTag()
        {
            ClearTagSelection();
            if (_selectedTagIndex >= 0 && _selectedTagIndex < TagsContainer.Children.Count - 1)
            {
                var tagBorder = TagsContainer.Children[_selectedTagIndex] as Border;
                if (tagBorder != null)
                {
                    // Добавляем визуальное выделение
                    tagBorder.BorderBrush = new SolidColorBrush(Colors.Blue);
                    tagBorder.BorderThickness = new Thickness(2);
                    tagBorder.BringIntoView();
                }
            }
        }

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

        private void MoveSelection(int direction)
        {
            if (_suggestions.Count == 0) return;

            _selectedIndex += direction;
            if (_selectedIndex < 0) _selectedIndex = _suggestions.Count - 1;
            if (_selectedIndex >= _suggestions.Count) _selectedIndex = 0;

            // Обновляем выделение
            for (int i = 0; i < SuggestionsPanel.Children.Count; i++)
            {
                var border = SuggestionsPanel.Children[i] as Border;
                if (border != null)
                {
                    if (i == _selectedIndex)
                    {
                        HighlightSuggestion(border);
                        // Прокручиваем к выбранному элементу
                        border.BringIntoView();
                    }
                    else
                    {
                        UnhighlightSuggestion(border);
                    }
                }
            }
        }

        private void AddTag(TagItem tag)
        {
            if (!SelectedTags.Contains(tag))
            {
                SelectedTags.Add(tag);
                UpdateTagsUI();
            }
        }

        private TagItem CreateNewTag(string text)
        {
            // Создаем новый тег с нужной структурой
            // Это можно настроить под ваш тип данных
            string cleanText = text.Trim();
            return new TagItem
            {
                DisplayName = cleanText,
                Value = cleanText.ToLower().Replace(" ", "_").Replace("#", "_sharp")
            };
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName)) return obj;

            var type = obj.GetType();
            var property = type.GetProperty(propertyName);
            return property?.GetValue(obj);
        }
    }
}
