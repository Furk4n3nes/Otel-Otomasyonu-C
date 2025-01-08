using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace otel
{
    public partial class adminkayit : Form
    {
        private bool sifreGorunur = false;
        public adminkayit()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-LTN03PA\\SQLEXPRESS;Initial Catalog=Otel;Integrated Security=True;Encrypt=False");

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into admin(kullanici_id,kullanici_sifre) values('" + txtkullanici.Text + "','" + txtsifre.Text + "')", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            admingiris fr=new admingiris();
            fr.Show();
            this.Hide();

            MessageBox.Show("Kayıt Yapıldı.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
           
            admingiris fr = new admingiris();
            fr.Show();
            this.Hide();
        }

        private void btnGosterGizle_Click(object sender, EventArgs e)
        {
            if (sifreGorunur)
            {
                // Şifreyi gizle
                txtsifre.PasswordChar = '*';
                sifreGorunur = false;
            }
            else
            {
                // Şifreyi göster
                txtsifre.PasswordChar = '\0'; // Şifreyi açık hale getirir
                sifreGorunur = true;
            }
        }

        private void adminkayit_Load(object sender, EventArgs e)
        {

        }
    }
}
