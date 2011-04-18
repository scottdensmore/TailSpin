//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Resources
{
    using System.IO;
    using System.Windows;
    using System.Windows.Media;

    public class ThemedResourceLocator
    {
        private readonly Wp7Theme theme;

        public ThemedResourceLocator()
        {
            var resource = Application.Current.Resources["PhoneForegroundColor"];
            if (resource != null)
            {
                this.theme = (Color)resource == Color.FromArgb(222, 0, 0, 0) ? Wp7Theme.Light : Wp7Theme.Dark;
            }
            else
            {
                this.theme = Wp7Theme.Dark;
            }
        }

        public enum Wp7Theme
        {
            Dark,
            Light
        }

        public string FavoriteImageSource 
        { 
            get 
            { 
                return this.GetImagePath("appbar.favs.rest.png"); 
            } 
        }

        public string ContextBackgroundColor
        {
            get
            {
                return this.theme == Wp7Theme.Dark ? "White" : "Black";
            }
        }

        public string ContextForegroundColor
        {
            get
            {
                return this.theme == Wp7Theme.Dark ? "Black" : "White";
            }
        }

        private string GetImagePath(string fileName)
        {
            var folder = this.theme == Wp7Theme.Dark ? "/Resources/Images/Dark/" : "/Resources/Images/Light/";
            return Path.Combine(folder, fileName);
        }
    }
}
