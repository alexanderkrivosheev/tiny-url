using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyURL
{
    public class AppContext
    {
        public CancellationTokenSource _cancellationTokenSource { get; set; }

        public CancellationToken CancellationToken => _cancellationTokenSource.Token;

        public bool IsStoped => _cancellationTokenSource.Token.IsCancellationRequested;

        public AppContext()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
