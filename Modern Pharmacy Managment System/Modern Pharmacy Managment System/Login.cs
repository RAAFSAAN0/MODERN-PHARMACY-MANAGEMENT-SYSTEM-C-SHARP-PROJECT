﻿using System;
using System.Data;
using System.Windows.Forms;

namespace Modern_Pharmacy_Managment_System
{
    public partial class Login : Form
    {
        Functions Con;

        

        public Login()
        {
            InitializeComponent();
            Con = new Functions();
        }
        private string customerPhone = "";
        private static Login instance;
        public static Login GetInstance()
        {
            if (instance == null)
            {
                instance = new Login();
            }
            return instance;
        }


        public static int EmpId;
        public static string EmpName = "";
       

        private void signupBtn_Click(object sender, EventArgs e)
        {
            if (signupName.Text == "" || signupPhone.Text == "")
            {
                MessageBox.Show("Missing Info!!!");
            }
            else
            {
                try
                {
                    // Check in the EmployeeTbl first
                    string empQuery = "Select * from EmployeeTbl where EmpName COLLATE Latin1_General_CS_AS = '{0}' and EmpPass COLLATE Latin1_General_CS_AS = '{1}'";
                    empQuery = string.Format(empQuery, signupName.Text, signupPhone.Text);
                    DataTable empDt = Con.GetData(empQuery);

                    // If not found in EmployeeTbl, check in the tbCustomer
                    if (empDt.Rows.Count == 0)
                    {
                        // Check in the AdminTbl for admin login
                        string adminQuery = "Select * from AdminTbl where AdminUsername COLLATE Latin1_General_CS_AS = '{0}' and AdminPassword COLLATE Latin1_General_CS_AS = '{1}'";
                        adminQuery = string.Format(adminQuery, signupName.Text, signupPhone.Text);
                        DataTable adminDt = Con.GetData(adminQuery);

                        // If found in AdminTbl, login as admin
                        if (adminDt.Rows.Count > 0)
                        {
                            // Now you can navigate to the AdminDashboard or perform other actions for admins
                            DashBoard ad = new DashBoard();
                            ad.Show();
                            this.Hide();
                        }
                        else
                        {
                            // If not found in AdminTbl, check in tbCustomer
                            string customerQuery = "Select * from tbCustomer where cphone COLLATE Latin1_General_CS_AS = '{0}' and cpassword COLLATE Latin1_General_CS_AS = '{1}'";
                            customerQuery = string.Format(customerQuery, signupName.Text, signupPhone.Text);
                            DataTable customerDt = Con.GetData(customerQuery);

                            // If found in tbCustomer, login as customer
                            if (customerDt.Rows.Count > 0)
                            {
                                string customerName = customerDt.Rows[0]["cname"].ToString();

                                // Passing Phone
                                string customerPhoneNumber = customerDt.Rows[0]["cphone"].ToString();
                                Login login = Login.GetInstance();
                                login.setCustomerPhone(customerPhoneNumber);

                                CustomerDashboard cd = new CustomerDashboard(customerName);
                                cd.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Incorrect Phone Number or Password!!!");
                                signupName.Text = "";
                                signupPhone.Text = "";
                            }
                        }
                    }
                    else
                    {
                        // If it's found in EmployeeTbl, it's staff login. Now open the StaffDashboard form.
                        EmpId = Convert.ToInt32(empDt.Rows[0][0].ToString());
                        EmpName = empDt.Rows[0][1].ToString(); // Set the staff name
                        StaffDashboard sd = new StaffDashboard();
                        sd.Show();
                        this.Hide();
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void lblSignup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SignupForm signup = new SignupForm();
            signup.Show();
            this.Hide();
        }


        public void setCustomerPhone(string customerPhone)
        {
            this.customerPhone = customerPhone;
        }

        public string getCustomerPhone()
        {
            return customerPhone;
        }

        private void checkShowPassword_CheckedChanged(object sender, EventArgs e)
        {

            if (checkShowPassword.Checked)
            {
                signupPhone.PasswordChar = '\0';
              
            }
            else
            {
                signupPhone.PasswordChar = '●';
                
            }
        }
    }
}
