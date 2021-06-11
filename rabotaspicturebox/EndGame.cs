using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rabotaspicturebox
{
    public partial class EndGame : Form
    {
        int score;
        SqlConnection sqlConnection;//обЪявление переменных
        public EndGame()
        {
            InitializeComponent();
        }

        private async void EndGame_Load(object sender, EventArgs e)
        {
            label1.Text = Convert.ToString(score);

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\USERS\79994\SOURCE\REPOS\RABOTASPICTUREBOX\DATABASE1.MDF;Integrated Security=True";

            sqlConnection = new SqlConnection(connectionString);

            await sqlConnection.OpenAsync();

            SqlDataReader sqlDataReader = null;

            SqlCommand command = new SqlCommand("SELECT TOP(10) * FROM [Table] ORDER BY [score] DESC", sqlConnection);

            try
            {
                sqlDataReader = await command.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                    listBox1.Items.Add(Convert.ToString(sqlDataReader["name"]) + "      " + Convert.ToString(sqlDataReader["score"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlDataReader != null)
                {
                    sqlDataReader.Close();
                }
            }
        }//вывод базы данных в listbox

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrWhiteSpace(textBox1.Text))
            {
                SqlCommand command2 = new SqlCommand("INSERT INTO [Table] (name, score)VALUES(@name, @score)", sqlConnection);

                command2.Parameters.AddWithValue("name", textBox1.Text);

                command2.Parameters.AddWithValue("score", score);

                await command2.ExecuteNonQueryAsync();

                listBox1.Items.Clear();

                SqlDataReader sqlDataReader = null;

                SqlCommand command = new SqlCommand("SELECT TOP(10) * FROM [Table] ORDER BY [score] DESC", sqlConnection);

                try
                {
                    sqlDataReader = await command.ExecuteReaderAsync();

                    while (await sqlDataReader.ReadAsync())
                    {
                        listBox1.Items.Add(Convert.ToString(sqlDataReader["name"]) + "      " + Convert.ToString(sqlDataReader["score"]));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), ex.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (sqlDataReader != null)
                    {
                        sqlDataReader.Close();
                    }
                }
            }
        }//вставка в базу данных и обновление listbox

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }//выход из приложения

        public int Score
        {//свойство с акцссорами
            get { return score; }
            set { score = value; }
        }//передача рекорда из игровой формы, в форму с базой данных

        private void EndGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }//действие при закрытии формы
    }
}
