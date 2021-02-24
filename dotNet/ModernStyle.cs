using Atomus.Control;
using Atomus.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Atomus.Windows.Style
{
    public class ModernStyle : ICore
    {
        private ResourceDictionary ResourceDictionaryRoot;

        #region Init
        public ModernStyle()
        {
        }
        public ModernStyle(ResourceDictionary resourceDictionary) : this()
        {
            if (resourceDictionary != null)
            {
                this.ResourceDictionaryRoot = resourceDictionary;
                this.CreateStyle(this.GetAttribute("Style"));
            }
        }
        public ModernStyle(object[] args) : this((args != null && args.Length > 0 && args[0] is ResourceDictionary) ? args[0] as ResourceDictionary : null)
        {
        }
        #endregion

        #region IO
        public void CreateStyle(string skin)
        {
            ResourceDictionary resourceDictionary;

            resourceDictionary = new ResourceDictionary();
            resourceDictionary.Source = new Uri($"pack://application:,,,/Atomus.Windows.Style.ModernStyle;component/ResourceDictionary/{skin}/Root.xaml");

            this.ResourceDictionaryRoot.MergedDictionaries.Add(resourceDictionary);
        }
        #endregion


        #region Event
        #endregion


        #region Etc
        #endregion
    }
}