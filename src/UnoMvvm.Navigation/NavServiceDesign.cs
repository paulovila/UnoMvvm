﻿using System;

namespace UnoMvvm.Navigation
{
    public class NavServiceDesign : INavService
    {
        public void Navigate<T>() where T : IViewModel { }
        public void Navigate<T, TP>(TP parameters) where T : IViewModel where TP : INavigationParameters { }
        public Action<Exception> NavigationFailed { get; set; }
    }
}