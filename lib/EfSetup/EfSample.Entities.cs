using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database = System.Data.Entity.Database;

public class MyDbContext : DbContext
{

    public MyDbContext()
        : base("MyDbContext")
    { }

    public DbSet<Program> Programs { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserAssignment> UserAssignments { get; set; }
    

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        Database.SetInitializer<MyDbContext>(null);

        Debug.WriteLine("OnModelCreating Running");

        Debug.WriteLine(string.Format("MyDbContext Connection String: {0}",
            (new MyDbContext()).Database.Connection.ConnectionString));

        modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
    }
}

public class Program
{
    [Key]
    public int ProgramId { get; set; }
    public string ProgramName { get; set; }

    public virtual ICollection<UserAssignment> UserAssignments { get; set; }
}

public class Role
{
    public int RoleId { get; set; }
    public string RoleName { get; set; }

    public virtual ICollection<UserAssignment> UserAssignments { get; set; }
}

public class District
{
    [Key]
    public int DistrictId { get; set; }
    [MaxLength(100)]
    public string DistrictName { get; set; }

    public virtual ICollection<Region> Regions { get; set; }
    public virtual ICollection<UserAssignment> UserAssignments { get; set; }
}

public class Region
{
    [Key]
    public int RegionId { get; set; }
    public string RegionName { get; set; }

    public int DistrictId { get; set; }
    public District District { get; set; }

    public virtual ICollection<UserAssignment> UserAssignments { get; set; }
}

public class User
{
    [Key]
    public int UserId { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; }
    [MaxLength(100)]
    public string LastName { get; set; }
    [MaxLength(250)]
    public string Email { get; set; }

    public virtual ICollection<UserAssignment> UserAssignments { get; set; }
    public virtual ICollection<UserProfile> UserProfiles { get; set; }
}

public class UserProfile
{
    [Key]
    public int UserProfileId { get; set; }

    [MaxLength(250)]
    public string Key { get; set; }

    [MaxLength(250)]
    public string Value { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }
}

public class UserAssignment
{
    [Key]
    public int UserAssignmentId { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int ProgramId { get; set; }
    public Program Program { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; }

    public int DistrictId { get; set; }
    public District District { get; set; }

    public int RegionId { get; set; }
    public Region Region { get; set; }
}
