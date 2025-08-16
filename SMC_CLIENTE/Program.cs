using SMC_CLIENTE.Data;
using SMC_CLIENTE.Forms;

namespace SMC_CLIENTE
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Verificar conexi�n a la base de datos
            if (!DatabaseConnection.TestConnection())
            {
                MessageBox.Show("No se puede conectar a la base de datos. Verifique que XAMPP est� ejecut�ndose y la base de datos est� configurada.",
                               "Error de Conexi�n", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Mostrar formulario de login
            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Si el login es exitoso, mostrar el formulario principal
                    Application.Run(new MainForm());
                }
            }
        }
    }
}