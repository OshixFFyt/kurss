using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace kurss
{
    public partial class Form1_auth : Form
    {
        string conStr = "server=chuc.caseum.ru;port=33333;user=st_3_20_19;database=is_3_20_st19_KURS;password=71290994";
        MySqlConnection conn;
        private void Form1_auth_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(conStr);
        }

        public Form1_auth()
        {
            InitializeComponent();
        }
        static string sha256(string randomString)
        {
            //Тут происходит криптографическая магия. Смысл данного метода заключается в том, что строка залетает в метод
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }
        public void UserInfo(string login)
        {
            //Объявлем переменную для запроса в БД
            string selected_id_stud = textBox1.Text;
            // устанавливаем соединение с БД
            conn.Open();
            // запрос
            string sql = $"SELECT * FROM Employ WHERE log='{login}'";
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(sql, conn);
            // объект для чтения ответа сервера
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            while (reader.Read())
            {
                // элементы массива [] - это значения столбцов из запроса SELECT
                Auth.auth_id = Convert.ToInt32(reader[0].ToString());
                Auth.auth_fio = reader[1].ToString();
                Auth.auth_role = Convert.ToInt32(reader[4].ToString());
            }
            reader.Close(); // закрываем reader
            // закрываем соединение с БД
            conn.Close();
        }

        public int connect(string login, string password)
        {
             
            conn.Open();
            string cmd1 = $"SELECT * FROM Employ WHERE log = '{login}' AND pass = '{password}";
            MySqlCommand conn1 = new MySqlCommand(cmd1, conn);
            int t = Convert.ToInt32(conn1.ExecuteScalar());
            if (t != 0)
            {
                string cmd2 = $"SELECT id FROM Employ WHERE log = '{login}' AND pass = '{password}`";
                MySqlCommand cmd21 = new MySqlCommand(cmd2, conn);
                Auth.auth_id = Convert.ToInt32(cmd21.ExecuteScalar());
                
                
            }
            conn.Close();
             return Auth.auth_id;

        }



        private void button1_Click(object sender, EventArgs e)
        {
            connect(textBox1.Text, textBox2.Text);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //В текстбокс3 формируется хэш по мере ввода текста во второй текстбокс, используется метод шифрования (хэширования)
            textBox3.Text = sha256(textBox2.Text);
        }

        private void Form1_auth_Load_1(object sender, EventArgs e)
        {
            conn = new MySqlConnection(conStr);
        }
    }

}
