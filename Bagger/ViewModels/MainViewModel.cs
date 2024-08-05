using Microsoft.Win32;
using Bagger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Fiddler;

using Bagger.Models.JSON;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Bagger.ViewModels
{
    public class MainViewModel : Interface.INotified
    {
        private string API => "bdff1.playfabapi.com";
        public ProxyModel Proxy { get; set; }
        public LoginWithSteam loginWithSteam => new LoginWithSteam();
        public DelegateCommand ChangeStateProxyCommand => new DelegateCommand(ChangeState);
        Dictionary<bool, Action<object>> States => new Dictionary<bool, Action<object>>()
        {
            { false, StartProxy },
            { true, StopProxy }
        };
        Dictionary<string, Func<Session, Session>> Corrupters => new Dictionary<string, Func<Session, Session>>()
        {
            { "/Client/LoginWithSteam", GetToken }
        };

        public MainViewModel()
        {
            Proxy = new ProxyModel()
            {
                Requests = new System.Collections.ObjectModel.ObservableCollection<string>()
            };
        }

        private void ChangeState(object data)
        {
            States[FiddlerApplication.IsStarted()](data);
        }

        private void StartProxy(object data)
        {
            FiddlerCoreStartupSettings startupSettings = new FiddlerCoreStartupSettingsBuilder()
                .RegisterAsSystemProxy()
                .DecryptSSL()
                .Build();

            if (FiddlerApplication.IsStarted() == false)
            {
                InstallCertificate();
                FiddlerApplication.Startup(startupSettings);
            }

            FiddlerApplication.BeforeResponse += Manipulate;
        }

        private void StopProxy(object data)
        {
            FiddlerApplication.BeforeResponse -= Manipulate;

            if (FiddlerApplication.IsStarted() == true)
            {
                FiddlerApplication.Shutdown();
                UninstallCertificate();
            }
        }

        private string? IsCorruptable(string query)
        {
            query = query.Replace(API, String.Empty);

            foreach (string key in Corrupters.Keys)
            {
                if (query.StartsWith(key) == true)
                    return (key);
            }

            return (null);
        }

        private bool InstallCertificate()
        {
            if (CertMaker.rootCertExists() == false)
            {
                if (CertMaker.createRootCert() == false)
                {
                    return (false);
                }

                if (CertMaker.trustRootCert() == false)
                {
                    return (false);
                }
            }

            return (true);
        }

        private bool UninstallCertificate()
        {
            if (CertMaker.rootCertExists() == true)
            {
                if (CertMaker.removeFiddlerGeneratedCerts(true) == false)
                {
                    return (false);
                }
            }

            return (true);
        }

        private void BeforeRequest(Session e)
        {
            return;
        }

        private Session GetToken(Session e)
        {
            Proxy.Token = loginWithSteam.data.EntityToken.EntityToken;

            return (e);
        }

        private void Manipulate(Session e)
        {
            string? key = null;

            if (e.oRequest.host != null)
            {
                if (e.hostname.Contains(API) == true)
                {
                    e.bBufferResponse = true;

                    Proxy.Requests.Add($"{e.hostname} {e.PathAndQuery}");
                    key = IsCorruptable(e.fullUrl);
                    if (key != null)
                    {
                        Corrupters[key](e);
                    }
                }
            }
        }
    }
}
