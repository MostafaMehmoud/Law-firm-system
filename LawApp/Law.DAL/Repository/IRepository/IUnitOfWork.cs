using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;

namespace Law.DAL.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositorySpecial<Issue> issues { get; }
        IRepositorySpecial<Court> courts { get; }
        IRepositorySpecial<Center> centers { get; }
        IRepositorySpecial<Client> clients { get; } 
        IRepositorySpecial<Party> parties { get; }
        IRepositorySpecial<IssueFile> issuesFile { get; }
        IRepositorySpecial<CourtSession> courtsSession { get; }
        IRepositorySpecial<Receipt> receipts { get; }   
        IRepositorySpecial<Payment> payments { get; }
        void Save();
        int Complete();
    }
}
