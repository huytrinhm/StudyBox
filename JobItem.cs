using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyBox
{
    public class JobItem
    {
        public JobItem(string Content, string StartText, string EndText)
        {
            this.Content = Content;
            this.StartText = StartText;
            this.EndText = EndText;
            this.StartTime = DateTime.ParseExact(StartText, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            this.EndTime = DateTime.ParseExact(EndText, "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
        }
        public string Content { get; set; }
        public string StartText { get; set; }
        public string EndText { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
