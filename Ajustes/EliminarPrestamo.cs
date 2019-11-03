﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;

namespace Bodega.Ajustes
{
    public partial class EliminarPrestamo : Form
    {
        string ConnStr = "Driver={MySQL ODBC 3.51 Driver};Server=localhost;Database=bodega_campito;uid=willi;pwd=1234";
        public EliminarPrestamo()
        {
            InitializeComponent();
            dtp_fecha.Enabled = false;
            btn_aceptarDate.Enabled = false;

            txt_codigo.Enabled = false;
            btn_aceptarCod.Enabled = false;

            cmb_propietario.Enabled = false;
            btn_aceparPro.Enabled = false;
            dtp_FechaPro.Enabled = false;

            cmb_propietario.DataSource = CapaDatosBodega.llenarPropietario();
            cmb_propietario.ValueMember = "Nombre";
        }

        public void PrestamosCodigo() ////////////////////////////////////////////////procedimiento para mostrar disponibilidad de producto en bodega

        {
            DataTable tabla = new DataTable();
            using (OdbcConnection con = new OdbcConnection(ConnStr))
            {
                con.Open();
                OdbcDataAdapter cmd = new OdbcDataAdapter("select * from encabezadoPrestamo b INNER JOIN DetallePrestamo a ON b.idPrestamo=a.FK_EncPrestamo WHERE b.idprestamo='" + txt_codigo.Text + "' AND a.FK_encPrestamo= '" + txt_codigo.Text + "' AND a.estado=1", con);//llama a la tabla de inventario para ver stock
                                                                                                                                                                                                    //OdbcDataReader queryResults = cmd.ExecuteReader();
                cmd.Fill(tabla);

            }

            dgv_Entradas.DataSource = tabla;

        }

        public void PrestamosFecha() ////////////////////////////////////////////////procedimiento para mostrar disponibilidad de producto en bodega

        {
            DataTable tabla = new DataTable();
            using (OdbcConnection con = new OdbcConnection(ConnStr))
            {
                con.Open();
                OdbcDataAdapter cmd = new OdbcDataAdapter("select * from encabezadoPrestamo b INNER JOIN detallePrestamo a ON b.idPrestamo=a.FK_EncPrestamo WHERE b.fecha='" + dtp_fecha.Value.ToString("yyyyMMdd") + "' and estado=1", con);//llama a la tabla de inventario para ver stock
                                                                                                                                                                                                                      //OdbcDataReader queryResults = cmd.ExecuteReader();
                cmd.Fill(tabla);

            }

            dgv_Entradas.DataSource = tabla;

        }

        public void PrestamosPropietario() ////////////////////////////////////////////////procedimiento para mostrar disponibilidad de producto en bodega

        {
            DataTable tabla = new DataTable();
            using (OdbcConnection con = new OdbcConnection(ConnStr))
            {
                con.Open();
                OdbcDataAdapter cmd = new OdbcDataAdapter("select * from encabezadoPrestamo b INNER JOIN detallePrestamo a ON b.idPrestamo=a.FK_EncPrestamo WHERE b.fecha='" + dtp_fecha.Value.ToString("yyyyMMdd") + "' and b.FK_Propietario='"+cmb_propietario.Text.ToString() +"' and estado=1", con);//llama a la tabla de inventario para ver stock

                cmd.Fill(tabla);

            }

            dgv_Entradas.DataSource = tabla;

        }

        private void btn_Fecha_Click(object sender, EventArgs e)
        {
            dtp_fecha.Enabled = true;
            btn_aceptarDate.Enabled = true;

            txt_codigo.Enabled = false;
            btn_aceptarCod.Enabled = false;
            cmb_propietario.Enabled = false;
            btn_aceparPro.Enabled = false;
            dtp_FechaPro.Enabled = false;
        }

        private void btn_codigo_Click(object sender, EventArgs e)
        {
            txt_codigo.Enabled = true;
            btn_aceptarCod.Enabled = true;

            cmb_propietario.Enabled = false;
            dtp_FechaPro.Enabled = false;
            btn_aceparPro.Enabled = false;
            dtp_fecha.Enabled = false;
            btn_aceptarDate.Enabled = false;
        }

        private void btn_propietario_Click(object sender, EventArgs e)
        {
            cmb_propietario.Enabled = true;
            btn_aceparPro.Enabled = true;
            dtp_FechaPro.Enabled = true;

            dtp_fecha.Enabled = false;
            btn_aceptarDate.Enabled = false;
            txt_codigo.Enabled = false;
            btn_aceptarCod.Enabled = false;
        }

        private void btn_aceptarDate_Click(object sender, EventArgs e)
        {
            PrestamosFecha();
        }

