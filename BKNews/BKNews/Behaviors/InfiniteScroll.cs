﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
namespace BKNews.Behaviors
{
    public class InfiniteScroll : Behavior<ListView>
    {
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create("LoadMoreCommand", typeof(ICommand), typeof(InfiniteScroll), null);
        public ICommand LoadMoreCommand
        {
            get
            {
                return (ICommand)GetValue(LoadMoreCommandProperty);
            }
            set
            {
                SetValue(LoadMoreCommandProperty, value);
            }
        }
        public ListView AssociatedObject
        {
            get;
            private set;
        }
        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            bindable.BindingContextChanged += Bindable_BindingContextChanged;
            bindable.ItemAppearing += InfiniteListView_ItemAppearing;
            bindable.ItemAppearing += CountDown_ItemAppearing;
        }
        private void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= Bindable_BindingContextChanged;
            bindable.ItemAppearing -= InfiniteListView_ItemAppearing;
            bindable.ItemAppearing -= CountDown_ItemAppearing;
        }
        void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = AssociatedObject.ItemsSource as IList;
            if (items != null && e.Item == items[items.Count - 1])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null)) LoadMoreCommand.Execute(null);
            }
        }
        public DateTime StartDateTime { get; private set; }
        int count = 0;
        void CountDown_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (count == 0)
            {
                StartDateTime = DateTime.Now;
                count++;
            }
            else
            {
                var delta = (DateTime.Now - StartDateTime).TotalSeconds;
                var items = AssociatedObject.ItemsSource as IList;
                if ((items != null && e.Item == items[items.Count - 1]) || (delta > 10))
                {
                    if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null)) LoadMoreCommand.Execute(null);
                    count--;
                    delta = 0;
                }
                Debug.WriteLine(delta);
            }

        }
    }
}
