using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CLI
{
    public class ScrollListBox :ListBox
    {
        public ScrollListBox() : base()
        {
            SelectionChanged += new SelectionChangedEventHandler(ScrollListBox_SelectionChanged);
        }

        private void ScrollListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //scrolling doesn't work
            ScrollIntoView(SelectedItem);
        }
    }
}
