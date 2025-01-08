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
    
    public partial class sifre : Form
    {
        private bool sifreGorunur = false;
        public sifre()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-LTN03PA\\SQLEXPRESS;Initial Catalog=Otel;Integrated Security=True;Encrypt=False");


        private void simpleButton3_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            // Parametreli sorgu ile şifreyi güncelle
            SqlCommand komut = new SqlCommand("UPDATE admin SET kullanici_sifre = @yeniSifre WHERE kullanici_id = @kullaniciAdi", baglanti);
            komut.Parameters.AddWithValue("@yeniSifre", txtsifre.Text);
            komut.Parameters.AddWithValue("@kullaniciAdi", txtkullanici.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("Şifre Güncellendi.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);

            admingiris fr = new admingiris();
            fr.Show();
            this.Hide();

        }
    

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void sifre_Load(object sender, EventArgs e)
        {

        }
    }
}
