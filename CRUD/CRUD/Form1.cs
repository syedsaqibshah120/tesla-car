using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD
{
    public partial class Form1 : Form
    {
        string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
        }

        private void InsertBtn_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(IDTb.Text) || string.IsNullOrEmpty(NameTb.Text)
                || GenderCombo.SelectedItem == null || DCombo.SelectedItem == null || string.IsNullOrEmpty(SalaryTb.Text))
            {
                MessageBox.Show("Please fill in all the fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the ID already exists
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                string query2 = "select * from employee where id = @id";
                SqlCommand cmd2 = new SqlCommand(query2, con);
                cmd2.Parameters.AddWithValue("@id", IDTb.Text);
                SqlDataReader dr = cmd2.ExecuteReader();
                if (dr.HasRows)
                {
                    MessageBox.Show(IDTb.Text + "ID already exists !!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                dr.Close();

                // Insert the new record
                string query = "insert into EMPLOYEE (id, name, gender, age, designation, Salary) values (@id,@name, @gender, @age, @designation, @Salary)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", IDTb.Text);
                cmd.Parameters.AddWithValue("@name", NameTb.Text);
                cmd.Parameters.AddWithValue("@gender", GenderCombo.SelectedItem);
                cmd.Parameters.AddWithValue("@age", numericUpDown1.Value);
                cmd.Parameters.AddWithValue("@designation", DCombo.SelectedItem);
                cmd.Parameters.AddWithValue("@Salary", SalaryTb.Text);

                int a = cmd.ExecuteNonQuery();
                if (a > 0)
                {
                    MessageBox.Show("Inserted Successfully !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BindGridView();
                }
                else
                {
                    MessageBox.Show("Insertion Fail !!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            resetcontrols();

        }
     
        void BindGridView()
        {
            SqlConnection con = new SqlConnection(cs);
            string query = "select * from Employee";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable data= new DataTable();
            sda.Fill(data);
            dataGridView1.DataSource = data;
                    
        }

        private void ViewBtn_Click(object sender, EventArgs e)
        {
            BindGridView();
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {

            SqlConnection con = new SqlConnection(cs);
            // for update
          
            string query = "update employee set id= @id,  gender = @gender, designation = @designation,salary= @salary where id= @id";


            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", IDTb.Text);
          
            cmd.Parameters.AddWithValue("@gender", GenderCombo.SelectedItem);
            cmd.Parameters.AddWithValue("@age", numericUpDown1.Value);
            cmd.Parameters.AddWithValue("@designation", DCombo.SelectedItem);
            cmd.Parameters.AddWithValue("@Salary", SalaryTb.Text);

            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Updated Successfully !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BindGridView();
            }
            else
            {
                MessageBox.Show("Update Fail !!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            con.Close();

            resetcontrols();


        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                IDTb.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                 NameTb.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                 GenderCombo.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                 numericUpDown1.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                 DCombo.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                 SalaryTb.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            // for Delete

            SqlConnection con = new SqlConnection(cs);
            string query = "delete from employee where id = @id";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", IDTb.Text);
           
            con.Open();
            int a = cmd.ExecuteNonQuery();
            if (a > 0)
            {
                MessageBox.Show("Delete Successfully !!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BindGridView();
            }
            else
            {
                MessageBox.Show("Delete Fail !!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            con.Close();
            resetcontrols();
        }

        private void ResetBtn_Click(object sender, EventArgs e)
        {
            resetcontrols();
        }
        void resetcontrols()
        {
            IDTb.Clear();
            NameTb.Clear();
            GenderCombo.SelectedItem = null;
            numericUpDown1.Value = 20;
            DCombo.SelectedItem = null;
            SalaryTb.Clear();
            IDTb.Focus();
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {

            string searchTerm = searchBtn.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //else
            //{
            //    MessageBox.Show("Please insert data");
            //}
               else
            {
                MessageBox.Show("Please insert data");
            }
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();

                string query = "SELECT * FROM Employee WHERE id LIKE @searchTerm OR name LIKE @searchTerm OR Salary LIKE @searchTerm";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable data = new DataTable();
                sda.Fill(data);
                dataGridView1.DataSource = data;
            }

        }
    }
}