        private void btn_salir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_aceptarCod_Click(object sender, EventArgs e)
        {
            PrestamosCodigo();
        }

        private void btn_aceparPro_Click(object sender, EventArgs e)
        {
            PrestamosPropietario();
        }

        private void dgv_Entradas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txt_codPrestamo.Text = Convert.ToString(dgv_Entradas.Rows[e.RowIndex].Cells[0].Value.ToString());//llena el textbox con los campos seleccionados en el datagrid
                txt_producto.Text = Convert.ToString(dgv_Entradas.Rows[e.RowIndex].Cells[9].Value.ToString());//llena el textbox con los campos seleccionados en el datagrid
                txt_propietario.Text = Convert.ToString(dgv_Entradas.Rows[e.RowIndex].Cells[3].Value.ToString());
                txt_codDetalle.Text = Convert.ToString(dgv_Entradas.Rows[e.RowIndex].Cells[6].Value.ToString());
                txt_cantidad.Text = Convert.ToString(dgv_Entradas.Rows[e.RowIndex].Cells[7].Value.ToString());
                txt_prestador.Text = Convert.ToString(dgv_Entradas.Rows[e.RowIndex].Cells[4].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            if (txt_propietario.Text == "" || txt_codPrestamo.Text == "" || txt_producto.Text == "" || txt_codDetalle.Text == "" || txt_cantidad.Text == "")
            {
                MessageBox.Show("No se ha seleccionado algun articulo para eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult result = MessageBox.Show("¿Desea eliminar el prestamo con el producto '" + txt_producto.Text + "' con la cantidad de '" + txt_cantidad.Text + " del propietario '" + txt_propietario.Text + "' y prestador '"+txt_prestador.Text+"''?", "Nuevo", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        OdbcConnection con = new OdbcConnection(ConnStr);//varibale para llamar la conexion ODBC

                        OdbcCommand cmd1 = new OdbcCommand("DELETE FROM detallePrestamo WHERE Fk_Producto='" + txt_producto.Text + "' AND FK_EncPrestamo='" + txt_codPrestamo.Text + "' AND cantidad='" + txt_cantidad.Text + "' AND idDetallePrestamo='" + txt_codDetalle.Text + "' ", con);
                        con.Open();//abre la conexion ;
                        cmd1.ExecuteNonQuery();
                        con.Close();//cierra la conexion

                        OdbcCommand cmd3 = new OdbcCommand("DELETE FROM detallePrestamo_respaldo WHERE Fk_Producto='" + txt_producto.Text + "' AND FK_EncPrestamo_resp='" + txt_codPrestamo.Text + "' AND cantidad='" + txt_cantidad.Text + "' AND idDetallePrestamo_resp='" + txt_codDetalle.Text + "' ", con);
                        con.Open();//abre la conexion ;
                        cmd3.ExecuteNonQuery();
                        con.Close();//cierra la conexion

                        OdbcCommand cmd4 = new OdbcCommand("UPDATE detalleinventario set cantidad=cantidad + '" + txt_cantidad.Text + "' WHERE Fk_Producto='" + txt_producto.Text + "' AND FK_Propietario='" + txt_propietario.Text + "'", con);
                        con.Open();//abre la conexion ;
                        cmd4.ExecuteNonQuery();
                        con.Close();//cierra la conexion       

                        OdbcCommand cmd2 = new OdbcCommand("UPDATE detalleinventario set cantidad=cantidad - '" + txt_cantidad.Text + "' WHERE Fk_Producto='" + txt_producto.Text + "' AND FK_Propietario='" + txt_prestador.Text + "'", con);
                        con.Open();//abre la conexion ;
                        cmd2.ExecuteNonQuery();
                        con.Close();//cierra la conexion                  

                        if (btn_Fecha.Enabled == true && btn_codigo.Enabled == false && btn_propietario.Enabled == false)
                        {
                            PrestamosFecha();
                        }
                        else if (btn_Fecha.Enabled == false && btn_codigo.Enabled == true && btn_propietario.Enabled == false)
                        {
                            PrestamosCodigo();
                        }
                        else if (btn_Fecha.Enabled == false && btn_codigo.Enabled == false && btn_propietario.Enabled == true)
                        {
                            PrestamosPropietario();
                        }

                        txt_codDetalle.Text = "";
                        txt_codPrestamo.Text = "";
                        txt_cantidad.Text = "";
                        txt_producto.Text = "";
                        txt_propietario.Text = "";
                        txt_prestador.Text = "";
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("No se ha seleccionado algun articulo para eliminar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        MessageBox.Show(ex.ToString());
                    }
                }
                else if (result == DialogResult.No)
                {

                }
            }
        }
    }
}
