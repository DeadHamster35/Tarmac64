using System.Collections.Generic;

namespace Tarmac64_Retail
{
    public class HelpTopic
    {
        public string Id;
        public string Title;
        public string ParentId;
        public string Summary;
        public List<HelpField> Fields = new List<HelpField>();
        public List<string> SeeAlso = new List<string>();

        public HelpTopic(string id, string title, string parentId)
        {
            Id = id;
            Title = title;
            ParentId = parentId;
            Summary = "";
        }
    }

    public class HelpField
    {
        public string Name;
        public string Format;
        public string Usage;
        public string[] ControlNames;

        public HelpField(string name, string format, string usage, params string[] controlNames)
        {
            Name = name;
            Format = format;
            Usage = usage;
            if (controlNames == null)
            {
                ControlNames = new string[0];
            }
            else
            {
                ControlNames = controlNames;
            }
        }
    }
}
