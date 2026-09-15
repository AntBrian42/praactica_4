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

namespace praactica_4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string Nombre = txbNombre.Text;
            string Apellido = txbApellido.Text;
            string Telefono = txbTelefono.Text;
            string Edad = txbEdad.Text;
            string Estatura = txbEstatura.Text;
            string genero = "";
            if (rbMasculino.Checked)
            {
                genero = "masculino";
            }
            else if (rbFemenina.Checked)
            {
                genero = "femenina";
            }
            else if (rbOtro.Checked)
            {
                genero = "otros";
            }
            string mensaje = $"Nombres: {Nombre}\n\rApellido: {Apellido}\n\rTelefono: {Telefono}\n\rEdad: {Edad}\n\rEstatura: {Estatura}";
            MessageBox.Show(mensaje, "Registro de Usuario", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

            //Ruta del Almacenamiento de nuestro documento
            string rutaFile = "C:\\Users\\brian\\OneDrive\\Documentos\\Practica.txt";
            //Valor booleano de verificacion del archivo existente
            bool ArchivoExistente = File.Exists(rutaFile);

            using (StreamWriter Escritor = new StreamWriter(rutaFile, true))
            {
                if (ArchivoExistente)
                {
                    Escritor.WriteLine();
                }
                Escritor.WriteLine(mensaje);
            }

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txbNombre.Clear();
            txbApellido.Clear();
            txbTelefono.Clear();
            txbEdad.Clear();
            txbEstatura.Clear();

            rbMasculino.Checked = false;
            rbFemenina.Checked = false;
            rbOtro.Checked = false;

        }
    }
}
