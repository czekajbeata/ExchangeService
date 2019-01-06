using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeService.Client.Services
{
    public class ReloadService
    {
        public Task ReloadPage()
        {
            return JSRuntime.Current.InvokeAsync<string>("wasmHelper.reloadPage");
        }
    }
}
