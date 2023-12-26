using System;
using System.Data.SqlClient;
using SD = System.Data;
using System.Windows.Forms;

namespace HeshAutorization
{
    public partial class Abonent : Form
    {
        public Abonent()
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
        private void Insert_Button_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            string data = dateTime.ToString();
            string description = richTextBox1.Text.ToString();
            string statysValues = comboBox1.SelectedItem.ToString();


            openConnection();
               
            string commandString = $"insert into applications(Дата_создания, Статус, Описание) values('{data}', '{statysValues}', '{description}')";
            SqlCommand sqlCommand = new SqlCommand(commandString, sqlConnection);

            if (sqlCommand.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Заявка добавлена", "Успех");
            }
            else
            {
                MessageBox.Show("Заявка не добавлена", "Ошибка");
            }

            closeConnection();
        }
    }
}
