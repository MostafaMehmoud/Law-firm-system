using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Law.CORE.Entities;
using Law.DAL.Data;
using Law.DAL.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Law.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LawAppDbContext _context;

        public IRepositorySpecial<Issue> issues { get; }

        public IRepositorySpecial<Court> courts { get; }

        public IRepositorySpecial<Center> centers { get; }

        public IRepositorySpecial<Client> clients { get; }
        public IRepositorySpecial<Party> parties { get; }

        public IRepositorySpecial<IssueFile> issuesFile { get; }
        public IRepositoryAuth auth { get; }
        public IRepositorySpecial<CourtSession> courtsSession { get; }
        public IRepositorySpecial<Receipt> receipts { get; }

        public IRepositorySpecial<Payment> payments { get; }
        public ICaseRepository cases { get; }

        public IOfferRepository OfferRepository { get; }

        public IOpinionRepository OpinionRepository { get; }

        public int Complete() => _context.SaveChanges();
        public void Dispose() => _context.Dispose();
       
              public UnitOfWork(
        LawAppDbContext context,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
          RoleManager<IdentityRole> roleManager
    )
        {
            _context = context;
            issues=new RepositoryIssues(_context);
            courts=new RepositoryCourt(_context);
            centers=new RepositoryCenter(_context);
            clients=new RepositoryClient(_context);
            parties=new RepositoryParty(_context);
            issuesFile=new RepositoryIssueFile(_context);
            courtsSession=new RepositoryCourtSession(_context);
            receipts=new RepositoryReceipt(_context);
            payments = new RepositoryPayment(_context);
            auth = new RepositoryAuth(userManager, signInManager);
            cases = new RepositoryCase(_context);
            OfferRepository = new OfferRepository(_context);
            OpinionRepository= new OpinionRepository(_context); 

        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
