using System;


namespace WikiApplicationV2
{
    internal class Information : IComparable<Information>
    {
        private string Name;
        private string Category;
        private string Structure;
        private string Definition;
        
        public Information() { }

        public void SetName(string dataName)
        {
            Name = dataName;
        }

        public string GetName()
        { 
            return Name; 
        }

        public void SetCategory(string dataCategory)
        {
            Category = dataCategory;
        }

        public string GetCategory() 
        { 
            return Category; 
        }

        public void SetStructure(string dataStructure)
        {
            Structure = dataStructure;
        }

        public string GetStructure()
        {
            return Structure;   

        }
        public void SetDefinition(string dataDefinition)
        {
            Definition = dataDefinition;
        }
        public string GetDefinition() 
        { 
            return Definition; 
        }

        public int CompareTo(Information compareInfo)
        {
            return Name.CompareTo(compareInfo.Name);
        }
 
    }
}
