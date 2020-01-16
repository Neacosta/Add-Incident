using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Add_Incident
{
    public partial class FrmCustomerIncidents : Form
    {
        public FrmCustomerIncidents()
        {
            InitializeComponent();
        }

        private void customersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.customersBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.techSupportDataSet);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'techSupportDataSet.Products' table. You can move, or remove it, as needed.
            //this.productsTableAdapter.Fill(this.techSupportDataSet.Products);
            // TODO: This line of code loads data into the 'techSupportDataSet.Incidents' table. You can move, or remove it, as needed.
            //this.incidentsTableAdapter.Fill(this.techSupportDataSet.Incidents);
            // TODO: This line of code loads data into the 'techSupportDataSet.Customers' table. You can move, or remove it, as needed.
            //this.customersTableAdapter.Fill(this.techSupportDataSet.Customers);
            //this.incidentsTableAdapter.Fill(this.techSupportDataSet.Incidents);

            //this.productsTableAdapter.FillByProductRegistration(this.techSupportDataSet.Products, CustomerID);
            
        }

        private void tsbtnGetCustomer_Click(object sender, EventArgs e)
        {

           if (this.incidentsBindingSource.Count > 0)
           {
                incidentsBindingSource.CancelEdit();
           }
                
                try
                {
                    int CustomerID = Convert.ToInt32(tstxtCustomerID.Text);
                    this.incidentsTableAdapter.Fill(this.techSupportDataSet.Incidents);
                    this.customersTableAdapter.FillByCustomerID(this.techSupportDataSet.Customers, CustomerID);



                    if (this.customersBindingSource.Count > 0)
                    {
                        techSupportDataSet.EnforceConstraints = false;
                        this.productsTableAdapter.FillByProductRegistration(this.techSupportDataSet.Products, CustomerID);
                        this.EnableControls();
                        incidentsBindingSource.AddNew();
                        customerIDTextBox.Text = CustomerID.ToString();
                        dateOpenedDateTimePicker.Text = DateTime.Today.ToShortDateString();
                    }
                    else
                    {
                        this.DisableControls();
                        MessageBox.Show("No customer found with this ID. "
                            + "Please try again.", "Customer Not Found");
                    }
                }
                catch (FormatException)
                {
                    MessageBox.Show("Customer ID must be an integer.", "Entry Error");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Database error # " + ex.Number +
                        ": " + ex.Message, ex.GetType().ToString());
                }


        }

          
            

        private void btnAddIncident_Click(object sender, EventArgs e)
        {
            if (IsValidData())
            {
                try
                {
                    this.incidentsBindingSource.EndEdit();
                    this.tableAdapterManager.UpdateAll(this.techSupportDataSet);
                    incidentsTableAdapter.Update(techSupportDataSet);
                    MessageBox.Show("Customer: " + txtCustomerName.Text + "\n\n" +
                        "Product: " +" cbProductName.SelectedValue,",
                        "Incident Added");

                    // These statements remove the selected customer 
                    // and the new incident from the dataset 
                    // so the data isn't displayed on the form
                    // after the incident is added.
                    this.customersBindingSource.RemoveCurrent();
                    this.incidentsBindingSource.RemoveCurrent();

                    tstxtCustomerID.Text = "";
                    this.DisableControls();
                }
                catch (DataException ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().ToString());
                    incidentsBindingSource.EndEdit();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Database error # " + ex.Number +
                        ": " + ex.Message, ex.GetType().ToString());
                }

            }

        }

        private bool IsValidData()
        {
            return
                IsPresent(dateOpenedDateTimePicker, "Date Opened") &&
                IsPresent(cbProductName, "Product") &&
                IsPresent(titleTextBox, "Title") &&
                IsPresent(descriptionTextBox, "Description");
        }
        private bool IsPresent(Control control, string name)
        {
            if (control.GetType().ToString() == "System.Windows.Forms.TextBox")
            {
                TextBox textBox = (TextBox)control;
                if (textBox.Text == "")
                {
                    MessageBox.Show(name + " is a required field.", "Entry Error");
                    textBox.Focus();
                    return false;
                }
            }
            else if (control.GetType().ToString() == "System.Windows.Forms.ComboBox")
            {
                ComboBox comboBox = (ComboBox)control;
                if (comboBox.SelectedIndex == -1)
                {
                    MessageBox.Show(name + " is a required field.", "Entry Error");
                    comboBox.Focus();
                    return false;
                }
            }
            return true;
        }
        private void DisableControls()
        {
            cbProductName.Enabled = false;
            titleTextBox.Enabled = false;
            descriptionTextBox.Enabled = false;
            btnAddIncident.Enabled = false;
            btnCancel.Enabled = false;
        }
        private void EnableControls()
        {
            cbProductName.Enabled = true;
            titleTextBox.Enabled = true;
            descriptionTextBox.Enabled = true;
            btnAddIncident.Enabled = true;
            btnCancel.Enabled = true;
            cbProductName.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            incidentsBindingSource.CancelEdit();
            customersBindingSource.RemoveCurrent();
            tstxtCustomerID.Text = "";
            this.DisableControls();
        }
    }
} 
