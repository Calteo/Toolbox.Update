using System;
using System.Reflection;
using System.Windows.Forms;

namespace Toolbox.Update.Example
{
    public partial class ExampleForm : Form
    {
        public ExampleForm()
        {
            InitializeComponent();

            Updater = new GitHubUpdater("Calteo", "Toolbox.Update");
        }

        private GitHubUpdater Updater { get; }

        private void ExampleFormLoad(object sender, EventArgs e)
        {
            Text += " - " + Assembly.GetExecutingAssembly().GetName().Version;

            var i = 0;
            foreach (var arg in Environment.GetCommandLineArgs())
            {
                textBoxArgs.Text += $"[{i++}] = '{arg}'" + Environment.NewLine;
            }

            foreach (var version in Updater.GetVersions())
            {
                textBoxVersions.Text += $"{version.Version} - {version.Name} - {version.Published} - {version.Description}" + Environment.NewLine;
                foreach (var asset in version.Assets)
                {
                    textBoxVersions.Text += $"  {asset.Name} - {asset.GetType().Name} - {asset.Uri}" + Environment.NewLine;
                }
            }
        }

        private void ButtonUpdateClick(object sender, EventArgs e)
        {
            var latest = Updater.GetLatestVersion();
            Updater.Install(latest);
            Close();
        }
    }
}
