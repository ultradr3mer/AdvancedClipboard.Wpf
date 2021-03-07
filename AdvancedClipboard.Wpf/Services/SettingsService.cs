using AdvancedClipboard.Wpf.Events;
using Prism.Events;
using System;
using System.Security.Cryptography;
using System.Text;

namespace AdvancedClipboard.Wpf.Services
{
  public class SettingsService
  {
    #region Fields

    private readonly byte[] entropy = { 8, 6, 2, 4, 9, 3 };
    private CredentialsChangedEvent credentialsChangedEvent;
    private string userPassword;

    #endregion Fields

    #region Constructors

    public SettingsService(IEventAggregator eventAggregator)
    {
      this.UserName = Properties.Settings.Default.UserName;
      this.userPassword = Properties.Settings.Default.UserPassword;

      this.credentialsChangedEvent = eventAggregator.GetEvent<CredentialsChangedEvent>();
    }

    #endregion Constructors

    #region Properties

    public bool HasCredentials => !string.IsNullOrEmpty(this.UserName) && !string.IsNullOrEmpty(this.userPassword);

    public string UserName { get; private set; }

    public string UserPassword
    {
      get => this.Decrypt(userPassword);

      private set => userPassword = this.Encrypt(value);
    }

    #endregion Properties

    #region Methods

    public void SetCredentials(string name, string password)
    {
      this.UserName = name;
      this.UserPassword = password;

      Properties.Settings.Default.UserName = this.UserName;
      Properties.Settings.Default.UserPassword = this.userPassword;
      Properties.Settings.Default.Save();

      var payload = new CredentialsChangedData(this);
      this.credentialsChangedEvent.Publish(payload);
    }

    private string Decrypt(string text)
    {
      if (string.IsNullOrEmpty(text))
      {
        return string.Empty;
      }

      byte[] encryptedText = Convert.FromBase64String(text);
      byte[] originalText = ProtectedData.Unprotect(encryptedText, entropy, DataProtectionScope.CurrentUser);
      return Encoding.Unicode.GetString(originalText);
    }

    private string Encrypt(string text)
    {
      if (string.IsNullOrEmpty(text))
      {
        return string.Empty;
      }

      byte[] originalText = Encoding.Unicode.GetBytes(text);
      byte[] encryptedText = ProtectedData.Protect(originalText, entropy, DataProtectionScope.CurrentUser);
      return Convert.ToBase64String(encryptedText);
    }

    #endregion Methods
  }
}