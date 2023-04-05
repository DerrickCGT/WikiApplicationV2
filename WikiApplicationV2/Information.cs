using System;

// 6.1 Create a separate class file to hold the four data items of the Data structure
// (use the Data structure Matrix as a guide). Use private properties for the fields
// which must be of type “string”. The class file must have separate setters and getters,
// add an appropriate IComparable for the name attribute. Save the class as “Information.cs”.

namespace WikiApplicationV2
{
    internal class Information : IComparable<Information>
    {

        private string name;
        private string category;
        private string structure;
        private string definition;
        
        //Default Constructor
        public Information() { }

        public void SetName(string dataName)
        {
            name = dataName;
        }

        //Assessor Methods
        public string GetName()
        { 
            return name; 
        }

        public void SetCategory(string dataCategory)
        {
            category = dataCategory;
        }

        public string GetCategory() 
        { 
            return category; 
        }

        public void SetStructure(string dataStructure)
        {
            structure = dataStructure;
        }

        public string GetStructure()
        {
            return structure;   

        }
        public void SetDefinition(string dataDefinition)
        {
            definition = dataDefinition;
        }
        public string GetDefinition() 
        { 
            return definition; 
        }

        //
        public int CompareTo(Information compareInfo)
        {
            return name.CompareTo(compareInfo.name);
        }
 
    }
}
