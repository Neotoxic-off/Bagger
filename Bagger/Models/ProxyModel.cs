using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bagger.Models
{
    public class ProxyModel : Interface.INotified
    {
        private bool _running;
        public bool Running
        {
            get { return _running; }
            set { SetProperty(ref _running, value); }
        }

        private ObservableCollection<string> _requests;
        public ObservableCollection<string> Requests
        {
            get { return _requests; }
            set { SetProperty(ref _requests, value); }
        }

        private string _token;
        public string Token
        {
            get { return _token; }
            set { SetProperty(ref _token, value); }
        }
    }
}
