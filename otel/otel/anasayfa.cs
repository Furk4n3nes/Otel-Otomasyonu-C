using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace otel
{
    public partial class anasayfa : Form
    {
        public anasayfa()
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

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            gorevli fr=new gorevli();
            fr.Show();
            this.Hide();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            musterikayit fr=new musterikayit();
            fr.Show();
            this.Hide();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {

            // Çıkış yapmak isteyip istemediğini soran MessageBox
            DialogResult result = MessageBox.Show(
                "Çıkış yapmak istiyor musunuz?",
                "Çıkış",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // Eğer kullanıcı "Evet" seçerse, giriş formuna yönlendir
            if (result == DialogResult.Yes)
            {
                // Giriş formunu oluştur
                admingiris fr = new admingiris();
                fr.Show();

                // Mevcut formu kapat
                this.Close();
            }
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            odalar fr=new odalar();
            fr.Show();
            this.Hide();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            gecmiskayit fr=new gecmiskayit();
            fr.Show();
            this.Hide();
        }

        private void anasayfa_Load(object sender, EventArgs e)
        {

        }
    }
}
