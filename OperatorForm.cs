using System;
using SD = System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace HeshAutorization
{
    public partial class OperatorForm : Form
    {
        public OperatorForm()
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

        private void OperatorForm_Load(object sender, EventArgs e)
        {
            openConnection();

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM applications", sqlConnection);

            DataSet db = new DataSet();

            dataAdapter.Fill(db);

            dataGridView1.DataSource = db.Tables[0];

            closeConnection();
        }

        private void RestoreTableButton_Click(object sender, EventArgs e)
        {
            openConnection();

            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM applications", sqlConnection);

            DataSet db = new DataSet();

            dataAdapter.Fill(db);

            dataGridView1.DataSource = db.Tables[0];

            closeConnection();
        }
    }
}
