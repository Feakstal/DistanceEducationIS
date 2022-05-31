using System;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace DistanceEducation.Classes
{
    public static class ValidatorExtensions
    {
        public static void PreviewTextInput_ControlLetters(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        public static void PreviewTextInput_ControlForDate(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0) && !(e.Text == ".") && !(e.Text == ":")) e.Handled = true;
        }

        public static void PreviewTextInput_ControlNumbers(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[абвгдеёжзийклмнопрстуфхцчшщьыъэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ]");
            if (!regex.IsMatch(e.Text)) e.Handled = true;
        }

        public static void PreviewTextInput_ControlSpec(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex(@"[,.1234567890абвгдеёжзийклмнопрстуфхцчшщьыъэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯabcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ]");
            if (!regex.IsMatch(e.Text)) e.Handled = true;
        }
    }
}
