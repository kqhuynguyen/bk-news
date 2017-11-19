﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
namespace BKNews
{   
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewsPage : ContentPage
	{
        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Refresh items only when authenticated.
        }

        public NewsPage (string category)
		{
            Debug.WriteLine("chichdemkhuya1");
            InitializeComponent();
            Debug.WriteLine("chichdemkhuya2");
            BindingContext = new NewsViewModel(category);
            Debug.WriteLine("chichdemkhuya3");
        }
        // Open a browser every time an item is tapped
        public void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            var news = (News)e.Item;
            Device.OpenUri(new Uri(news.NewsUrl));
            ((ListView)sender).SelectedItem = null;
        }
    }
}