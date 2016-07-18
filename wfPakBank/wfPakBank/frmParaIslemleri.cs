using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wfPakBank
{
    public partial class frmParaIslemleri : Form
    {
        public frmParaIslemleri()
        {
            InitializeComponent();
        }

        private void frmParaIslemleri_Load(object sender, EventArgs e)
        {
            lblTarih.Text = DateTime.Now.ToShortDateString();
            cbIslemTipleri.SelectedIndex = 1; //cekilen işlem tipini seçili hale getiriyoruz.
            //frmHesapDokumu frm = new frmHesapDokumu();
            //lblHesapID.Text = frmHesapDokumu.HesapID;
            //lblHesapNo.Text = frmHesapDokumu.HesapNo;
        }
        public void HesapBilgileriAl(string ID, string HesapNumarasi)
        {
            lblHesapID.Text = ID;
            lblHesapNo.Text = HesapNumarasi;
        }

        private void btnTamam_Click(object sender, EventArgs e)
        {
            StreamWriter HareketYaz = new StreamWriter("HesapHareketleri.txt", true);
            HareketYaz.WriteLine(lblHesapID.Text + ";" + lblHesapNo.Text + ";" + lblTarih.Text + ";" + txtTutar.Text + ";" + cbIslemTipleri.SelectedItem.ToString());
            HareketYaz.Close();
            MessageBox.Show("Para Hareketi kayıt edildi!");
            this.Close();
        }
    }
}
