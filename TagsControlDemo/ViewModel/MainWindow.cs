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
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TagItem> AvailableTags { get; set; }
        public ObservableCollection<TagItem> SelectedTags { get; set; }

        public MainViewModel()
        {
            // Инициализация доступных тегов
            AvailableTags = new ObservableCollection<TagItem>
        {
            new TagItem { DisplayName = "C#", Value = "csharp" },
            new TagItem { DisplayName = "WPF", Value = "wpf" },
            new TagItem { DisplayName = "MVVM", Value = "mvvm" },
            new TagItem { DisplayName = "XAML", Value = "xaml" },
            new TagItem { DisplayName = ".NET", Value = "dotnet" }
        };

            SelectedTags = new ObservableCollection<TagItem>();

            SelectedTags.CollectionChanged += SelectedTagsCollectionChanged;
        }

        private void SelectedTagsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Обрабатываем только добавление элементов
            if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                return;

            // Проверяем, что есть новые элементы
            if (e.NewItems == null || e.NewItems.Count == 0)
                return;

            var newTag = e.NewItems[0] as TagItem;
            if (newTag == null)
                return;

            // Если такой тег уже есть в списке доступных, ничего не делаем
            if (AvailableTags.Any(x => x.Value == newTag.Value))
                return;

            // Добавляем новый тег в список доступных
            AvailableTags.Add(newTag);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
