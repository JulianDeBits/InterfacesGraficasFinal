﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InterfacesGraficas
{
    public partial class editarRegistro : Form
    {
        private DataGridViewRow fila;
        private String tareaSeleccionada;
        private String descripcionSeleccionada;
        private String estadoSeleccionado;
        ConexionDB conexiondb = new ConexionDB();

        public editarRegistro(DataGridViewRow fila, String tareaSeleccionada, String descripcionSeleccionada, String estadoSeleccionado)
        {
            InitializeComponent();
            this.fila = fila;
            this.tareaSeleccionada = tareaSeleccionada;
            this.descripcionSeleccionada = descripcionSeleccionada;
            this.estadoSeleccionado = estadoSeleccionado;
            ObtenerDatosRegistro();
        }

        private void ObtenerDatosRegistro()
        {
            txt2NombreTarea.Text = tareaSeleccionada;
            txt2DescripcionTarea.Text = descripcionSeleccionada;
            cbox2EstadoTarea.SelectedItem = estadoSeleccionado;
        }

        private void btnCancelarModificacio_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEditarTarea_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt2NombreTarea.Text) || string.IsNullOrEmpty(txt2DescripcionTarea.Text) || string.IsNullOrEmpty(cbox2EstadoTarea.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos");
            }
            else
            {
                DialogResult result = MessageBox.Show(
                    "¿Estás seguro de que deseas editar este registro?",
                    "Confirmar modificación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string nombreTarea = txt2NombreTarea.Text;
                        string descripcionTarea = txt2DescripcionTarea.Text;
                        string estadoTarea = cbox2EstadoTarea.SelectedItem.ToString();

                        string query = "UPDATE Tareas SET Nombre = @nombre, Descripcion = @descripcion, Estado = @estado WHERE Nombre = @nombreTarea";

                        using (SqlConnection conn = new SqlConnection(conexiondb.ObtenerCadenaConexion()))
                        {
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@nombre", nombreTarea);
                            cmd.Parameters.AddWithValue("@descripcion", descripcionTarea);
                            cmd.Parameters.AddWithValue("@estado", estadoTarea);
                            cmd.Parameters.AddWithValue("@nombreTarea", tareaSeleccionada);

                            conn.Open();
                            cmd.ExecuteNonQuery();

                            fila.Cells[0].Value = nombreTarea; 
                            fila.Cells[1].Value = descripcionTarea; 
                            fila.Cells[2].Value = estadoTarea; 

                            MessageBox.Show("Registro actualizado correctamente");

                            this.Close(); 
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al editar el registro: " + ex.Message);
                    }
                }
            }
        }
    }
}
