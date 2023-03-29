using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Name: Derrick Choong
// Student ID: 30066568
// Application: Wiki form to provide data structure definitions and categories.
// Version 1.1: Up to question 6.13, missing tooltips on double mouse click on nameTextBox and keypress error

namespace WikiApplicationV2
{
    public partial class WikiApp : Form
    {
        public WikiApp()
        {
            InitializeComponent();
            loadComboBox();

        }

        //6.2 Create a global List<T> of type Information called Wiki.
        List<Information> wiki = new List<Information>();

        //6.3 Create a button method to ADD a new item to the list.
        //Use a TextBox for the Name input, ComboBox for the Category,
        //Radio group for the Structure and Multiline TextBox for the Definition.
        private void addButton_Click(object sender, EventArgs e)
        {
            bool valid = validName(nameTextBox.Name);
            if (valid)
            {
                Information addData = new Information();
                addData.SetName(nameTextBox.Text);
                addData.SetStructure(getStructureRadioButton());
                addData.SetCategory(categoryComboBox.Text);
                addData.SetDefinition(definitionTextBox.Text);
                wiki.Add(addData);
                displayListView();
                clearDisplay();
            }
            else
            {
                MessageBox.Show("Error! Duplicate Name.");
            }
                       
        }


        //6.9 Create a single custom method that will sort and then display
        //the Name and Category from the wiki information in the list.
        private void displayListView()
        {
            listViewDisplay.Items.Clear();
            wiki.Sort();
            foreach (var item in wiki)
            {
                ListViewItem lvi = new ListViewItem(item.GetName());
                lvi.SubItems.Add(item.GetStructure());
                listViewDisplay.Items.Add(lvi);
            }
        }

        //6.12 Create a custom method that will clear and reset the TextBoxes, ComboBox and Radio button
        private void clearDisplay()
        {
            nameTextBox.Clear();
            categoryComboBox.SelectedIndex = 0;
            definitionTextBox.Clear();
            searchTextBox.Clear();
            foreach(RadioButton rb in structureRadioGroup.Controls.OfType<RadioButton>())
            {
                rb.Checked = false;
            }
        }

        private void focusDisplay(int index)
        {
            nameTextBox.Text = wiki[index].GetName();
            categoryComboBox.Text = wiki[index].GetCategory();
            setStructureRadioButton(index);
            definitionTextBox.Text = wiki[index].GetDefinition();
        }


        //6.6 Create two methods to highlight and return the values from the Radio button GroupBox.
        //The first method must return a string value from the selected radio button (Linear or Non-Linear).
        //The second method must send an integer index which will highlight an appropriate radio button.
        private string getStructureRadioButton()
        {
            string rbStructure = "";
            bool check = false;
            foreach(RadioButton rb in structureRadioGroup.Controls.OfType<RadioButton>())
            {
                if (rb.Checked)
                {
                    rbStructure = rb.Text;
                    check = true;
                    break;
                }
            }
            if (!check)
            {
                MessageBox.Show("Please select the structure type!");
            }
            return rbStructure;
        }

        private void setStructureRadioButton(int index)
        {
            foreach (RadioButton rb in structureRadioGroup.Controls.OfType<RadioButton>())
            {
                if (rb.Text == wiki[index].GetStructure())
                {
                    rb.Checked = true;                    
                }
            }
            
        }



        //6.4 Create a custom method to populate the ComboBox when the Form Load method is called.
        //The six categories must be read from a simple text file.
        public void loadComboBox()
        {
            categoryComboBox.Items.Clear();
            if (File.Exists("categorylist.txt"))
            {
                using (StreamReader sr = new StreamReader("categorylist.txt", Encoding.UTF8, false))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        categoryComboBox.Items.Add(line);
                    }
                }
            }
        }

        //6.5 Create a custom ValidName method which will take a parameter string value
        //from the Textbox Name and returns a Boolean after checking for duplicates.
        //Use the built in List<T> method “Exists” to answer this requirement.
        private bool validName(string checkName)
        {
            if (wiki.Exists(dup => dup.GetName() == checkName))
            {
                return false;
            }
            else { return true; }
        }

        //Pass message to create error trapping and color 
        public void errorMessage(string message)
        {

        }

        // 6.7 Create a button method that will delete the currently selected record in the ListView.
        // Ensure the user has the option to backout of this action by using a dialog box.
        // Display an updated version of the sorted list at the end of this process.
        private void deleteButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = listViewDisplay.SelectedIndices[0];
            var confirmResult = MessageBox.Show("Do you want to edit this record?", "Confirm Edit!", MessageBoxButtons.YesNo);

            if (confirmResult == DialogResult.Yes)
            {
                if (selectedIndex > -1)
                {
                    Information selectedInfo = wiki[selectedIndex];
                    wiki.Remove(selectedInfo);
                }
            }
            displayListView();
        }

        //6.8 Create a button method that will save the edited record of the currently selected item
        //in the ListView. All the changes in the input controls will be written back to the list.
        //Display an updated version of the sorted list at the end of this process.
        private void editButton_Click(object sender, EventArgs e)
        {
            if (listViewDisplay.SelectedIndices.Count > 0)
            {
                int selectedIndex = listViewDisplay.SelectedIndices[0];
                var confirmResult = MessageBox.Show("Do you want to edit this record?", "Confirmation of Edit", MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    if(selectedIndex > -1)
                    {
                        wiki[selectedIndex].SetName(nameTextBox.Text);
                        wiki[selectedIndex].SetStructure(getStructureRadioButton());
                        wiki[selectedIndex].SetCategory(categoryComboBox.Text);
                        wiki[selectedIndex].SetDefinition(definitionTextBox.Text);
                    }
                    displayListView();
                }
            }
        }

        //6.11 Create a ListView event so a user can select a Data Structure Name from
        //the list of Names and the associated information will be displayed in the related
        //text boxes combo box and radio button.
        private void listViewDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            clearDisplay();
            if (listViewDisplay.SelectedIndices.Count > 0)
            {
                int index = listViewDisplay.SelectedIndices[0];
                focusDisplay(index);
            }
        }

        //6.10 Create a button method that will use the builtin binary search to
        //find a Data Structure name. If the record is found the associated details
        //will populate the appropriate input controls and highlight the name in the ListView.
        //At the end of the search process the search input TextBox must be cleared.
        private void searchButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchTextBox.Text))
            {
                Information searchInfo = new Information();
                searchInfo.SetName(searchTextBox.Text);
                int found = wiki.BinarySearch(searchInfo);

                if (found >= 0)
                {
                    listViewDisplay.SelectedItems.Clear();
                    listViewDisplay.Items[found].Selected = true;
                    listViewDisplay.Focus();
                    focusDisplay(found);
                    searchTextBox.Clear();
                }
                else
                {
                    MessageBox.Show("\"" + searchTextBox.Text + "\" is not found. Please try again!");
                    clearDisplay();
                }
            }
            else
            {
                MessageBox.Show("Null Input. Please try again!");
            }
        }

        //6.13 Create a double click event on the Name TextBox to clear the TextBboxes, ComboBox and Radio button.
        private void nameTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clearDisplay();
            nameTextBox.Focus();
        }

        //6.14 Create two buttons for the manual open and save option; this must
        //use a dialog box to select a file or rename a saved file. All Wiki data
        //is stored/retrieved using a binary reader/writer file format.
        private void saveButton_Click(object sender, EventArgs e)
        {

        }

        private void loadButton_Click(object sender, EventArgs e)
        {

        }

        //private void keypress is needed 
    }
}
