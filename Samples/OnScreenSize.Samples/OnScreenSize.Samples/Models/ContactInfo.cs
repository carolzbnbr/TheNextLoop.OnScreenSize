using System;
using System.Collections.Generic;

namespace OnScreenSize.Samples.Models
{
  

    public class ContactInfo
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        public string ImageUrl { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class ContactGroup : List<ContactInfo>
    {
        public string Name { get; private set; }

        public ContactGroup(string name, List<ContactInfo> animals) : base(animals)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Monkey
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFavorite { get; set; }
    }
}
