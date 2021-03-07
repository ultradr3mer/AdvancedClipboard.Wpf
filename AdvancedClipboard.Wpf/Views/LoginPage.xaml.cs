using AdvancedClipboard.Wpf.Services;
using Prism.Regions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AdvancedClipboard.Wpf.Views
{
  /// <summary>
  /// Interaction logic for LoginPage.xaml
  /// </summary>
  public partial class LoginPage : UserControl
  {
    #region Fields

    private readonly Client client;
    private readonly IRegionManager regionManager;
    private readonly SettingsService settingsService;

    #endregion Fields

    #region Constructors

    public LoginPage(SettingsService settingsService, Client client, IRegionManager regionManager)
    {
      InitializeComponent();

      this.settingsService = settingsService;
      this.client = client;
      this.regionManager = regionManager;

      this.username.Text = settingsService.UserName;
      this.password.Password = settingsService.UserPassword;
    }

    #endregion Constructors

    #region Methods

    private async void LoginClick(object sender, RoutedEventArgs e)
    {
      try
      {
        this.message.Text = "Signing in...";
        this.message.Visibility = Visibility.Visible;
        this.settingsService.SetCredentials(this.username.Text, this.password.Password);
        await this.client.ClipboardAuthorizeAsync();
        this.regionManager.RequestNavigate("MainRegion", new Uri(nameof(HistoryPage), UriKind.Relative));

        this.message.Text = string.Empty;
        this.message.Visibility = Visibility.Collapsed;
      }
      catch (Exception ex)
      {
        this.message.Text = ex.Message;
        this.message.Visibility = Visibility.Visible;
      }
    }

    #endregion Methods
  }
}