using DeviceFinanceApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DeviceFinanceApp.DataManager
{
    public class DeviceFinanceContext : DbContext
    {
        public DeviceFinanceContext(DbContextOptions<DeviceFinanceContext> options) : base(options)
        {

        }
        public DbSet<ApplicationStatus> ApplicationStatus { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<DevicePlan> DevicePlan { get; set; }
        public DbSet<DeviceApplication> DeviceApplication { get; set; }
        public DbSet<DeviceBrand> DeviceBrand { get; set; }
        public DbSet<DeviceType> DeviceType { get; set; }
        public DbSet<OEM> OEM { get; set; }
        public DbSet<Partner> Partner { get; set; }
        public DbSet<PricingModel> PricingModel { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Subscription> Subscription { get; set; }
        public DbSet<CustomerOrder> CustomerOrder { get; set; }
        public DbSet<ParticpatingStore> ParticpatingStore { get; set; }
        public DbSet<NigerianState> NigerianState { get; set; }
        public DbSet<DeviceImage> DeviceImage { get; set; }
        public DbSet<DeviceChange> DeviceChange { get; set; }
        public DbSet<RefurbishedDevice> RefurbishedDevice { get; set; }
        public DbSet<OEM_Store> OEM_Store { get; set; }
        public DbSet<OEM_State> OEM_State { get; set; }
        public DbSet<OEM_LGA> OEM_LGA { get; set; }
        public DbSet<RepairCenter> RepairCenter { get; set; }
        public DbSet<CustomerRepayment> CustomerRepayment { get; set; }
        public DbSet<DeviceManager> DeviceManager { get; set; }
        public DbSet<DeviceStatus> DeviceStatus { get; set; }
        public DbSet<UtilityPartner> UtilityPartner { get; set; }
        public DbSet<SmsManager> SmsManager { get; set; }
        public DbSet<CustomerLedger> CustomerLedger { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<CustomerLedger>(entity =>
        //    {
        //        entity.HasKey(e => e.ID);

        //        entity.ToTable("CustomerLedger");

        //        entity.Property(e => e.LoanReference).IsRequired();

        //        entity.Property(e => e.LoanReference).IsRequired();
        //    });
        
        //    // [Asma Khalid]: Regster store procedure custom object.  
        //    modelBuilder.Query<SpGetProductByPriceGreaterThan1000>();
        //    modelBuilder.Query<SpGetProductByID>();
        //}
        //#region Get products whose price is greater than equal to 1000 store procedure method.  

        ///// <summary>  
        ///// Get products whose price is greater than equal to 1000 store procedure method.  
        ///// </summary>  
        ///// <returns>Returns - List of products whose price is greater than equal to 1000</returns>  
        //public async Task<List<SpGetProductByPriceGreaterThan1000>> GetProductByPriceGreaterThan1000Async()
        //{
        //    // Initialization.  
        //    List<SpGetProductByPriceGreaterThan1000> lst = new List<SpGetProductByPriceGreaterThan1000>();

        //    try
        //    {
        //        // Processing.  
        //        string sqlQuery = "EXEC [dbo].[GetProductByPriceGreaterThan1000] ";

        //        lst = await this.Query<SpGetProductByPriceGreaterThan1000>().FromSql(sqlQuery).ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    // Info.  
        //    return lst;
        //}

        //#endregion

        //#region Get product by ID store procedure method.  

        ///// <summary>  
        ///// Get product by ID store procedure method.  
        ///// </summary>  
        ///// <param name="productId">Product ID value parameter</param>  
        ///// <returns>Returns - List of product by ID</returns>  
        //public async Task<List<SpGetProductByID>> GetProductByIDAsync(int productId)
        //{
        //    // Initialization.  
        //    List<SpGetProductByID> lst = new List<SpGetProductByID>();

        //    try
        //    {
        //        // Settings.  
        //        SqlParameter usernameParam = new SqlParameter("@product_ID", productId.ToString() ?? (object)DBNull.Value);

        //        // Processing.  
        //        string sqlQuery = "EXEC [dbo].[GetProductByID] " +
        //                            "@product_ID";

        //        lst = await this.Query<SpGetProductByID>().FromSql(sqlQuery, usernameParam).ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    // Info.  
        //    return lst;
        //}

        //#endregion
    }

}
