﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cms
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class cmsEntities1 : DbContext
    {
        public cmsEntities1()
            : base("name=cmsEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Cemetery> Cemeteries { get; set; }
        public virtual DbSet<GraveOwner> GraveOwners { get; set; }
        public virtual DbSet<Mortuary> Mortuaries { get; set; }
        public virtual DbSet<ReportHeader> ReportHeaders { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<DualApplication> DualApplications { get; set; }
        public virtual DbSet<Grave> Graves { get; set; }
    }
}
