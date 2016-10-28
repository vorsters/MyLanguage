using System.Text;

namespace MyLanguageImpl.Runtime
{
    public class ListLog
    {
        const int INDENT = 4;
        public StringBuilder sb = new StringBuilder();

        private bool _newLine = true;
        private string _pad;

        public int level { get; private set; }

        public void StepDown()
        {
            level++;
            _pad = "".PadLeft(level*INDENT);
        }

        public void StepUp()
        {
            level--;
            _pad = "".PadLeft(level * INDENT);
        }

        public ListLog Append(string text)
        {
            if (_newLine)
            {
                sb.Append(_pad).Append(text);
            }
            else
            {
                sb.Append(text);
            }

            _newLine = false;

            return this;
        }

        public ListLog NewLine()
        {
            sb.AppendLine();
            _newLine = true;
            return this;
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }
}