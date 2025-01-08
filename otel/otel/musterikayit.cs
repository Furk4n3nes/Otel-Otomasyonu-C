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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace otel
{
    public partial class musterikayit : Form
    {
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-LTN03PA\\SQLEXPRESS;Initial Catalog=Otel;Integrated Security=True;Encrypt=False");

        private void odadurumuguncelle()
        {
            // Veritabanı bağlantı dizesini buraya ekleyin
            string connectionString = "Data Source=DESKTOP-LTN03PA\\SQLEXPRESS;Initial Catalog=Otel;Integrated Security=True;Encrypt=False";
            string query = "SELECT oda_id, oda_durum FROM odalar";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    // Her oda kaydını kontrol et
                    while (reader.Read())
                    {
                        int odaId = reader.GetInt32(0); // oda_id
                        bool odaDurum = reader.GetBoolean(1); // oda_durum

                        // GroupBox içindeki butona erişim sağla
                        Button odaButton = groupControl1.Controls["button" + odaId.ToString()] as Button;

                        if (odaButton != null)
                        {
                            // Eğer oda_durum true ise, butonun rengini kırmızı yap
                            if (odaDurum)
                            {
                                odaButton.BackColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                odaButton.BackColor = System.Drawing.Color.Green; // False ise farklı bir renk
                            }
                        }
                        else
                        {
                            // Buton bulunamazsa hata mesajı
                            MessageBox.Show("Button not found for oda_id: " + odaId.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantısı başarısız: " + ex.Message);
            }
        }

        public musterikayit()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            anasayfa fr=new anasayfa();
            fr.Show();
            this.Close();
        }

        private void cikistrh_ValueChanged(object sender, EventArgs e)
        {
            int ucret;
            DateTime kucuktrh = Convert.ToDateTime(giristrh.Text);
            DateTime buyuktrh = Convert.ToDateTime(cikistrh.Text);

            TimeSpan sonuc = buyuktrh - kucuktrh;

            gunfark.Text = sonuc.TotalDays.ToString();

            ucret = Convert.ToInt32(gunfark.Text) * 500;

            txtucret.Text = ucret.ToString();
        }

       

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();

                // 1. Oda durumunu kontrol et
                string odaDurumSorgu = "SELECT oda_durum FROM odalar WHERE oda_id = @oda_id";
                SqlCommand odaDurumCmd = new SqlCommand(odaDurumSorgu, baglanti);
                odaDurumCmd.Parameters.AddWithValue("@oda_id", int.Parse(txtoda.Text.Trim()));

                object odaDurumObj = odaDurumCmd.ExecuteScalar();
                if (odaDurumObj == null)
                {
                    MessageBox.Show("Seçtiğiniz oda mevcut değil. Lütfen geçerli bir oda seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool odaDurum = (bool)odaDurumObj;
                if (odaDurum)
                {
                    MessageBox.Show("Bu oda zaten dolu! Lütfen başka bir oda seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Müşteri kaydını ekle
                string musteriEkleSorgu = @"
        INSERT INTO musteriler (musteri_ad, musteri_soyad, musteri_telefon, musteri_tc, ucret, oda_id, giris, cikis)
        VALUES (@musteri_ad, @musteri_soyad, @musteri_telefon, @musteri_tc, @ucret, @oda_id, @giris, @cikis)";

                SqlCommand musteriEkleCmd = new SqlCommand(musteriEkleSorgu, baglanti);
                musteriEkleCmd.Parameters.AddWithValue("@musteri_ad", txtadi.Text.Trim());
                musteriEkleCmd.Parameters.AddWithValue("@musteri_soyad", txtsoyad.Text.Trim());
                musteriEkleCmd.Parameters.AddWithValue("@musteri_telefon", msktelefon.Text.Trim());
                musteriEkleCmd.Parameters.AddWithValue("@musteri_tc", msktc.Text.Trim());
                musteriEkleCmd.Parameters.AddWithValue("@ucret", decimal.Parse(txtucret.Text.Trim()));
                musteriEkleCmd.Parameters.AddWithValue("@oda_id", int.Parse(txtoda.Text.Trim()));
                musteriEkleCmd.Parameters.AddWithValue("@giris", giristrh.Value.Date);
                musteriEkleCmd.Parameters.AddWithValue("@cikis", cikistrh.Value.Date);

                musteriEkleCmd.ExecuteNonQuery();

                // 3. Oda durumunu güncelle
                string odaDurumGuncelleSorgu = "UPDATE odalar SET oda_durum = 1 WHERE oda_id = @oda_id";
                SqlCommand odaDurumGuncelleCmd = new SqlCommand(odaDurumGuncelleSorgu, baglanti);
                odaDurumGuncelleCmd.Parameters.AddWithValue("@oda_id", int.Parse(txtoda.Text.Trim()));
                odaDurumGuncelleCmd.ExecuteNonQuery();

                MessageBox.Show("Müşteri başarıyla kaydedildi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Müşteri eklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (baglanti.State == System.Data.ConnectionState.Open)
                {
                    baglanti.Close();
                }
            }

            odadurumuguncelle();

        }

        private void musterikayit_Load(object sender, EventArgs e)
        {
            odadurumuguncelle();
        }

        private void Oda1_Click(object sender, EventArgs e)
        {
            txtoda.Text="1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtoda.Text = "2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtoda.Text = "3";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txtoda.Text = "4";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txtoda.Text = "5";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txtoda.Text = "6";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            txtoda.Text = "7";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            txtoda.Text = "8";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txtoda.Text = "9";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            txtoda.Text = "10";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            txtoda.Text = "11";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            txtoda.Text = "12";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            txtoda.Text = "13";
        }

        private void button14_Click(object sender, EventArgs e)
        {
            txtoda.Text = "14";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            txtoda.Text = "15";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            txtoda.Text = "16";

        }
    }
}
