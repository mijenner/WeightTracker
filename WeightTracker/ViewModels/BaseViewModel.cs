﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace WeightTracker.ViewModels
{
    public partial class BaseViewModel : ObservableObject  
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy;
        [ObservableProperty]
        string title = string.Empty;
        public bool IsNotBusy => !IsBusy;
    }
}
