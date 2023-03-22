using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WikiApplicationV2
{
    public partial class WikiApp : Form
    {
        public WikiApp()
        {
            InitializeComponent();
        }

        List<Information> wiki = new List<Information>();

        private void addButton_Click(object sender, EventArgs e)
        {
            Information addData = new Information();
            addData.SetName(nameTextBox.Text);
            addData.SetStructure(getStructureRadioButton());
            addData.SetCategory(categoryComboBox.Text);
            addData.SetDefinition(definitionTextBox.Text);
            wiki.Add(addData);
            displayListView();

        }

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

        private string getStructureRadioButton()
        {
            string rbStructure = "";
            foreach(RadioButton rb in structureRadioGroup.Controls.OfType<RadioButton>())
            {
                if (rb.Checked)
                {
                    rbStructure = rb.Text;
                    break;
                }
                else
                {
                    MessageBox.Show("Please select the structure type");
                }
            }
            return rbStructure;
        }
    }
}
