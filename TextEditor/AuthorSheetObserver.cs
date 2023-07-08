using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace TextEditor
{
    public interface IObserver
    {
        void Notify(string message);
    }

    public class AuthorSheetObserver : IObserver
    {
        public int _threshold;
        public AuthorSheetObserver(int threshold)
        {
            _threshold = threshold;
        }

        public void Notify(string message)
        {
            if (message.Length >= _threshold)
            {
                MessageBox.Show($"A new author sheet has been created with {message.Length} characters.");
                _threshold += 1800;
            }
        }
    }

    public class ObservableTextControl
    {
        private readonly List<IObserver> _observers = new List<IObserver>();
        private readonly Control _textControl;

        public ObservableTextControl(Control textControl)
        {
            _textControl = textControl;//maintext
            _textControl.TextChanged += TextControl_TextChanged;//method text changed
        }

        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void UnregisterObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }
        private void TextControl_TextChanged(object sender, EventArgs e)
        {
            var message = _textControl.Text;

            foreach (var observer in _observers)
            {
                observer.Notify(message);
            }
        }
    }
}
