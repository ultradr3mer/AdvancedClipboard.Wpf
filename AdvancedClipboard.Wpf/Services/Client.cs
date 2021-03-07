using AdvancedClipboard.Wpf.Events;
using Prism.Events;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AdvancedClipboard.Wpf.Services
{
  public class CustomHttpClient : HttpClient
  {
    #region Fields

    private const string AuthorizationHeaderName = "Authorization";

    private readonly HttpClientHandler httpClient;

    #endregion Fields

    #region Constructors

    public CustomHttpClient(SettingsService settingsService, IEventAggregator eventAggregator, HttpClientHandler httpClient) : base(new HttpClientHandler())
    {
      this.httpClient = httpClient;
      eventAggregator.GetEvent<CredentialsChangedEvent>().Subscribe(this.OnCredentialsChanged);

      if (settingsService.HasCredentials)
      {
        this.SetCredentials(settingsService);
      }
    }

    #endregion Constructors

    #region Methods

    private void OnCredentialsChanged(CredentialsChangedData data)
    {
      this.SetCredentials(data.SettingsService);
    }

    private void SetCredentials(SettingsService settingsService)
    {
      this.httpClient.Credentials = new NetworkCredential(settingsService.UserName, settingsService.UserPassword);

      byte[] authBytes = Encoding.UTF8.GetBytes(settingsService.UserName + ":" + settingsService.UserPassword);
      if (this.DefaultRequestHeaders.Contains(AuthorizationHeaderName))
      {
        this.DefaultRequestHeaders.Remove(AuthorizationHeaderName);
      }

      this.DefaultRequestHeaders.Add(AuthorizationHeaderName, "BASIC " + Convert.ToBase64String(authBytes));
    }

    #endregion Methods
  }
}