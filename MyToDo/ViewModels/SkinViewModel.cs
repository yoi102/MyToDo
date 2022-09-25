using MaterialDesignColors;
using MaterialDesignColors.ColorManipulation;
using MaterialDesignThemes.Wpf;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace MyToDo.ViewModels
{
    public class SkinViewModel : BindableBase
    {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;

        private bool _IsDarkTheme = true;

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;
            set
            {
                if (SetProperty(ref _IsDarkTheme, value))
                {
                    ModifyTheme(theme => theme.SetBaseTheme(value ? Theme.Dark : Theme.Light));
                }
            }
        }

        private DelegateCommand<object> _ChangeHueCommand;

        public DelegateCommand<object> ChangeHueCommand => _ChangeHueCommand ??= new DelegateCommand<object>((obj) =>
        {
            var hue = (Color)obj;

            ITheme theme = _paletteHelper.GetTheme();
            theme.PrimaryLight = new ColorPair(hue.Lighten());
            theme.PrimaryMid = new ColorPair(hue);
            theme.PrimaryDark = new ColorPair(hue.Darken());
            _paletteHelper.SetTheme(theme);
        });

        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            modificationAction?.Invoke(theme);

            paletteHelper.SetTheme(theme);
        }
    }
}