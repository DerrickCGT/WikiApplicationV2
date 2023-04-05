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

// name: Derrick Choong
// Student ID: 30066568
// Application: Wiki form to provide data structure definitions and categories.
// Version 1.3: All completed and form close with same save method as save button

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


        #region "Add Button"
        //6.3 Create a button method to ADD a new item to the list.
        //Use a TextBox for the name input, ComboBox for the category,
        //Radio group for the structure and Multiline TextBox for the definition.
        private void addButton_Click(object sender, EventArgs e)
        {
            clearStatusStrip();

            //Check if the name is valid (not a duplicate)
            bool valid = validName(nameTextBox.Text);

            if (valid)
            {
                if (!string.IsNullOrEmpty(nameTextBox.Text))
                    {
                    //create an object and add object to the list
                    Information addData = new Information();
                    addData.SetName(nameTextBox.Text);
                    addData.SetStructure(getStructureRadioButton());
                    addData.SetCategory(categoryComboBox.Text);
                    addData.SetDefinition(definitionTextBox.Text);
                    wiki.Add(addData);
                    displayListView();
                    statusStripInfo("Data Added", "green");
                    clearDisplay();
                }
                else
                {
                    MessageBox.Show("Error! Null Input on Structure Name.");
                    statusStripInfo("Error", "red");
                }
            }
            else
            {
                MessageBox.Show("Error! Duplicate Name.");
                statusStripInfo("Duplicate Found", "red");
            }
                       
        }
        #endregion

        #region "Display Method"
        //6.9 Create a single custom method that will sort and then display
        //the name and category from the wiki information in the list.
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
        #endregion

        #region "Getter and Setter for Radio Button"
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
        #endregion

        #region "Load ComboBox"
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
        #endregion

        #region "ValidName Method"
        //6.5 Create a custom ValidName method which will take a parameter string value
        //from the Textbox name and returns a Boolean after checking for duplicates.
        //Use the built in List<T> method “Exists” to answer this requirement.
        private bool validName(string checkName)
        {
            if (wiki.Exists(duplicate => duplicate.GetName() == checkName))
            {
                return false;
            }
            else { return true; }
        }
        #endregion

        #region "Status Strip"
        //Pass message to create error trapping and color 
        public void statusStripInfo(string message, string color)
        {
            statusLabel1.Text = message;
            if (color.ToLower() == "green")
            {
                statusLabel1.BackColor = Color.Green;
            }
            if (color.ToLower() == "red")
            {
                statusLabel1.BackColor = Color.Red;
            }
        }

        //clear the status strip
        private void clearStatusStrip()
        {
            statusLabel1.Text = "";
            statusLabel1.BackColor = Color.White;
        }
        #endregion

        #region "Delete Button"
        // 6.7 Create a button method that will delete the currently selected record in the ListView.
        // Ensure the user has the option to backout of this action by using a dialog box.
        // Display an updated version of the sorted list at the end of this process.
        private void deleteButton_Click(object sender, EventArgs e)
        {
            clearStatusStrip();
            if (listViewDisplay.SelectedIndices.Count > 0)
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
                statusStripInfo("Data Deleted", "Green");
                displayListView();
                clearDisplay();
            }           
        }
        #endregion

        #region "Edit Button"
        //6.8 Create a button method that will save the edited record of the currently selected item
        //in the ListView. All the changes in the input controls will be written back to the list.
        //Display an updated version of the sorted list at the end of this process.
        private void editButton_Click(object sender, EventArgs e)
        {
            clearStatusStrip();

            //Check if an item in the list view is selected
            if (listViewDisplay.SelectedIndices.Count > 0)
            {
                int selectedIndex = listViewDisplay.SelectedIndices[0];
                var confirmResult = MessageBox.Show("Do you want to edit this record?", "Confirmation of Edit", MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    //Check if the selected index is valid              
                    if(selectedIndex > -1)
                    {

                        //Check if the name is valid (not a duplicate)
                        if (validName(nameTextBox.Text))
                        {
                            wiki[selectedIndex].SetName(nameTextBox.Text);
                            wiki[selectedIndex].SetStructure(getStructureRadioButton());
                            wiki[selectedIndex].SetCategory(categoryComboBox.Text);
                            wiki[selectedIndex].SetDefinition(definitionTextBox.Text);
                        }
                        else
                        {
                            MessageBox.Show("Error! Duplicate Name.");
                            statusStripInfo("Duplicate Found", "red");
                        }                        
                    }
                    statusStripInfo("Data Edited", "Green");
                    displayListView();
                    clearDisplay();
                }
            }
        }
        #endregion

        #region "Event Method"
        //6.11 Create a ListView event so a user can select a Data structure name from
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

        //6.13 Create a double click event on the name TextBox to clear the TextBboxes, ComboBox and Radio button.
        private void nameTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clearDisplay();
            nameTextBox.Focus();
            statusStripInfo("Clear", "Green");
        }
        #endregion

        #region "Search Button"
        //6.10 Create a button method that will use the builtin binary search to
        //find a Data structure name. If the record is found the associated details
        //will populate the appropriate input controls and highlight the name in the ListView.
        //At the end of the search process the search input TextBox must be cleared.
        private void searchButton_Click(object sender, EventArgs e)
        {
            clearStatusStrip();
            if (!string.IsNullOrEmpty(searchTextBox.Text))
            {
                Information searchInfo = new Information();
                searchInfo.SetName(searchTextBox.Text);

                //Built-in BinarySearch for List and using the interface IComparable with overloading CompareTo Method 
                //To compare the object.name
                int found = wiki.BinarySearch(searchInfo);

                //BinarySearch will return negative value if not found and positive value if found
                if (found >= 0)
                {
                    listViewDisplay.SelectedItems.Clear();
                    listViewDisplay.Items[found].Selected = true;
                    listViewDisplay.Focus();
                    focusDisplay(found);
                    statusStripInfo("Found", "Green");
                    searchTextBox.Clear();
                    
                }
                else
                {
                    MessageBox.Show("\"" + searchTextBox.Text + "\" is not found. Please try again!");
                    statusStripInfo("Not Found", "Red");
                    clearDisplay();
                }
            }
            else
            {
                MessageBox.Show("Null Input. Please try again!");
                statusStripInfo("Error", "Red");
            }
        }
        #endregion

        #region "Save and Load File"
        //6.14 Create two buttons for the manual open and save option; this must
        //use a dialog box to select a file or rename a saved file. All Wiki data
        //is stored/retrieved using a binary reader/writer file format.
        //Save all data to Binary File
        private void saveButton_Click(object sender, EventArgs e)
        {
            clearStatusStrip();
            saveMethod();            
        }

        //When form closed, Save Dialog box will opt user to save the file
        private void WikiApp_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveMethod();
        }

        //Save method using a dialog box to opt user to select a file or rename a saved file using binary writer format.
        private void saveMethod()
        {

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = Application.StartupPath;
            saveFile.Filter = "Binary File (*.bin)|*.bin";
            saveFile.Title = "Save your bin file";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = saveFile.FileName;
                    //create a new FileStream to write to the file
                    var stream = File.Open(fileName, FileMode.Create);

                    //create a new BinaryWriter object to write binary data to the file
                    // and ensure the file is closed when finished
                    using (BinaryWriter bw = new BinaryWriter(stream, Encoding.UTF8, false))
                    {
                        foreach (var information in wiki)
                        {
                            bw.Write(information.GetName());
                            bw.Write(information.GetCategory());
                            bw.Write(information.GetStructure());
                            bw.Write(information.GetDefinition());
                        }
                    }
                    statusStripInfo("File Saved", "Green");
                }
                catch (IOException)
                {
                    MessageBox.Show("Could not save Wiki information", "Critical Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusStripInfo("Error", "Red");
                }
            }                
        }        

        //Load file to add all data to Wiki List 
        private void loadButton_Click(object sender, EventArgs e)
        {
            clearStatusStrip();

            //create an OpenFileDialog to allow user to select a file to open
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.InitialDirectory = Application.StartupPath;
            openFile.Filter = "BIN | *.bin";
            openFile.Title = "Open a binary file";

            //Show dialog box and prompt user to press ok or cancel
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string openFileName = openFile.FileName;
                try
                {
                    wiki.Clear(); //clear the conetent of wiki list

                    //Open the file for reading using a FileStream Object
                    using (Stream stream = File.Open(openFileName, FileMode.Open))
                    {
                        //Create a BinaryReader object to read data from the file
                        using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8, false))
                        {
                            //Read the data from the file and add it to the wiki list
                            while (stream.Position < stream.Length)
                            {
                                Information addData = new Information();
                                addData.SetName(br.ReadString());
                                addData.SetCategory(br.ReadString());
                                addData.SetStructure(br.ReadString());
                                addData.SetDefinition(br.ReadString());
                                wiki.Add(addData);
                            }
                        }
                    }
                    statusStripInfo("File Loaded", "Green");
                    displayListView();
                }
                catch (IOException)
                {
                    MessageBox.Show("Could not open Wiki information", "Critical Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    statusStripInfo("Error", "Red");
                }
            }
        }
        #endregion

        #region "Keypress Event Method"
        //Filter out numeric or special character input. Ignore any keyPress besides alphabets.    
        //Filter Name TextBox Keypress
        private void nameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //Filter Definition TextBox Keypress
        private void definitionTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        #region "Other Utility Method"
        //Set PlaceHolder text in searchTextBox
        //Clear the text in searchTextBox when user tries to input
        private void searchTextBox_Enter_1(object sender, EventArgs e)
        {
            if (searchTextBox.Text == "Search Structure Name")
            {
                searchTextBox.Text = "";
                searchTextBox.ForeColor = Color.Black;
            }
        }

        //Set PlaceHolder text in searchTextBox
        //Input PlaceHolder text searchTextBox when user exits
        private void searchTextBox_Leave(object sender, EventArgs e)
        {
            if (searchTextBox.Text == "")
            {
                searchTextBox.Text = "Search Structure Name";

                searchTextBox.ForeColor = Color.Silver;
            }
        }

        //Clear listview indices everytime search textbox is clicked
        private void searchTextBox_Click(object sender, EventArgs e)
        {
            listViewDisplay.SelectedIndices.Clear();
            searchTextBox.ForeColor = Color.Black;
        }
        #endregion

        
    }
}
