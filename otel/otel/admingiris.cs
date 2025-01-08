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
    public partial class admingiris : Form
    {
        private bool sifreGorunur = false;

        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-LTN03PA\\SQLEXPRESS;Initial Catalog=Otel;Integrated Security=True;Encrypt=False");
        public admingiris()
        {
            InitializeComponent();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            adminkayit fr = new adminkayit();
            fr.Show();
            this.Hide();

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            sifre fr = new sifre();
            fr.Show(); 
            this.Hide();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                string sql = "SELECT * FROM admin WHERE kullanici_id=@kullaniciAdi AND kullanici_sifre=@kullaniciSifre";
                SqlCommand komut = new SqlCommand(sql, baglanti);

                // Parametreleri ekle
                komut.Parameters.AddWithValue("@kullaniciAdi", txtkullanici.Text.Trim());
                komut.Parameters.AddWithValue("@kullaniciSifre", txtsifre.Text.Trim());

                // DataTable'e veri çekmek için SqlDataAdapter
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(komut);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    anasayfa fr = new anasayfa();
                    fr.Show();
                    this.Hide();
                    MessageBox.Show("Giriş Başarılı");
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre hatalı.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Giriş başarısız: " + ex.Message);
            }
            finally
            {
                if (baglanti.State == ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }



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

        private void admingiris_Load(object sender, EventArgs e)
        {

        }

        
    }
}
