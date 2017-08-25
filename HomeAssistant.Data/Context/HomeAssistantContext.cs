using HomeAssistant.Data.Exceptions;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace HomeAssistant.Data.Context
{

    class HomeAssistantContext : DbContext, IHomeAssistantContext
    {
        private string userName;

        public HomeAssistantContext()
            : base("HomeAssistantContext")
        {
        }

        public HomeAssistantContext(string userName, string connectionString)
           : base(connectionString)
        {
            this.userName = userName;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<Plant> Plants { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<InspLotChar>().Property(x => x.ValueNumeric).HasPrecision(13, 3);
            
        }

        //public UserPolicyAccess[] CheckUserPolicyes(int userId, string userRightPolicyId)
        //{
        //    return this.Database.SqlQuery<UserPolicyAccess>("usp_LabUserCheckUserPolicyAccess @hUser = @p0, @bCheckUserRight = 1, @nProcessAreaLink = -1, @szUserPolicies = @p1", userId, userRightPolicyId).ToArray();
        //}

        public LoginInfo Login(string login, string password)
        {
            var result = this.Database.SqlQuery<LoginInfo>("usp_LabUserLogon @szUser = @p0, @szPassword = @p1", login, password);
            if (result == null)
            {
                return null;
            }
            return result.FirstOrDefault();
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                throw new FormattedDbEntityValidationException(ex);
            }
        }

        public override async Task<int> SaveChangesAsync()
        {
            try
            {
                return await base.SaveChangesAsync();
            }
            catch (DbEntityValidationException ex)
            {
                throw new FormattedDbEntityValidationException(ex);
            }
        }


    }
    public class LoginInfo
    {
        public int hUser { get; set; }
        public string szFullName { get; set; }
    }
}
