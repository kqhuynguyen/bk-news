﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using Xamarin.Forms;
using System.ComponentModel;
//adb connect 169.254.138.177 
using System.Threading.Tasks;

namespace BKNews
{
    class NewsViewModel : INotifyPropertyChanged
    {
        // skip & step
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 5;
        // category's name
        public string Category { get; set; }
        // mixed collection of news
        public ObservableCollection<News> NewsCollection { get; private set; }
        // command to bind with button
        public ICommand ScrapeCommand { get; set; }
        public ICommand LoadMore { get; set; }
        // list for storing scrapers
        // IsRefreshing property of ListView
        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged("IsBusy");
                }

            }
        }
        // propagate property changes
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        /// <summary>
        /// Refresh the entire list and scrape from the database at the start
        /// </summary>
        public async void RefreshAsync()
        {
            IsBusy = true;
            // await ScrapingSystem.ScrapeAsync(Category);
            NewsCollection.Clear();
            Skip = 0;
            LoadFromDatabaseAsync();
            IsBusy = false;
        }
        // load items from database with pagination
        public async void LoadFromDatabaseAsync()
        {
            try
            {
                var actualAmountToSkip = ScrapingSystem.Updates.Count + Skip;
                var collection = await NewsManager.DefaultManager.GetNewsFromCategoryAsync(Category, actualAmountToSkip, Take);
                foreach (var item in collection)
                {
                    NewsUser s = new NewsUser(item.Id, "chich");
                    await NewsManager.DefaultManager.SaveNewsUserAsync(s);
                    NewsCollection.Add(item);
                }
                Skip += Take;
                Debug.WriteLine("NOOOOOOOOOOOOOOOO {0}. Skip: {1}. Take: {2}", DateTime.Now, Skip, Take);
            } catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public NewsViewModel(String category)
        {
            Category = category;
            NewsCollection = new ObservableCollection<News>();
            // load more when pull to refresh
            ScrapeCommand = new Command(RefreshAsync);
            // load more at the end of the list
            LoadMore = new Command(LoadFromDatabaseAsync);
            LoadMore.Execute(null);
            // take 5 news from database 
        }
    }
}