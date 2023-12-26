using System;
using System.Collections.Generic;
using System.ComponentModel;
using SD = System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace HeshAutorization
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        private SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-RNGC4SV; Initial Catalog=wslab; Integrated Security=True");
        SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();

        public void openConnection()
        {
            if (sqlConnection.State == SD.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void closeConnection()
        {
            if (sqlConnection.State == SD.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button_Aut_Click(object sender, EventArgs e)
        {
            string Login = textBox1.Text.ToString();
            string Password = textBox2.Text.ToString();
            DataTable dt = new DataTable();
            string PasswordZap = CreateMD5(Password);

            openConnection();
            string commandString = $"select role from UserRoles where Login='{Login}' and Password='{PasswordZap}'";
            SqlCommand sqlCommand = new SqlCommand(commandString, sqlConnection);

            sqlDataAdapter.SelectCommand = sqlCommand;
            sqlDataAdapter.Fill(dt);
            try
            {
                string role = Convert.ToString(dt.Rows[0][0]);

                if (Proverka(Login, Password) == true)
                {
                    if (role == "Админ")
                    {

                        MessageBox.Show("Авторизация успешна", "Успех");
                        AdminForm admin = new AdminForm();
                        admin.Show();
                    }
                    else if (role == "Оператор")
                    {
                        MessageBox.Show("Авторизация успешна", "Успех");
                        OperatorForm operatorForm = new OperatorForm();
                        operatorForm.Show();
                    }
                    else if (role == "Абонент")
                    {
                        MessageBox.Show("Авторизация успешна", "Успех");
                        Abonent abonent = new Abonent();
                        abonent.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Выввели неверный логин или пароль", "Ошибка");
                }
                closeConnection();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void button_Reg_Click(object sender, EventArgs e)
        {
            string LoginReg = textBox3.Text.ToString();
            string PasswordReg1 = textBox4.Text.ToString();
            string PasswordReg2 = textBox5.Text.ToString();
            string Name = textBox8.Text.ToString();
            string lastName = textBox7.Text.ToString();
            string post = textBox6.Text.ToString();
            string role = textBox9.Text.ToString();

            if (PasswordReg1 == PasswordReg2)
            {


                openConnection();

                if (Proverka(LoginReg, PasswordReg1) == true)
                {
                    MessageBox.Show("Такой аккаунт уже существует", "Ошибка");
                }
                else
                {
                    PasswordReg1 = CreateMD5(PasswordReg1);

                    string commandString = $"INSERT INTO UserRoles(Login, Password, role) VALUES('{LoginReg}', '{PasswordReg1}', '{role}')";
                    string commandString2 = $"INSERT INTO Posts(Имя, Фамилия, Должность) VALUES('{Name}', '{lastName}', '{post}')";

                    SqlCommand sqlCommand = new SqlCommand(commandString, sqlConnection);
                    SqlCommand sqlCommand2 = new SqlCommand(commandString2, sqlConnection);

                    try
                    {
                        if (sqlCommand.ExecuteNonQuery() == 1 && sqlCommand2.ExecuteNonQuery() == 1)
                        {
                            MessageBox.Show("Аккаунт был создан", "Успех");
                        }
                        else
                        {
                            MessageBox.Show("Аккаунт не был создан", "Ошибка");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка");
                    }
                }

                closeConnection();
            }
            else
            {
                MessageBox.Show("Вы ввели неодинаковые пароли", "Ошибка");
            }

            
        }

        private Boolean Proverka(string log, string pass)
        {
            DataTable table = new DataTable();
            string Login = log;
            string Password = pass;

            Password = CreateMD5(Password);

            string commandString = $"select Login, Password from UserRoles where Login='{Login}' and Password='{Password}'";

            SqlCommand sqlCommand = new SqlCommand(commandString, sqlConnection);

            sqlDataAdapter.SelectCommand = sqlCommand;
            sqlDataAdapter.Fill(table);

            if(table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string CreateMD5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
