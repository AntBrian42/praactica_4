using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; // Para validar estructuras de texto
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; // Para la conexión y uso de MySQL

namespace praactica_4
{
    public partial class Form1 : Form
    {
        // Cadena de conexión (cambia usuario, contraseña y base si son otros)
        string SQLConection = "Server=localhost;Port=3306;Database=programacionavanzada;Uid=root;Pwd=1234567;";

        public Form1()
        {
            InitializeComponent();
            // Declaración de los manejadores de eventos
            txbEdad.TextChanged += ValidarEdad;
            txbApellido.TextChanged += ValidarApellidos;
            txbNombre.TextChanged += ValidarNombre;
            txbEstatura.TextChanged += ValidarEstatura;
            txbTelefono.Leave += ValidarTelefono;
        }

        private void insertarRegistros(string nombre, string apellidos, int edad, decimal estatura, string telefono, string genero)
        {
            using (MySqlConnection conectar = new MySqlConnection(SQLConection))
            {
                conectar.Open();
                string insertQuery = "INSERT INTO registros (nombres, apellidos, edad, estatura, telefono, genero) " +
                                     "VALUES (@nombres, @apellidos, @edad, @estatura, @telefono, @genero)";
                using (MySqlCommand comando = new MySqlCommand(insertQuery, conectar))
                {
                    comando.Parameters.AddWithValue("@nombres", nombre);
                    comando.Parameters.AddWithValue("@apellidos", apellidos);
                    comando.Parameters.AddWithValue("@edad", edad);
                    comando.Parameters.AddWithValue("@estatura", estatura);
                    comando.Parameters.AddWithValue("@telefono", telefono);
                    comando.Parameters.AddWithValue("@genero", genero);
                    comando.ExecuteNonQuery();
                }
            }
        }

        private void ValidarNombre(object sender, EventArgs e)
        {
            TextBox cajaNombre = (TextBox)sender;
            if (cajaNombre.Text != "" && !EsTextoValido(cajaNombre.Text))
            {
                MessageBox.Show("Ingrese valores correctos para el Nombre", "Error Nombre", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cajaNombre.Clear();
            }
        }

        private void ValidarApellidos(object sender, EventArgs e)
        {
            TextBox cajaApellidos = (TextBox)sender;
            if (cajaApellidos.Text != "" && !EsTextoValido(cajaApellidos.Text))
            {
                MessageBox.Show("Ingrese valores correctos para el apellido", "Error Apellido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cajaApellidos.Clear();
            }
        }

        private bool EsTextoValido(string valor)
        {
            return Regex.IsMatch(valor, @"^[a-zA-Z\s]+$");
        }

        private void ValidarEdad(object sender, EventArgs e)
        {
            TextBox textboxEdad = (TextBox)sender;
            if (textboxEdad.Text != "" && !EsEnteroValido(textboxEdad.Text))
            {
                MessageBox.Show("Ingrese valores correctos para la edad", "Error edad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textboxEdad.Clear();
            }
        }

        private bool EsEnteroValido(string valor)
        {
            int resultado;
            return int.TryParse(valor, out resultado);
        }

        private void ValidarEstatura(object sender, EventArgs e)
        {
            TextBox textBoxEstatura = (TextBox)sender;
            if (textBoxEstatura.Text != "" && !EsFlotanteValido(textBoxEstatura.Text))
            {
                MessageBox.Show("Ingrese valores correctos para la estatura", "Error Estatura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxEstatura.Clear();
            }
        }

        private bool EsFlotanteValido(string valor)
        {
            float resultado;
            return float.TryParse(valor, out resultado);
        }

        private void ValidarTelefono(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string input = textBox.Text;
            if (input.Length > 10)
            {
                if (!EsEnteroValidoDe10Digitos(input))
                {
                    textBox.BackColor = Color.Red;
                }
            }
            else if (!EsEnteroValidoDe10Digitos(input))
            {
                textBox.BackColor = Color.Yellow;
            }
            else
            {
                textBox.BackColor = Color.SeaGreen;
            }
        }

        private bool EsEnteroValidoDe10Digitos(string valor)
        {
            long resultado;
            return long.TryParse(valor, out resultado) && valor.Length == 10;
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

            // Validar que los campos tengan el formato correcto
            if (EsEnteroValido(Edad) && EsFlotanteValido(Estatura) &&
                EsEnteroValidoDe10Digitos(Telefono) && EsTextoValido(Nombre) && EsTextoValido(Apellido))
            {
                string mensaje = $"Nombres: {Nombre}\n\rApellido: {Apellido}\n\rTelefono: {Telefono}\n\rEdad: {Edad}\n\rEstatura: {Estatura}\n\rGenero: {genero}";

                // Primero se inserta en la base de datos
                insertarRegistros(Nombre, Apellido, int.Parse(Edad), decimal.Parse(Estatura), Telefono, genero);

                //Ruta del Almacenamiento de nuestro documento
                string rutaFile = "C:\\Users\\brian\\OneDrive\\Módulo\\practica5.txt";
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

                MessageBox.Show("Datos insertados en la Base de Datos:\n\n" + mensaje, "Información BD", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Por favor, ingrese datos válidos en los campos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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