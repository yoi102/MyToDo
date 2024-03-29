﻿using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace MyToDo.Extensions
{
    //    public class PasswordExtensions
    //    {
    //        public static string GetPassword(DependencyObject obj)
    //        {
    //            return (string)obj.GetValue(PasswordProperty);
    //        }

    //        public static void SetPasword(DependencyObject obj, string value)
    //        {
    //            obj.SetValue(PasswordProperty, value);
    //        }

    //        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
    //        public static readonly DependencyProperty PasswordProperty =
    //            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordExtensions), new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

    //        static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    //        {
    //            var passwordBox = sender as PasswordBox;

    //            if (passwordBox != null && passwordBox.Password != (string)e.NewValue)
    //            {
    //                passwordBox.Password = (string)e.NewValue;

    //            }
    //        }

    //    }

    //    public  class PasswordBehavior : Behavior<PasswordBox>
    //    {
    //        protected override void OnAttached( )
    //        {
    //            base.OnAttached();
    //            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;

    //        }

    //        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
    //        {
    //            var passwordBox = sender as PasswordBox;

    //            string password = PasswordExtensions.GetPassword(passwordBox);

    //            if (password != null && passwordBox.Password!=password)
    //            {
    //                PasswordExtensions.SetPasword(passwordBox, passwordBox.Password);
    //            }
    //        }

    //        protected override void OnDetaching( )
    //        {
    //            base.OnDetaching();
    //            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;

    //        }

    //    }

    //}
    public class PassWordExtensions
    {
        public static string GetPassWord(DependencyObject obj)
        {
            return (string)obj.GetValue(PassWordProperty);
        }

        public static void SetPassWord(DependencyObject obj, string value)
        {
            obj.SetValue(PassWordProperty, value);
        }

        public static readonly DependencyProperty PassWordProperty =
            DependencyProperty.RegisterAttached("PassWord", typeof(string), typeof(PassWordExtensions), new FrameworkPropertyMetadata(string.Empty, OnPassWordPropertyChanged));

        private static void OnPassWordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passWord = sender as PasswordBox;
            string password = (string)e.NewValue;

            if (passWord != null && passWord.Password != password)
                passWord.Password = password;
        }
    }

    public class PasswordBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            string password = PassWordExtensions.GetPassWord(passwordBox);

            if (passwordBox != null && passwordBox.Password != password)
                PassWordExtensions.SetPassWord(passwordBox, passwordBox.Password);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
        }
    }
}