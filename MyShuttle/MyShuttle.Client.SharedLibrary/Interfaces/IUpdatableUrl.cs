using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShuttle.Client.Services.Interfaces
{
    public interface IUpdatableUrl
    {
        string UrlPrefix { get; set; }
    }
}
