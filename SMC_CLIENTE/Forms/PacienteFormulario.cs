using SMC_CLIENTE.Data;
using SMC_CLIENTE.Models;
using SMC_CLIENTE.Services; // ← AGREGAR ESTA LÍNEA


namespace SMC_CLIENTE.Forms
{
    public partial class PacienteFormulario : Form
    {
        private Paciente paciente;
        private bool esEdicion;

        public PacienteFormulario(Paciente pacienteExistente = null)
        {
            paciente = pacienteExistente ?? new Paciente();
            esEdicion = pacienteExistente != null;
            InitializeComponent();
            ConfigurarFormulario();
            CargarDatos();
        }

        private void ConfigurarFormulario()
        {
            this.Text = esEdicion ? "Editar Paciente" : "Nuevo Paciente";

            // Configurar DateTimePicker
            dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
            dtpFechaNacimiento.MaxDate = DateTime.Now.AddYears(-1);

            // Configurar ComboBoxes
            cboGenero.Items.AddRange(new string[] { "", "Masculino", "Femenino", "Otro" });
            cboEstadoCivil.Items.AddRange(new string[] { "", "Soltero", "Casado", "Divorciado", "Viudo" });

            // Configurar eventos
            btnGuardar.Click += BtnGuardar_Click;
            btnCancelar.Click += (s, e) => { this.DialogResult = DialogResult.Cancel; this.Close(); };
        }

        private void CargarDatos()
        {
            if (esEdicion && paciente != null)
            {
                txtNombre.Text = paciente.NombreCompleto ?? "";
                txtCedula.Text = paciente.Cedula ?? "";
                txtTelefono.Text = paciente.Telefono ?? "";
                txtEmail.Text = paciente.Email ?? "";
                txtDireccion.Text = paciente.Direccion ?? "";
                txtOcupacion.Text = paciente.Ocupacion ?? "";
                txtContactoEmergencia.Text = paciente.ContactoEmergencia ?? "";
                txtTelefonoEmergencia.Text = paciente.TelefonoEmergencia ?? "";

                dtpFechaNacimiento.Value = paciente.FechaNacimiento;

                if (!string.IsNullOrEmpty(paciente.Genero))
                    cboGenero.SelectedItem = paciente.Genero;

                if (!string.IsNullOrEmpty(paciente.EstadoCivil))
                    cboEstadoCivil.SelectedItem = paciente.EstadoCivil;
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormulario())
                return;

            try
            {
                // Asignar valores del formulario al objeto paciente
                paciente.NombreCompleto = txtNombre.Text.Trim();
                paciente.Cedula = txtCedula.Text.Trim();
                paciente.FechaNacimiento = dtpFechaNacimiento.Value;
                paciente.Genero = cboGenero.SelectedItem?.ToString();
                paciente.EstadoCivil = cboEstadoCivil.SelectedItem?.ToString();
                paciente.Telefono = txtTelefono.Text.Trim();
                paciente.Email = txtEmail.Text.Trim();
                paciente.Direccion = txtDireccion.Text.Trim();
                paciente.Ocupacion = txtOcupacion.Text.Trim();
                paciente.ContactoEmergencia = txtContactoEmergencia.Text.Trim();
                paciente.TelefonoEmergencia = txtTelefonoEmergencia.Text.Trim();

                bool resultado;

                if (esEdicion)
                {
                    // Actualizar paciente existente
                    resultado = PacienteService.Actualizar(paciente);
                }
                else
                {
                    // Crear nuevo paciente
                    paciente.FechaRegistro = DateTime.Now;
                    paciente.Activo = true;
                    resultado = PacienteService.Insertar(paciente);
                }

                if (resultado)
                {
                    MessageBox.Show(
                        esEdicion ? "Paciente actualizado correctamente." : "Paciente registrado correctamente.",
                        "Éxito",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "No se pudo guardar el paciente. Inténtelo de nuevo.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error al guardar el paciente: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private bool ValidarFormulario()
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                errores.Add("- El nombre es obligatorio");

            if (dtpFechaNacimiento.Value >= DateTime.Now.AddYears(-1))
                errores.Add("- La fecha de nacimiento debe ser válida");

            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !EsEmailValido(txtEmail.Text))
                errores.Add("- El formato del email no es válido");

            if (errores.Count > 0)
            {
                MessageBox.Show(
                    "Por favor, corrija los siguientes errores:\n\n" + string.Join("\n", errores),
                    "Errores de validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool EsEmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Método para usar tu clase DatabaseConnection existente
        public static bool TestConnection()
        {
            return DatabaseConnection.TestConnection();
        }

        private void PacienteFormulario_Load(object sender, EventArgs e)
        {

        }

        // Propiedad para obtener el paciente después de guardar
        public Paciente PacienteGuardado
        {
            get { return paciente; }
        }
    }
}