using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagingToolkit.QRCode.Codec;
using System.Drawing;
using System.IO;
namespace vCardGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDownloadVCard_Click(object sender, EventArgs e)
        {
            string content = GenerateVCardCode();
            SaveFileDialog svd = new SaveFileDialog();
            svd.Title = "Save vCard";
            svd.Filter = "VCard File (*.vcf)|.vcf";
            svd.ShowDialog();

            if (svd.FileName != "")
            {
                StreamWriter file = new StreamWriter(svd.FileName);
                file.WriteLine(content);
                file.Close();
                MessageBox.Show("Arquivo salvo com sucesso!");
            }
            else
            {
                MessageBox.Show("Selecione uma pasta e infome o nome do arquivo");
            }
        }
        private string GenerateVCardCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCARD");
            sb.AppendLine("VERSION:2.1");
            if (txtName.Text != "" && txtLastName.Text != "")
            {
                sb.AppendLine($"N;LANGUAGE=pt-br;CHARSET=Windows-1252:{txtLastName.Text};{txtName.Text}");
                sb.AppendLine($"FN;CHARSET=Windows-1252:{txtName.Text} {txtLastName.Text}");
            }
            if(txtCompanyName.Text != "")
                sb.AppendLine($"ORG:{txtCompanyName.Text}");
            if (txtTitle.Text != "")
                sb.AppendLine($"TITLE:{txtTitle.Text}");
            if (txtTelWork.Text != "")
                sb.AppendLine($"TEL;WORK;VOICE:{txtTelWork.Text}");
            if (txtTelHome.Text != "")
                sb.AppendLine($"TEL;HOME;VOICE:{txtTelHome.Text}");
            if (txtTelCel.Text != "")
                sb.AppendLine($"TEL;CELL;VOICE:{txtTelCel.Text}");
            if (txtAddress.Text != "")
                sb.AppendLine($"ADR;WORK;PREF:{txtAddress.Text}");
            if (txtURL.Text != "")
                sb.AppendLine($"URL;WORK:{txtURL.Text}");
            if (txtEmail.Text != "")
                sb.AppendLine($"EMAIL;PREF;INTERNET:{txtEmail.Text}");
            sb.AppendLine("END:VCARD");
            return sb.ToString();
        }

        private void btnDownloadQr_Click(object sender, EventArgs e)
        {
            try
            {
                QRCodeEncoder qrEnc = new QRCodeEncoder();
                qrEnc.QRCodeBackgroundColor = System.Drawing.Color.White;
                qrEnc.QRCodeForegroundColor = System.Drawing.Color.Black;
                //qrEnc.CharacterSet = "UTF-8";
                qrEnc.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrEnc.QRCodeScale = 3;
                qrEnc.QRCodeVersion = 0;
                qrEnc.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;

                Image img;
                string data = GenerateVCardCode();
                img = qrEnc.Encode(data);
                pbImage.Image = img;

                SaveFileDialog svd = new SaveFileDialog();
                svd.Title = "Save QR Code";
                svd.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg; *.jpeg; *.png";
                svd.ShowDialog();

                if(svd.FileName != "")
                {
                    img.Save(svd.FileName);
                    MessageBox.Show("Arquivo salvo com sucesso!");
                }
                else
                {
                    MessageBox.Show("Selecione uma pasta e infome o nome do arquivo");
                }


            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro: {ex.Message}","QrCode Generator");
            }
        }
    }
}
