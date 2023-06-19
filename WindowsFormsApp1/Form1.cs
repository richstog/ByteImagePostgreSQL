using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Npgsql;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public NpgsqlConnection connection = new NpgsqlConnection(
                "server=localhost;port=5432;username=postgres;password=rhfcbkjdf29;database=test_image"
                );
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            

            ImageConverter imgCon = new ImageConverter();
            var gg = (byte[])imgCon.ConvertTo(pictureBox1.Image, typeof(byte[]));
            
            try
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand("insert into test (img) values (@dfdf)", connection);
                command.Parameters.Add("@dfdf", NpgsqlTypes.NpgsqlDbType.Bytea).Value = gg;
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(textBox1.Text);

            

            ImageConverter imgCon = new ImageConverter();
            var gg = (byte[])imgCon.ConvertTo(pictureBox1.Image, typeof(byte[]));

            try
            {
                connection.Open();

                NpgsqlCommand command = new NpgsqlCommand("select * from test where id = @fg", connection);
                command.Parameters.Add("@fg", NpgsqlTypes.NpgsqlDbType.Integer).Value = id;
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    pictureBox1.Image = (Bitmap)((new ImageConverter()).ConvertFrom(reader["img"]));
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                connection.Close();
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
    }
}
