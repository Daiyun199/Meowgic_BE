using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Data.Data
{
    public interface IAppDbContext : IDisposable
    {
        DatabaseFacade DatabaseFacade { get; }
        EntityEntry Add(object entity);
    }
}
