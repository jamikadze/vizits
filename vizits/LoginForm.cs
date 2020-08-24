using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using vizits.Properties;

namespace vizits
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        private void tbUser_GotFocus(Object sender, EventArgs e)
        {
            if (tbUser.Text == "Имя пользователя")
            { tbUser.Text = ""; tbUser.ForeColor = Color.Black; }
        }
        private void tbPass_GotFocus(Object sender, EventArgs e)
        {
            if (tbPass.Text == "Пароль")
            { tbPass.Text = ""; tbPass.ForeColor = Color.Black; tbPass.PasswordChar = '#'; }
        }
        private void tbUser_Leave(object sender, EventArgs e)
        {
            if (tbUser.Text == "")
            {
                tbUser.Text = "Имя пользователя";
                tbUser.ForeColor = Color.Silver;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            tbUser.GotFocus += new System.EventHandler(this.tbUser_GotFocus);
            tbPass.GotFocus += new System.EventHandler(this.tbPass_GotFocus);
            if (Settings.Default.username != "")
                tbUser.ForeColor = Color.Black;
            tbUser.Text = Settings.Default.username;
            
        }

        private void tbPass_Leave(object sender, EventArgs e)
        {
            if (tbPass.Text == "")
            {
                tbPass.PasswordChar = '\0';
                tbPass.Text = "Пароль";
                tbPass.ForeColor = Color.Silver;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (tbPass.Text == "Пароль" || tbUser.Text == "Имя пользователя")
                label2.Text = "Введите имя пользователя и пароль";
            else
            {
                try
                {
                    this.usersTableAdapter.FillByUserPass(this.visits.users, tbUser.Text, tbPass.Text);
                    if (visits != null && visits.users.Count > 0)
                    {
                        Settings.Default.username = tbUser.Text;
                        Settings.Default.Save();
                        Global.usersId = (int)((DataRowView)usersBindingSource.Current).Row["id"];
                        Global.fio = (string)((DataRowView)usersBindingSource.Current).Row["fio"];
                        Global.dos = (int)((DataRowView)usersBindingSource.Current).Row["dos"];

                        Visible = false;
                        label2.Text = "";
                        Form mf = new MainForm();
                        mf.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        tbUser.SelectAll();
                        tbUser.Text = "";
                        tbUser.Focus();
                        label2.Text = "Не правильное имя или пароль";
                    }
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }

            }
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { btnOk_Click(null, null); }
        }

    }
}
