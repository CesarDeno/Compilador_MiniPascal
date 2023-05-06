using System.IO;

namespace Compilador
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            tbxTexto.Clear();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Title = "Open a file...";
            if (openfile.ShowDialog()==DialogResult.OK)
            {
                tbxTexto.Clear();
                using (StreamReader sr = new StreamReader(openfile.FileName))
                {
                    tbxTexto.Text = sr.ReadToEnd();
                    sr.Close();
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "Save file as...";
            if (savefile.ShowDialog()==DialogResult.OK)
            {
                StreamWriter txtouput = new StreamWriter(savefile.FileName);
                txtouput.Write(tbxTexto.Text);
                txtouput.Close();
            }
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            tbxTexto.Cut();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            tbxTexto.Copy();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            tbxTexto.Paste();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newToolStripButton.PerformClick();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openToolStripButton.PerformClick();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveToolStripButton.PerformClick();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbxTexto.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbxTexto.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cutToolStripButton.PerformClick();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            copyToolStripButton.PerformClick();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pasteToolStripButton.PerformClick();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbxTexto.SelectAll();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ejecutarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lexico lx = new Lexico(tbxTexto.Text);
            dgvTokens.DataSource = lx.EjecutarLexico();
            dgvErrores.DataSource = lx.getListaError;
        }

        private void ejecutarSintacticoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lexico lx = new Lexico(tbxTexto.Text);
            Sintactico stc = new Sintactico(lx.EjecutarLexico(), lx.getListaError);
            try
            {
                stc.ejecutarSintactico();
            }
            catch (Exception)
            {
                MessageBox.Show("Error en Sintactico/Semantico", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dgvErrores.DataSource = stc.getListaError;
            dgvPolish.DataSource = stc.getListaPolish;
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}