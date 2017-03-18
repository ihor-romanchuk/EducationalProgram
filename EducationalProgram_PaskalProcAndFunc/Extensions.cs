using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace EducationalProgram_PaskalProcAndFunc
{
    public static class Extensions
    {
        private static Random _random = new Random();

        public static List<T> Shuffle<T>(this IList<T> list)
        {
            List<T> shuffledList = new List<T>(list);
            int n = shuffledList.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = shuffledList[k];
                shuffledList[k] = shuffledList[n];
                shuffledList[n] = value;
            }

            return shuffledList;
        }

        public static T GetChildOfType<T>(this DependencyObject depObj)
            where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }
    }
}