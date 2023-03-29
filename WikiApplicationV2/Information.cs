using System;

// 6.1 Create a separate class file to hold the four data items of the Data Structure
// (use the Data Structure Matrix as a guide). Use private properties for the fields
// which must be of type “string”. The class file must have separate setters and getters,
// add an appropriate IComparable for the Name attribute. Save the class as “Information.cs”.

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

        public string ISBN
        {
            get; set;
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
