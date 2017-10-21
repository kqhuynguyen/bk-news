﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BKNews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewsPage : ContentPage
	{
		public NewsPage ()
		{
			InitializeComponent ();
            BindingContext = new NewsViewModel();
		}
        void OnListViewItemTapped(object sender, ItemTappedEventArgs args)
        {
        }
    }
}